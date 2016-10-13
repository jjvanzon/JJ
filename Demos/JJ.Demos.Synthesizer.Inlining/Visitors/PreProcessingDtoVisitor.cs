﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Demos.Synthesizer.Inlining.Dto;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Demos.Synthesizer.Inlining.Visitors
{
    internal class PreProcessingDtoVisitor
    {
        public void Execute(OperatorDto operatorDto)
        {
            if (operatorDto == null) throw new NullException(() => operatorDto);

            new MathematicalPropertyDtoVisitor().Execute(operatorDto);
            new SpecializationDtoVisitor().Execute(operatorDto);
        }
    }
}
