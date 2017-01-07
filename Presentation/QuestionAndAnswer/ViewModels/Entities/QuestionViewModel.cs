using System.Collections.Generic;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels.Entities
{
    public sealed class QuestionViewModel
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsManual { get; set; }

        public SourceViewModel Source { get; set; }
        public QuestionTypeViewModel Type { get; set; }

        public IList<QuestionLinkViewModel> Links { get; set; }
        public IList<QuestionCategoryViewModel> Categories { get; set; }

        public bool IsFlagged { get; set; }

        /// <summary> Not available in RandomQuestionViewModel. </summary>
        public IList<QuestionFlagViewModel> Flags { get; set; }
    }
}