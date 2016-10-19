using System.Collections.Generic;

namespace JJ.Business.Canonical
{
    public static class ValidationMessageExtensions
    {
        public static JJ.Data.Canonical.Message ToCanonical(this JJ.Framework.Validation.ValidationMessage sourceEntity)
        {
            return new JJ.Data.Canonical.Message
            {
                PropertyKey = sourceEntity.PropertyKey,
                Text = sourceEntity.Text
            };
        }

        public static List<JJ.Data.Canonical.Message> ToCanonical(this IEnumerable<JJ.Framework.Validation.ValidationMessage> sourceList)
        {
            var destList = new List<JJ.Data.Canonical.Message>();

            foreach (JJ.Framework.Validation.ValidationMessage sourceItem in sourceList)
            {
                JJ.Data.Canonical.Message destItem = sourceItem.ToCanonical();
                destList.Add(destItem);
            }

            return destList;
        }

        public static KeyValuePair<string, string> ToKeyValuePair(this JJ.Data.Canonical.Message sourceEntity)
        {
            return new KeyValuePair<string, string>(sourceEntity.PropertyKey, sourceEntity.Text);
        }

        public static IList<KeyValuePair<string, string>> ToKeyValuePairs(this IEnumerable<JJ.Data.Canonical.Message> sourceList)
        {
            var destList = new List<KeyValuePair<string, string>>();

            foreach (JJ.Data.Canonical.Message sourceItem in sourceList)
            {
                KeyValuePair<string, string> destItem = sourceItem.ToKeyValuePair();
                destList.Add(destItem);
            }

            return destList;
        }
    }
}
