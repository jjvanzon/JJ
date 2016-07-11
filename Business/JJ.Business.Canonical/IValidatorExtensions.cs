using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Data.Canonical;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;

namespace JJ.Business.Canonical
{
    public static class IValidatorExtensions
    {
        public static VoidResult ToResult(this IValidator validator)
        {
            if (validator == null) throw new NullException(() => validator);

            var result = new VoidResult
            {
                Successful = false,
                Messages = validator.ValidationMessages.ToCanonical()
            };

            return result;
        }
    }
}
