﻿using System.Collections.Generic;
using JJ.Data.Canonical;
using JJ.Presentation.Synthesizer.ViewModels.Items;

namespace JJ.Presentation.Synthesizer.ViewModels
{
	public sealed class MidiMappingDetailsViewModel : ViewModelBase
	{
		public IDAndName MidiMapping { get; set; }
		public IList<MidiMappingElementItemViewModel> MidiMappingElements { get; set; }
	}
}
