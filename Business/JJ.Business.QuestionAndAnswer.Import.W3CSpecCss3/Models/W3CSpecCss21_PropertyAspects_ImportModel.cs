using System;
using System.Collections.Generic;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models
{
    public class W3CSpecCss21_PropertyAspects_ImportModel
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
