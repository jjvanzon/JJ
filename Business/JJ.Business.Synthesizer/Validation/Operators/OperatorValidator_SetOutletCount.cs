﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class OperatorValidator_SetOutletCount : FluentValidator<Operator>
    {
        private readonly int _newOutletCount;

        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OperatorValidator_SetOutletCount(Operator obj, int newOutletCount)
            : base(obj, postponeExecute: true)
        {
            _newOutletCount = newOutletCount;

            Execute();
        }

        protected override void Execute()
        {
            Operator op = Object;

            OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();
            if (operatorTypeEnum != OperatorTypeEnum.Unbundle)
            {
                string operatorTypeDisplayName = ResourceHelper.GetDisplayName(operatorTypeEnum);
                ValidationMessages.Add(() => op.OperatorType, MessageFormatter.OperatorTypeMustBeOfType(operatorTypeDisplayName));
            }

            For(_newOutletCount, PropertyNames.OutletCount, CommonTitleFormatter.ObjectCount(PropertyDisplayNames.Outlets))
                .GreaterThan(0);

            IList<Outlet> sortedOutlets = op.Outlets.OrderBy(x => x.ListIndex).ToArray();
            for (int i = _newOutletCount; i < sortedOutlets.Count; i++)
            {
                Outlet outlet = sortedOutlets[i];

                if (outlet.ConnectedInlets.Count > 0)
                {
                    string message = MessageFormatter.CannotChangeOutletsBecauseOneIsStillFilledIn(i + 1);
                    ValidationMessages.Add(PropertyNames.Outlets, message);
                }
            }
        }
    }
}
