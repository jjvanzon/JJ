﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class QuestionFlagViewModel
    {
        public LoginViewModel Login { get; set; }
        public QuestionViewModel Question { get; set; }
        public bool IsFlagged { get; set; }
        public string Comment { get; set; }
    }
}
