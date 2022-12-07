//using System.Web.Mvc;
//using System.Web.Routing;
//using JJ.Presentation.QuestionAndAnswer.Mvc.Names;

//namespace JJ.Presentation.QuestionAndAnswer.Mvc
//{
//	public static class RouteConfig
//	{
//		public static void RegisterRoutes(RouteCollection routes)
//		{
//			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

//		    routes.MapRoute(
//		        name: "Home",
//		        url: "",
//		        defaults: new
//		        {
//		            controller = nameof(ControllerNames.Questions),
//		            action = nameof(ActionNames.Random)
//		        });

//		    routes.MapRoute(
//		        name: "NoID",
//		        url: "{controller}/{action}",
//		        defaults: new
//		        {
//		            controller = nameof(ControllerNames.Questions),
//		            action = nameof(ActionNames.Index)
//		        });

//            routes.MapRoute(
//		        name: "Default",
//		        url: "{controller}/{action}/{id}",
//		        defaults: new
//		        {
//		            controller = nameof(ControllerNames.Questions),
//		            action = nameof(ActionNames.Index),
//		            id = ""
//		        });
//        }
//    }
//}