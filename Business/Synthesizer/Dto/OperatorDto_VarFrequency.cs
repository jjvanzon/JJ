﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Business.Synthesizer.Dto
{
    internal abstract class OperatorDto_VarFrequency : OperatorDto
    {
        public OperatorDto FrequencyOperatorDto => InputOperatorDtos[0];

        public OperatorDto_VarFrequency(OperatorDto frequencyOperatorDto)
            : base(new OperatorDto[] { frequencyOperatorDto })
        { }
    }
}
