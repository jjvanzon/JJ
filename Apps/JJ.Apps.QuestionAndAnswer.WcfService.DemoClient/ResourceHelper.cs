using JJ.Apps.QuestionAndAnswer.WcfService.DemoClient.QuestionService;
using JJ.Apps.QuestionAndAnswer.WcfService.DemoClient.ResourceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.WcfService.DemoClient
{
    public static class ResourceHelper
    {
        private static ResourceServiceClient _service = new ResourceServiceClient();

        static ResourceHelper()
        {
            InitializeResources();
        }

        public static Labels Labels { get; private set; }
        public static Titles Titles { get; private set; }
        public static Messages Messages { get; private set; }

        private static void InitializeResources()
        {
            string cultureName = GetCultureName();

            Labels = _service.GetLabels(cultureName);
            Titles = _service.GetTitles(cultureName);
            Messages = _service.GetMessages(cultureName);
        }

        private static string GetCultureName()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}
