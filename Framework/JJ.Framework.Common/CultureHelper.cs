﻿using JJ.Framework.PlatformCompatibility;
using System.Globalization;
using System.Threading;

namespace JJ.Framework.Common
{
    public static class CultureHelper
    {
        public static void SetThreadCulture(string cultureName)
        {
            CultureInfo cultureInfo = CultureInfo_PlatformSafe.GetCultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
