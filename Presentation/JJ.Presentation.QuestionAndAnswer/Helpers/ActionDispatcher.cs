using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.QuestionAndAnswer.Helpers
{
    internal static class ActionDispatcher
    {
        /// <summary>
        /// Gets a view model dynamically from the described presenter and action.
        /// Overloaded action methods are not supported.
        /// Also: only simple parameter types are supported.
        /// </summary>
        /// <param name="authenticatedUserName">nullable</param>
        public static object GetViewModel(ActionInfo actionInfo, Repositories repositories, string authenticatedUserName)
        {
            if (actionInfo == null) throw new NullException(() => actionInfo);
            if (repositories == null) throw new NullException(() => repositories);

            object presenter = CreatePresenter(actionInfo.PresenterName, repositories, authenticatedUserName);
            object viewModel = ActionHelper.DispatchAction(presenter, actionInfo);
            return viewModel;
        }

        private static object CreatePresenter(string presenterName, Repositories repositories, string authenticatedUserName)
        {
            Type presenterType = GetPresenterType(presenterName);
            
            object presenter;

            if (presenterType == typeof(CategorySelectorPresenter))
            {
                presenter = new CategorySelectorPresenter(
                    repositories.CategoryRepository,
                    repositories.QuestionRepository,
                    repositories.QuestionFlagRepository,
                    repositories.FlagStatusRepository,
                    repositories.UserRepository,
                    authenticatedUserName);
            }
            else if (presenterType == typeof(RandomQuestionPresenter))
            {
                presenter = new RandomQuestionPresenter(
                    repositories.QuestionRepository,
                    repositories.CategoryRepository,
                    repositories.QuestionFlagRepository,
                    repositories.FlagStatusRepository,
                    repositories.UserRepository,
                    authenticatedUserName);
            }
            else
            {
                presenter = Activator.CreateInstance(presenterType, repositories, authenticatedUserName);
            }

            return presenter;
        }

        private static Type GetPresenterType(string presenterName)
        {
            Type templateType = typeof(QuestionDetailsPresenter);
            string typeName = String.Format("{0}.{1}", templateType.Namespace, presenterName);
            Type type = templateType.Assembly.GetType(typeName);
            if (type == null)
            {
                throw new Exception(String.Format("Type '{0}' not found.", typeName));
            }
            return type;
        }
    }
}
