using JJ.Framework.PlatformCompatibility;
using System.Globalization;
using System.Threading;

namespace JJ.Presentation.QuestionAndAnswer.Helpers
{
    internal static class CultureHelper
    {
        private static readonly string[] _availableCultureNames = new string[] { "en-US", "nl-NL" };
        public const string DEFAULT_CULTURE_NAME = "en-US";

        public static string[] GetAvailableCultureNames()
        {
            return _availableCultureNames;
        }

        public static void SetCulture(string cultureName)
        {
            CultureInfo culture = CultureInfo_PlatformSafe.GetCultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static string GetCurrentCultureName()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}
