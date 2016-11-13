﻿using JJ.Framework.Reflection.Exceptions;

namespace JJ.Framework.Validation
{
    public abstract class FluentValidator<TRootObject> : FluentValidator_WithoutConstructorArgumentNullCheck<TRootObject>
    {
        /// <param name="postponeExecute">
        /// When set to true, you can do initializations in your constructor
        /// before Execute goes off. If so, then you have to call Execute in your own constructor.
        /// </param>
        public FluentValidator(TRootObject obj, bool postponeExecute = false)
            : base(obj, postponeExecute: true)
        {
            if (obj == null) throw new NullException(() => obj);

            if (!postponeExecute)
            {
                Execute();
            }
        }
    }
}