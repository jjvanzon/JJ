using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Entities
{
    public sealed class QuestionFlagViewModel
    {
        public int ID { get; set; }
        public string Comment { get; set; }
        public DateTime DateAndTime { get; set; }
        public string FlaggedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public FlagStatusViewModel Status { get; set; }
    }
}

