using System.Collections.Generic;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels.Entities
{
    public sealed class CategoryViewModel
    {
        public int ID { get; set; }
        public bool Visible { get; set; }
        public IList<string> NameParts { get; set; }
        public IList<CategoryViewModel> SubCategories { get; set; }
    }
}
