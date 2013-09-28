using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex
{
    public class ImportModel
    {
        public string Name { get; set; }
        public string Values { get; set; }
        public string InitialValue { get; set; }
        public string AppliesTo  { get; set; }
        public string Inherited { get; set; }
        public string Percentages { get; set; }
        public string Media { get; set; }

        public List<LinkModel> NameLinks { get; set; }
        public List<LinkModel> ValuesLinks { get; set; }
        public List<LinkModel> InitialValueLinks { get; set; }
        public List<LinkModel> AppliesToLinks { get; set; }
        public List<LinkModel> InheritedLinks { get; set; }
        public List<LinkModel> PercentagesLinks { get; set; }
        public List<LinkModel> MediaLinks { get; set; }
    }
}
