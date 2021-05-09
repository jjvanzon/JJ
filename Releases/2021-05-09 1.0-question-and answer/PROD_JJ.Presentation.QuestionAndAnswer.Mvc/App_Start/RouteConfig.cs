using System.Web.Mvc;
using System.Web.Routing;
using JJ.Presentation.QuestionAndAnswer.Mvc.Names;

namespace JJ.Presentation.QuestionAndAnswer.Mvc
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Index",
				url: "{controller}/" + nameof(ActionNames.Index) + "/{page}",
				defaults: new
				{
					controller = nameof(ControllerNames.Questions),
					action = nameof(ActionNames.Index),
					page = 1
				});

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new
				{
					controller = nameof(ControllerNames.Questions),
					action = nameof(ActionNames.Random),
					id = UrlParameter.Optional
				});
		}
	}
}