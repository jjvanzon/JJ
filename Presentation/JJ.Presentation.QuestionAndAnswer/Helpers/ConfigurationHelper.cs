using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JJ.Presentation.QuestionAndAnswer.Helpers
{
    /// <summary>
    /// For using configuration settings without being dependent on System.Configuration.
    /// </summary>
    public static class ConfigurationHelper
    {
        private static object _sectionsLock = new object();
        private static IDictionary<Type, object> _sections = new Dictionary<Type, object>();

        internal static T GetSection<T>()
        {
            lock (_sectionsLock)
            {
                object section;
                if (!_sections.TryGetValue(typeof(T), out section))
                {
                    throw new Exception(String.Format(
                        "Configuration section of type '{0}' was not set. Call {1}.SetSection to allow {2} to use the configuration section.", 
                        typeof(T).FullName, 
                        typeof(ConfigurationHelper).FullName, 
                        typeof(ConfigurationHelper).Assembly.GetName().Name));
                }
                return (T)section;
            }
        }

        public static void SetSection<T>(T section)
        {
            if (section == null) throw new NullException(() => section);

            lock (_sectionsLock)
            {
                if (_sections.ContainsKey(typeof(T)))
                {
                    throw new Exception(String.Format("Configuration section of type '{0}' was already set.", typeof(T).FullName));
                }
                _sections.Add(typeof(T), section);
            }
        }
    }
}
