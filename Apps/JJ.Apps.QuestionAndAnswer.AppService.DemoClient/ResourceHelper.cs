using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Apps.QuestionAndAnswer.AppService.DemoClient.RandomQuestionService;
using JJ.Apps.QuestionAndAnswer.AppService.DemoClient.ResourceService;

namespace JJ.Apps.QuestionAndAnswer.AppService.DemoClient
{
    public static class ResourceHelper
    {
        static ResourceHelper()
        {
            LoadResources();
        }

        public static Labels Labels { get; private set; }
        public static Titles Titles { get; private set; }
        public static Messages Messages { get; private set; }

        private static void LoadResources()
        {
            string cultureName = GetCultureName();

            using (var service = new ResourceServiceClient())
            {
                Labels = service.GetLabels(cultureName);
                Titles = service.GetTitles(cultureName);
                Messages = service.GetMessages(cultureName);
            }
        }

        private static string GetCultureName()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}
