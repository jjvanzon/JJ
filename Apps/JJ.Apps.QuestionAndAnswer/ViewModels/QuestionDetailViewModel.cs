using JJ.Apps.QuestionAndAnswer.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class QuestionDetailViewModel
    {
        public int ID { get; set; }
        public string Question { get; set;}
        public string Answer { get; set; }
        public bool AnswerIsVisible { get; set; }
        public string UserAnswer { get; set; }
    }
}
