using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
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

        public CommonTitles GetCommonTitles(string cultureName)
        {
            return ConvertResources<JJ.Framework.Presentation.Resources.CommonTitles, CommonTitles>(cultureName);
        }

        private TDest ConvertResources<TSource, TDest>(string cultureName)
            where TDest : new()
        {
            SetCulture(cultureName);

            var dest = new TDest();

            foreach (PropertyInfo property in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                PropertyInfo property2 = typeof(TDest).GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public);

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

            if (!String.IsNullOrEmpty(cultureName))
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
