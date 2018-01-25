using System.Reflection;
using ActionDispatcher = JJ.Framework.Mvc.ActionDispatcher;

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