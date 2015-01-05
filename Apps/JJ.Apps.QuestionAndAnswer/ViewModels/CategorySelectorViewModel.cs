using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public sealed class CategorySelectorViewModel
    {
        public bool NoCategoriesAvailable { get; set; }

        public List<CategoryViewModel> AvailableCategories { get; set; }

        public List<CategoryViewModel> SelectedCategories { get; set; }
    }
}
