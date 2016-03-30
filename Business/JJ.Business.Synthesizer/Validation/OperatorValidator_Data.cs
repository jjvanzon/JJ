﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Common;
using JJ.Business.Synthesizer.Configuration;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Validation
{
    /// <summary> Validates the inlet and outlet ListIndexes and that the inlet names are NOT filled in. </summary>
    public class OperatorValidator_Data : FluentValidator<Operator>
    {
        private readonly static int? _dataMaxLength = GetDataMaxLength();

        /// <summary> HashSet for unicity and value comparisons. </summary>
        private HashSet<string> _allowedDataKeysHashSet;

        public OperatorValidator_Data(
            Operator obj,
            IList<string> allowedDataKeys)
            : base(obj, postponeExecute: true)
        {
            if (allowedDataKeys == null) throw new NullException(() => allowedDataKeys);

            int uniqueExpectedDataPropertyKeyCount = allowedDataKeys.Distinct().Count();
            if (uniqueExpectedDataPropertyKeyCount != allowedDataKeys.Count)
            {
                throw new NotUniqueException(() => allowedDataKeys);
            }

            _allowedDataKeysHashSet = allowedDataKeys.ToHashSet();

            Execute();
        }

        protected override void Execute()
        {
            Operator op = Object;

            // Check length
            if (_dataMaxLength.HasValue)
            {
                For(() => op.Data, PropertyDisplayNames.Data).MaxLength(_dataMaxLength.Value);
            }

            // Check well-formedness
            if (!DataPropertyParser.DataIsWellFormed(Object))
            {
                ValidationMessages.AddIsInvalidMessage(() => op.Data, PropertyDisplayNames.Data);
            }
            else
            {
                IList<string> actualDataKeysList = DataPropertyParser.GetKeys(op); // List, not HashSet, so we can do a unicity check.

                // Check unicity
                int uniqueActualDataKeyCount = actualDataKeysList.Distinct().Count();
                if (uniqueActualDataKeyCount != actualDataKeysList.Count)
                {
                    ValidationMessages.AddNotUniqueMessage(PropertyNames.DataKeys, PropertyDisplayNames.DataKeys);
                }
                else
                {
                    HashSet<string> actualDataKeysHashSet = actualDataKeysList.ToHashSet(); // HashSet, not List, so we can do value comparisons.

                    foreach (string actualDataKey in actualDataKeysHashSet)
                    {
                        // Check non-existence
                        bool dataKeyIsAllowed = _allowedDataKeysHashSet.Contains(actualDataKey);
                        if (!dataKeyIsAllowed)
                        {
                            string dataKeyIdentifier = ValidationHelper.GetDataKeyIdentifier(actualDataKey);
                            ValidationMessages.AddNotInListMessage(PropertyNames.DataKey, dataKeyIdentifier);
                        }
                    }
                }
            }
        }

        private string GetPropertyDisplayName_ForInletCount()
        {
            return CommonTitleFormatter.ObjectCount(PropertyDisplayNames.Inlets);
        }

        private static int? GetDataMaxLength()
        {
            return ConfigurationHelper.GetSection<ConfigurationSection>().OperatorDataMaxLength;
        }
    }
}
