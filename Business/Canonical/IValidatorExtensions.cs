using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JJ.Data.Canonical;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;

namespace JJ.Business.Canonical
{
    // ReSharper disable once InconsistentNaming
    public static class IValidatorExtensions
    {
        public static VoidResult ToResult(this IValidator validator)
        {
            if (validator == null) throw new NullException(() => validator);

            var result = new VoidResult
            {
                Successful = validator.IsValid,
                Messages = validator.ValidationMessages.ToCanonical()
            };

            return result;
        }

        public static VoidResult ToResult([NotNull] this IEnumerable<IValidator> validators)
        {
            if (validators == null) throw new NullException(() => validators);

            // Prevent multiple enumeration.
            validators = validators.ToArray();

            return new VoidResult
            {
                Successful = validators.All(x => x.IsValid),
                Messages = validators.SelectMany(x => x.ValidationMessages).ToCanonical()
            };
        }
    }
}