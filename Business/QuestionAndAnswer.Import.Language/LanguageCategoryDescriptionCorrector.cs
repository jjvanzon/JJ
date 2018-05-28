using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;

namespace JJ.Business.QuestionAndAnswer.Import.Language
{
	public class LanguageCategoryDescriptionCorrector : CategoryDescriptionCorrectorBase
	{
		public LanguageCategoryDescriptionCorrector(ICategoryRepository categoryRepository)
			: base(categoryRepository) { }

		public override void Execute()
		{
			CorrectCategoryDescription("DutchToPolish", "Dutch to Polish");
			CorrectCategoryDescription("PolishToDutch", "Polish to Dutch");
			CorrectCategoryDescription("HomeImprovement", "Home Improvement");
			CorrectCategoryDescription("DutchPolish", "Dutch / Polish");
		}
	}
}