using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    public static class QuestionDetailViewModelExtensions
    {
        public static TextualQuestion ToModel(this QuestionDetailViewModel viewModel, ITextualQuestionRepository repository)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (repository == null) throw new ArgumentNullException("repository");

            TextualQuestion model = repository.TryGet(viewModel.ID);
            if (model == null)
            {
                model = repository.CreateWithRelatedEntities();
            }

            model.Text = viewModel.Question;
            model.TextualAnswer.Text = viewModel.Answer;

            return model;
        }
    }
}
