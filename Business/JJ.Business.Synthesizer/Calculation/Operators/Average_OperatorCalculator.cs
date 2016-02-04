﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Average_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly double _sampleDuration;
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _sampleCountDouble;

        private Queue<double> _queue;

        private double _sum;
        private double _average;
        private double _previousTime;
        private double _passedSampleTime;

        public Average_OperatorCalculator(OperatorCalculatorBase signalCalculator, double timeSliceDuration, int sampleCount)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            if (timeSliceDuration <= 0.0) throw new LessThanException(() => timeSliceDuration, 0.0);
            if (sampleCount <= 0) throw new LessThanOrEqualException(() => sampleCount, 0);

            _signalCalculator = signalCalculator;
            _sampleCountDouble = sampleCount;
            _sampleDuration = timeSliceDuration / _sampleCountDouble;

            _queue = CreateQueue();
        }

        public override double Calculate(double time, int channelIndex)
        {
            // Update _passedSampleTime
            double dt = time - _previousTime;
            if (dt >= 0)
            {
                _passedSampleTime += dt;
            }
            else
            {
                // Substitute for Math.Abs().
                // This makes it work for time that goes in reverse and even time that quickly goes back and forth.
                _passedSampleTime -= dt;
            }

            if (_passedSampleTime >= _sampleDuration)
            {
                // Use a queing trick to update the average without traversing a whole list.
                // This also makes the average update more continually.
                double oldValue = _queue.Dequeue();
                double newValue = _signalCalculator.Calculate(time, channelIndex);
                _queue.Enqueue(newValue);

                _sum -= oldValue;
                _sum += newValue;

                _average = _sum / _sampleCountDouble;

                // It may not be arithmetically perfect, that we ignore the fact that
                // _passedSampleTime may be significantly greater than _sampleDuration,
                // but in practice for this application it might not matter very much.
                _passedSampleTime = 0.0;
            }

            _previousTime = time;

            return _average;
        }

        public override void ResetState()
        {
            _queue = CreateQueue();
            _sum = 0.0;
            _average = 0.0;
            _previousTime = 0.0;
            _passedSampleTime = 0.0;

            base.ResetState();
        }

        private Queue<double> CreateQueue()
        {
            int sampleCountInt = (int)(_sampleCountDouble);

            Queue<double> queue = new Queue<double>(sampleCountInt);

            for (int i = 0; i < sampleCountInt; i++)
            {
                queue.Enqueue(0.0);
            }

            return queue;
        }
    }
}
