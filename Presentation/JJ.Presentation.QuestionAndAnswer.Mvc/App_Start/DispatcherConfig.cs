using ActionDispatcher = JJ.Framework.Presentation.Mvc.ActionDispatcher;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Mvc.Extensions;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JJ.Framework.Presentation;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.App_Start
{
    internal class DispatcherConfig
    {
        public static void AddMappings()
        {
            ActionDispatcher.Map<LoginViewModel>(ControllerNames.Login, ActionNames.Index, ViewNames.Index);
            ActionDispatcher.Map<NotAuthorizedViewModel>(null, null, ViewNames.NotAuthorized);
            ActionDispatcher.Map<CategorySelectorViewModel>(ControllerNames.CategorySelector, ActionNames.Index, ViewNames.Index);
            ActionDispatcher.Map<RandomQuestionViewModel>(ControllerNames.Questions, ActionNames.Random, ViewNames.Random);
            ActionDispatcher.Map<QuestionListViewModel>(ControllerNames.Questions, ActionNames.Index, ViewNames.Index, x => new { page = x.Pager.PageNumber });
            ActionDispatcher.Map<QuestionDetailsViewModel>(ControllerNames.Questions, ActionNames.Details, ViewNames.Details, x => new { id = x.Question.ID });
            ActionDispatcher.Map<QuestionConfirmDeleteViewModel>(ControllerNames.Questions, ActionNames.Delete, ViewNames.Delete, x => new { id = x.ID });
            ActionDispatcher.Map<QuestionDeleteConfirmedViewModel>(ControllerNames.Questions, null, ViewNames.Deleted);
            ActionDispatcher.Map<QuestionNotFoundViewModel>(ControllerNames.Questions, null, ViewNames.NotFound);

            ActionDispatcher.Map<QuestionEditViewModel>(
                x => !x.IsNew,
                ControllerNames.Questions, ActionNames.Edit, ViewNames.Edit,
                x => new { id = x.Question.ID },
                x => x.ValidationMessages.ToKeyValuePairs());

            ActionDispatcher.Map<QuestionEditViewModel>(
                x => x.IsNew,
                ControllerNames.Questions, ActionNames.Create, ViewNames.Edit,
                x => new { id = x.Question.ID },
                x => x.ValidationMessages.ToKeyValuePairs());
        }
    }
}