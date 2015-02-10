using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Helpers
{
    public static class ActionDispatcher
    {
        private const int DEFAULT_PAGE_SIZE = 20;
        private const int DEFAULT_MAX_VISIBLE_PAGE_NUMBERS = 7;

        /// <summary>
        /// Gets a view model dynamically from the described presenter and action.
        /// Overloaded action methods are not supported.
        /// Also: only simple parameter types are supported.
        /// </summary>
        /// <param name="authenticatedUserName">nullable</param>
        public static object GetViewModel(ActionDescriptor actionDescriptor, Repositories repositories, string authenticatedUserName)
        {
            if (repositories == null) throw new NullException(() => repositories);
            if (actionDescriptor == null) throw new NullException(() => actionDescriptor);

            // Get presenter and action method.
            object presenter = GetPresenter(actionDescriptor.PresenterName, repositories, authenticatedUserName);
            Type type = presenter.GetType();
            MethodInfo method = type.GetMethod(actionDescriptor.ActionName);
            if (method == null)
            {
                throw new Exception(String.Format("Method '{0}' of type '{1}' not found.", actionDescriptor.ActionName, type.Name));
            }

            // Convert parameter values.
            IList<ParameterInfo> parameterInfos = method.GetParameters();
            actionDescriptor.Parameters = actionDescriptor.Parameters ?? new ParameterDescriptor[0];
            if (parameterInfos.Count != actionDescriptor.Parameters.Count)
            {
                throw new Exception("MethodInfo and ActionDescriptor must have the same amount of parameters.");
            }
            object[] parameterValues = new object[parameterInfos.Count];
            for (int i = 0; i < parameterInfos.Count; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                ParameterDescriptor parameterDescriptor = actionDescriptor.Parameters[i];
                parameterDescriptor = parameterDescriptor ?? new ParameterDescriptor();
                object parameterValue = ConvertValue(parameterDescriptor.Value, parameterInfo.ParameterType);
                parameterValues[i] = parameterValue;
            }

            // Call action.
            object viewModel = method.Invoke(presenter, parameterValues);
            return viewModel;
        }

        private static object GetPresenter(string presenterName, Repositories repositories, string authenticatedUserName)
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
            else if (presenterType == typeof(QuestionListPresenter))
            {
                // TODO: I do not know what the paging parameters must be here, 
                // because I cannot make the business dependent on config.
                // So now I revert to defaults.
                presenter = new QuestionListPresenter(
                    repositories, 
                    authenticatedUserName, 
                    DEFAULT_PAGE_SIZE, 
                    DEFAULT_MAX_VISIBLE_PAGE_NUMBERS);
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

        private static object ConvertValue(string value, Type type)
        {
            return Convert.ChangeType(value, type);
        }
    }
}
