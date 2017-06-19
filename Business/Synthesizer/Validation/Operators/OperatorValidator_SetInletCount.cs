﻿using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class OperatorValidator_SetInletCount : VersatileValidator<Operator>
    {
        private static readonly HashSet<OperatorTypeEnum> _allowedOperatorTypeEnums = new HashSet<OperatorTypeEnum>
        {
            OperatorTypeEnum.Add,
            OperatorTypeEnum.AverageOverInlets,
            OperatorTypeEnum.ClosestOverInlets,
            OperatorTypeEnum.ClosestOverInletsExp,
            OperatorTypeEnum.InletsToDimension,
            OperatorTypeEnum.MaxOverInlets,
            OperatorTypeEnum.MinOverInlets,
            OperatorTypeEnum.Multiply,
            OperatorTypeEnum.SortOverInlets
        };

        private static readonly IList<string> _allowedOperatorTypeDisplayNames = _allowedOperatorTypeEnums.Select(x => ResourceFormatter.GetDisplayName(x)).ToArray();

        public OperatorValidator_SetInletCount(Operator op, int newInletCount)
            : base(op)
        {
            OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();

            if (!_allowedOperatorTypeEnums.Contains(operatorTypeEnum))
            {
                ValidationMessages.AddNotInListMessage(() => operatorTypeEnum, ResourceFormatter.OperatorType, _allowedOperatorTypeDisplayNames);
                return;
            }

            For(newInletCount, PropertyNames.InletCount, CommonResourceFormatter.Count_WithNamePlural(ResourceFormatter.Inlets))
                .GreaterThan(1);

            IList<Inlet> sortedInlets = op.Inlets.OrderBy(x => x.Position).ToArray();
            for (int i = newInletCount; i < sortedInlets.Count; i++)
            {
                Inlet inlet = sortedInlets[i];

                // ReSharper disable once InvertIf
                if (inlet.InputOutlet != null)
                {
                    string message = ResourceFormatter.CannotChangeInletsBecauseOneIsStillFilledIn(i + 1);
                    ValidationMessages.Add(nameof(op.Inlets), message);
                }
            }
        }
    }
}