﻿using JJ.Business.Synthesizer.Resources;
using JJ.Business.Synthesizer.Extensions;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation
{
    internal class OperatorValidator_Bundle : FluentValidator<Operator>
    {
        public OperatorValidator_Bundle(Operator obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            Operator op = Object;

            For(() => op.GetOperatorTypeEnum(), PropertyDisplayNames.OperatorType).Is(OperatorTypeEnum.Bundle);
            For(() => op.Inlets.Count, CommonTitleFormatter.EntityCount(PropertyDisplayNames.Inlets)).Above(1);
            For(() => op.Outlets.Count, CommonTitleFormatter.EntityCount(PropertyDisplayNames.Outlets)).Is(1);
            For(() => op.Data, PropertyDisplayNames.Data).IsNull();
        }
    }
}