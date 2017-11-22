using System.Web.Mvc;

namespace JJ.Presentation.QuestionAndAnswer.Mvc.App_Start
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}