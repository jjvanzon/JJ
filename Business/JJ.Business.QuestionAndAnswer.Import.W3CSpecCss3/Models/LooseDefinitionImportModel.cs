using System;
using System.Collections.Generic;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models
{
    public class LooseDefinitionImportModel
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
