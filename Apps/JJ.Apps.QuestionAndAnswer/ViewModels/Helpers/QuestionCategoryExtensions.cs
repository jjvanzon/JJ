using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    public static class QuestionCategoryExtensions
    {
        public static QuestionCategoryViewModel ToViewModel(this QuestionCategory questionCategory)
        {
            if (questionCategory == null) throw new ArgumentNullException("questionCategory");

            var questionCategoryViewModel = new QuestionCategoryViewModel
            {
                QuestionCategoryID = questionCategory.ID,
                TemporaryID = Guid.NewGuid()
            };

            if (questionCategory.Category != null)
            {
                questionCategoryViewModel.Category = questionCategory.Category.ToViewModel();
            }
            else
            {
                questionCategoryViewModel.Category = GetEmptyCategoryViewModel();
            }

            return questionCategoryViewModel;
        }

        // TODO: Find a better place for this.
        private static CategoryViewModel GetEmptyCategoryViewModel()
        {
            return new CategoryViewModel
            {
                ID = 0,
                NameParts = new List<string>(),
                SubCategories = new List<CategoryViewModel>(),
                Visible = true
            };
        }
    }
}
