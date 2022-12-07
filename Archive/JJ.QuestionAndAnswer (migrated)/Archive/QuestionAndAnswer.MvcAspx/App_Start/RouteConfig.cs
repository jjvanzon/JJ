using JJ.Presentation.QuestionAndAnswer.MvcAspx.Names;
using System.Web.Mvc;
using System.Web.Routing;

namespace JJ.Presentation.QuestionAndAnswer.MvcAspx
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Index",
				url: "{controller}/" + ActionNames.Index + "/{page}",
				defaults: new
				{
					controller = ControllerNames.Questions,
					action = ActionNames.Index,
					page = 1
				});

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new 
				{ 
					controller =  ControllerNames.Questions, 
					action = ActionNames.Random, 
					id = UrlParameter.Optional
				});
		}
	}
}