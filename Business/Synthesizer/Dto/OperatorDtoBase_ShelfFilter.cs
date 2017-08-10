﻿using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Dto
{
    internal abstract class OperatorDtoBase_ShelfFilter_AllVars : OperatorDtoBase_Filter_VarSound
    {
        public InputDto TransitionFrequency { get; set; }
        public InputDto TransitionSlope { get; set; }
        public InputDto DBGain { get; set; }

        public override IEnumerable<InputDto> Inputs
        {
            get => new[]
            {
                Sound,
                TransitionFrequency,
                TransitionSlope,
                DBGain
            };
            set
            {
                var array = value.ToArray();
                Sound = array[0];
                TransitionFrequency = array[1];
                TransitionSlope = array[2];
                DBGain = array[3];
            }
        }
    }

    internal abstract class OperatorDtoBase_ShelfFilter_ManyConsts : OperatorDtoBase_Filter_ManyConsts
    {
        public override InputDto Frequency => TransitionFrequency;

        public InputDto TransitionFrequency { get; set; }
        public InputDto TransitionSlope { get; set; }
        public InputDto DBGain { get; set; }

        public override IEnumerable<InputDto> Inputs
        {
            get => new[]
            {
                Sound,
                TransitionFrequency,
                TransitionSlope,
                DBGain
            };
            set
            {
                var array = value.ToArray();
                Sound = array[0];
                TransitionFrequency = array[1];
                TransitionSlope = array[2];
                DBGain = array[3];
            }
        }
    }
}
