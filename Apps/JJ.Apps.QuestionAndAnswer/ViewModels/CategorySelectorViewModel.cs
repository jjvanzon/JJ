using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class CategorySelectorViewModel
    {
        public bool NoCategoriesAvailable { get; set; }

        public List<CategoryViewModel> AvailableCategories { get; set; }

        public List<CategoryViewModel> SelectedCategories { get; set; }
    }
}
