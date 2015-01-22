using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public sealed class CategorySelectorViewModel
    {
        public LoginPartialViewModel Login { get; set; }
        public bool NoCategoriesAvailable { get; set; }
        public IList<CategoryViewModel> AvailableCategories { get; set; }
        public IList<CategoryViewModel> SelectedCategories { get; set; }
    }
}
