using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class QuestionViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }

        public List<LinkViewModel> Links { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public QuestionFlagViewModel Flag { get; set; }
    }
}
