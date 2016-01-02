using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Data.Canonical;
using JJ.Framework.PlatformCompatibility;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Canonical
{
    public static class ResultHelper
    {
        public static void Assert(IResult result)
        {
            if (result == null) throw new NullException(() => result);

            if (!result.Successful)
            {
                string formattedMessages = FormatMessages(result);
                throw new Exception(formattedMessages);
            }
        }

        public static string FormatMessages(IResult result)
        {
            if (result == null) throw new NullException(() => result);

            string formattedMessages = MessageHelper.FormatMessages(result.Messages);
            return formattedMessages;
        }
    }
}