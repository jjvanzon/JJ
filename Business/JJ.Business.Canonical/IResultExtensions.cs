using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Data.Canonical;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Canonical
{
    public static class IResultExtensions
    {
        public static void Combine(this IResult destResult, IResult sourceResult)
        {
            if (destResult == null) throw new NullException(() => destResult);
            if (sourceResult == null) throw new NullException(() => sourceResult);

            destResult.Successful &= sourceResult.Successful;
            destResult.Messages.AddRange(sourceResult.Messages);
        }
    }
}
