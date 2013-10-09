using JJ.Apps.QuestionAndAnswer.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class QuestionDetailViewModel
    {
        public QuestionViewModel Question { get; set; }

        public bool NotFound { get; set; }
        public bool AnswerIsVisible { get; set; }
        public string UserAnswer { get; set; }

        /// <summary>
        /// Used internally for selecting the next question.
        /// </summary>
        public List<CategoryViewModel> SelectedCategories { get; set; }
    }
}
