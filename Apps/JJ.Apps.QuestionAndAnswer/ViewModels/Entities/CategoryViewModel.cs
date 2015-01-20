using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Entities
{
    public sealed class CategoryViewModel
    {
        public int ID { get; set; }
        public bool Visible { get; set; }
        public IList<string> NameParts { get; set; }
        public IList<CategoryViewModel> SubCategories { get; set; }
    }
}
