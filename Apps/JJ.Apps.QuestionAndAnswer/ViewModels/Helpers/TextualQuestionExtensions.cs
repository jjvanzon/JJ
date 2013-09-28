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
        public static QuestionDetailViewModel ToViewModel(this Question entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            var model = new QuestionDetailViewModel
            {
                ID = entity.ID,
                Question = entity.Text,
                // TODO: Refactor
                Answer = entity.Answers[0].Text
            };


            model.Links = new List<LinkViewModel>();

            foreach (QuestionLink questionLink in entity.QuestionLinks)
            {
                var linkModel = new LinkViewModel(questionLink.Description, questionLink.Url);
                model.Links.Add(linkModel);
            }

            return model;
        }
    }
}
