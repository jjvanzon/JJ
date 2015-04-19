using ActionDispatcher = JJ.Framework.Presentation.Mvc.ActionDispatcher;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;
using JJ.Presentation.QuestionAndAnswer.Mvc.Extensions;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JJ.Framework.Presentation;
using JJ.Framework.Presentation.Mvc;
using System.Reflection;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.App_Start
{
    internal class DispatcherConfig
    {
        public static void AddMappings()
        {
            ActionDispatcher.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}