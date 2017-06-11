﻿using JJ.Business.Synthesizer.Extensions;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;
using System.Collections.Generic;
using JetBrains.Annotations;
using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.RepositoryInterfaces;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class OperatorWarningValidator_WithUnderlyingEntities : VersatileValidator<Operator>
    {
        /// <summary>
        /// Validates an operator, but not its descendant operators.
        /// Does validate underlying curves and samples.
        /// Makes sure that objects are only validated once to 
        /// prevent excessive validation messages.
        /// The reason that underlying entities such as samples and curves are validated here,
        /// is because even though it already happens when you validate a whole document,
        /// in some cases you do not validate the whole document, but a narrower scope,
        /// such as a patch.
        /// </summary>
        public OperatorWarningValidator_WithUnderlyingEntities(
            [NotNull] Operator op,
            [NotNull] ISampleRepository sampleRepository,
            [CanBeNull] HashSet<object> alreadyDone = null)
            : base(op)
        {
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            alreadyDone = alreadyDone ?? new HashSet<object>();

            if (alreadyDone.Contains(op)) return;
            alreadyDone.Add(op);

            ExecuteValidator(new OperatorWarningValidator_Versatile(op));

            if (op.GetOperatorTypeEnum() == OperatorTypeEnum.Sample)
            {
                if (int.TryParse(op.Data, out int sampleID))
                {
                    Sample sample = sampleRepository.TryGet(sampleID);
                    if (sample != null)
                    {
                        byte[] bytes = sampleRepository.TryGetBytes(sampleID);
                        ExecuteValidator(new SampleWarningValidator(sample, bytes, alreadyDone));
                    }
                }
            }

            // There are no curve warnings.
        }
    }
}
