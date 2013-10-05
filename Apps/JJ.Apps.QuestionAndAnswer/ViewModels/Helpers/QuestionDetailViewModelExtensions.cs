﻿using JJ.Framework.Persistence;
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
    internal static class QuestionDetailViewModelExtensions
    {
        public static Question ToModel(this QuestionDetailViewModel viewModel, IQuestionRepository textualQuestionrepository, IAnswerRepository textualAnswerRepository)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (textualQuestionrepository == null) throw new ArgumentNullException("textualQuestionrepository");
            if (textualAnswerRepository == null) throw new ArgumentNullException("textualAnswerRepository");

            Question model = textualQuestionrepository.TryGet(viewModel.ID);
            if (model == null)
            {
                model = textualQuestionrepository.Create();
                model.AutoCreateRelatedEntities(textualAnswerRepository);
            }

            model.Text = viewModel.Question;
            // TODO: Refactor
            model.Answers[0].Text = viewModel.Answer;

            return model;
        }
    }
}
