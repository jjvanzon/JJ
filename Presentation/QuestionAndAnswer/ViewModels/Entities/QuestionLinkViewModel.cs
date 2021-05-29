using System;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels.Entities
{
    public sealed class QuestionLinkViewModel
    {
        /// <summary> Available for both committed and newly added entities. </summary>
        public Guid TemporaryID { get; set; }

        /// <summary> 0 for newly added items. </summary>
        public int ID { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ReturnUrl { get; set; }
    }
}
