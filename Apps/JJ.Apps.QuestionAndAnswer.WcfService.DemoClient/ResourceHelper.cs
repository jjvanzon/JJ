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

        public static Dictionary<string, string> Labels { get; private set; }
        public static Dictionary<string, string> Titles { get; private set; }
        public static Dictionary<string, string> Messages { get; private set; }

        private static void InitializeResources()
        {
            Labels = new Dictionary<string, string>();
            Titles = new Dictionary<string, string>();
            Messages = new Dictionary<string, string>();

            foreach (var x in _service.GetLabels(GetCultureName(), new string[] { "Answer" }))
            {
                Labels.Add(x.Name, x.Text);
            }

            foreach (var x in _service.GetTitles(GetCultureName(), new string[] { "Question", "NextQuestion", "ShowAnswer" }))
            {
                Titles.Add(x.Name, x.Text);
            }

            foreach (var x in _service.GetMessages(GetCultureName(), new string[] { "QuestionNotFound" }))
            {
                Messages.Add(x.Name, x.Text);
            }
        }

        private static string GetCultureName()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}
