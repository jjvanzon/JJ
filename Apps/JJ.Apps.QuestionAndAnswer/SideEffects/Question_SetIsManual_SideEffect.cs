using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Framework.Common;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.SideEffects
{
    internal class Question_SetIsManual_SideEffect : ISideEffect
    {
        private Question _question;
        private QuestionViewModel _viewModel;

        public Question_SetIsManual_SideEffect(Question question, QuestionViewModel viewModel)
        {
            if (question == null) throw new NullException(() => question);
            if (viewModel == null) throw new NullException(() => viewModel);

            _question = question;
            _viewModel = viewModel;
        }

        public void Execute()
        {
            if (MustSetIsManual(_viewModel))
            {
                _question.IsManual = true;
            }
        }

        private bool MustSetIsManual(QuestionViewModel viewModel)
        {
            // MustSetIsManual is almost determined by 'anything is dirty' except for question flag status changes.

            return viewModel.IsDirty ||
                   viewModel.IsNew ||
                   viewModel.Type.IsDirty ||
                   viewModel.Type.IsNew ||
                   viewModel.Source.IsDirty ||
                   viewModel.Source.IsNew ||
                   viewModel.Categories.IsDirty ||
                   viewModel.Categories.Any(x => x.IsDirty) ||
                   viewModel.Categories.Any(x => x.IsNew) ||
                   viewModel.Links.IsDirty ||
                   viewModel.Links.Any(x => x.IsDirty) ||
                   viewModel.Links.Any(x => x.IsNew);
        }
    }
}
