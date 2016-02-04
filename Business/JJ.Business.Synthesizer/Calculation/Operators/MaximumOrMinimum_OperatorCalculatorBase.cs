﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Collections;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    /// <summary>
    /// Base class for Maximum_OperatorCalculator and Minimum_OperatorCalculator that have almost the same implementation.
    /// </summary>
    internal abstract class MaximumOrMinimum_OperatorCalculatorBase : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly double _sampleDuration;
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _sampleCountDouble;

        private Queue<double> _queue;

        /// <summary>
        /// Even though the RedBlackTree does not store duplicates,
        /// which is something you would want, this might not significantly affect the outcome.
        /// </summary>
        private RedBlackTree<double, double> _redBlackTree;

        private double _maximumOrMinimum;
        private double _previousTime;
        private double _nextSampleTime;
        private readonly double _timeSliceDuration;

        public MaximumOrMinimum_OperatorCalculatorBase(
            OperatorCalculatorBase signalCalculator,
            double timeSliceDuration,
            int sampleCount)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            if (timeSliceDuration <= 0.0) throw new LessThanException(() => timeSliceDuration, 0.0);
            if (sampleCount <= 0) throw new LessThanOrEqualException(() => sampleCount, 0);

            _signalCalculator = signalCalculator;
            _sampleCountDouble = sampleCount;

            _timeSliceDuration = timeSliceDuration;
            _sampleDuration = timeSliceDuration / _sampleCountDouble;

            ResetState();
        }

        protected abstract double GetMaximumOrMinimum(RedBlackTree<double, double> redBlackTree);

        public override double Calculate(double time, int channelIndex)
        {
            bool isForwardInTime = time >= _previousTime;

            if (isForwardInTime)
            {
                bool mustUpdate = time > _nextSampleTime;
                if (mustUpdate)
                {
                    // Fake last sample time if time difference too much.
                    // This prevents excessive sampling in case of a large jump in time.
                    // (Also takes care of the assumption that time would start at 0.)
                    double timeDifference = time - _nextSampleTime;
                    double timeDifferenceTooMuch = timeDifference - _timeSliceDuration;
                    if (timeDifferenceTooMuch > 0.0)
                    {
                        _nextSampleTime += timeDifferenceTooMuch;
                    }

                    do
                    {
                        CalculateValueAndUpdateCollections(_nextSampleTime, channelIndex);

                        _nextSampleTime += _sampleDuration;
                    }
                    while (time > _nextSampleTime);

                    _maximumOrMinimum = GetMaximumOrMinimum(_redBlackTree);
                }
            }
            else
            {
                // Is backwards in time
                bool mustUpdate = time < _nextSampleTime;
                if (mustUpdate)
                {
                    // Fake last sample time if time difference too much.
                    // This prevents excessive sampling in case of a large jump in time.
                    // (Also takes care of the assumption that time would start at 0.)
                    double timeDifference = _nextSampleTime - time;
                    double timeDifferenceTooMuch = timeDifference - _timeSliceDuration;
                    if (timeDifferenceTooMuch > 0.0)
                    {
                        _nextSampleTime -= timeDifferenceTooMuch;
                    }

                    do
                    {
                        CalculateValueAndUpdateCollections(_nextSampleTime, channelIndex);

                        _nextSampleTime -= _sampleDuration;
                    }
                    while (time < _nextSampleTime);

                    _maximumOrMinimum = GetMaximumOrMinimum(_redBlackTree);
                }
            }

            // Check difference with brute force
            // (slight difference due to RedBlackTree not adding duplicates):
            //double treeMax = _maximum;
            //_maximum = _queue.Max();
            //if (treeMax != _maximum)
            //{
            //    int i = 0;
            //}

            _previousTime = time;

            return _maximumOrMinimum;
        }

        private void CalculateValueAndUpdateCollections(double time, int channelIndex)
        {
            double newValue = _signalCalculator.Calculate(time, channelIndex);

            double oldValue = _queue.Dequeue();
            _queue.Enqueue(newValue);

            _redBlackTree.Delete(oldValue);
            _redBlackTree.Insert(newValue, newValue);
        }

        public override void ResetState()
        {
            _redBlackTree = new RedBlackTree<double, double>();
            _queue = CreateQueue();
            _maximumOrMinimum = 0.0;
            _nextSampleTime = 0.0;

            base.ResetState();
        }

        private Queue<double> CreateQueue()
        {
            int sampleCountInt = (int)(_sampleCountDouble);

            var queue = new Queue<double>(sampleCountInt);
            for (int i = 0; i < sampleCountInt; i++)
            {
                queue.Enqueue(0.0);
            }

            return queue;
        }
    }
}
