using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class LinkViewModel
    {
        // For serialization.
        public LinkViewModel()
        { }

        public LinkViewModel(string description, string url)
        {
            Description = description;
            Url = url;
        }

        public string Description { get; set; }
        public string Url { get; set; }
    }
}
