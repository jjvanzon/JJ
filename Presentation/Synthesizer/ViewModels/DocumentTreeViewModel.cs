﻿using JJ.Data.Canonical;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using JJ.Presentation.Synthesizer.ViewModels.Items;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class DocumentTreeViewModel : ViewModelBase
    {
        public int ID { get; set; }
        public PatchesTreeNodeViewModel PatchesNode { get; set; }
        public TreeLeafViewModel CurvesNode { get; set; }
        public TreeLeafViewModel SamplesNode { get; set; }
        public TreeLeafViewModel ScalesNode { get; set; }
        public TreeLeafViewModel AudioOutputNode { get; set; }
        public TreeLeafViewModel AudioFileOutputListNode { get; set; }
        public LibrariesTreeNodeViewModel LibrariesNode { get; set; }

        public DocumentTreeNodeTypeEnum SelectedNodeType { get; set; }
        public int? SelectedItemID { get; set; }
        public int? SelectedPatchGroupLowerDocumentReferenceID { get; set; }
        public string SelectedPatchGroup { get; set; }
        public bool CanPlay { get; set; }
        public bool CanOpen { get; set; }
        internal int? OutletIDToPlay { get; set; }
        internal IDAndName DocumentToOpen { get; set; }
        internal IDAndName PatchToOpen { get; set; }
    }
}
