﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Collections;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using JJ.Presentation.Synthesizer.ViewModels.Partials;

namespace JJ.Presentation.Synthesizer.Presenters
{
	public partial class MainPresenter
	{
		private readonly Dictionary<Type, Action<ScreenViewModelBase>> _dispatchDelegateDictionary;

		private Dictionary<Type, Action<ScreenViewModelBase>> CreateDispatchDelegateDictionary()
		{
			var dictionary = new Dictionary<Type, Action<ScreenViewModelBase>>
			{
				{ typeof(AudioFileOutputGridViewModel), DispatchAudioFileOutputGridViewModel },
				{ typeof(AudioFileOutputPropertiesViewModel), DispatchAudioFileOutputPropertiesViewModel },
				{ typeof(AudioOutputPropertiesViewModel), DispatchAudioOutputPropertiesViewModel },
				{ typeof(AutoPatchPopupViewModel), DispatchAutoPatchViewModel },
				{ typeof(CurrentInstrumentBarViewModel), DispatchCurrentInstrumentBarViewModel },
				{ typeof(CurveDetailsViewModel), DispatchCurveDetailsViewModel },
				{ typeof(DocumentCannotDeleteViewModel), DispatchDocumentCannotDeleteViewModel },
				{ typeof(DocumentDeletedViewModel), DispatchDocumentDeletedViewModel },
				{ typeof(DocumentDeleteViewModel), DispatchDocumentDeleteViewModel },
				{ typeof(DocumentDetailsViewModel), DispatchDocumentDetailsViewModel },
				{ typeof(DocumentGridViewModel), DispatchDocumentGridViewModel },
				{ typeof(DocumentOrPatchNotFoundPopupViewModel), DispatchDocumentOrPatchNotFoundPopupViewModel },
				{ typeof(DocumentPropertiesViewModel), DispatchDocumentPropertiesViewModel },
				{ typeof(DocumentTreeViewModel), DispatchDocumentTreeViewModel },
				{ typeof(LibraryPropertiesViewModel), DispatchLibraryPropertiesViewModel },
				{ typeof(LibrarySelectionPopupViewModel), DispatchLibrarySelectionPopupViewModel },
				{ typeof(MenuViewModel), DispatchMenuViewModel },
				{ typeof(MidiMappingDetailsViewModel), DispatchMidiMappingDetailsViewModel },
				{ typeof(MidiMappingElementPropertiesViewModel), DispatchMidiMappingElementPropertiesViewModel },
				{ typeof(NodePropertiesViewModel), DispatchNodePropertiesViewModel },
				{ typeof(OperatorPropertiesViewModel), DispatchOperatorPropertiesViewModel },
				{ typeof(OperatorPropertiesViewModel_ForCache), DispatchOperatorPropertiesViewModel_ForCache },
				{ typeof(OperatorPropertiesViewModel_ForCurve), DispatchOperatorPropertiesViewModel_ForCurve },
				{ typeof(OperatorPropertiesViewModel_ForInletsToDimension), DispatchOperatorPropertiesViewModel_ForInletsToDimension },
				{ typeof(OperatorPropertiesViewModel_ForNumber), DispatchOperatorPropertiesViewModel_ForNumber },
				{ typeof(OperatorPropertiesViewModel_ForPatchInlet), DispatchOperatorPropertiesViewModel_ForPatchInlet },
				{ typeof(OperatorPropertiesViewModel_ForPatchOutlet), DispatchOperatorPropertiesViewModel_ForPatchOutlet },
				{ typeof(OperatorPropertiesViewModel_ForSample), DispatchOperatorPropertiesViewModel_ForSample },
				{ typeof(OperatorPropertiesViewModel_WithInterpolation), DispatchOperatorPropertiesViewModel_WithInterpolation },
				{ typeof(OperatorPropertiesViewModel_WithCollectionRecalculation), DispatchOperatorPropertiesViewModel_WithCollectionRecalculation },
				{ typeof(PatchDetailsViewModel), DispatchPatchDetailsViewModel },
				{ typeof(PatchPropertiesViewModel), DispatchPatchPropertiesViewModel },
				{ typeof(SampleFileBrowserViewModel), DispatchSampleFileBrowserViewModel },
				{ typeof(SaveChangesPopupViewModel), DispatchSaveChangesPopupViewModel },
				{ typeof(ScaleGridViewModel), DispatchScaleGridViewModel },
				{ typeof(ScalePropertiesViewModel), DispatchScalePropertiesViewModel },
				{ typeof(ToneGridEditViewModel), DispatchToneGridEditViewModel }
			};

			return dictionary;
		}

		/// <summary>
		/// Closes the deal with regards to the action method,
		/// combining a temporary partial view model with the MainViewModel.
		/// Applies it in the right way to the MainViewModel. 
		/// This means assigning a partial ViewModel to a property of the MainViewModel,
		/// but also for instance yielding over the validation message from a partial
		/// ViewModel to the MainViewModel, and showing and hiding views currently not on the foreground.
		/// </summary>
		private void DispatchViewModel(ScreenViewModelBase viewModel)
		{
			if (viewModel == null) throw new NullException(() => viewModel);

			Type viewModelType = viewModel.GetType();

			if (!_dispatchDelegateDictionary.TryGetValue(viewModelType, out Action<ScreenViewModelBase> dispatchDelegate))
			{
				throw new Exception($"{nameof(_dispatchDelegateDictionary)} does not contain {nameof(viewModelType)} '{viewModelType}'.");
			}

			dispatchDelegate(viewModel);
		}

		private void DispatchAudioFileOutputGridViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (AudioFileOutputGridViewModel)viewModel;

			MainViewModel.Document.AudioFileOutputGrid = (AudioFileOutputGridViewModel)viewModel;

			if (castedViewModel.Visible)
			{
				HideAllGridAndDetailsViewModels();
				castedViewModel.Visible = true;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchAudioFileOutputPropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (AudioFileOutputPropertiesViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.AudioFileOutputPropertiesDictionary;
			int id = castedViewModel.Entity.ID;

			dictionary[id] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleAudioFileOutputProperties = castedViewModel;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchAudioOutputPropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (AudioOutputPropertiesViewModel)viewModel;

			MainViewModel.Document.AudioOutputProperties = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
			}
			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchAutoPatchViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (AutoPatchPopupViewModel)viewModel;

			MainViewModel.Document.AutoPatchPopup = castedViewModel;

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchCurrentInstrumentBarViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (CurrentInstrumentBarViewModel)viewModel;

			MainViewModel.Document.CurrentInstrument = castedViewModel;

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchCurveDetailsViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (CurveDetailsViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.CurveDetailsDictionary;
			dictionary[castedViewModel.Curve.ID] = castedViewModel;

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchDocumentCannotDeleteViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (DocumentCannotDeleteViewModel)viewModel;

			MainViewModel.DocumentCannotDelete = castedViewModel;

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchDocumentDeletedViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (DocumentDeletedViewModel)viewModel;

			MainViewModel.DocumentDeleted = castedViewModel;

			// TODO: This is quite an assumption.
			MainViewModel.DocumentDelete.Visible = false;
			MainViewModel.DocumentDetails.Visible = false;

			if (!castedViewModel.Visible)
			{
				// Also: this might better be done in the action method.
				DocumentGridRefresh();
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchDocumentDeleteViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (DocumentDeleteViewModel)viewModel;
			MainViewModel.DocumentDelete = castedViewModel;

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchDocumentDetailsViewModel(ScreenViewModelBase viewModel)
		{
			var documentDetailsViewModel = (DocumentDetailsViewModel)viewModel;

			MainViewModel.DocumentDetails = documentDetailsViewModel;

			if (documentDetailsViewModel.Visible)
			{
				HideAllGridAndDetailsViewModels();
				documentDetailsViewModel.Visible = true;
			}

			DispatchViewModelBase(documentDetailsViewModel);
		}

		private void DispatchDocumentGridViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (DocumentGridViewModel)viewModel;

			MainViewModel.DocumentGrid = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllGridAndDetailsViewModels();
				castedViewModel.Visible = true;
			}

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchDocumentOrPatchNotFoundPopupViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewmodel = (DocumentOrPatchNotFoundPopupViewModel)viewModel;

			MainViewModel.DocumentOrPatchNotFound = castedViewmodel;

			DispatchViewModelBase(castedViewmodel);
		}

		private void DispatchDocumentPropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (DocumentPropertiesViewModel)viewModel;

			MainViewModel.Document.DocumentProperties = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
			}

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchDocumentTreeViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (DocumentTreeViewModel)viewModel;
			MainViewModel.Document.DocumentTree = castedViewModel;

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			if (castedViewModel.DocumentToOpenExternally != null)
			{
				MainViewModel.Document.DocumentToOpenExternally = castedViewModel.DocumentToOpenExternally;
				castedViewModel.DocumentToOpenExternally = null;
			}

			if (castedViewModel.PatchToOpenExternally != null)
			{
				MainViewModel.Document.PatchToOpenExternally = castedViewModel.PatchToOpenExternally;
				castedViewModel.PatchToOpenExternally = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchLibraryPropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (LibraryPropertiesViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.LibraryPropertiesDictionary;
			dictionary[castedViewModel.DocumentReferenceID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleLibraryProperties = castedViewModel;
			}

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			if (castedViewModel.DocumentToOpenExternally != null)
			{
				MainViewModel.Document.DocumentToOpenExternally = castedViewModel.DocumentToOpenExternally;
				castedViewModel.DocumentToOpenExternally = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchLibrarySelectionPopupViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (LibrarySelectionPopupViewModel)viewModel;

			MainViewModel.Document.LibrarySelectionPopup = castedViewModel;

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			if (castedViewModel.DocumentToOpenExternally != null)
			{
				MainViewModel.Document.DocumentToOpenExternally = castedViewModel.DocumentToOpenExternally;
				castedViewModel.DocumentToOpenExternally = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchMenuViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (MenuViewModel)viewModel;
			MainViewModel.Menu = castedViewModel;

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchMidiMappingElementPropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (MidiMappingElementPropertiesViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.MidiMappingElementPropertiesDictionary;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleMidiMappingElementProperties = castedViewModel;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchMidiMappingDetailsViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (MidiMappingDetailsViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.MidiMappingDetailsDictionary;
			dictionary[castedViewModel.MidiMapping.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllGridAndDetailsViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleMidiMappingDetails = castedViewModel;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchNodePropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (NodePropertiesViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = ViewModelSelector.GetNodePropertiesViewModelDictionary_ByCurveID(MainViewModel.Document, castedViewModel.CurveID);
			dictionary[castedViewModel.Entity.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleNodeProperties = castedViewModel;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_ForCache(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_ForCache)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_ForCaches;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_ForCache = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_ForCurve(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_ForCurve)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_ForCurves;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_ForCurve = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_ForInletsToDimension(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_ForInletsToDimension)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_ForInletsToDimension;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_ForInletsToDimension = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_ForNumber(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_ForNumber)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_ForNumbers;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_ForNumber = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_ForPatchInlet(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_ForPatchInlet)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_ForPatchInlets;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_ForPatchInlet = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_ForPatchOutlet(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_ForPatchOutlet)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_ForPatchOutlets;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_ForPatchOutlet = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_ForSample(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_ForSample)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_ForSamples;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_ForSample = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_WithInterpolation(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_WithInterpolation)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_WithInterpolation;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_WithInterpolation = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModel_WithCollectionRecalculation(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (OperatorPropertiesViewModel_WithCollectionRecalculation)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.OperatorPropertiesDictionary_WithCollectionRecalculation;
			dictionary[castedViewModel.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleOperatorProperties_WithCollectionRecalculation = castedViewModel;
			}

			DispatchOperatorPropertiesViewModelBase(castedViewModel);
		}

		private void DispatchOperatorPropertiesViewModelBase(OperatorPropertiesViewModelBase viewModel)
		{
			if (viewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = viewModel.OutletIDToPlay;
				viewModel.OutletIDToPlay = null;
			}

			DispatchViewModelBase(viewModel);
		}

		private void DispatchPatchDetailsViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (PatchDetailsViewModel)viewModel;

			// Try Dispatch to AutoPatchPopupViewModel
			if (MainViewModel.Document.AutoPatchPopup?.PatchDetails.Entity.ID == castedViewModel.Entity.ID)
			{
				MainViewModel.Document.AutoPatchPopup.PatchDetails = castedViewModel;
			}
			else
			{
				// Otherwise dispatch to DocumentViewModel
				// ReSharper disable once SuggestVarOrType_Elsewhere
				var dictionary = MainViewModel.Document.PatchDetailsDictionary;
				dictionary[castedViewModel.Entity.ID] = castedViewModel;

				if (castedViewModel.Visible)
				{
					HideAllGridAndDetailsViewModels();
					castedViewModel.Visible = true;
					MainViewModel.Document.VisiblePatchDetails = castedViewModel;
				}
			}

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchPatchPropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (PatchPropertiesViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.PatchPropertiesDictionary;
			dictionary[castedViewModel.ID]= castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisiblePatchProperties = castedViewModel;
			}

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchSampleFileBrowserViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (SampleFileBrowserViewModel)viewModel;

			MainViewModel.Document.SampleFileBrowser = castedViewModel;

			DispatchViewModelBase(viewModel);
		}

		private void DispatchSaveChangesPopupViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (SaveChangesPopupViewModel)viewModel;

			MainViewModel.Document.SaveChangesPopup = castedViewModel;

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchScaleGridViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (ScaleGridViewModel)viewModel;

			MainViewModel.Document.ScaleGrid = (ScaleGridViewModel)viewModel;

			if (castedViewModel.Visible)
			{
				HideAllGridAndDetailsViewModels();
				castedViewModel.Visible = true;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchScalePropertiesViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (ScalePropertiesViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.ScalePropertiesDictionary;
			dictionary[castedViewModel.Entity.ID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllPropertiesViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleScaleProperties = castedViewModel;
			}

			DispatchViewModelBase(castedViewModel);
		}

		private void DispatchToneGridEditViewModel(ScreenViewModelBase viewModel)
		{
			var castedViewModel = (ToneGridEditViewModel)viewModel;

			// ReSharper disable once SuggestVarOrType_Elsewhere
			var dictionary = MainViewModel.Document.ToneGridEditDictionary;
			dictionary[castedViewModel.ScaleID] = castedViewModel;

			if (castedViewModel.Visible)
			{
				HideAllGridAndDetailsViewModels();
				castedViewModel.Visible = true;
				MainViewModel.Document.VisibleToneGridEdit = castedViewModel;
			}

			if (castedViewModel.OutletIDToPlay.HasValue)
			{
				MainViewModel.Document.OutletIDToPlay = castedViewModel.OutletIDToPlay;
				castedViewModel.OutletIDToPlay = null;
			}

			DispatchViewModelBase(castedViewModel);
		}

		/// <summary>
		/// Note that this does not assign the view model to the right MainViewModel property.
		/// You have to do that yourself.
		/// </summary>
		private void DispatchViewModelBase(ScreenViewModelBase castedViewModel)
		{
			MainViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
			castedViewModel.ValidationMessages.Clear();
			MainViewModel.Successful = castedViewModel.Successful;

			MainViewModel.PropertiesPanelVisible =
				MainViewModel.Document.VisibleAudioFileOutputProperties != null |
				MainViewModel.Document.AudioOutputProperties.Visible ||
				MainViewModel.Document.DocumentProperties.Visible ||
				MainViewModel.Document.VisibleLibraryProperties != null ||
				MainViewModel.Document.VisibleNodeProperties != null ||
				MainViewModel.Document.VisibleMidiMappingElementProperties != null ||
				MainViewModel.Document.VisibleOperatorProperties != null ||
				MainViewModel.Document.VisibleOperatorProperties_ForCache != null ||
				MainViewModel.Document.VisibleOperatorProperties_ForCurve != null ||
				MainViewModel.Document.VisibleOperatorProperties_ForInletsToDimension != null ||
				MainViewModel.Document.VisibleOperatorProperties_ForNumber != null ||
				MainViewModel.Document.VisibleOperatorProperties_ForPatchInlet != null ||
				MainViewModel.Document.VisibleOperatorProperties_ForPatchOutlet != null ||
				MainViewModel.Document.VisibleOperatorProperties_ForSample != null ||
				MainViewModel.Document.VisibleOperatorProperties_WithInterpolation != null ||
				MainViewModel.Document.VisibleOperatorProperties_WithCollectionRecalculation != null ||
				MainViewModel.Document.VisiblePatchProperties != null ||
				MainViewModel.Document.VisibleScaleProperties != null;

			MainViewModel.DetailsOrGridPanelVisible =
				MainViewModel.Document.VisibleMidiMappingDetails != null ||
				MainViewModel.Document.VisiblePatchDetails != null ||
				MainViewModel.Document.VisibleToneGridEdit != null ||
				MainViewModel.Document.AudioFileOutputGrid.Visible ||
				MainViewModel.Document.ScaleGrid.Visible;

			MainViewModel.CurveDetailsPanelVisible = MainViewModel.Document.CurveDetailsDictionary.Any(x => x.Value.Visible);
		}
	}
}
