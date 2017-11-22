using System.Collections.Generic;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models
{
	public class PropertyAspectsImportModel
	{
		public string HashTag { get; set; }

		public string PropertyName { get; set; }
		public string PossibleValues { get; set; }
		public string InitialValue { get; set; }
		public string AppliesTo { get; set; }
		public string IsInherited { get; set; }
		public string Percentages { get; set; }
		public string Media { get; set; }
		public string ComputedValue { get; set; }
		public string IsAnimatable { get; set; }

		public List<LinkModel> NameLinks { get; set; }
		public List<LinkModel> PossibleValuesLinks { get; set; }
		public List<LinkModel> InitialValueLinks { get; set; }
		public List<LinkModel> AppliesToLinks { get; set; }
		public List<LinkModel> IsInheritedLinks { get; set; }
		public List<LinkModel> PercentagesLinks { get; set; }
		public List<LinkModel> MediaLinks { get; set; }
		public List<LinkModel> ComputedValueLinks { get; set; }
		public List<LinkModel> IsAnimatableLinks { get; set; }
	}
}
