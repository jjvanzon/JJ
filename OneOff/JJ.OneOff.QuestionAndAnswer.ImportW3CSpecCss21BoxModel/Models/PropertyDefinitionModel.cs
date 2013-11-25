using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Models
{
    public class PropertyDefinitionModel
    {
        public string HashTag { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
        public string Initial { get; set; }
        public string AppliesTo { get; set; }
        public string Inherited { get; set; }
        public string Percentages { get; set; }
        public string Media { get; set; }
        public string ComputedValue { get; set; }

        public List<LinkModel> NameLinks { get; set; }
        public List<LinkModel> ValueLinks { get; set; }
        public List<LinkModel> InitialLinks { get; set; }
        public List<LinkModel> AppliesToLinks { get; set; }
        public List<LinkModel> InheritedLinks { get; set; }
        public List<LinkModel> PercentagesLinks { get; set; }
        public List<LinkModel> MediaLinks { get; set; }
        public List<LinkModel> ComputedValueLinks { get; set; }
    }
}
