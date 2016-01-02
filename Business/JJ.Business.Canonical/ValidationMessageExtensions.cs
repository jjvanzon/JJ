using System.Collections.Generic;

namespace JJ.Business.Canonical
{
    public static class ValidationMessageExtensions
    {
        public static JJ.Business.CanonicalModel.Message ToCanonical(this JJ.Framework.Validation.ValidationMessage sourceEntity)
        {
            return new JJ.Business.CanonicalModel.Message
            {
                PropertyKey = sourceEntity.PropertyKey,
                Text = sourceEntity.Text
            };
        }

        public static List<JJ.Business.CanonicalModel.Message> ToCanonical(this IEnumerable<JJ.Framework.Validation.ValidationMessage> sourceList)
        {
            var destList = new List<JJ.Business.CanonicalModel.Message>();

            foreach (JJ.Framework.Validation.ValidationMessage sourceItem in sourceList)
            {
                JJ.Business.CanonicalModel.Message destItem = sourceItem.ToCanonical();
                destList.Add(destItem);
            }

            return destList;
        }

        public static KeyValuePair<string, string> ToKeyValuePair(this JJ.Business.CanonicalModel.Message sourceEntity)
        {
            return new KeyValuePair<string, string>(sourceEntity.PropertyKey, sourceEntity.Text);
        }

        public static IList<KeyValuePair<string, string>> ToKeyValuePairs(this IEnumerable<JJ.Business.CanonicalModel.Message> sourceList)
        {
            var destList = new List<KeyValuePair<string, string>>();

            foreach (JJ.Business.CanonicalModel.Message sourceItem in sourceList)
            {
                KeyValuePair<string, string> destItem = sourceItem.ToKeyValuePair();
                destList.Add(destItem);
            }

            return destList;
        }
    }
}
