using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class CategoryViewModel
    {
        public int ID { get; set; }

        public bool Visible { get; set; }

        public List<string> NameParts { get; set; }

        public List<CategoryViewModel> SubCategories { get; set; }
    }
}
