using System;

namespace JJ.Data.Canonical.Helpers
{
    internal static class DebugHelper
    {
        public static string GetDebuggerDisplay(IDAndName idAndName)
        {
            if (idAndName == null) throw new ArgumentNullException(nameof(idAndName));

            string debuggerDisplay = $"{{{nameof(IDAndName)}}} {idAndName.ID} {idAndName.Name}";
            return debuggerDisplay;
        }

        internal static string GetDebuggerDisplay(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            string debuggerDisplay = $"{{{nameof(Message)}}} {message.PropertyKey} - '{message.Text}'";
            return debuggerDisplay;
        }
    }
}
