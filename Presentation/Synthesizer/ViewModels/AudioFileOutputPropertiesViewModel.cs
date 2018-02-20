﻿using JJ.Data.Canonical;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using System.Collections.Generic;

namespace JJ.Presentation.Synthesizer.ViewModels
{
	public sealed class AudioFileOutputPropertiesViewModel : ScreenViewModelBase
	{
		public AudioFileOutputViewModel Entity { get; set; }

		public IList<IDAndName> AudioFileFormatLookup { get; set; }
		public IList<IDAndName> SampleDataTypeLookup { get; set; }
		public IList<IDAndName> SpeakerSetupLookup { get; set; }

		public IList<IDAndName> OutletLookup { get; set; }
	}
}
