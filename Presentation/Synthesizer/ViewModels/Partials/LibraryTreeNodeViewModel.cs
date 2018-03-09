﻿using System.Collections.Generic;

namespace JJ.Presentation.Synthesizer.ViewModels.Partials
{
	public sealed class LibraryTreeNodeViewModel
	{
		public int LowerDocumentReferenceID { get; set; }
		public string Caption { get; set; }

		public IList<PatchGroupTreeNodeViewModel> PatchGroupNodes { get; set; }

		/// <summary> Contains patches without a group. </summary>
		public IList<PatchTreeNodeViewModel> PatchNodes { get; set; }

		public SimpleTreeNodeViewModel MidiNode { get; set; }
		public SimpleTreeNodeViewModel ScalesNode { get; set; }
	}
}