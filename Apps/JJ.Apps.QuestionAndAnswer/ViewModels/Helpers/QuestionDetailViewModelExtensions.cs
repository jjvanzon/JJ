using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class QuestionDetailViewModelExtensions
    {
        public static Question ToModel(this QuestionDetailViewModel viewModel, IQuestionRepository questionrepository, IAnswerRepository answerRepository)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (questionrepository == null) throw new ArgumentNullException("questionrepository");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");
            if (viewModel.Question == null) throw new ArgumentNullException("viewModel.Question");

            Question model = questionrepository.TryGet(viewModel.Question.ID);
            if (model == null)
            {
                model = questionrepository.Create();
                model.AutoCreateRelatedEntities(answerRepository);
            }

            model.Text = viewModel.Question.Text;

            // TODO: Refactor
            if (model.Answers.Count == 0)
            {
                model.Answers.Add(new Answer());
            }
            model.Answers[0].Text = viewModel.Question.Answer;

            return model;
        }
    }
}
