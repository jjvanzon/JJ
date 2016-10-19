using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Data.Canonical;

namespace JJ.Business.CanonicalModel.Helpers
{
    internal static class DebugHelper
    {
        public static string GetDebuggerDisplay(IDAndName idAndName)
        {
            if (idAndName == null) throw new ArgumentNullException(nameof(idAndName));

            string debuggerDisplay = String.Format("{{{0}}} {1} {2}", nameof(IDAndName), idAndName.ID, idAndName.Name);
            return debuggerDisplay;
        }

        internal static string GetDebuggerDisplay(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            string debuggerDisplay = String.Format("{{{0}}} {1} - '{2}'", nameof(Message), message.PropertyKey, message.Text);
            return debuggerDisplay;
        }
    }
}
