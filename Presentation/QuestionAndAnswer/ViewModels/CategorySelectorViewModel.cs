using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Partials;
using System.Collections.Generic;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels
{
    public sealed class CategorySelectorViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public bool NoCategoriesAvailable { get; set; }
        public IList<CategoryViewModel> AvailableCategories { get; set; }
        public IList<CategoryViewModel> SelectedCategories { get; set; }
    }
}
