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
    }
}
