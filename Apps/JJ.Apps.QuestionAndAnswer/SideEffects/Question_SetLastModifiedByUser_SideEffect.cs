using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Framework.Business;
using JJ.Framework.Reflection;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.SideEffects
{
    internal class Question_SetLastModifiedByUser_SideEffect : ISideEffect
    {
        private Question _question;
        private User _user;
        private QuestionViewModel _viewModel;

        public Question_SetLastModifiedByUser_SideEffect(Question question, User user, QuestionViewModel viewModel)
        {
            if (question == null) throw new NullException(() => question);
            if (user == null) throw new NullException(() => user);
            if (viewModel == null) throw new NullException(() => viewModel);

            _question = question;
            _user = user;
            _viewModel = viewModel;
        }

        public void Execute()
        {
            if (MustSetLastModifiedByUser(_viewModel))
            {
                _question.LastModifiedByUser = _user;
            }
        }

        private bool MustSetLastModifiedByUser(QuestionViewModel viewModel)
        {
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
                   viewModel.Links.Any(x => x.IsNew) ||
                   viewModel.Flags.IsDirty ||
                   viewModel.Flags.Any(x => x.IsDirty) ||
                   viewModel.Flags.Any(x => x.IsNew);
        }
    }
}
