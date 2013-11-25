using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Models
{
    public class ImportModel
    {
        public string Pattern { get; set; }
        public string Meaning { get; set; }
        public string DescribedInSection { get; set; }
        public string FirstDefinedInLevel { get; set; }
        public LinkModel DescribedInSectionLink { get; set; }
    }
}
