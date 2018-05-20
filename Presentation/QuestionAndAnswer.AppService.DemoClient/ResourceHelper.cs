using System.Threading;
using JJ.Presentation.QuestionAndAnswer.AppService.DemoClient.ResourceService;

namespace JJ.Presentation.QuestionAndAnswer.AppService.DemoClient
{
	internal static class ResourceHelper
	{
		static ResourceHelper()
		{
			LoadResources();
		}

		public static Titles Titles { get; private set; }
		public static Messages Messages { get; private set; }
		public static PropertyDisplayNames PropertyDisplayNames { get; private set; }

		private static void LoadResources()
		{
			string cultureName = GetCultureName();

			using (var service = new ResourceServiceClient())
			{
				Titles = service.GetTitles(cultureName);
				Messages = service.GetMessages(cultureName);
				PropertyDisplayNames = service.GetPropertyDisplayNames(cultureName);
			}
		}

		private static string GetCultureName() => Thread.CurrentThread.CurrentUICulture.Name;
	}
}