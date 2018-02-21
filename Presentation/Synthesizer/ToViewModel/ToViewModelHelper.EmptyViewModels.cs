﻿using System.Collections.Generic;
using JJ.Data.Canonical;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using JJ.Presentation.Synthesizer.ViewModels.Partials;

namespace JJ.Presentation.Synthesizer.ToViewModel
{
	internal static partial class ToViewModelHelper
	{
		public static AudioFileOutputGridViewModel CreateEmptyAudioFileOutputGridViewModel()
		{
			var viewModel = new AudioFileOutputGridViewModel
			{
				List = new List<AudioFileOutputListItemViewModel>(),
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		public static AudioOutputPropertiesViewModel CreateEmptyAudioOutputPropertiesViewModel()
		{
			AudioOutputPropertiesViewModel viewModel = CreateEmptyAudioOutputPropertiesViewModel_WithoutOriginalState();
			viewModel.OriginalState = CreateEmptyAudioOutputPropertiesViewModel_WithoutOriginalState();
			return viewModel;
		}

		private static AudioOutputPropertiesViewModel CreateEmptyAudioOutputPropertiesViewModel_WithoutOriginalState()
		{
			return new AudioOutputPropertiesViewModel
			{
				Entity = CreateEmptyAudioOutputViewModel(),
				ValidationMessages = new List<string>(),
				SpeakerSetupLookup = GetSpeakerSetupLookupViewModel(),
				Successful = true
			};
		}

		public static AudioOutputViewModel CreateEmptyAudioOutputViewModel()
		{
			var viewModel = new AudioOutputViewModel
			{
				SpeakerSetup = CreateEmptyIDAndName(),
				MaxConcurrentNotes = 1
			};

			return viewModel;
		}

		public static AutoPatchPopupViewModel CreateEmptyAutoPatchViewModel()
		{
			var viewModel = new AutoPatchPopupViewModel
			{
				PatchDetails = CreateEmptyPatchDetailsViewModel(),
				PatchProperties = CreateEmptyPatchPropertiesViewModel(),
				OperatorPropertiesDictionary = new Dictionary<int, OperatorPropertiesViewModel>(),
				OperatorPropertiesDictionary_ForCaches = new Dictionary<int, OperatorPropertiesViewModel_ForCache>(),
				OperatorPropertiesDictionary_ForCurves = new Dictionary<int, OperatorPropertiesViewModel_ForCurve>(),
				OperatorPropertiesDictionary_ForInletsToDimension = new Dictionary<int, OperatorPropertiesViewModel_ForInletsToDimension>(),
				OperatorPropertiesDictionary_ForNumbers = new Dictionary<int, OperatorPropertiesViewModel_ForNumber>(),
				OperatorPropertiesDictionary_ForPatchInlets = new Dictionary<int, OperatorPropertiesViewModel_ForPatchInlet>(),
				OperatorPropertiesDictionary_ForPatchOutlets = new Dictionary<int, OperatorPropertiesViewModel_ForPatchOutlet>(),
				OperatorPropertiesDictionary_ForSamples = new Dictionary<int, OperatorPropertiesViewModel_ForSample>(),
				OperatorPropertiesDictionary_WithInterpolation = new Dictionary<int, OperatorPropertiesViewModel_WithInterpolation>(),
				OperatorPropertiesDictionary_WithCollectionRecalculation = new Dictionary<int, OperatorPropertiesViewModel_WithCollectionRecalculation>(),
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		public static CurrentInstrumentViewModel CreateEmptyCurrentInstrumentViewModel()
		{
			var viewModel = new CurrentInstrumentViewModel
			{
				Scale = CreateEmptyIDAndName(),
				Patches = new List<CurrentInstrumentItemViewModel>(),
				MidiMappings = new List<CurrentInstrumentItemViewModel>(),
				ValidationMessages = new List<string>(),
				Successful = true,
				Visible = true
			};

			return viewModel;
		}

		public static DocumentViewModel CreateEmptyDocumentViewModel()
		{
			var viewModel = new DocumentViewModel
			{
				AudioFileOutputGrid = CreateEmptyAudioFileOutputGridViewModel(),
				AudioFileOutputPropertiesDictionary = new Dictionary<int, AudioFileOutputPropertiesViewModel>(),
				AudioOutputProperties = CreateEmptyAudioOutputPropertiesViewModel(),
				AutoPatchPopup = CreateEmptyAutoPatchViewModel(),
				CurrentInstrument = CreateEmptyCurrentInstrumentViewModel(),
				CurveDetailsDictionary = new Dictionary<int, CurveDetailsViewModel>(),
				DocumentProperties = CreateEmptyDocumentPropertiesViewModel(),
				DocumentTree = new RecursiveDocumentTreeViewModelFactory().CreateEmptyDocumentTreeViewModel(),
				LibraryPropertiesDictionary = new Dictionary<int, LibraryPropertiesViewModel>(),
				LibrarySelectionPopup = CreateEmptyLibrarySelectionPopupViewModel(),
				MidiMappingDetailsDictionary = new Dictionary<int, MidiMappingDetailsViewModel>(),
				MidiMappingElementPropertiesDictionary = new Dictionary<int, MidiMappingElementPropertiesViewModel>(),
				NodePropertiesDictionary = new Dictionary<int, NodePropertiesViewModel>(),
				OperatorPropertiesDictionary = new Dictionary<int, OperatorPropertiesViewModel>(),
				OperatorPropertiesDictionary_ForCaches = new Dictionary<int, OperatorPropertiesViewModel_ForCache>(),
				OperatorPropertiesDictionary_ForCurves = new Dictionary<int, OperatorPropertiesViewModel_ForCurve>(),
				OperatorPropertiesDictionary_ForInletsToDimension = new Dictionary<int, OperatorPropertiesViewModel_ForInletsToDimension>(),
				OperatorPropertiesDictionary_ForNumbers = new Dictionary<int, OperatorPropertiesViewModel_ForNumber>(),
				OperatorPropertiesDictionary_ForPatchInlets = new Dictionary<int, OperatorPropertiesViewModel_ForPatchInlet>(),
				OperatorPropertiesDictionary_ForPatchOutlets = new Dictionary<int, OperatorPropertiesViewModel_ForPatchOutlet>(),
				OperatorPropertiesDictionary_ForSamples = new Dictionary<int, OperatorPropertiesViewModel_ForSample>(),
				OperatorPropertiesDictionary_WithInterpolation = new Dictionary<int, OperatorPropertiesViewModel_WithInterpolation>(),
				OperatorPropertiesDictionary_WithCollectionRecalculation = new Dictionary<int, OperatorPropertiesViewModel_WithCollectionRecalculation>(),
				PatchDetailsDictionary = new Dictionary<int, PatchDetailsViewModel>(),
				PatchPropertiesDictionary = new Dictionary<int, PatchPropertiesViewModel>(),
				SampleFileBrowser = CreateEmptySampleFileBrowserViewModel(),
				SaveChangesPopup = CreateEmptySaveChangesPopupViewModel(),
				ScaleGrid = CreateEmptyScaleGridViewModel(),
				ScaleLookup = new List<IDAndName>(),
				ScalePropertiesDictionary = new Dictionary<int, ScalePropertiesViewModel>(),
				ToneGridEditDictionary = new Dictionary<int, ToneGridEditViewModel>(),
				UnderlyingPatchLookup = new List<IDAndName>(),
				UndoHistory = new Stack<UndoItemViewModelBase>(),
				RedoFuture = new Stack<UndoItemViewModelBase>()
			};

			return viewModel;
		}

		public static DocumentCannotDeleteViewModel CreateEmptyDocumentCannotDeleteViewModel()
		{
			var viewModel = new DocumentCannotDeleteViewModel
			{
				Document = new IDAndName(),
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		public static DocumentDeleteViewModel CreateEmptyDocumentDeleteViewModel()
		{
			var viewModel = new DocumentDeleteViewModel
			{
				Document = new IDAndName(),
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		public static DocumentDeletedViewModel CreateEmptyDocumentDeletedViewModel()
		{
			var viewModel = new DocumentDeletedViewModel
			{
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		public static DocumentDetailsViewModel CreateEmptyDocumentDetailsViewModel()
		{
			var viewModel = new DocumentDetailsViewModel
			{
				Document = new IDAndName(),
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		public static DocumentGridViewModel CreateEmptyDocumentGridViewModel()
		{
			var viewModel = new DocumentGridViewModel
			{
				List = new List<IDAndName>(),
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		private static DocumentOrPatchNotFoundPopupViewModel CreateEmptyDocumentOrPatchNotFoundPopupViewModel()
		{
			var viewModel = new DocumentOrPatchNotFoundPopupViewModel
			{
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		public static DocumentPropertiesViewModel CreateEmptyDocumentPropertiesViewModel()
		{
			DocumentPropertiesViewModel viewModel = CreateEmptyDocumentPropertiesViewModel_WithoutOriginalState();
			viewModel.OriginalState = CreateEmptyDocumentPropertiesViewModel_WithoutOriginalState();
			return viewModel;
		}

		private static DocumentPropertiesViewModel CreateEmptyDocumentPropertiesViewModel_WithoutOriginalState()
		{
			return new DocumentPropertiesViewModel
			{
				Entity = new IDAndName(),
				ValidationMessages = new List<string>(),
				Successful = true
			};
		}

		public static IDAndName CreateEmptyIDAndName()
		{
			var idAndName = new IDAndName();
			return idAndName;
		}

		public static LibrarySelectionPopupViewModel CreateEmptyLibrarySelectionPopupViewModel()
		{
			var viewModel = new LibrarySelectionPopupViewModel
			{
				ValidationMessages = new List<string>(),
				List = new List<IDAndName>(),
				Successful = true
			};

			return viewModel;
		}

		public static MainViewModel CreateEmptyMainViewModel()
		{
			return new MainViewModel
			{
				Menu = CreateEmptyMenuViewModel(),
				ValidationMessages = new List<string>(),
				WarningMessages = new List<string>(),
				PopupMessages = new List<string>(),
				Document = CreateEmptyDocumentViewModel(),
				DocumentCannotDelete = CreateEmptyDocumentCannotDeleteViewModel(),
				DocumentDelete = CreateEmptyDocumentDeleteViewModel(),
				DocumentDeleted = CreateEmptyDocumentDeletedViewModel(),
				DocumentDetails = CreateEmptyDocumentDetailsViewModel(),
				DocumentGrid = CreateEmptyDocumentGridViewModel(),
				DocumentOrPatchNotFound = CreateEmptyDocumentOrPatchNotFoundPopupViewModel(),
				Successful = true
			};
		}

		public static MenuViewModel CreateEmptyMenuViewModel()
		{
			MenuViewModel viewModel = CreateMenuViewModel(documentIsOpen: false);
			return viewModel;
		}

		public static ScaleGridViewModel CreateEmptyScaleGridViewModel()
		{
			var viewModel = new ScaleGridViewModel
			{
				Dictionary = new Dictionary<int, IDAndName>(),
				ValidationMessages = new List<string>(),
				Successful = true
			};

			return viewModel;
		}

		public static PatchDetailsViewModel CreateEmptyPatchDetailsViewModel()
		{
			PatchDetailsViewModel viewModel = CreateEmptyPatchDetailsViewModel_WithoutOriginalState();
			viewModel.OriginalState = CreateEmptyPatchDetailsViewModel_WithoutOriginalState();
			return viewModel;
		}

		private static PatchDetailsViewModel CreateEmptyPatchDetailsViewModel_WithoutOriginalState()
		{
			return new PatchDetailsViewModel
			{
				Entity = CreateEmptyPatchViewModel(),
				ValidationMessages = new List<string>(),
				Successful = true
			};
		}

		public static PatchPropertiesViewModel CreateEmptyPatchPropertiesViewModel()
		{
			PatchPropertiesViewModel viewModel = CreateEmptyPatchPropertiesViewModel_WithoutOriginalState();
			viewModel.OriginalState = CreateEmptyPatchPropertiesViewModel_WithoutOriginalState();
			return viewModel;
		}

		private static PatchPropertiesViewModel CreateEmptyPatchPropertiesViewModel_WithoutOriginalState()
		{
			return new PatchPropertiesViewModel
			{
				ValidationMessages = new List<string>(),
				Successful = true
			};
		}

		public static PatchViewModel CreateEmptyPatchViewModel()
		{
			var viewModel = new PatchViewModel
			{
				OperatorDictionary = new Dictionary<int, OperatorViewModel>()
			};

			return viewModel;
		}

		public static SampleFileBrowserViewModel CreateEmptySampleFileBrowserViewModel()
		{
			var viewModel = new SampleFileBrowserViewModel
			{
				ValidationMessages = new List<string>(),
				AutoCreatedNumberOperatorIDs = new List<int>(),
				Successful = true
			};

			return viewModel;
		}

		public static SaveChangesPopupViewModel CreateEmptySaveChangesPopupViewModel()
		{
			return new SaveChangesPopupViewModel
			{
				ValidationMessages = new List<string>()
			};
		}
	}
}