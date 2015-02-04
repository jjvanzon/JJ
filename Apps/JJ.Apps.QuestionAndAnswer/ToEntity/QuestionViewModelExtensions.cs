using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Framework.Business;

namespace JJ.Apps.QuestionAndAnswer.ToEntity
{
    internal static class QuestionViewModelExtensions
    {
        public static Question ToEntity(
            this QuestionViewModel viewModel, 
            IQuestionRepository questionRepository, 
            IAnswerRepository answerRepository,
            ISourceRepository sourceRepository,
            IQuestionTypeRepository questionTypeRepository,
            EntityStatusManager entityStatusManager)
        {
            Question question = questionRepository.TryGet(viewModel.ID);

            if (question == null)
            {
                question = questionRepository.Create();

                entityStatusManager.SetIsNew(question);
            }

            question.Text = viewModel.Text;
            question.IsActive = viewModel.IsActive;
            question.Source = sourceRepository.Get(viewModel.Source.ID);
            question.QuestionType = questionTypeRepository.Get(viewModel.Type.ID);

            return question;
        }

        public static Answer ToAnswer(this QuestionViewModel viewModel, IAnswerRepository answerRepository, EntityStatusManager entityStatusManager)
        {
            // TODO: Low prio: Maybe it is better to simply use the question entity as the source of the answer, instead of the repository.
            Answer answer = answerRepository.TryGetByQuestionID(viewModel.ID);
            if (answer == null)
            {
                answer = answerRepository.Create();

                entityStatusManager.SetIsNew(answer);
            }

            answer.IsCorrectAnswer = true;
            answer.Text = viewModel.Answer;
            return answer;
        }
    }
}
