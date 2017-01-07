using ActionDispatcher = JJ.Framework.Presentation.Mvc.ActionDispatcher;
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