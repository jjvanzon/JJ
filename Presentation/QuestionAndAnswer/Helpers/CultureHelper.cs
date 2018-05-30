using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using JJ.Framework.Configuration;
using JJ.Framework.PlatformCompatibility;
using JJ.Presentation.QuestionAndAnswer.Configuration;

namespace JJ.Presentation.QuestionAndAnswer.Helpers
{
    internal static class CultureHelper
    {
        public static IList<string> AvailableCultureNames { get; } = CustomConfigurationManager.GetSection<ConfigurationSection>().AvailableCultureCodes;

        public static void SetCulture(string cultureName)
        {
            CultureInfo culture = CultureInfo_PlatformSafe.GetCultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static string GetCurrentCultureName() => Thread.CurrentThread.CurrentUICulture.Name;
    }
}