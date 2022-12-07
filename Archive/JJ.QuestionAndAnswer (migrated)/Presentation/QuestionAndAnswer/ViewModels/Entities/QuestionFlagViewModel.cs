using System;
using System.Collections.Generic;
using JJ.Data.Canonical;

namespace JJ.Presentation.QuestionAndAnswer.ViewModels.Entities
{
    public sealed class QuestionFlagViewModel
    {
        public int ID { get; set; }
        public string Comment { get; set; }
        public DateTime DateAndTime { get; set; }
        public string FlaggedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public IDAndName Status { get; set; }
        public IList<IDAndName> FlagStatusLookup { get; set; }
    }
}

