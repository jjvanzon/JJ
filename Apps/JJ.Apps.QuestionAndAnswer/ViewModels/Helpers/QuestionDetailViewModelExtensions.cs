using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    public static class QuestionDetailViewModelExtensions
    {
        public static TextualQuestion ToModel(this QuestionDetailViewModel viewModel, IContext context)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (context == null) throw new ArgumentNullException("context");

            var repository = new TextualQuestionRepository(context, context.Location);

            TextualQuestion model = repository.Get(viewModel.ID);

            model.Text = viewModel.Question;
            model.TextualAnswer.Text = viewModel.Answer;
            
            return model;
        }
    }
}
