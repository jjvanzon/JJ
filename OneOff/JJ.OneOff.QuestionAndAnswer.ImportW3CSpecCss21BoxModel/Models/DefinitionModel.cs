using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Models
{
    public class DefinitionModel
    {
        public string Context { get; set; }
        public string Term { get; set; }
        public string Meaning { get; set; }
        public string HashTag { get; set; }
        public string HashTagLinkText { get; set; }

        public List<LinkModel> ContextLinks { get; set; }
        public List<LinkModel> TermLinks { get; set; }
        public List<LinkModel> MeaningLinks { get; set; }
    }
}
