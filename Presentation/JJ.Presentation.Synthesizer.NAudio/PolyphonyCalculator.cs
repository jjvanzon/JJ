﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JJ.Framework.Common;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Framework.Reflection.Exceptions;
using JJ.Business.Synthesizer.Enums;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.NAudio
{
    public class PolyphonyCalculator
    {
        private class PatchCalculatorInfo
        {
            public PatchCalculatorInfo(IPatchCalculator patchCalculator, int noteListIndex)
            {
                if (patchCalculator == null) throw new NullException(() => patchCalculator);

                PatchCalculator = patchCalculator;
                NoteListIndex = noteListIndex;
            }

            public IPatchCalculator PatchCalculator { get; private set; }
            public int NoteListIndex { get; private set; }

            public bool IsActive { get; set; }
            public double Delay { get; set; }
        }

        private class ThreadInfo
        {
            public ThreadInfo(Thread thread)
            {
                if (thread == null) throw new NullException(() => thread);

                Thread = thread;
                PatchCalculatorInfos = new List<PatchCalculatorInfo>(MAX_EXPECTED_PATCH_CALCULATORS_PER_THREAD);
                Lock = new AutoResetEvent(false);
            }

            public Thread Thread { get; private set; }
            public IList<PatchCalculatorInfo> PatchCalculatorInfos { get; private set; }
            public AutoResetEvent Lock { get; private set; }
        }

        private const int MAX_EXPECTED_PATCH_CALCULATORS_PER_THREAD = 32;
        private const int DEFAULT_CHANNEL_INDEX = 0; // TODO: Make multi-channel.

        private CountdownEvent _countdownEvent;
        private readonly IList<PatchCalculatorInfo> _patchCalculatorInfos;
        private readonly IList<ThreadInfo> _threadInfos;
        private readonly double _sampleDuration;
        private readonly double[] _buffer;
        private readonly object[] _bufferLocks;

        private double _t0;

        public PolyphonyCalculator(int threadCount, int bufferSize, double sampleDuration)
        {
            if (threadCount < 0) throw new LessThanException(() => threadCount, 0);
            if (bufferSize < 0) throw new LessThanException(() => bufferSize, 0);

            _sampleDuration = sampleDuration;
            _patchCalculatorInfos = new List<PatchCalculatorInfo>();
            _threadInfos = new ThreadInfo[threadCount];

            _buffer = new double[bufferSize];
            _bufferLocks = new object[bufferSize];
            for (int i = 0; i < bufferSize; i++)
            {
                _bufferLocks[i] = new object();
            }

            //_countdownEvent = new CountdownEvent(threadCount);

            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(CalculateSingleThread);
                thread.Priority = ThreadPriority.AboveNormal;

                var threadInfo = new ThreadInfo(thread);

                thread.Start(threadInfo);

                _threadInfos[i] = threadInfo;
            }
        }

        // Adding and removing calculators.

        public int AddPatchCalculator(IPatchCalculator patchCalculator)
        {
            if (patchCalculator == null) throw new NullException(() => patchCalculator);

            var patchCalculatorInfo = new PatchCalculatorInfo(patchCalculator, _patchCalculatorInfos.Count);
            patchCalculatorInfo.IsActive = true;

            _patchCalculatorInfos.Add(patchCalculatorInfo);

            ApplyToThreadInfos();

            return patchCalculatorInfo.NoteListIndex;
        }

        public void AddPatchCalculators(IList<IPatchCalculator> patchCalculators)
        {
            for (int i = 0; i < patchCalculators.Count; i++)
            {
                IPatchCalculator patchCalculator = patchCalculators[i];

                var patchCalculatorInfo = new PatchCalculatorInfo(patchCalculator, _patchCalculatorInfos.Count);
                patchCalculatorInfo.IsActive = true;

                _patchCalculatorInfos.Add(patchCalculatorInfo);
            }

            ApplyToThreadInfos();
        }

        public void RemovePatchCalculator(int index)
        {
            AssertPatchCalculatorInfosListIndex(index);

            _patchCalculatorInfos.RemoveAt(index);

            ApplyToThreadInfos();
        }

        public void RemovePatchCalculator(IPatchCalculator patchCalculator)
        {
            if (patchCalculator == null) throw new NullException(() => patchCalculator);

            _patchCalculatorInfos.RemoveFirst(x => x.PatchCalculator == patchCalculator);

            ApplyToThreadInfos();
        }

        private void ApplyToThreadInfos()
        {
            // Clear threads
            for (int i = 0; i < _threadInfos.Count; i++)
            {
                ThreadInfo threadInfo = _threadInfos[i];
                threadInfo.PatchCalculatorInfos.Clear();
            }

            // Determine calculators per thread
            IList<PatchCalculatorInfo> activePatchCalculators = _patchCalculatorInfos.Where(x => x.IsActive).ToArray();

            int threadIndex = 0;
            for (int activePatchCalculatorInfoIndex = 0; activePatchCalculatorInfoIndex < activePatchCalculators.Count; activePatchCalculatorInfoIndex++)
            {
                PatchCalculatorInfo activePatchCalculatorInfo = activePatchCalculators[activePatchCalculatorInfoIndex];
                ThreadInfo threadInfo = _threadInfos[threadIndex];

                threadInfo.PatchCalculatorInfos.Add(activePatchCalculatorInfo);

                threadIndex++;
                threadIndex = threadIndex % _threadInfos.Count;
            }
        }

        // Calculate

        /// <param name="channelIndex">
        /// This parameter is currently not used, but I want this abstraction to stay similar
        /// to PatchCalculator, or I would be refactoring my brains out.
        /// </param>
        public double[] Calculate(double t0, int channelIndex)
        {
            _t0 = t0;

            Array.Clear(_buffer, 0, _buffer.Length);

            _countdownEvent = new CountdownEvent(_threadInfos.Count);

            for (int i = 0; i < _threadInfos.Count; i++)
            {
                ThreadInfo threadInfo = _threadInfos[i];
                threadInfo.Lock.Set();
            }

            _countdownEvent.Wait();

            //_countdownEvent.Reset(_threadInfos.Count);

            return _buffer;
        }

        private void CalculateSingleThread(object threadInfoObject)
        {
            var threadInfo = (ThreadInfo)threadInfoObject;

            IList<PatchCalculatorInfo> patchCalculatorInfos = threadInfo.PatchCalculatorInfos;

Suspend:
            threadInfo.Lock.WaitOne();

            try
            {
                for (int i = 0; i < patchCalculatorInfos.Count; i++)
                {
                    PatchCalculatorInfo patchCalculatorInfo = patchCalculatorInfos[i];

                    if (!patchCalculatorInfo.IsActive)
                    {
                        continue;
                    }

                    IPatchCalculator patchCalculator = patchCalculatorInfo.PatchCalculator;
                    double delay = patchCalculatorInfo.Delay;

                    double t = _t0 - delay;

                    for (int j = 0; j < _buffer.Length; j++)
                    {
                        double value = patchCalculator.Calculate(t, DEFAULT_CHANNEL_INDEX);

                        // TODO: Not sure how to do a quicker interlocked add for doubles.
                        lock (_bufferLocks[j])
                        {
                            _buffer[j] += value;
                        }

                        t += _sampleDuration;
                    }
                }
            }
            finally
            {
                _countdownEvent.Signal();
            }

            goto Suspend;
        }

        // Source: http://stackoverflow.com/questions/1400465/why-is-there-no-overload-of-interlocked-add-that-accepts-doubles-as-parameters
        //private static double InterlockedAddDouble(ref double location1, double value)
        //{
        //    double newCurrentValue = 0;
        //    while (true)
        //    {
        //        double currentValue = newCurrentValue;
        //        double newValue = currentValue + value;
        //        newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
        //        if (newCurrentValue == currentValue)
        //            return newValue;
        //    }
        //}

        // Values

        public void SetDelay(int patchCalculatorIndex, double delay)
        {
            PatchCalculatorInfo patchCalculatorInfo = GetPatchCalculatorInfo(patchCalculatorIndex);

            patchCalculatorInfo.Delay = delay;
        }

        public void SetValue(InletTypeEnum inletTypeEnum, int noteListIndex, double value)
        {
            PatchCalculatorInfo patchCalculatorInfo = GetPatchCalculatorInfo(noteListIndex);

            if (inletTypeEnum == InletTypeEnum.NoteStart)
            {
                patchCalculatorInfo.Delay = value;
            }
            else
            {
                patchCalculatorInfo.PatchCalculator.SetValue(inletTypeEnum, value);
            }
        }

        public void SetValue(InletTypeEnum inletTypeEnum, double value)
        {
            for (int i = 0; i < _patchCalculatorInfos.Count; i++)
            {
                PatchCalculatorInfo patchCalculatorInfo = _patchCalculatorInfos[i];
                patchCalculatorInfo.PatchCalculator.SetValue(inletTypeEnum, value);
            }
        }

        public double GetValue(InletTypeEnum inletTypeEnum)
        {
            PatchCalculatorInfo patchCalculatorInfo = _patchCalculatorInfos.FirstOrDefault();
            if (patchCalculatorInfo != null)
            {
                return patchCalculatorInfo.PatchCalculator.GetValue(inletTypeEnum);
            }
            return 0.0;
        }

        public void ResetState(int noteListIndex)
        {
            PatchCalculatorInfo patchCalculatorInfo = GetPatchCalculatorInfo(noteListIndex);
            patchCalculatorInfo.PatchCalculator.ResetState();
        }

        public double GetValue(InletTypeEnum inletTypeEnum, int noteListIndex)
        {
            PatchCalculatorInfo patchCalculatorInfo = GetPatchCalculatorInfo(noteListIndex);

            if (inletTypeEnum == InletTypeEnum.NoteStart)
            {
                return patchCalculatorInfo.Delay;
            }
            else
            {
                double value = patchCalculatorInfo.PatchCalculator.GetValue(inletTypeEnum);
                return value;
            }
        }

        public void CloneValues(PolyphonyCalculator sourceCalculator)
        {
            for (int i = 0; i < _patchCalculatorInfos.Count; i++)
            {
                PatchCalculatorInfo source = sourceCalculator._patchCalculatorInfos[i];
                PatchCalculatorInfo dest = _patchCalculatorInfos[i];

                dest.Delay = source.Delay;
                dest.PatchCalculator.CloneValues(source.PatchCalculator);
            }
        }

        // Helpers

        private void AssertPatchCalculatorInfosListIndex(int patchCalcultorInfosListIndex)
        {
            if (patchCalcultorInfosListIndex < 0) throw new LessThanException(() => patchCalcultorInfosListIndex, 0);
            if (patchCalcultorInfosListIndex >= _patchCalculatorInfos.Count) throw new GreaterThanOrEqualException(() => patchCalcultorInfosListIndex, () => _patchCalculatorInfos.Count);
        }

        private PatchCalculatorInfo GetPatchCalculatorInfo(int noteListIndex)
        {
            AssertPatchCalculatorInfosListIndex(noteListIndex);

            PatchCalculatorInfo patchCalculatorInfo = _patchCalculatorInfos[noteListIndex];
            return patchCalculatorInfo;
        }
    }
}
