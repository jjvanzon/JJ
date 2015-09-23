﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Extensions
{
    public static class ToneExtensions
    {
        public static double GetFrequency(Tone tone)
        {
            if (tone == null) throw new NullException(() => tone);

            // Officially this is an unnecessary null-check, but I suspect there could be programming errors.
            if (tone.Scale == null) throw new NullException(() => tone.Scale); 

            ScaleTypeEnum scaleTypeEnum = tone.Scale.GetScaleTypeEnum();

            switch (scaleTypeEnum)
            {
                case ScaleTypeEnum.LiteralFrequency:
                    return tone.Number;

                case ScaleTypeEnum.Factor:
                    {
                        AssertBaseFrequency(tone);
                        // BaseFrequency * 2 ^ octave * number
                        double frequency = tone.Scale.BaseFrequency.Value * Math.Pow(2, tone.Octave - 1) * tone.Number;
                        return frequency;
                    }

                case ScaleTypeEnum.Exponent:
                    {
                        // BaseFrequency * 2 ^ octave ^ number
                        // Notice that the multiplication is equvalent to (2 ^ octave) ^ number.
                        AssertBaseFrequency(tone);
                        double frequency = tone.Scale.BaseFrequency.Value * Math.Pow(2, tone.Octave - 1 * tone.Number);
                        return frequency;
                    }

                case ScaleTypeEnum.SemiTone:
                    {
                        // BaseFrequency * (2 ^ octave) ^ (1 / 12 * tone)
                        // You can probably simplify this formula, but it is more readable this way,
                        // and this method is not supposed to be called frequently.
                        AssertBaseFrequency(tone);
                        double frequency = tone.Scale.BaseFrequency.Value * Math.Pow(2, tone.Octave - 1) * Math.Pow(1.0 / 12.0, tone.Number);
                        return frequency;
                    }

                default:
                    throw new InvalidValueException(scaleTypeEnum);
            }
        }

        private static void AssertBaseFrequency(Tone tone)
        {
            if (!tone.Scale.BaseFrequency.HasValue) throw new NullException(() => tone.Scale.BaseFrequency);
        }
    }
}
