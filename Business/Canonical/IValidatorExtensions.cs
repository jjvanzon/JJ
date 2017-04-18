using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JJ.Data.Canonical;
using JJ.Framework.Collections;
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

        /// <summary>
        /// Mind that destResult.Successful should be set to true,
        /// if it is ever te be set to true.
        /// </summary>
        public static void ToResult([NotNull] this IEnumerable<IValidator> validators, [NotNull] IResult destResult)
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if (validators == null) throw new NullException(() => validators);
            if (destResult == null) throw new ArgumentNullException(nameof(destResult));

            // Prevent multiple enumeration.
            validators = validators.ToArray();

            destResult.Successful &= validators.All(x => x.IsValid);

            destResult.Messages = destResult.Messages ?? new List<Message>();

            destResult.Messages.AddRange(validators.SelectMany(x => x.ValidationMessages).ToCanonical());
        }

        public static VoidResult ToResult([NotNull] this IEnumerable<IValidator> validators)
        {
            var result = new VoidResult { Successful = true, Messages = new List<Message>() };

            ToResult(validators, result);

            return result;
        }
    }
}