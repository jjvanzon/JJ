﻿using System;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Calculation.Patches
{
    public interface IPatchCalculator
    {
        double Calculate(double time);
        double Calculate(double time, int channelIndex);

        /// <param name="frameCount">
        /// You cannot use buffer.Length as a basis for frameCount, 
        /// because if you write to the buffer beyond frameCount, then the audio driver might fail.
        /// A frameCount based on the entity model can differ from the frameCount you get from the driver,
        /// and you only know the frameCount at the time the driver calls us.
        /// </param>
        void Calculate(float[] buffer, int frameCount, double t0);

        double GetValue(int listIndex);
        void SetValue(int listIndex, double value);

        double GetValue(string name);
        void SetValue(string name, double value);

        double GetValue(string name, int listIndex);
        void SetValue(string name, int listIndex, double value);

        double GetValue(DimensionEnum dimensionEnum);
        void SetValue(DimensionEnum dimensionEnum, double value);

        double GetValue(DimensionEnum dimensionEnum, int listIndex);
        void SetValue(DimensionEnum dimensionEnum, int listIndex, double value);

        void CloneValues(IPatchCalculator sourcePatchCalculator);

        void Reset(double time);
        void Reset(double time, string name);
        void Reset(double time, int listIndex);
    }
}