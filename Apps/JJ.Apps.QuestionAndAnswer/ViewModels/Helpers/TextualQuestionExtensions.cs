using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Apps.QuestionAndAnswer.ViewModels;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    public static class TextualQuestionExtensions
    {
        public static QuestionDetailViewModel ToViewModel(this TextualQuestion textualQuestion)
        {
            if (textualQuestion == null) throw new ArgumentNullException("textualQuestion");

            return new QuestionDetailViewModel
            {
                ID = textualQuestion.ID,
                Question = textualQuestion.Text,
                Answer = textualQuestion.TextualAnswer().Text,
            };
        }
    }
}
