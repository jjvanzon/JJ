using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JJ.Framework.Configuration;

namespace JJ.Framework.Data.NHibernate
{
    internal static class SqlLogger
    {
        private static bool _enabled = GetEnabled();
        private static string _filePath = GetFilePath();
        private static object _lock = new object();

        /// <summary> Controlled in the config. </summary>
        public static bool Enabled { get { return _enabled; } }

        /// <summary> Writes a line, if SqlLogging is Enabled in the config. </summary>
        public static void WriteLine(string str)
        {
            if (!_enabled)
            {
                return;
            }

            lock (_lock)
            {
                using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    using (var streamWriter = new StreamWriter(stream))
                    {
                        streamWriter.WriteLine(str);
                    }
                }
            }
        }

        private static bool GetEnabled()
        {
            ConfigurationSection config = CustomConfigurationManager.TryGetSection<ConfigurationSection>();
            if (config == null)
            {
                return false;
            }

            if (config.SqlLogging == null)
            {
                return false;
            }

            return config.SqlLogging.Enabled;
        }

        private static string GetFilePath()
        {
            ConfigurationSection config = CustomConfigurationManager.TryGetSection<ConfigurationSection>();
            if (config == null)
            {
                return null;
            }

            if (config.SqlLogging == null)
            {
                return null;
            }

            return config.SqlLogging.FilePath;
        }
    }
}
