using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.Extensions
{
    internal static class ValidationMessageExtensions
    {
        public static KeyValuePair<string, string> ToKeyValuePair(this JJ.Business.CanonicalModel.ValidationMessage sourceEntity)
        {
            return new KeyValuePair<string, string>(sourceEntity.PropertyKey, sourceEntity.Text);
        }

        public static IList<KeyValuePair<string, string>> ToKeyValuePairs(this IEnumerable<JJ.Business.CanonicalModel.ValidationMessage> sourceList)
        {
            var destList = new List<KeyValuePair<string, string>>();

            foreach (JJ.Business.CanonicalModel.ValidationMessage sourceItem in sourceList)
            {
                KeyValuePair<string, string> destItem = sourceItem.ToKeyValuePair();
                destList.Add(destItem);
            }

            return destList;
        }
    }
}