using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.QuestionAndAnswer.Extensions;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    public static class QuestionDetailViewModelExtensions
    {
        public static TextualQuestion ToModel(this QuestionDetailViewModel viewModel, ITextualQuestionRepository textualQuestionrepository, ITextualAnswerRepository textualAnswerRepository)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (textualQuestionrepository == null) throw new ArgumentNullException("textualQuestionrepository");
            if (textualAnswerRepository == null) throw new ArgumentNullException("textualAnswerRepository");

            TextualQuestion model = textualQuestionrepository.TryGet(viewModel.ID);
            if (model == null)
            {
                model = textualQuestionrepository.Create();
                model.AutoCreateRelatedEntities(textualAnswerRepository);
            }

            model.Text = viewModel.Question;
            model.TextualAnswer().Text = viewModel.Answer;

            return model;
        }
    }
}
