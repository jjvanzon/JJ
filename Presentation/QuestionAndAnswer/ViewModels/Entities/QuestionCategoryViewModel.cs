using System;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels.Entities
{
	public sealed class QuestionCategoryViewModel
	{
		/// <summary> Available for both committed and newly added entities. </summary>
		public Guid TemporaryID { get; set; }

		/// <summary> 0 for newly added items. </summary>
		public int QuestionCategoryID { get; set; }

		public CategoryViewModel Category { get; set; }
	}
}
