using System;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace JJ.Presentation.QuestionAndAnswer.AppService
{
    public class ResourceService : IResourceService
    {
        public PropertyDisplayNames GetPropertyDisplayNames(string cultureName)
        {
            return ConvertResources<JJ.Business.QuestionAndAnswer.Resources.PropertyDisplayNames, PropertyDisplayNames>(cultureName);
        }

        public Messages GetMessages(string cultureName)
        {
            return ConvertResources<Resources.Messages, Messages>(cultureName);
        }

        public Titles GetTitles(string cultureName)
        {
            return ConvertResources<Resources.Titles, Titles>(cultureName);
        }

        public CommonResources GetCommonResources(string cultureName)
        {
            return (CommonResources)ConvertResources(typeof(JJ.Framework.Presentation.Resources.CommonResourceFormatter), typeof(CommonResources), cultureName);
        }

        private TDest ConvertResources<TSource, TDest>(string cultureName)
            where TDest : new()
        {
            Type sourceType = typeof(TSource);
            Type destType = typeof(TDest);

            return (TDest)ConvertResources(sourceType, destType, cultureName);
        }

        private object ConvertResources(Type sourceType, Type destType, string cultureName) 
        {
            SetCulture(cultureName);

            var dest = Activator.CreateInstance(destType);

            foreach (PropertyInfo property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                PropertyInfo property2 = destType.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public);

                if (property2 != null)
                {
                    property2.SetValue(dest, property.GetValue(null));
                }
            }

            return dest;
        }

        private void SetCulture(string cultureName)
        {
            CultureInfo cultureInfo;;

            if (!string.IsNullOrEmpty(cultureName))
            {
                cultureInfo = CultureInfo.GetCultureInfo(cultureName);
            }
            else
            {
                cultureInfo = CultureInfo.InvariantCulture;
            }

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
