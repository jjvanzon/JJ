using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.WcfService.DemoClient
{
    public static class Titles
    {
        public static string Question { get { return ResourceHelper.Titles["Question"]; } }

        public static string NextQuestion { get { return ResourceHelper.Titles["NextQuestion"]; } }

        public static string ShowAnswer { get { return ResourceHelper.Titles["ShowAnswer"]; } }
    }
}
