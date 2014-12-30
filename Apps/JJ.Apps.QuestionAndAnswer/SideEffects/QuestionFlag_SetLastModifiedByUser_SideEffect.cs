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
    internal class QuestionFlag_SetLastModifiedByUser_SideEffect : ISideEffect
    {
        private QuestionFlag _questionFlag;
        private User _user;
        private QuestionFlagViewModel _viewModel;

        public QuestionFlag_SetLastModifiedByUser_SideEffect(QuestionFlag questionFlag, User user, QuestionFlagViewModel viewModel)
        {
            if (questionFlag == null) throw new NullException(() => questionFlag);
            if (user == null) throw new NullException(() => user);
            if (viewModel == null) throw new NullException(() => viewModel);

            _questionFlag = questionFlag;
            _user = user;
            _viewModel = viewModel;
        }

        public void Execute()
        {
            if (MustSetLastModifiedByUser(_viewModel))
            {
                _questionFlag.LastModifiedByUser = _user;
            }
        }

        private bool MustSetLastModifiedByUser(QuestionFlagViewModel viewModel)
        {
            return viewModel.IsDirty ||
                   viewModel.IsNew;
        }
    }
}
