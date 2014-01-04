using JJ.Apps.QuestionAndAnswer.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class RandomQuestionViewModel
    {
        public QuestionViewModel Question { get; set; }

        public bool AnswerIsVisible { get; set; }
        public string UserAnswer { get; set; }

        public CurrentUserQuestionFlagViewModel CurrentUserQuestionFlag { get; set; }

        /// <summary>
        /// Used internally for selecting the next question.
        /// </summary>
        public List<CategoryViewModel> SelectedCategories { get; set; }
    }
}
