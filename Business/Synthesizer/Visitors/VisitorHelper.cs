﻿using System;
using System.Collections;
using JJ.Framework.Exceptions;
using JJ.Framework.Reflection;

namespace JJ.Business.Synthesizer.Visitors
{
    internal static class VisitorHelper
    {
        /// <summary>
        /// Checks whether the stack count is incremented by exactly 1 after the action.
        /// It looks like this functionality belongs in a base visitor,
        /// but there already is a base class, and this feature applies only to very specific derived
        /// classes and I cannot just add on features without repeating code,
        /// unless I put it in a helper method like this.
        /// </summary>
        /// <param name="stack">
        /// I need to call it stack in the error messages,
        /// ICollection is used in lack of an IStack interface.
        /// </param>
        public static void WithStackCheck(ICollection stack, Action action)
        {
            if (stack == null) throw new NullException(() => stack);
            if (action == null) throw new NullException(() => action);

            int stackCountBefore = stack.Count;

            action();

            int expectedStackCount = stackCountBefore + 1;

            if (stack.Count != expectedStackCount)
            {
                throw new Exception(String.Format(
                    "{2} was not incremented by exactly 1 after visitation. {0} = {1}, {2} = {3}.",
                    nameof(expectedStackCount),
                    expectedStackCount,
                    ExpressionHelper.GetText(() => stack.Count),
                    stack.Count));
            }
        }
    }
}