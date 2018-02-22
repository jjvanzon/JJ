﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;
using JJ.Framework.Collections;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToEntity;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.Validators;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using JJ.Presentation.Synthesizer.ViewModels.Partials;

// ReSharper disable InvertIf
// ReSharper disable RedundantCaseLabel
// ReSharper disable CoVariantArrayConversion

namespace JJ.Presentation.Synthesizer.Presenters
{
	public partial class MainPresenter
	{
		// General Actions

		public void Close()
		{
			if (MainViewModel.Document.IsOpen)
			{
				DocumentClose();

				if (MainViewModel.Document.SaveChangesPopup.Visible)
				{
					return;
				}
			}

			MainViewModel.MustClose = true;
		}

		public void PopupMessagesOK()
		{
			MainViewModel.PopupMessages = new List<string>();
		}

		/// <param name="documentName">nullable</param>
		/// <param name="patchName">nullable</param>
		public void Show(string documentName, string patchName)
		{
			// Redirect
			if (string.IsNullOrEmpty(documentName) && string.IsNullOrEmpty(patchName))
			{
				ShowWithoutDocumentNameOrPatchName();
			}
			else if (!string.IsNullOrEmpty(documentName) && string.IsNullOrEmpty(patchName))
			{
				ShowWithDocumentName(documentName);
			}
			else if (string.IsNullOrEmpty(documentName) && !string.IsNullOrEmpty(patchName))
			{
				throw new Exception($"if {nameof(documentName)} is empty, {nameof(patchName)} cannot be filled in.");
			}
			else if (!string.IsNullOrEmpty(documentName) && !string.IsNullOrEmpty(patchName))
			{
				ShowWithDocumentNameAndPatchName(documentName, patchName);
			}
		}

		private void ShowWithoutDocumentNameOrPatchName()
		{
			// Create ViewModel
			MainViewModel = ToViewModelHelper.CreateEmptyMainViewModel();

			// Partial Actions
			MenuViewModel menuViewModel = _menuPresenter.Show(documentIsOpen: false);
			DocumentGridViewModel documentGridViewModel = MainViewModel.DocumentGrid;
			documentGridViewModel = _documentGridPresenter.Load(documentGridViewModel);
			string titleBar = _titleBarPresenter.Show();

			// DispatchViewModel
			MainViewModel.TitleBar = titleBar;
			DispatchViewModel(menuViewModel);
			DispatchViewModel(documentGridViewModel);
		}

		private void ShowWithDocumentName(string documentName)
		{
			// Create ViewModel
			MainViewModel = ToViewModelHelper.CreateEmptyMainViewModel();

			// Businesss
			Document document = _repositories.DocumentRepository.TryGetByName(documentName);
			if (document == null)
			{
				// GetUserInput
				DocumentOrPatchNotFoundPopupViewModel userInput = MainViewModel.DocumentOrPatchNotFound;

				// Template Method
				ExecuteReadAction(null, () => _documentOrPatchNotFoundPresenter.Show(userInput, documentName));
			}
			else
			{
				// Redirect
				DocumentOpen(document);
			}
		}

		private void ShowWithDocumentNameAndPatchName(string documentName, string patchName)
		{
			// Create ViewModel
			MainViewModel = ToViewModelHelper.CreateEmptyMainViewModel();

			// Businesss
			Document document = _repositories.DocumentRepository.TryGetByName(documentName);
			string canonicalPatchName = NameHelper.ToCanonical(patchName);
			Patch patch = document?.Patches
			                      .Where(x => string.Equals(NameHelper.ToCanonical(x.Name), canonicalPatchName))
			                      .SingleWithClearException(new { canonicalPatchName });

			if (document == null || patch == null)
			{
				// GetUserInput
				DocumentOrPatchNotFoundPopupViewModel userInput = MainViewModel.DocumentOrPatchNotFound;

				// Template Method
				ExecuteReadAction(userInput, () => _documentOrPatchNotFoundPresenter.Show(userInput, documentName, patchName));
			}
			else
			{
				// Redirect
				DocumentOpen(document);
				PatchDetailsShow(patch.ID);
			}
		}

		// AudioFileOutput

		public void AudioFileOutputGridClose()
		{
			// GetViewModel
			AudioFileOutputGridViewModel viewModel = MainViewModel.Document.AudioFileOutputGrid;

			// TemplateMethod
			ExecuteNonPersistedAction(viewModel, () => _audioFileOutputGridPresenter.Close(viewModel));
		}

		public void AudioFileOutputGridCreate()
		{
			// GetViewModel
			AudioFileOutputGridViewModel userInput = MainViewModel.Document.AudioFileOutputGrid;

			// Template Method
			AudioFileOutputGridViewModel gridViewModel = ExecuteCreateAction(userInput, () => _audioFileOutputGridPresenter.Create(userInput));

			if (gridViewModel.Successful)
			{
				// GetEntity
				AudioFileOutput audioFileOutput = _repositories.AudioFileOutputRepository.Get(gridViewModel.CreatedAudioFileOutputID);

				// ToViewModel
				AudioFileOutputPropertiesViewModel propertiesViewModel = audioFileOutput.ToPropertiesViewModel();

				// DispatchViewModel
				DispatchViewModel(propertiesViewModel);

				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.AudioFileOutput, audioFileOutput.ID).ToViewModel().AsArray(),
					States = GetAudioFileOutputStates(audioFileOutput.ID)
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);

				// Refresh
				DocumentTreeRefresh();
				AudioFileOutputGridRefresh();
			}
		}

		public void AudioFileOutputGridDelete(int id)
		{
			// GetViewModel
			AudioFileOutputGridViewModel userInput = MainViewModel.Document.AudioFileOutputGrid;

			// Undo History
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.AudioFileOutput, id).ToViewModel().AsArray(),
				States = GetAudioFileOutputStates(id)
			};

			// Template Method
			AudioFileOutputGridViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _audioFileOutputGridPresenter.Delete(userInput, id));

			if (viewModel.Successful)
			{
				// ToViewModel
				MainViewModel.Document.AudioFileOutputPropertiesDictionary.Remove(id);

				if (MainViewModel.Document.VisibleAudioFileOutputProperties?.Entity.ID == id)
				{
					MainViewModel.Document.VisibleAudioFileOutputProperties = null;
				}

				// Refresh
				DocumentTreeRefresh();
				AudioFileOutputGridRefresh();
			}
		}

		public void AudioFileOutputGridShow()
		{
			// GetViewModel
			AudioFileOutputGridViewModel viewModel = MainViewModel.Document.AudioFileOutputGrid;

			// TemplateMethod
			ExecuteNonPersistedAction(viewModel, () => _audioFileOutputGridPresenter.Show(viewModel));
		}

		public void AudioFileOutputPropertiesShow(int id)
		{
			AudioFileOutputPropertiesViewModel viewModel = ViewModelSelector.GetAudioFileOutputPropertiesViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(viewModel, () => _audioFileOutputPropertiesPresenter.Show(viewModel));
		}

		public void AudioFileOutputPropertiesClose(int id)
		{
			// GetViewModel
			AudioFileOutputPropertiesViewModel userInput = ViewModelSelector.GetAudioFileOutputPropertiesViewModel(MainViewModel.Document, id);

			// TemplateMethod
			AudioFileOutputPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _audioFileOutputPropertiesPresenter.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleAudioFileOutputProperties = null;

				// Refresh
				AudioFileOutputGridRefresh();
			}
		}

		public void AudioFileOutputPropertiesDelete(int id)
		{
			// GetViewModel
			AudioFileOutputPropertiesViewModel userInput = ViewModelSelector.GetAudioFileOutputPropertiesViewModel(MainViewModel.Document, id);

			// Undo History
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.AudioFileOutput, id).ToViewModel().AsArray(),
				States = GetAudioFileOutputStates(id)
			};

			// Template Method
			AudioFileOutputPropertiesViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _audioFileOutputPropertiesPresenter.Delete(userInput));

			if (viewModel.Successful)
			{
				// ToViewModel
				MainViewModel.Document.AudioFileOutputPropertiesDictionary.Remove(id);

				if (MainViewModel.Document.VisibleAudioFileOutputProperties?.Entity.ID == id)
				{
					MainViewModel.Document.VisibleAudioFileOutputProperties = null;
				}

				// Refresh
				DocumentTreeRefresh();
				AudioFileOutputGridRefresh();
			}
		}

		public void AudioFileOutputPropertiesLoseFocus(int id)
		{
			// GetViewModel
			AudioFileOutputPropertiesViewModel userInput = ViewModelSelector.GetAudioFileOutputPropertiesViewModel(MainViewModel.Document, id);

			// TemplateMethod
			AudioFileOutputPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _audioFileOutputPropertiesPresenter.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				AudioFileOutputGridRefresh();
			}
		}

		// AudioOutput

		private void AudioOutputPropertiesShow()
		{
			AudioOutputPropertiesViewModel viewModel = MainViewModel.Document.AudioOutputProperties;

			ExecuteNonPersistedAction(viewModel, () => _audioOutputPropertiesPresenter.Show(viewModel));
		}

		private void AudioOutputPropertiesSwitch()
		{
			if (MainViewModel.PropertiesPanelVisible)
			{
				AudioOutputPropertiesShow();
			}
		}

		public void AudioOutputPropertiesClose()
		{
			AudioOutputPropertiesViewModel userInput = MainViewModel.Document.AudioOutputProperties;

			ExecuteUpdateAction(userInput, () => _audioOutputPropertiesPresenter.Close(userInput));
		}

		public void AudioOutputPropertiesLoseFocus()
		{
			AudioOutputPropertiesViewModel userInput = MainViewModel.Document.AudioOutputProperties;

			ExecuteUpdateAction(userInput, () => _audioOutputPropertiesPresenter.LoseFocus(userInput));
		}

		public void AudioOutputPropertiesPlay()
		{
			// NOTE:
			// Cannot use partial presenter, because this action uses both
			// AudioOutputProperties and CurrentInstrument view model.

			// GetViewModel
			AudioOutputPropertiesViewModel userInput = MainViewModel.Document.AudioOutputProperties;

			// TemplateMethod
			ExecuteReadAction(
				userInput,
				() =>
				{
					// GetEntities
					AudioOutput audioOutput = _repositories.AudioOutputRepository.Get(userInput.Entity.ID);
					IList<Patch> entities = MainViewModel.Document.CurrentInstrument.Patches.Select(x => _repositories.PatchRepository.Get(x.EntityID)).ToArray();

					// Business
					Patch autoPatch = _autoPatcher.AutoPatch(entities);
					_autoPatcher.SubstituteSineForUnfilledInSoundPatchInlets(autoPatch);
					Result<Outlet> result = _autoPatcher.AutoPatch_TryCombineSounds(autoPatch);
					Outlet outlet = result.Data;

					// ToViewModel
					AudioOutputPropertiesViewModel viewModel = audioOutput.ToPropertiesViewModel();

					// Non-Persisted
					viewModel.Visible = userInput.Visible;
					viewModel.ValidationMessages = result.Messages;
					viewModel.Successful = result.Successful;
					viewModel.OutletIDToPlay = outlet?.ID;

					return viewModel;
				});
		}

		// AutoPatch

		public void AutoPatchPopupClose()
		{
			AutoPatchPopupViewModel userInput = MainViewModel.Document.AutoPatchPopup;

			ExecuteReadAction(
				userInput,
				() =>
				{
					// RefreshCounter
					userInput.RefreshID = RefreshIDProvider.GetRefreshID();
					userInput.PatchDetails.RefreshID = RefreshIDProvider.GetRefreshID();

					// Action
					AutoPatchPopupViewModel viewModel = ToViewModelHelper.CreateEmptyAutoPatchViewModel();

					// Non-Persisted
					viewModel.RefreshID = userInput.RefreshID;
					viewModel.PatchDetails.RefreshID = userInput.PatchDetails.RefreshID;

					return viewModel;
				});
		}

		public void AutoPatchPopupSave()
		{
			AutoPatchPopupViewModel userInput = MainViewModel.Document.AutoPatchPopup;

			AutoPatchPopupViewModel viewModel = ExecuteUpdateAction(
				userInput,
				() =>
				{
					// RefreshCounter
					userInput.RefreshID = RefreshIDProvider.GetRefreshID();
					userInput.PatchDetails.RefreshID = RefreshIDProvider.GetRefreshID();

					// Set !Successful
					userInput.Successful = false;

					// Get Entities
					Document document = _repositories.DocumentRepository.Get(MainViewModel.Document.ID);

					// ToEntity
					Patch patch = userInput.ToEntityWithRelatedEntities(_repositories);

					// Business
					patch.LinkTo(document);
					IResult result = _patchFacade.SavePatch(patch);

					AutoPatchPopupViewModel viewModel2;
					if (result.Successful)
					{
						// ToViewModel
						viewModel2 = ToViewModelHelper.CreateEmptyAutoPatchViewModel();
					}
					else
					{
						// ToViewModel
						viewModel2 = patch.ToAutoPatchViewModel(
							_repositories.SampleRepository,
							_repositories.CurveRepository,
							_repositories.InterpolationTypeRepository);

						viewModel2.Visible = userInput.Visible;
					}

					// Non-Persisted
					viewModel2.ValidationMessages.AddRange(result.Messages);
					viewModel2.RefreshID = userInput.RefreshID;
					viewModel2.PatchDetails.RefreshID = userInput.PatchDetails.RefreshID;

					// Successful?
					viewModel2.Successful = result.Successful;

					return viewModel2;
				});

			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
				PatchDetailsShow(userInput.PatchDetails.Entity.ID);
				PatchPropertiesShow(userInput.PatchDetails.Entity.ID);
			}
		}

		// CurrentInstrument

		private void AddPatchToCurrentInstrument(int patchID)
		{
			CurrentInstrumentBarViewModel userInput = MainViewModel.Document.CurrentInstrument;

			ExecuteReadAction(userInput, () => _currentInstrumentBarPresenter.AddPatch(userInput, patchID));
		}

		public void CurrentInstrumentBarExpand()
		{
			// GetViewModel
			CurrentInstrumentBarViewModel currentInstrumentUserInput = MainViewModel.Document.CurrentInstrument;
			AutoPatchPopupViewModel autoPatchPopupUserInput = MainViewModel.Document.AutoPatchPopup;

			// RefreshCounter
			currentInstrumentUserInput.RefreshID = RefreshIDProvider.GetRefreshID();
			autoPatchPopupUserInput.RefreshID = RefreshIDProvider.GetRefreshID();
			autoPatchPopupUserInput.PatchDetails.RefreshID = RefreshIDProvider.GetRefreshID();

			// Set !Successful
			currentInstrumentUserInput.Successful = false;

			// ToEntity
			Document document = MainViewModel.ToEntityWithRelatedEntities(_repositories);

			// Get Entities
			IList<Patch> underlyingPatches = currentInstrumentUserInput.Patches.Select(x => _repositories.PatchRepository.Get(x.EntityID)).ToArray();

			// Business
			Patch autoPatch = _autoPatcher.AutoPatch(underlyingPatches);

			// Business
			IResult validationResult = _documentFacade.Save(document);
			if (!validationResult.Successful)
			{
				// Non-Persisted
				currentInstrumentUserInput.ValidationMessages.AddRange(validationResult.Messages);

				// DispatchViewModel
				DispatchViewModel(currentInstrumentUserInput);

				return;
			}

			// ToViewModel
			AutoPatchPopupViewModel autoPatchPopupViewModel = autoPatch.ToAutoPatchViewModel(
				_repositories.SampleRepository,
				_repositories.CurveRepository,
				_repositories.InterpolationTypeRepository);

			// Non-Persisted
			autoPatchPopupViewModel.Visible = true;
			autoPatchPopupViewModel.RefreshID = autoPatchPopupUserInput.RefreshID;
			autoPatchPopupViewModel.PatchDetails.RefreshID = autoPatchPopupUserInput.PatchDetails.RefreshID;

			// Successful
			currentInstrumentUserInput.Successful = true;
			autoPatchPopupViewModel.Successful = true;

			// DispatchViewModel
			DispatchViewModel(autoPatchPopupViewModel);
		}

		public void CurrentInstrumentBarExpandPatch(int patchID)
		{
			// Redirect
			PatchExpand(patchID);
		}

		public void CurrentInstrumentBarMovePatch(int patchID, int newPosition)
		{
			CurrentInstrumentBarViewModel viewModel = MainViewModel.Document.CurrentInstrument;

			ExecuteReadAction(viewModel, () => _currentInstrumentBarPresenter.MovePatch(viewModel, patchID, newPosition));
		}

		public void CurrentInstrumentBarMovePatchBackward(int patchID)
		{
			CurrentInstrumentBarViewModel viewModel = MainViewModel.Document.CurrentInstrument;

			ExecuteReadAction(viewModel, () => _currentInstrumentBarPresenter.MovePatchBackward(viewModel, patchID));
		}

		public void CurrentInstrumentBarMovePatchForward(int patchID)
		{
			CurrentInstrumentBarViewModel viewModel = MainViewModel.Document.CurrentInstrument;

			ExecuteReadAction(viewModel, () => _currentInstrumentBarPresenter.MovePatchForward(viewModel, patchID));
		}

		public void CurrentInstrumentBarPlay()
		{
			CurrentInstrumentBarViewModel userInput = MainViewModel.Document.CurrentInstrument;

			ExecuteReadAction(userInput, () => _currentInstrumentBarPresenter.Play(userInput));
		}

		public void CurrentInstrumentBarPlayPatch(int patchID)
		{
			CurrentInstrumentBarViewModel userInput = MainViewModel.Document.CurrentInstrument;

			ExecuteReadAction(userInput, () => _currentInstrumentBarPresenter.PlayPatch(userInput, patchID));
		}

		public void CurrentInstrumentBarRemovePatch(int patchID)
		{
			CurrentInstrumentBarViewModel userInput = MainViewModel.Document.CurrentInstrument;

			ExecuteReadAction(userInput, () => _currentInstrumentBarPresenter.RemovePatch(userInput, patchID));
		}

		// Curve

		private void CurveExpand(int id)
		{
			ExecuteReadAction(
				null,
				() =>
				{
					// GetEntity
					int operatorID = GetOperatorIDByCurveID(id);
					Operator op = _repositories.OperatorRepository.Get(operatorID);
					int patchID = op.Patch.ID;

					// Redirect
					OperatorPropertiesShow(operatorID);
					CurveDetailsShow(id);
					PatchDetailsShow(patchID);
					OperatorSelect(patchID, operatorID);
				});
		}

		private void CurveDetailsShow(int id)
		{
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(userInput, () => _curveDetailsPresenter.Show(userInput));
		}

		public void CurveDetailsClose(int id)
		{
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, id);

			ExecuteUpdateAction(userInput, () => _curveDetailsPresenter.Close(userInput));
		}

		public void CurveDetailsExpand(int curveID)
		{
			// Redirect
			CurveExpand(curveID);
		}

		public void CurveDetailsExpandNode(int curveID, int nodeID)
		{
			// Redirect
			NodeExpand(curveID, nodeID);
		}

		public void CurveDetailsLoseFocus(int id)
		{
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, id);

			ExecuteUpdateAction(userInput, () => _curveDetailsPresenter.LoseFocus(userInput));
		}

		private void CurveDetailsSelectNode(int curveID, int nodeID)
		{
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

			ExecuteNonPersistedAction(userInput, () => _curveDetailsPresenter.SelectNode(userInput, nodeID));
		}

		public void CurveSelect(int id)
		{
			ExecuteReadAction(
				null,
				() =>
				{
					// GetEntity
					int operatorID = GetOperatorIDByCurveID(id);
					Operator op = _repositories.OperatorRepository.Get(operatorID);
					int patchID = op.Patch.ID;

					// Redirect
					OperatorPropertiesSwitch(operatorID);
					PatchDetailsSelectOperator(patchID, operatorID);
				});
		}

		// Document Grid

		public void DocumentCannotDeleteOK()
		{
			// GetViewModel
			DocumentCannotDeleteViewModel userInput = MainViewModel.DocumentCannotDelete;

			// Partial Action
			ExecuteNonPersistedAction(userInput, () => _documentCannotDeletePresenter.OK(userInput));
		}

		public void DocumentDeleteCancel()
		{
			// GetViewModel
			DocumentDeleteViewModel viewModel = MainViewModel.DocumentDelete;

			// Partial Action
			ExecuteNonPersistedAction(viewModel, () => _documentDeletePresenter.Cancel(viewModel));
		}

		public void DocumentDeleteConfirm()
		{
			// GetViewModel
			DocumentDeleteViewModel viewModel = MainViewModel.DocumentDelete;

			// Partial Action
			ScreenViewModelBase viewModel2 = _documentDeletePresenter.Confirm(viewModel);

			// RefreshCounter
			viewModel.RefreshID = RefreshIDProvider.GetRefreshID();

			// Set !Successful
			viewModel.Successful = false;

			if (viewModel2 is DocumentDeletedViewModel)
			{
				_repositories.Commit();
			}

			// Successful
			viewModel.Successful = true;

			// DispatchViewModel
			DispatchViewModel(viewModel2);
		}

		public void DocumentDeletedOK()
		{
			// GetViewModel
			DocumentDeletedViewModel viewModel = MainViewModel.DocumentDeleted;

			// Partial Action
			ExecuteNonPersistedAction(viewModel, () => _documentDeletedPresenter.OK(viewModel));
		}

		public void DocumentDeleteShow(int id)
		{
			// GetViewModel
			DocumentGridViewModel gridViewModel = MainViewModel.DocumentGrid;

			// RefreshCounter
			gridViewModel.RefreshID = RefreshIDProvider.GetRefreshID();

			// Set !Successful
			gridViewModel.Successful = false;

			// Partial Action
			ScreenViewModelBase viewModel2 = _documentDeletePresenter.Show(id);

			// Successful
			gridViewModel.Successful = true;

			// DispatchViewModel
			DispatchViewModel(viewModel2);
		}

		public void DocumentDetailsClose()
		{
			// GetViewModel
			DocumentDetailsViewModel viewModel = MainViewModel.DocumentDetails;

			// Partial Action
			_documentDetailsPresenter.Close(viewModel);

			// DispatchViewModel
			DispatchViewModel(viewModel);
		}

		public void DocumentCreate()
		{
			// Dirty Check
			if (MainViewModel.Document.IsDirty)
			{
				SaveChangesPopupShow(mustGoToDocumentCreateAfterConfirmation: true);
				return;
			}

			// GetViewModel
			DocumentGridViewModel gridViewModel = MainViewModel.DocumentGrid;

			// RefreshCounter
			gridViewModel.RefreshID = RefreshIDProvider.GetRefreshID();

			// Set !Successful
			gridViewModel.Successful = false;

			// Partial Action
			DocumentDetailsViewModel viewModel = _documentDetailsPresenter.Create();

			// Successful
			viewModel.Successful = true;

			// DispatchViewModel
			DispatchViewModel(viewModel);
		}

		public void DocumentDetailsSave()
		{
			// GetViewModel
			DocumentDetailsViewModel userInput = MainViewModel.DocumentDetails;

			// Partial Action
			DocumentDetailsViewModel viewModel = _documentDetailsPresenter.Save(userInput);

			// Commit
			// (do it before opening the document, which does a big query, which requires at least a flush.)
			if (viewModel.Successful)
			{
				_repositories.DocumentRepository.Commit();
			}

			// DispatchViewModel
			DispatchViewModel(viewModel);

			if (viewModel.Successful)
			{
				// Refresh
				DocumentGridRefresh();

				// Redirect
				DocumentOpen(viewModel.Document.ID);
			}
		}

		public void DocumentGridClose()
		{
			DocumentGridViewModel viewModel = MainViewModel.DocumentGrid;

			ExecuteNonPersistedAction(viewModel, () => _documentGridPresenter.Close(viewModel));
		}

		public void DocumentGridPlay(int id)
		{
			DocumentGridViewModel userInput = MainViewModel.DocumentGrid;

			ExecuteReadAction(userInput, () => _documentGridPresenter.Play(userInput, id));
		}

		public void DocumentGridShow()
		{
			DocumentGridViewModel viewModel = MainViewModel.DocumentGrid;

			ExecuteReadAction(viewModel, () => _documentGridPresenter.Load(viewModel));
		}

		// Document

		public void DocumentActivate()
		{
			DocumentRefresh();
		}

		public void DocumentClose()
		{
			// Dirty Check
			if (MainViewModel.Document.IsDirty)
			{
				SaveChangesPopupShow();
				return;
			}

			// Redirect
			DocumentCloseConfirmed();
		}

		private void DocumentCloseConfirmed()
		{
			// Partial Actions
			string titleBar = _titleBarPresenter.Show();
			MenuViewModel menuViewModel = _menuPresenter.Show(documentIsOpen: false);
			DocumentViewModel documentViewModel = ToViewModelHelper.CreateEmptyDocumentViewModel();

			// DispatchViewModel
			MainViewModel.TitleBar = titleBar;
			MainViewModel.Menu = menuViewModel;
			MainViewModel.Document = documentViewModel;

			// Redirect
			DocumentGridShow();
		}

		public void DocumentOpen(string name)
		{
			Document document = _documentFacade.Get(name);

			DocumentOpen(document);
		}

		public void DocumentOpen(int id)
		{
			Document document = _documentFacade.Get(id);

			DocumentOpen(document);
		}

		private void DocumentOpen(Document document)
		{
			// Dirty Check
			if (MainViewModel.Document.IsDirty)
			{
				SaveChangesPopupShow(document.ID);
				return;
			}

			// ToViewModel
			DocumentViewModel viewModel = document.ToViewModel(_repositories);

			// Non-Persisted
			viewModel.DocumentTree.Visible = true;
			viewModel.IsOpen = true;

			// Set everything to successful.
			viewModel.AudioFileOutputGrid.Successful = true;
			viewModel.AudioFileOutputPropertiesDictionary.Values.ForEach(x => x.Successful = true);
			viewModel.AudioOutputProperties.Successful = true;
			viewModel.AutoPatchPopup.PatchDetails.Successful = true;
			viewModel.AutoPatchPopup.PatchProperties.Successful = true;
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_ForCaches.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_ForCurves.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_ForInletsToDimension.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_ForNumbers.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_ForPatchInlets.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_ForPatchOutlets.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_ForSamples.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_WithCollectionRecalculation.Values.ForEach(x => x.Successful = true);
			viewModel.AutoPatchPopup.OperatorPropertiesDictionary_WithInterpolation.Values.ForEach(x => x.Successful = true);
			viewModel.CurrentInstrument.Successful = true;
			viewModel.CurveDetailsDictionary.Values.ForEach(x => x.Successful = true);
			viewModel.DocumentProperties.Successful = true;
			viewModel.DocumentTree.Successful = true;
			viewModel.NodePropertiesDictionary.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_ForCaches.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_ForCurves.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_ForInletsToDimension.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_ForNumbers.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_ForPatchInlets.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_ForPatchOutlets.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_ForSamples.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_WithCollectionRecalculation.Values.ForEach(x => x.Successful = true);
			viewModel.OperatorPropertiesDictionary_WithInterpolation.Values.ForEach(x => x.Successful = true);
			viewModel.PatchDetailsDictionary.Values.ForEach(x => x.Successful = true);
			viewModel.PatchPropertiesDictionary.Values.ForEach(x => x.Successful = true);
			viewModel.ScaleGrid.Successful = true;
			viewModel.ScalePropertiesDictionary.Values.ForEach(x => x.Successful = true);
			viewModel.ToneGridEditDictionary.Values.ForEach(x => x.Successful = true);

			// Partials
			string titleBar = _titleBarPresenter.Show(document);
			MenuViewModel menuViewModel = _menuPresenter.Show(documentIsOpen: true);
			viewModel.CurrentInstrument = _currentInstrumentBarPresenter.Load(viewModel.CurrentInstrument);

			// DispatchViewModel
			MainViewModel.Document = viewModel;
			MainViewModel.TitleBar = titleBar;
			MainViewModel.Menu = menuViewModel;

			// Redirect
			MainViewModel.DocumentGrid.Visible = false;
			if (document.Patches.Count == 1)
			{
				PatchDetailsShow(document.Patches[0].ID);
			}
		}

		public void DocumentOrPatchNotFoundOK()
		{
			DocumentOrPatchNotFoundPopupViewModel userInput = MainViewModel.DocumentOrPatchNotFound;

			ExecuteNonPersistedAction(userInput, () => _documentOrPatchNotFoundPresenter.OK(userInput));
		}

		public void DocumentPropertiesShow()
		{
			DocumentPropertiesViewModel viewModel = MainViewModel.Document.DocumentProperties;

			ExecuteNonPersistedAction(viewModel, () => _documentPropertiesPresenter.Show(viewModel));
		}

		public void DocumentPropertiesClose()
		{
			DocumentPropertiesCloseOrLoseFocus(_documentPropertiesPresenter.Close);
		}

		public void DocumentPropertiesLoseFocus()
		{
			DocumentPropertiesCloseOrLoseFocus(_documentPropertiesPresenter.LoseFocus);
		}

		private void DocumentPropertiesCloseOrLoseFocus(Func<DocumentPropertiesViewModel, DocumentPropertiesViewModel> partialAction)
		{
			// GetViewModel
			DocumentPropertiesViewModel userInput = MainViewModel.Document.DocumentProperties;

			// Template Method
			DocumentPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => partialAction(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				TitleBarRefresh();
				DocumentGridRefresh();
				DocumentTreeRefresh();
			}
		}

		public void DocumentPropertiesPlay()
		{
			DocumentPropertiesViewModel userInput = MainViewModel.Document.DocumentProperties;

			ExecuteReadAction(userInput, () => _documentPropertiesPresenter.Play(userInput));
		}

		/// <summary>
		/// Will do a a ViewModel to Entity conversion.
		/// (The private Refresh methods do not.)
		/// Will also apply (external) UnderlyingPatches.
		/// </summary>
		public void DocumentRefresh()
		{
			if (MainViewModel.Document.IsOpen)
			{
				// ToEntity
				MainViewModel.ToEntityWithRelatedEntities(_repositories);

				// Business
				_documentFacade.Refresh(MainViewModel.Document.ID);

				// ToViewModel
				DocumentViewModelRefresh();
			}
		}

		public void DocumentSave()
		{
			// ToEntity
			// Get rid of AutoPatch view model temporarily from the DocumentViewModel.
			// It should not be saved and this is the only action upon which it should not be converted to Entity.
			Document document;
			AutoPatchPopupViewModel originalAutoPatchPopup = null;
			try
			{
				originalAutoPatchPopup = MainViewModel.Document.AutoPatchPopup;
				MainViewModel.Document.AutoPatchPopup = null;

				document = MainViewModel.ToEntityWithRelatedEntities(_repositories);
			}
			finally
			{
				if (originalAutoPatchPopup != null)
				{
					MainViewModel.Document.AutoPatchPopup = originalAutoPatchPopup;
				}
			}

			// Business
			IResult validationResult = _documentFacade.Save(document);
			IResult warningsResult = _documentFacade.GetWarningsRecursive(document);

			// Commit
			if (validationResult.Successful)
			{
				_repositories.Commit();
				_systemFacade.RefreshSystemDocumentIfNeeded(document);

				MainViewModel.Document.IsDirty = false;
			}

			// ToViewModel
			MainViewModel.ValidationMessages = validationResult.Messages;
			MainViewModel.WarningMessages = warningsResult.Messages;
		}

		public void DocumentTreeAddToInstrument()
		{
			// Involves both DocumentTree and CurrentInstrument view,
			// so cannot be handled by a single sub-presenter.

			if (!MainViewModel.Document.DocumentTree.SelectedItemID.HasValue)
			{
				throw new NotHasValueException(() => MainViewModel.Document.DocumentTree.SelectedItemID);
			}

			int patchID = MainViewModel.Document.DocumentTree.SelectedItemID.Value;

			// Redirect
			AddPatchToCurrentInstrument(patchID);
		}

		public void DocumentTreeCreate()
		{
			// GetViewModel
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			switch (userInput.SelectedNodeType)
			{
				case DocumentTreeNodeTypeEnum.Libraries:
					// Redirect
					DocumentTreeCreateLibrary();
					break;

				case DocumentTreeNodeTypeEnum.Midi:
					// Redirect
					DocumentTreeCreateMidiMapping();
					break;

				case DocumentTreeNodeTypeEnum.PatchGroup:
					// Redirect
					DocumentTreeCreatePatch();
					break;

				case DocumentTreeNodeTypeEnum.Patch:
				case DocumentTreeNodeTypeEnum.LibraryPatch:
					// Redirect
					DocumentTreeCreateOperator();
					break;

				default:
					throw new ValueNotSupportedException(userInput.SelectedNodeType);
			}
		}

		private void DocumentTreeCreateLibrary()
		{
			LibrarySelectionPopupViewModel userInput = MainViewModel.Document.LibrarySelectionPopup;

			ExecuteUpdateAction(userInput, () => _librarySelectionPopupPresenter.Load(userInput));
		}

		private void DocumentTreeCreateMidiMapping()
		{
			// GetViewModel
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			// Template Method
			DocumentTreeViewModel viewModel = ExecuteCreateAction(userInput, () => _documentTreePresenter.Create(userInput));

			if (viewModel.Successful)
			{
				// Refresh
				DocumentViewModelRefresh();

				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.MidiMapping, viewModel.CreatedEntityID).ToViewModel().AsArray(),
					States = GetMidiMappingStates(viewModel.CreatedEntityID)
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);

				// Redirect
				MidiMappingDetailsShow(viewModel.CreatedEntityID);
			}
		}

		private void DocumentTreeCreateOperator()
		{
			if (MainViewModel.Document.VisiblePatchDetails == null)
			{
				throw new NullException(() => MainViewModel.Document.VisiblePatchDetails);
			}

			// GetViewModel
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			if (!userInput.SelectedItemID.HasValue)
			{
				throw new NotHasValueException(() => MainViewModel.Document.DocumentTree.SelectedItemID);
			}

			// TemplateMethod
			Operator op = null;
			IList<Operator> createdOperators = new List<Operator>();

			DocumentTreeViewModel viewModel = ExecuteCreateAction(
				userInput,
				() =>
				{
					// RefreshCounter
					userInput.RefreshID = RefreshIDProvider.GetRefreshID();

					// Set !Successful
					userInput.Successful = false;

					// GetEntities
					Patch underlyingPatch = _repositories.PatchRepository.Get(userInput.SelectedItemID.Value);
					Patch patch = _repositories.PatchRepository.Get(MainViewModel.Document.VisiblePatchDetails.Entity.ID);

					bool isSamplePatch = underlyingPatch.IsSamplePatch();
					if (isSamplePatch)
					{
						// ToViewModel
						MainViewModel.Document.SampleFileBrowser.Visible = true;
						MainViewModel.Document.SampleFileBrowser.DestPatchID = patch.ID;
					}
					else
					{
						// Business
						var operatorFactory = new OperatorFactory(patch, _repositories);
						op = operatorFactory.New(underlyingPatch, GetVariableInletOrOutletCount(underlyingPatch));

						IList<Operator> autoCreatedNumberOperators =
							_autoPatcher.CreateNumbersForEmptyInletsWithDefaultValues(op, ESTIMATED_OPERATOR_WIDTH, OPERATOR_HEIGHT);

						createdOperators.AddRange(autoCreatedNumberOperators);
						// Put main operator last so it is dispatched last upon redo and put on top.
						createdOperators.Add(op);
					}

					// Successful
					userInput.Successful = true;

					// Do not do bother with ToViewModel. We will do a full Refresh later.
					return userInput;
				});

			if (viewModel.Successful)
			{
				// Refresh
				DocumentViewModelRefresh();

				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = createdOperators.Select(x => x.ToEntityTypeAndIDViewModel()).ToArray(),
					States = createdOperators.SelectMany(x => GetOperatorStates(x.ID)).Distinct().ToArray()
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);

				// Redirect
				if (op != null)
				{
					OperatorExpand(op.ID);
				}
			}
		}

		private void DocumentTreeCreatePatch()
		{
			// GetViewModel
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			// Template Method
			DocumentTreeViewModel viewModel = ExecuteCreateAction(userInput, () => _documentTreePresenter.Create(userInput));

			if (viewModel.Successful)
			{
				// Refresh
				DocumentViewModelRefresh();

				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.Patch, viewModel.CreatedEntityID).ToViewModel().AsArray(),
					States = GetPatchStates(viewModel.CreatedEntityID)
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);

				// Redirect
				PatchDetailsShow(viewModel.CreatedEntityID);
				PatchPropertiesShow(viewModel.CreatedEntityID);
			}
		}

		public void DocumentTreeClose()
		{
			ExecuteNonPersistedDocumentTreeAction(_documentTreePresenter.Close);
		}

		public void DocumentTreeDelete()
		{
			// GetViewModel
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			// Redirect
			switch (userInput.SelectedNodeType)
			{
				case DocumentTreeNodeTypeEnum.MidiMapping:
					DocumentTreeDeleteMidiMapping();
					break;

				case DocumentTreeNodeTypeEnum.Library:
					DocumentTreeDeleteLibrary();
					break;

				case DocumentTreeNodeTypeEnum.Patch:
					DocumentTreeDeletePatch();
					break;

				default:
					throw new ValueNotSupportedException(userInput.SelectedNodeType);
			}
		}

		private void DocumentTreeDeleteMidiMapping()
		{
			// GetViewModel
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			// Undo History
			int id = userInput.SelectedItemID ?? 0;
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.MidiMapping, id).ToViewModel().AsArray(),
				States = GetMidiMappingStates(id)
			};

			// Template Method
			DocumentTreeViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _documentTreePresenter.Delete(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		private void DocumentTreeDeleteLibrary()
		{
			// GetViewModel
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			// Undo History
			int id = userInput.SelectedItemID ?? 0;
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.DocumentReference, id).ToViewModel().AsArray(),
				States = GetLibraryStates(id)
			};

			// Template Method
			DocumentTreeViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _documentTreePresenter.Delete(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		private void DocumentTreeDeletePatch()
		{
			// GetViewModel
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			// Undo History
			int id = userInput.SelectedItemID ?? 0;
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.Patch, id).ToViewModel().AsArray(),
				States = GetPatchStates(id)
			};

			// Template Method
			DocumentTreeViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _documentTreePresenter.Delete(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void DocumentTreeHoverPatch(int id)
		{
			DocumentTreeViewModel viewModel = MainViewModel.Document.DocumentTree;

			ExecuteReadAction(viewModel, () => _documentTreePresenter.HoverPatch(viewModel, id));
		}

		public void DocumentTreeOpenItemExternally()
		{
			DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

			ExecuteReadAction(userInput, () => _documentTreePresenter.OpenItemExternally(userInput));
		}

		public void DocumentTreePlay()
		{
			// GetViewModel
			DocumentTreeViewModel viewModel = MainViewModel.Document.DocumentTree;

			// TemplateMethod
			ExecuteReadAction(viewModel, func);

			DocumentTreeViewModel func()
			{
				// RefreshCounter
				viewModel.RefreshID = RefreshIDProvider.GetRefreshID();

				// Set !Successful
				viewModel.Successful = false;

				// GetEntities
				Document document = _repositories.DocumentRepository.Get(viewModel.ID);

				Result<Outlet> result;
				switch (viewModel.SelectedNodeType)
				{
					case DocumentTreeNodeTypeEnum.AudioOutput:
					{
						// GetEntities
						IList<Patch> entities = MainViewModel.Document.CurrentInstrument.Patches
						                                     .Select(x => _repositories.PatchRepository.Get(x.EntityID))
						                                     .ToArray();
						// Business
						Patch autoPatch = _autoPatcher.AutoPatch(entities);
						_autoPatcher.SubstituteSineForUnfilledInSoundPatchInlets(autoPatch);
						result = _autoPatcher.AutoPatch_TryCombineSounds(autoPatch);

						break;
					}

					case DocumentTreeNodeTypeEnum.Library:
					{
						if (!viewModel.SelectedItemID.HasValue) throw new NullException(() => viewModel.SelectedItemID);

						// GetEntity
						DocumentReference documentReference = _repositories.DocumentReferenceRepository.Get(viewModel.SelectedItemID.Value);

						// Business
						result = _autoPatcher.TryAutoPatchFromDocumentRandomly(documentReference.LowerDocument, mustIncludeHidden: false);
						if (result.Data != null)
						{
							_autoPatcher.SubstituteSineForUnfilledInSoundPatchInlets(result.Data.Operator.Patch);
						}

						break;
					}

					case DocumentTreeNodeTypeEnum.Patch:
					case DocumentTreeNodeTypeEnum.LibraryPatch:
					{
						if (!viewModel.SelectedItemID.HasValue) throw new NullException(() => viewModel.SelectedItemID);

						// GetEntities
						Patch patch = _repositories.PatchRepository.Get(viewModel.SelectedItemID.Value);

						// Business
						result = _autoPatcher.AutoPatch_TryCombineSounds(patch);
						if (result.Data != null)
						{
							_autoPatcher.SubstituteSineForUnfilledInSoundPatchInlets(result.Data.Operator.Patch);
						}

						break;
					}

					case DocumentTreeNodeTypeEnum.LibraryPatchGroup:
					{
						if (!viewModel.SelectedPatchGroupLowerDocumentReferenceID.HasValue) throw new NullException(() => viewModel.SelectedPatchGroupLowerDocumentReferenceID);

						// GetEntities
						DocumentReference lowerDocumentReference = _repositories.DocumentReferenceRepository.Get(viewModel.SelectedPatchGroupLowerDocumentReferenceID.Value);

						// Business
						result = _autoPatcher.TryAutoPatchFromPatchGroupRandomly(
							lowerDocumentReference.LowerDocument,
							viewModel.SelectedCanonicalPatchGroup,
							mustIncludeHidden: false);
						if (result.Data != null)
						{
							_autoPatcher.SubstituteSineForUnfilledInSoundPatchInlets(result.Data.Operator.Patch);
						}

						break;
					}

					case DocumentTreeNodeTypeEnum.PatchGroup:
					{
						// Business
						result = _autoPatcher.TryAutoPatchFromPatchGroupRandomly(document, viewModel.SelectedCanonicalPatchGroup, mustIncludeHidden: false);

						break;
					}

					case DocumentTreeNodeTypeEnum.Libraries:
					{
						// Business
						IList<Document> lowerDocuments = document.LowerDocumentReferences.Select(x => x.LowerDocument).ToArray();
						result = _autoPatcher.TryAutoPatchFromDocumentsRandomly(lowerDocuments, mustIncludeHidden: false);
						if (result.Data != null)
						{
							_autoPatcher.SubstituteSineForUnfilledInSoundPatchInlets(result.Data.Operator.Patch);
						}

						break;
					}

					case DocumentTreeNodeTypeEnum.AudioFileOutputList:
					case DocumentTreeNodeTypeEnum.Scales:
					default:
					{
						// Successful
						viewModel.Successful = true;

						return viewModel;
					}
				}

				// Business
				Outlet outlet = result.Data;

				// Non-Persisted
				viewModel.OutletIDToPlay = outlet?.ID;
				viewModel.Successful = result.Successful;
				viewModel.ValidationMessages.AddRange(result.Messages);

				return viewModel;
			}
		}

		public void DocumentTreeSelectAudioFileOutputs()
		{
			ExecuteNonPersistedDocumentTreeAction(_documentTreePresenter.SelectAudioFileOutputs);
		}

		public void DocumentTreeSelectAudioOutput()
		{
			ExecuteNonPersistedDocumentTreeAction(_documentTreePresenter.SelectAudioOutput);

			// Redirect
			AudioOutputPropertiesSwitch();
		}

		public void DocumentTreeSelectLibraries()
		{
			ExecuteNonPersistedDocumentTreeAction(_documentTreePresenter.SelectLibraries);
		}

		public void DocumentTreeSelectLibrary(int documentReferenceID)
		{
			ExecuteNonPersistedDocumentTreeAction(x => _documentTreePresenter.SelectLibrary(x, documentReferenceID));

			// Redirect
			LibraryPropertiesSwitch(documentReferenceID);
		}

		public void DocumentTreeSelectLibraryPatch(int id)
		{
			ExecuteNonPersistedDocumentTreeAction(x => _documentTreePresenter.SelectLibraryPatch(x, id));
		}

		public void DocumentTreeSelectLibraryPatchGroup(int lowerDocumentReferenceID, string patchGroup)
		{
			ExecuteNonPersistedDocumentTreeAction(x => _documentTreePresenter.SelectLibraryPatchGroup(x, lowerDocumentReferenceID, patchGroup));
		}

		public void DocumentTreeSelectMidi() => ExecuteNonPersistedDocumentTreeAction(_documentTreePresenter.SelectMidi);

		public void DocumentTreeSelectMidiMapping(int id)
		{
			ExecuteNonPersistedDocumentTreeAction(x => _documentTreePresenter.SelectMidiMapping(x, id));

			// Redirect
			MidiMappingDetailsSwitch(id);
		}

		public void DocumentTreeSelectScales()
		{
			ExecuteNonPersistedDocumentTreeAction(_documentTreePresenter.SelectScales);
		}

		public void DocumentTreeSelectPatch(int id)
		{
			ExecuteNonPersistedDocumentTreeAction(x => _documentTreePresenter.SelectPatch(x, id));

			// Redirect
			PatchPropertiesSwitch(id);
		}

		public void DocumentTreeSelectPatchGroup(string group)
		{
			ExecuteNonPersistedDocumentTreeAction(x => _documentTreePresenter.SelectPatchGroup(x, group));
		}

		public void DocumentTreeShow()
		{
			ExecuteNonPersistedDocumentTreeAction(_documentTreePresenter.Show);
		}

		public void DocumentTreeShowAudioOutput()
		{
			// Redirect
			AudioOutputPropertiesShow();
		}

		public void DocumentTreeShowLibrary(int documentReferenceID)
		{
			// Redirect
			LibraryPropertiesShow(documentReferenceID);
		}

		public void DocumentTreeShowMidiMapping(int id)
		{
			// Redirect
			MidiMappingDetailsShow(id);
		}

		public void DocumentTreeShowPatch(int id)
		{
			// Redirect
			PatchDetailsShow(id);
		}

		/// <summary>
		/// On top of the regular ExecuteNonPersistedAction,
		/// will set CanCreate, which cannot be determined by entities or DocumentTreeViewModel alone.
		/// </summary>
		private void ExecuteNonPersistedDocumentTreeAction(Action<DocumentTreeViewModel> partialAction)
		{
			DocumentTreeViewModel viewModel = MainViewModel.Document.DocumentTree;

			ExecuteNonPersistedAction(
				viewModel,
				() =>
				{
					partialAction(viewModel);
					SetCanCreate(viewModel);
				});
		}

		private void SetCanCreate(DocumentTreeViewModel viewModel)
		{
			bool patchDetailsVisible = MainViewModel.Document.VisiblePatchDetails != null;
			viewModel.CanCreate = ToViewModelHelper.GetCanCreate(viewModel.SelectedNodeType, patchDetailsVisible);
		}

		// Library

		public void LibraryPropertiesClose(int documentReferenceID)
		{
			// GetViewModel
			LibraryPropertiesViewModel userInput = ViewModelSelector.GetLibraryPropertiesViewModel(MainViewModel.Document, documentReferenceID);

			// Template Method
			LibraryPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _libraryPropertiesPresenter.Close(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleLibraryProperties = null;

				DocumentViewModelRefresh();
			}
		}

		public void LibraryPropertiesLoseFocus(int documentReferenceID)
		{
			// GetViewModel
			LibraryPropertiesViewModel userInput = ViewModelSelector.GetLibraryPropertiesViewModel(MainViewModel.Document, documentReferenceID);

			// Template Method
			LibraryPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _libraryPropertiesPresenter.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void LibraryPropertiesOpenExternally(int documentReferenceID)
		{
			LibraryPropertiesViewModel userInput = ViewModelSelector.GetLibraryPropertiesViewModel(MainViewModel.Document, documentReferenceID);

			ExecuteReadAction(userInput, () => _libraryPropertiesPresenter.OpenExternally(userInput));
		}

		public void LibraryPropertiesPlay(int documentReferenceID)
		{
			LibraryPropertiesViewModel userInput = ViewModelSelector.GetLibraryPropertiesViewModel(MainViewModel.Document, documentReferenceID);

			ExecuteReadAction(userInput, () => _libraryPropertiesPresenter.Play(userInput));
		}

		public void LibraryPropertiesRemove(int documentReferenceID)
		{
			// GetViewModel
			LibraryPropertiesViewModel userInput = ViewModelSelector.GetLibraryPropertiesViewModel(MainViewModel.Document, documentReferenceID);

			// Template Method
			LibraryPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _libraryPropertiesPresenter.Remove(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		private void LibraryPropertiesShow(int documentReferenceID)
		{
			LibraryPropertiesViewModel viewModel = ViewModelSelector.GetLibraryPropertiesViewModel(MainViewModel.Document, documentReferenceID);

			ExecuteNonPersistedAction(viewModel, () => _libraryPropertiesPresenter.Show(viewModel));
		}

		private void LibraryPropertiesSwitch(int documentReferenceID)
		{
			if (MainViewModel.PropertiesPanelVisible)
			{
				LibraryPropertiesShow(documentReferenceID);
			}
		}

		/// <see cref="LibrarySelectionPopupPresenter.Cancel"/>
		public void LibrarySelectionPopupCancel()
		{
			LibrarySelectionPopupViewModel userInput = MainViewModel.Document.LibrarySelectionPopup;

			ExecuteNonPersistedAction(userInput, () => _librarySelectionPopupPresenter.Cancel(userInput));
		}

		/// <see cref="LibrarySelectionPopupPresenter.Close"/>
		public void LibrarySelectionPopupClose()
		{
			LibrarySelectionPopupViewModel userInput = MainViewModel.Document.LibrarySelectionPopup;

			ExecuteNonPersistedAction(userInput, () => _librarySelectionPopupPresenter.Close(userInput));
		}

		public void LibrarySelectionPopupOK(int? lowerDocumentID)
		{
			// GetViewModel
			LibrarySelectionPopupViewModel userInput = MainViewModel.Document.LibrarySelectionPopup;

			// Template Method
			LibrarySelectionPopupViewModel viewModel = ExecuteCreateAction(userInput, () => _librarySelectionPopupPresenter.OK(userInput, lowerDocumentID));

			if (viewModel.Successful)
			{
				// Refresh
				DocumentViewModelRefresh();

				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.DocumentReference, viewModel.CreatedDocumentReferenceID).ToViewModel().AsArray(),
					States = GetLibraryStates(viewModel.CreatedDocumentReferenceID)
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);
			}
		}

		public void LibrarySelectionPopupOpenItemExternally(int lowerDocumentID)
		{
			LibrarySelectionPopupViewModel userInput = MainViewModel.Document.LibrarySelectionPopup;

			ExecuteReadAction(userInput, () => _librarySelectionPopupPresenter.OpenItemExternally(userInput, lowerDocumentID));
		}

		public void LibrarySelectionPopupPlay(int lowerDocumentID)
		{
			LibrarySelectionPopupViewModel userInput = MainViewModel.Document.LibrarySelectionPopup;

			ExecuteReadAction(userInput, () => _librarySelectionPopupPresenter.Play(userInput, lowerDocumentID));
		}

		// MidiMapping

		private void MidiMappingDetailsShow(int id)
		{
			MidiMappingDetailsViewModel userInput = ViewModelSelector.GetMidiMappingDetailsViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(userInput, () => _midiMappingDetailsPresenter.Show(userInput));
		}

		private void MidiMappingDetailsSwitch(int id)
		{
			if (MainViewModel.DetailsOrGridPanelVisible)
			{
				MidiMappingDetailsShow(id);
			}
		}

		public void MidiMappingDetailsClose(int id)
		{
			MidiMappingDetailsViewModel userInput = ViewModelSelector.GetMidiMappingDetailsViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(userInput, () => _midiMappingDetailsPresenter.Close(userInput));

			MainViewModel.Document.VisibleMidiMappingDetails = null;
		}

		public void MidiMappingDetailsCreateElement(int midiMappingID)
		{
			// GetViewModel
			MidiMappingDetailsViewModel userInput = ViewModelSelector.GetMidiMappingDetailsViewModel(MainViewModel.Document, midiMappingID);

			// Template Method
			MidiMappingDetailsViewModel viewModel = ExecuteCreateAction(userInput, () => _midiMappingDetailsPresenter.CreateElement(userInput));

			if (viewModel.Successful)
			{
				// Refresh
				DocumentViewModelRefresh();

				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.MidiMappingElement, viewModel.CreatedElementID).ToViewModel().AsArray(),
					States = GetMidiMappingElementStates(viewModel.CreatedElementID)
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);

				// Redirect
				MidiMappingElementExpand(midiMappingID, viewModel.CreatedElementID);
			}
		}

		public void MidiMappingDetailsDeleteSelectedElement(int midiMappingID)
		{
			// GetViewModel
			MidiMappingDetailsViewModel userInput = ViewModelSelector.GetMidiMappingDetailsViewModel(MainViewModel.Document, midiMappingID);

			// Undo History
			int id = userInput.SelectedElement?.ID ?? 0;
			UndoDeleteViewModel undoItem = null;
			if (id != 0)
			{
				undoItem = new UndoDeleteViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.MidiMappingElement, id).ToViewModel().AsArray(),
					States = GetMidiMappingElementStates(id)
				};
			}

			// Template Method
			MidiMappingDetailsViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _midiMappingDetailsPresenter.DeleteSelectedElement(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void MidiMappingDetailsExpandElement(int midiMappingID, int midiMappingElementID)
		{
			// Redirect
			MidiMappingElementPropertiesShow(midiMappingElementID);
		}

		public void MidiMappingDetailsMoveElement(int midiMappingID, int midiMappingElementID, float centerX, float centerY)
		{
			MidiMappingDetailsViewModel userInput = ViewModelSelector.GetMidiMappingDetailsViewModel(MainViewModel.Document, midiMappingID);

			ExecuteUpdateAction(userInput, () => _midiMappingDetailsPresenter.MoveElement(userInput, midiMappingElementID, centerX, centerY));

		}

		/// <summary> Only selecting the element, not e.g. switching properties. </summary>
		private void MidiMappingDetailsSelectElement(int midiMappingID, int midiMappingElementID)
		{
			MidiMappingDetailsViewModel userInput = ViewModelSelector.GetMidiMappingDetailsViewModel(MainViewModel.Document, midiMappingID);

			ExecuteNonPersistedAction(userInput, () => _midiMappingDetailsPresenter.SelectElement(userInput, midiMappingElementID));
		}

		/// <summary> Affects multiple partials. </summary>
		private void MidiMappingElementExpand(int midiMappingID, int midiMappingElementID)
		{
			// Redirect
			MidiMappingElementPropertiesShow(midiMappingElementID);
			MidiMappingDetailsShow(midiMappingID);
			MidiMappingDetailsSelectElement(midiMappingID, midiMappingElementID);
		}

		/// <summary> Affects multiple partials. </summary>
		public void MidiMappingElementSelect(int midiMappingID, int midiMappingElementID)
		{
			// Redirect
			MidiMappingDetailsSelectElement(midiMappingID, midiMappingElementID);
			MidiMappingElementPropertiesSwitch(midiMappingElementID);
		}

		public void MidiMappingElementPropertiesClose(int id)
		{
			// GetViewModel
			MidiMappingElementPropertiesViewModel userInput = ViewModelSelector.GetMidiMappingElementPropertiesViewModel(MainViewModel.Document, id);

			// TemplateMethod
			MidiMappingElementPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _midiMappingElementPropertiesPresenter.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleMidiMappingElementProperties = null;

				// Refresh
				DocumentViewModelRefresh();
			}
		}

		public void MidiMappingElementPropertiesDelete(int id)
		{
			// GetViewModel
			MidiMappingElementPropertiesViewModel userInput = ViewModelSelector.GetMidiMappingElementPropertiesViewModel(MainViewModel.Document, id);

			// Undo History
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.MidiMappingElement, id).ToViewModel().AsArray(),
				States = GetMidiMappingElementStates(id)
			};

			// Template Method
			MidiMappingElementPropertiesViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _midiMappingElementPropertiesPresenter.Delete(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void MidiMappingElementPropertiesExpand(int id)
		{
			MidiMappingElementPropertiesViewModel viewModel = ViewModelSelector.GetMidiMappingElementPropertiesViewModel(MainViewModel.Document, id);

			// Redirect
			MidiMappingElementExpand(viewModel.MidiMappingID, id);
		}

		public void MidiMappingElementPropertiesLoseFocus(int id)
		{
			// GetViewModel
			MidiMappingElementPropertiesViewModel userInput = ViewModelSelector.GetMidiMappingElementPropertiesViewModel(MainViewModel.Document, id);

			// TemplateMethod
			MidiMappingElementPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _midiMappingElementPropertiesPresenter.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		private void MidiMappingElementPropertiesShow(int id)
		{
			MidiMappingElementPropertiesViewModel viewModel = ViewModelSelector.GetMidiMappingElementPropertiesViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(viewModel, () => _midiMappingElementPropertiesPresenter.Show(viewModel));
		}

		private void MidiMappingElementPropertiesSwitch(int id)
		{
			if (MainViewModel.PropertiesPanelVisible)
			{
				// Redirect
				MidiMappingElementPropertiesShow(id);
			}
		}

		// Node

		/// <summary>
		/// Rotates between node types for the selected node.
		/// If no node is selected, nothing happens.
		/// </summary>
		public void NodeChangeSelectedNodeType(int curveID)
		{
			// GetViewModel
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

			// Undo History
			IList<ScreenViewModelBase> oldStates = default;
			if (userInput.SelectedNodeID.HasValue)
			{
				oldStates = GetNodeStates(userInput.SelectedNodeID.Value);
			}

			// Template Method
			CurveDetailsViewModel viewModel = ExecuteSpecialUpdateAction(userInput, () => _curveDetailsPresenter.ChangeSelectedNodeType(userInput));

			if (viewModel.Successful && userInput.SelectedNodeID.HasValue)
			{
				// Refresh
				int nodeID = userInput.SelectedNodeID.Value;
				CurveDetailsNodeRefresh(curveID, nodeID);
				NodePropertiesRefresh(nodeID);

				// Undo History
				IList<ScreenViewModelBase> newStates = GetNodeStates(userInput.SelectedNodeID.Value);
				var undoItem = new UndoUpdateViewModel
				{
					OldStates = oldStates,
					NewStates = newStates
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);
			}
		}

		public void NodeCreate(int curveID)
		{
			// GetViewModel
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

			// Template Method
			CurveDetailsViewModel viewModel = ExecuteCreateAction(userInput, () => _curveDetailsPresenter.CreateNode(userInput));

			if (viewModel.Successful)
			{
				// Refresh
				DocumentViewModelRefresh();

				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.Node, viewModel.CreatedNodeID).ToViewModel().AsArray(),
					States = GetNodeStates(viewModel.CreatedNodeID)
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);
			}
		}

		public void NodeDeleteSelected(int curveID)
		{
			// GetViewModel
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

			// Undo History
			int id = userInput.SelectedNodeID ?? 0;
			UndoDeleteViewModel undoItem = null;
			if (id != 0)
			{
				undoItem = new UndoDeleteViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.Node, id).ToViewModel().AsArray(),
					States = GetNodeStates(id)
				};
			}

			// Template Method
			CurveDetailsViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _curveDetailsPresenter.DeleteSelectedNode(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void NodeMoving(int curveID, int nodeID, double x, double y)
		{
			// Opted to not use the TemplateActionMethod
			// (which would do a complete DocumentViewModel to Entity conversion),
			// because this is faster but less robust.
			// Because it is not nice when moving nodes is slow.
			// When you work in-memory backed with zipped XML,
			// you might use the more robust method again.
			// The overhead is mainly in the database queries.

			// GetViewModel
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

			// Partial ToEntity
			userInput.ToEntityWithNodes(_curveRepositories);

			// Partial Action
			CurveDetailsViewModel viewModel = _curveDetailsPresenter.NodeMoving(userInput, nodeID, x, y);

			// Refresh
			if (viewModel.Successful)
			{
				CurveDetailsNodeRefresh(curveID, nodeID);
				NodePropertiesRefresh(nodeID);
			}
		}

		public void NodeMoved(int curveID, int nodeID, double x, double y)
		{
			// GetViewModel
			CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

			// TemplateMethod
			CurveDetailsViewModel viewModel = ExecuteUpdateAction(userInput, () => _curveDetailsPresenter.NodeMoved(userInput, nodeID, x, y));

			// Refresh
			if (viewModel.Successful)
			{
				CurveDetailsNodeRefresh(curveID, nodeID);
				NodePropertiesRefresh(nodeID);
			}
		}

		public void NodePropertiesClose(int id)
		{
			// GetViewModel
			NodePropertiesViewModel userInput = ViewModelSelector.GetNodePropertiesViewModel(MainViewModel.Document, id);

			// TemplateMethod
			NodePropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _nodePropertiesPresenter.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleNodeProperties = null;

				// Refresh
				Node node = _repositories.NodeRepository.Get(id);
				CurveDetailsNodeRefresh(node.Curve.ID, id);
			}
		}

		public void NodePropertiesDelete(int id)
		{
			// GetViewModel
			NodePropertiesViewModel userInput = ViewModelSelector.GetNodePropertiesViewModel(MainViewModel.Document, id);

			// Undo History
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.Node, id).ToViewModel().AsArray(),
				States = GetNodeStates(id)
			};

			// Template Method
			NodePropertiesViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _nodePropertiesPresenter.Delete(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void NodePropertiesExpand(int id)
		{
			NodePropertiesViewModel viewModel = ViewModelSelector.GetNodePropertiesViewModel(MainViewModel.Document, id);

			// Redirect
			NodeExpand(viewModel.CurveID, id);
		}

		public void NodePropertiesLoseFocus(int id)
		{
			// GetViewModel
			NodePropertiesViewModel userInput = ViewModelSelector.GetNodePropertiesViewModel(MainViewModel.Document, id);

			// TemplateMethod
			NodePropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _nodePropertiesPresenter.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				Node node = _repositories.NodeRepository.Get(id);
				CurveDetailsNodeRefresh(node.Curve.ID, id);
			}
		}

		private void NodePropertiesShow(int id)
		{
			NodePropertiesViewModel viewModel = ViewModelSelector.GetNodePropertiesViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(viewModel, () => _nodePropertiesPresenter.Show(viewModel));
		}

		private void NodePropertiesSwitch(int id)
		{
			if (MainViewModel.PropertiesPanelVisible)
			{
				NodePropertiesShow(id);
			}
		}

		public void NodeSelect(int curveID, int nodeID)
		{
			// Redirect
			CurveDetailsSelectNode(curveID, nodeID);
			NodePropertiesSwitch(nodeID);
		}

		private void NodeExpand(int curveID, int nodeID)
		{
			ExecuteReadAction(
				null,
				() =>
				{
					// GetEntity
					int operatorID = GetOperatorIDByCurveID(curveID);
					Operator op = _repositories.OperatorRepository.Get(operatorID);
					int patchID = op.Patch.ID;

					// Redirect
					NodePropertiesShow(nodeID);
					CurveDetailsShow(curveID);
					CurveDetailsSelectNode(curveID, nodeID);
					PatchDetailsShow(patchID);
					PatchDetailsSelectOperator(patchID, operatorID);
				});
		}

		// Operator

		public void OperatorChangeInputOutlet(int patchID, int inletID, int inputOutletID)
		{
			PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

			ExecuteUpdateAction(userInput, () => _patchDetailsPresenter.ChangeInputOutlet(userInput, inletID, inputOutletID));
		}

		public void OperatorMove(int patchID, int operatorID, float centerX, float centerY)
		{
			PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

			ExecuteUpdateAction(userInput, () => _patchDetailsPresenter.MoveOperator(userInput, operatorID, centerX, centerY));
		}

		public void OperatorPropertiesClose(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel userInput = ViewModelSelector.GetOperatorPropertiesViewModel(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties = null;

				// Refresh
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesClose_ForCache(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForCache userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForCache(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForCache viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForCache.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_ForCache = null;

				// Refresh
				PatchDetails_RefreshOperator(userInput.ID);
			}
		}

		public void OperatorPropertiesClose_ForCurve(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForCurve userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForCurve_ByOperatorID(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForCurve viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForCurve.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_ForCurve = null;

				// Refresh
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesClose_ForInletsToDimension(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForInletsToDimension userInput =
				ViewModelSelector.GetOperatorPropertiesViewModel_ForInletsToDimension(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForInletsToDimension viewModel = ExecuteUpdateAction(
				userInput,
				() => _operatorPropertiesPresenter_ForInletsToDimension.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_ForInletsToDimension = null;

				// Refresh
				PatchDetails_RefreshOperator(userInput.ID);
			}
		}

		public void OperatorPropertiesClose_ForNumber(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForNumber userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForNumber(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForNumber viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForNumber.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_ForNumber = null;

				// Refresh
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesClose_ForPatchInlet(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForPatchInlet userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForPatchInlet(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForPatchInlet viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForPatchInlet.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_ForPatchInlet = null;

				// Refresh
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesClose_ForPatchOutlet(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForPatchOutlet userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForPatchOutlet(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForPatchOutlet viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForPatchOutlet.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_ForPatchOutlet = null;

				// Refresh
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesClose_ForSample(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForSample userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForSample(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForSample viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForSample.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_ForSample = null;

				// Refresh
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesClose_WithInterpolation(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_WithInterpolation userInput = ViewModelSelector.GetOperatorPropertiesViewModel_WithInterpolation(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_WithInterpolation viewModel =
				ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_WithInterpolation.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_WithInterpolation = null;

				// Refresh
				PatchDetails_RefreshOperator(userInput.ID);
			}
		}

		public void OperatorPropertiesClose_WithCollectionRecalculation(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_WithCollectionRecalculation userInput =
				ViewModelSelector.GetOperatorPropertiesViewModel_WithCollectionRecalculation(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_WithCollectionRecalculation viewModel = ExecuteUpdateAction(
				userInput,
				() => _operatorPropertiesPresenter_WithCollectionRecalculation.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleOperatorProperties_WithCollectionRecalculation = null;

				// Refresh
				PatchDetails_RefreshOperator(userInput.ID);
			}
		}

		public void OperatorPropertiesDelete(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModelBase userInput = ViewModelSelector.GetOperatorPropertiesViewModelPolymorphic(MainViewModel.Document, id);

			// Undo History
			IList<int> deletedOperatorIDs = GetOperatorIDsToDelete(userInput.PatchID, id);
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = deletedOperatorIDs.Select(x => (EntityTypeEnum.Operator, x).ToViewModel()).ToArray(),
				States = deletedOperatorIDs.SelectMany(GetOperatorStates).Distinct().ToArray()
			};

			// TemplateMethod
			OperatorPropertiesViewModelBase viewModel = ExecuteDeleteAction(userInput, undoItem, () => GetOperatorPropertiesPresenter(id).Delete(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesExpand(int id)
		{
			// Redirect
			OperatorExpand(id);
		}

		public void OperatorPropertiesLoseFocus(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel userInput = ViewModelSelector.GetOperatorPropertiesViewModel(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesLoseFocus_ForCache(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForCache userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForCache(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForCache viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForCache.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				PatchDetails_RefreshOperator(userInput.ID);
			}
		}

		public void OperatorPropertiesLoseFocus_ForCurve(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForCurve userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForCurve_ByOperatorID(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForCurve viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForCurve.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesLoseFocus_ForInletsToDimension(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForInletsToDimension userInput =
				ViewModelSelector.GetOperatorPropertiesViewModel_ForInletsToDimension(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForInletsToDimension viewModel = ExecuteUpdateAction(
				userInput,
				() => _operatorPropertiesPresenter_ForInletsToDimension.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				PatchDetails_RefreshOperator(userInput.ID);
			}
		}

		public void OperatorPropertiesLoseFocus_ForNumber(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForNumber userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForNumber(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForNumber viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForNumber.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesLoseFocus_ForPatchInlet(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForPatchInlet userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForPatchInlet(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForPatchInlet viewModel =
				ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForPatchInlet.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesLoseFocus_ForPatchOutlet(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForPatchOutlet userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForPatchOutlet(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForPatchOutlet viewModel =
				ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForPatchOutlet.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesLoseFocus_ForSample(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_ForSample userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForSample(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_ForSample viewModel = ExecuteUpdateAction(userInput, () => _operatorPropertiesPresenter_ForSample.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void OperatorPropertiesLoseFocus_WithInterpolation(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_WithInterpolation userInput = ViewModelSelector.GetOperatorPropertiesViewModel_WithInterpolation(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_WithInterpolation viewModel = ExecuteUpdateAction(
				userInput,
				() => _operatorPropertiesPresenter_WithInterpolation.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				PatchDetails_RefreshOperator(userInput.ID);
			}
		}

		public void OperatorPropertiesLoseFocus_WithCollectionRecalculation(int id)
		{
			// GetViewModel
			OperatorPropertiesViewModel_WithCollectionRecalculation userInput =
				ViewModelSelector.GetOperatorPropertiesViewModel_WithCollectionRecalculation(MainViewModel.Document, id);

			// TemplateMethod
			OperatorPropertiesViewModel_WithCollectionRecalculation viewModel = ExecuteUpdateAction(
				userInput,
				() => _operatorPropertiesPresenter_WithCollectionRecalculation.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				PatchDetails_RefreshOperator(userInput.ID);
			}
		}

		public void OperatorPropertiesPlay(int id)
		{
			OperatorPropertiesViewModelBase userInput = ViewModelSelector.GetOperatorPropertiesViewModelPolymorphic(MainViewModel.Document, id);

			ExecuteReadAction(userInput, () => GetOperatorPropertiesPresenter(id).Play(userInput));
		}

		private void OperatorPropertiesShow(int id)
		{
			{
				OperatorPropertiesViewModel viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_ForCache viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForCache(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_ForCache.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_ForCurve viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForCurve_ByOperatorID(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_ForCurve.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_ForInletsToDimension
					viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForInletsToDimension(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_ForInletsToDimension.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_ForNumber viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForNumber(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_ForNumber.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_ForPatchInlet viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForPatchInlet(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_ForPatchInlet.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_ForPatchOutlet viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForPatchOutlet(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_ForPatchOutlet.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_ForSample viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForSample(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_ForSample.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_WithInterpolation viewModel = ViewModelSelector.TryGetOperatorPropertiesViewModel_WithInterpolation(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_WithInterpolation.Show(viewModel));
					return;
				}
			}
			{
				OperatorPropertiesViewModel_WithCollectionRecalculation viewModel =
					ViewModelSelector.TryGetOperatorPropertiesViewModel_WithCollectionRecalculation(MainViewModel.Document, id);
				if (viewModel != null)
				{
					ExecuteNonPersistedAction(viewModel, () => _operatorPropertiesPresenter_WithCollectionRecalculation.Show(viewModel));
					return;
				}
			}

			throw new NotFoundException<OperatorPropertiesViewModelBase>(new { OperatorID = id });
		}

		private void OperatorPropertiesSwitch(int id)
		{
			if (MainViewModel.PropertiesPanelVisible)
			{
				OperatorPropertiesShow(id);
			}
		}

		public void OperatorSelect(int patchID, int operatorID)
		{
			// Redirect
			PatchDetailsSelectOperator(patchID, operatorID);
			OperatorPropertiesSwitch(operatorID);
		}

		public void OperatorExpand(int operatorID)
		{
			ExecuteReadAction(
				null,
				() =>
				{
					// GetEntities
					Operator op = _repositories.OperatorRepository.Get(operatorID);
					Curve curve = op.Curve;

					// Redirect
					if (curve != null)
					{
						CurveExpand(curve.ID);
					}
					else
					{
						int patchID = op.Patch.ID;
						OperatorPropertiesShow(operatorID);
						PatchDetailsShow(patchID);
					}
				});
		}

		// Patch

		public void PatchDetailsAddToInstrument(int id)
		{
			AddPatchToCurrentInstrument(id);
		}

		public void PatchDetailsClose(int id)
		{
			PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(userInput, () => _patchDetailsPresenter.Close(userInput));

			MainViewModel.Document.VisiblePatchDetails = null;
		}

		public void PatchDetailsDeleteSelectedOperator(int patchID)
		{
			// GetViewModel
			PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

			// Undo History
			IList<int> operatorIDsToDelete = GetOperatorIDsToDelete(patchID, userInput.SelectedOperator?.ID);
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = operatorIDsToDelete.Select(x => (EntityTypeEnum.Operator, x).ToViewModel()).ToArray(),
				States = operatorIDsToDelete.SelectMany(GetOperatorStates).Distinct().ToArray()
			};

			// Template Method
			PatchDetailsViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _patchDetailsPresenter.DeleteOperator(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void PatchDetailsPlay(int id)
		{
			PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, id);

			ExecuteReadAction(userInput, () => _patchDetailsPresenter.Play(userInput));
		}

		private void PatchDetailsSelectOperator(int patchID, int operatorID)
		{
			PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

			ExecuteNonPersistedAction(userInput, () => _patchDetailsPresenter.SelectOperator(userInput, operatorID));
		}

		public void PatchDetailsShow(int id)
		{
			PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(userInput, () => _patchDetailsPresenter.Show(userInput));
		}

		public void PatchDetailsExpand(int id)
		{
			// Redirect
			PatchExpand(id);
		}

		public void PatchDetailsSelect(int id)
		{
			// Redirect
			PatchPropertiesSwitch(id);
			DocumentTreeSelectPatch(id);
		}

		private void PatchExpand(int id)
		{
			ExecuteReadAction(
				null,
				() =>
				{
					// GetEntities
					Document document = _repositories.DocumentRepository.Get(MainViewModel.Document.ID);
					Patch patch = _repositories.PatchRepository.Get(id);

					// Business
					bool isExternal = patch.IsExternal(document);

					if (isExternal)
					{
						// Non-Persisted
						MainViewModel.Document.DocumentToOpenExternally = patch.Document.ToIDAndName();
						MainViewModel.Document.PatchToOpenExternally = patch.ToIDAndName();
					}
					else
					{
						// Redirect
						PatchPropertiesShow(id);
						PatchDetailsShow(id);
						DocumentTreeSelectPatch(id);
					}
				});
		}

		public void PatchPropertiesAddToInstrument(int id)
		{
			AddPatchToCurrentInstrument(id);
		}

		public void PatchPropertiesChangeHasDimension(int id)
		{
			PatchPropertiesViewModel userInput = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, id);

			ExecuteUpdateAction(userInput, () => _patchPropertiesPresenter.ChangeHasDimension(userInput));
		}

		public void PatchPropertiesClose(int id)
		{
			// GetViewModel
			PatchPropertiesViewModel userInput = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, id);

			// Template Method
			PatchPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _patchPropertiesPresenter.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisiblePatchProperties = null;

				// Refresh
				DocumentViewModelRefresh();
			}
		}

		public void PatchPropertiesDelete(int id)
		{
			// GetViewModel
			PatchPropertiesViewModel userInput = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, id);

			// Undo History
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.Patch, id).ToViewModel().AsArray(),
				States = GetPatchStates(id)
			};

			// Template Method
			PatchPropertiesViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _patchPropertiesPresenter.Delete(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void PatchPropertiesExpand(int id)
		{
			// Redirect
			PatchExpand(id);
		}

		public void PatchPropertiesLoseFocus(int id)
		{
			// GetViewModel
			PatchPropertiesViewModel userInput = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, id);

			// Template Method
			PatchPropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _patchPropertiesPresenter.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void PatchPropertiesPlay(int id)
		{
			PatchPropertiesViewModel userInput = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, id);

			ExecuteReadAction(userInput, () => _patchPropertiesPresenter.Play(userInput));
		}

		private void PatchPropertiesShow(int id)
		{
			PatchPropertiesViewModel viewModel = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, id);

			ExecuteNonPersistedAction(viewModel, () => _patchPropertiesPresenter.Show(viewModel));
		}

		private void PatchPropertiesSwitch(int id)
		{
			if (MainViewModel.PropertiesPanelVisible)
			{
				PatchPropertiesShow(id);
			}
		}

		// Sample

		public void SampleFileBrowserCancel()
		{
			SampleFileBrowserViewModel userInput = MainViewModel.Document.SampleFileBrowser;

			ExecuteNonPersistedAction(userInput, () => _sampleFileBrowserPresenter.Cancel(userInput));
		}

		public void SampleFileBrowserOK()
		{
			// GetViewModel
			SampleFileBrowserViewModel userInput = MainViewModel.Document.SampleFileBrowser;

			// TemplateMethod
			SampleFileBrowserViewModel viewModel = ExecuteCreateAction(userInput, () => _sampleFileBrowserPresenter.OK(userInput));

			if (viewModel.Successful)
			{
				// Refresh
				DocumentViewModelRefresh();

				// Undo History
				// (Put main operator last so it is dispatched last upon redo and put on top.)
				IList<int> createdOperatorIDs = viewModel.AutoCreatedNumberOperatorIDs.Union(viewModel.CreatedMainOperatorID).ToArray();
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = createdOperatorIDs.Select(x => (EntityTypeEnum.Operator, x).ToViewModel()).ToArray(),
					States = createdOperatorIDs.SelectMany(GetOperatorStates).Distinct().ToArray()
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);
			}
		}

		// SaveChanges

		private void SaveChangesPopupShow(int? documentIDToOpenAfterConfirmation = null, bool mustGoToDocumentCreateAfterConfirmation = false)
		{
			SaveChangesPopupViewModel viewModel = MainViewModel.Document.SaveChangesPopup;

			ExecuteNonPersistedAction(
				viewModel,
				() => _saveChangesPopupPresenter.Show(viewModel, documentIDToOpenAfterConfirmation, mustGoToDocumentCreateAfterConfirmation));
		}

		public void SaveChangesPopupCancel()
		{
			SaveChangesPopupViewModel viewModel = MainViewModel.Document.SaveChangesPopup;

			ExecuteNonPersistedAction(viewModel, () => _saveChangesPopupPresenter.Cancel(viewModel));
		}

		public void SaveChangesPopupNo()
		{
			// GetViewModel
			SaveChangesPopupViewModel viewModel = MainViewModel.Document.SaveChangesPopup;

			// TemplateMethod
			ExecuteNonPersistedAction(
				viewModel,
				() =>
				{
					_saveChangesPopupPresenter.No(viewModel);
					MainViewModel.Document.IsDirty = false;
				});

			// Redirect
			RedirectAfterSaveChangesPopup(viewModel);
		}

		public void SaveChangesPopupYes()
		{
			// GetViewModel
			SaveChangesPopupViewModel viewModel = MainViewModel.Document.SaveChangesPopup;

			// TemplateMethod
			ExecuteNonPersistedAction(viewModel, () => _saveChangesPopupPresenter.Yes(viewModel));

			// Redirect
			DocumentSave();
			RedirectAfterSaveChangesPopup(viewModel);
		}

		private void RedirectAfterSaveChangesPopup(SaveChangesPopupViewModel viewModel)
		{
			DocumentClose();

			if (viewModel.DocumentIDToOpenAfterConfirmation.HasValue)
			{
				DocumentOpen(viewModel.DocumentIDToOpenAfterConfirmation.Value);
			}
			else if (viewModel.MustGoToDocumentCreateAfterConfirmation)
			{
				DocumentCreate();
			}

			viewModel.DocumentIDToOpenAfterConfirmation = null;
			viewModel.MustGoToDocumentCreateAfterConfirmation = false;
		}

		// Scale

		public void ScaleGridShow()
		{
			ScaleGridViewModel viewModel = MainViewModel.Document.ScaleGrid;

			ExecuteNonPersistedAction(viewModel, () => _scaleGridPresenter.Show(viewModel));
		}

		public void ScaleGridClose()
		{
			ScaleGridViewModel userInput = MainViewModel.Document.ScaleGrid;

			ExecuteNonPersistedAction(userInput, () => _scaleGridPresenter.Close(userInput));
		}

		public void ScaleGridCreate()
		{
			// GetViewModel
			ScaleGridViewModel userInput = MainViewModel.Document.ScaleGrid;

			// Template Method
			ScaleGridViewModel viewModel = ExecuteCreateAction(userInput, () => _scaleGridPresenter.Create(userInput));

			if (viewModel.Successful)
			{
				// Refresh
				DocumentViewModelRefresh();

				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.Scale, viewModel.CreatedScaleID).ToViewModel().AsArray(),
					States = GetScaleStates(viewModel.CreatedScaleID)
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);
			}
		}

		public void ScaleGridDelete(int id)
		{
			// GetViewModel
			ScaleGridViewModel userInput = MainViewModel.Document.ScaleGrid;

			// Undo History
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.Scale, id).ToViewModel().AsArray(),
				States = GetScaleStates(id)
			};

			// Template Method
			ScaleGridViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _scaleGridPresenter.Delete(userInput, id));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void ScaleShow(int id)
		{
			// GetViewModel
			ScalePropertiesViewModel viewModel1 = ViewModelSelector.GetScalePropertiesViewModel(MainViewModel.Document, id);
			ToneGridEditViewModel viewModel2 = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID: id);

			// Partial Actions
			_scalePropertiesPresenter.Show(viewModel1);
			_toneGridEditPresenter.Show(viewModel2);

			// DispatchViewModel
			DispatchViewModel(viewModel1);
			DispatchViewModel(viewModel2);
		}

		public void ScalePropertiesClose(int id)
		{
			// Get ViewModel
			ScalePropertiesViewModel userInput = ViewModelSelector.GetScalePropertiesViewModel(MainViewModel.Document, id);

			// Template Method
			ScalePropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _scalePropertiesPresenter.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleScaleProperties = null;

				// Refresh
				ToneGridEditRefresh(userInput.Entity.ID);
				ScaleGridRefresh();
				ScaleLookupRefresh();
			}
		}

		public void ScalePropertiesDelete(int id)
		{
			// GetViewModel
			ScalePropertiesViewModel userInput = ViewModelSelector.GetScalePropertiesViewModel(MainViewModel.Document, id);

			// Undo History
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.Scale, id).ToViewModel().AsArray(),
				States = GetScaleStates(id)
			};

			// Template Method
			ScalePropertiesViewModel viewModel = ExecuteDeleteAction(userInput, undoItem, () => _scalePropertiesPresenter.Delete(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				DocumentViewModelRefresh();
			}
		}

		public void ScalePropertiesLoseFocus(int id)
		{
			// Get ViewModel
			ScalePropertiesViewModel userInput = ViewModelSelector.GetScalePropertiesViewModel(MainViewModel.Document, id);

			// Template Method
			ScalePropertiesViewModel viewModel = ExecuteUpdateAction(userInput, () => _scalePropertiesPresenter.LoseFocus(userInput));

			// Refresh
			if (viewModel.Successful)
			{
				ToneGridEditRefresh(userInput.Entity.ID);
				ScaleGridRefresh();
				ScaleLookupRefresh();
			}
		}

		// Tone

		public void ToneCreate(int scaleID)
		{
			// GetViewModel
			ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

			// TemplateMethod
			ToneGridEditViewModel viewModel = ExecuteCreateAction(userInput, () => _toneGridEditPresenter.CreateTone(userInput));

			if (viewModel.Successful)
			{
				// Undo History
				var undoItem = new UndoCreateViewModel
				{
					EntityTypesAndIDs = (EntityTypeEnum.Tone, viewModel.CreatedToneID).ToViewModel().AsArray(),
					States = GetToneStates(scaleID)
				};
				MainViewModel.Document.UndoHistory.Push(undoItem);
			}
		}

		public void ToneDelete(int scaleID, int toneID)
		{
			// GetViewModel
			ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

			// Undo History
			var undoItem = new UndoDeleteViewModel
			{
				EntityTypesAndIDs = (EntityTypeEnum.Tone, toneID).ToViewModel().AsArray(),
				States = GetToneStates(scaleID)
			};

			// Template Method
			ExecuteDeleteAction(userInput, undoItem, () => _toneGridEditPresenter.DeleteTone(userInput, toneID));
		}

		public void ToneGridEditClose(int scaleID)
		{
			ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

			ToneGridEditViewModel viewModel = ExecuteUpdateAction(userInput, () => _toneGridEditPresenter.Close(userInput));

			if (viewModel.Successful)
			{
				MainViewModel.Document.VisibleToneGridEdit = null;
			}
		}

		public void ToneGridEditLoseFocus(int scaleID)
		{
			ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

			ExecuteUpdateAction(userInput, () => _toneGridEditPresenter.LoseFocus(userInput));
		}

		public void ToneGridEditEdit(int scaleID)
		{
			ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

			ExecuteUpdateAction(userInput, () => _toneGridEditPresenter.Edit(userInput));
		}

		public void TonePlay(int scaleID, int toneID)
		{
			// NOTE:
			// Cannot use partial presenter, because this action uses both
			// ToneGridEditViewModel and CurrentInstrument view model.

			// GetEntity
			ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

			// Template Method
			ExecuteReadAction(
				userInput,
				() =>
				{
					// ViewModel Validator
					IValidator viewModelValidator = new ToneGridEditViewModelValidator(userInput);
					if (!viewModelValidator.IsValid)
					{
						userInput.ValidationMessages = viewModelValidator.Messages;
						DispatchViewModel(userInput);
						return null;
					}

					// GetEntities
					Tone tone = _repositories.ToneRepository.Get(toneID);

					var underlyingPatches = new List<Patch>(MainViewModel.Document.CurrentInstrument.Patches.Count);
					foreach (CurrentInstrumentItemViewModel itemViewModel in MainViewModel.Document.CurrentInstrument.Patches)
					{
						Patch underlyingPatch = _repositories.PatchRepository.Get(itemViewModel.EntityID);
						underlyingPatches.Add(underlyingPatch);
					}

					// Business
					Outlet outlet = null;
					if (underlyingPatches.Count != 0)
					{
						outlet = _autoPatcher.TryAutoPatchWithTone(tone, underlyingPatches);
					}

					if (outlet != null)
					{
						_autoPatcher.SubstituteSineForUnfilledInSoundPatchInlets(outlet.Operator.Patch);
					}

					if (outlet == null) // Fallback to Sine
					{
						Patch patch = _patchFacade.CreatePatch();

						var operatorFactory = new OperatorFactory(patch, _repositories);
						double frequency = tone.GetFrequency();
						outlet = operatorFactory.Sine(operatorFactory.PatchInlet(DimensionEnum.Frequency, frequency));
					}

					// ToViewModel
					ToneGridEditViewModel viewModel = tone.Scale.ToToneGridEditViewModel();

					// Non-Persisted
					viewModel.OutletIDToPlay = outlet.ID;
					viewModel.Visible = userInput.Visible;
					viewModel.Successful = true;

					return viewModel;
				});
		}

		// Helpers

		/// <summary>
		/// A template method for a MainPresenter action method,
		/// that will not read or write the entity model,
		/// but works with non-entity model data only.
		///
		/// Most steps otherwise needed in for instance write actions are not needed.
		/// 
		/// Executes a sub-presenter's action and surrounds it with:
		/// a) Dispatching the view model (for instance needed to hide other view models if a new view model is displayed over it).
		/// 
		/// All you need to do is provide the right sub-viewmodel,
		/// provide a delegate to the sub-presenter's action method.
		/// </summary>
		private void ExecuteNonPersistedAction<TViewModel>(TViewModel viewModelToDispatch, Action partialAction)
			where TViewModel : ScreenViewModelBase
		{
			if (viewModelToDispatch == null) throw new ArgumentNullException(nameof(viewModelToDispatch));

			// Partial Action
			partialAction();

			// DispatchViewModel
			DispatchViewModel(viewModelToDispatch);
		}

		/// <summary>
		/// A template method for a MainPresenter action method,
		/// that will read the document model, but not write to it.
		///
		/// This version omits the full document validation and successful flags.
		/// 
		/// Executes a sub-presenter's action and surrounds it with:
		/// a) Converting the full document view model to entity.
		/// b) Dispatching the view model (for instance needed to hide other view models if a new view model is displayed over it).
		/// 
		/// All you need to do is provide the right sub-viewmodel,
		/// provide a delegate to the sub-presenter's action method.
		/// </summary>
		/// <param name="viewModelToDispatch">
		/// Can be null if no view model is relevant. But if you have a relevant view model, please pass it along.
		/// </param>
		private void ExecuteReadAction(ScreenViewModelBase viewModelToDispatch, Action partialAction)
		{
			// ToEntity
			if (MainViewModel.Document.IsOpen)
			{
				MainViewModel.ToEntityWithRelatedEntities(_repositories);
			}

			// Partial Action
			partialAction();

			// DispatchViewModel
			if (viewModelToDispatch != null)
			{
				DispatchViewModel(viewModelToDispatch);
			}
		}

		/// <summary>
		/// A template method for a MainPresenter action method,
		/// that will read the document model, but not write to it.
		///
		/// This version omits the full document validation and successful flags
		/// but allows the partial action to return a new view model.
		/// 
		/// Executes a sub-presenter's action and surrounds it with:
		/// a) Converting the full document view model to entity.
		/// b) Dispatching the view model (for instance needed to hide other view models if a new view model is displayed over it).
		/// 
		/// All you need to do is provide the right sub-viewmodel,
		/// provide a delegate to the sub-presenter's action method.
		/// </summary>
		/// <param name="viewModelToDoNothingWith">
		/// You can pass null to it if you want.
		/// Or a specific view model that your action is about.
		/// This parameter is not used in this method.
		/// However, it is there for a very important reason.
		/// If it were not, you could by accident call the other overload
		/// of ExecuteReadAction and not use the return value of your delegate.
		/// By having this parameter, the overload that is called, is always
		/// only dependent on partialAction returning or not returning a view model.
		/// </param>
		// ReSharper disable once UnusedParameter.Local
		private void ExecuteReadAction<TViewModel>(TViewModel viewModelToDoNothingWith, Func<TViewModel> partialAction)
			where TViewModel : ScreenViewModelBase
		{
			// ToEntity
			if (MainViewModel.Document.IsOpen)
			{
				MainViewModel.ToEntityWithRelatedEntities(_repositories);
			}

			// Partial Action
			TViewModel viewModel = partialAction();

			// DispatchViewModel
			DispatchViewModel(viewModel);
		}

		private TViewModel ExecuteCreateAction<TViewModel>(TViewModel userInput, Func<TViewModel> partialAction)
			where TViewModel : ScreenViewModelBase
		{
			return ExecuteWriteAction(userInput, partialAction, undoHistoryDelegate: () => MainViewModel.Document.RedoFuture.Clear());
		}

		/// <summary>
		/// Works only if userInput is the final state of the action, not if other data changes have to be applied,
		/// before having the 'new state' of the undo action.
		/// </summary>
		private TViewModel ExecuteUpdateAction<TViewModel>(TViewModel userInput, Func<TViewModel> partialAction)
			where TViewModel : ScreenViewModelBase
		{
			return ExecuteWriteAction(
				userInput,
				partialAction,
				undoHistoryDelegate: () =>
				{
					var undoItemViewModel = new UndoUpdateViewModel
					{
						OldStates = userInput.OriginalState.AsArray(),
						NewStates = userInput.AsArray()
					};
					MainViewModel.Document.UndoHistory.Push(undoItemViewModel);

					MainViewModel.Document.RedoFuture.Clear();
				});
		}

		/// <summary>
		/// The normal ExecuteUpdateAction will handle undo state for most update actions.
		/// Too bad it is only suitable for when the userInput is the 'new state' for the undo history.
		/// For instance for the ChangeSelectedNodeType action, the user input is not the final state of the action.
		/// </summary>
		private TViewModel ExecuteSpecialUpdateAction<TViewModel>(TViewModel userInput, Func<TViewModel> partialAction)
			where TViewModel : ScreenViewModelBase
		{
			return ExecuteWriteAction(
				userInput,
				partialAction,
				undoHistoryDelegate: () => MainViewModel.Document.RedoFuture.Clear());
		}

		/// <param name="undoItemViewModel">
		/// For delete actions the undo item must be created before executing this template method,
		/// since afterwards the state to remember is already gone.
		/// </param>
		private TViewModel ExecuteDeleteAction<TViewModel>(TViewModel userInput, UndoDeleteViewModel undoItemViewModel, Func<TViewModel> partialAction)
			where TViewModel : ScreenViewModelBase
		{
			return ExecuteWriteAction(
				userInput,
				partialAction,
				undoHistoryDelegate: () =>
				{
					MainViewModel.Document.UndoHistory.Push(undoItemViewModel);
					MainViewModel.Document.RedoFuture.Clear();
				});
		}

		/// <summary>
		/// A template method for a MainPresenter action method,
		/// that will write to the document entity.
		/// 
		/// Works for most write actions. Less suitable for specialized cases:
		/// In particular the ones that are not about the open document.
		///
		/// Executes a sub-presenter's action and surrounds it with:
		/// a) Converting the full document view model to entity.
		/// b) Doing a full document validation.
		/// c) Managing view model transactionality.
		/// d) Dispatching the view model (for instance needed to hide other view models if a new view model is displayed over it).
		/// e) Setting dirty flag
		/// f) Undo history handling (calls the undoHistoryDelegate)
		/// 
		/// All you need to do is provide the right sub-viewmodel,
		/// provide a delegate to the sub-presenter's action method
		/// and possibly do some refreshing of other view models afterwards.
		/// </summary>
		private TViewModel ExecuteWriteAction<TViewModel>(TViewModel userInput, Func<TViewModel> partialAction, Action undoHistoryDelegate)
			where TViewModel : ScreenViewModelBase
		{
			if (userInput == null) throw new NullException(() => userInput);

			// Set !Successful
			userInput.Successful = false;

			// ToEntity
			Document document = null;
			if (MainViewModel.Document.IsOpen)
			{
				document = MainViewModel.ToEntityWithRelatedEntities(_repositories);
			}

			// Partial Action
			TViewModel viewModel = partialAction();
			if (!viewModel.Successful)
			{
				// DispatchViewModel
				DispatchViewModel(viewModel);

				return viewModel;
			}

			// Set !Successful
			viewModel.Successful = false;

			if (MainViewModel.Document.IsOpen)
			{
				// Business
				IResult validationResult = _documentFacade.Save(document);
				if (!validationResult.Successful)
				{
					// Non-Persisted
					viewModel.ValidationMessages.AddRange(validationResult.Messages);

					// DispatchViewModel
					DispatchViewModel(viewModel);

					return viewModel;
				}

				// Undo History
				undoHistoryDelegate();

				// Dirty Flag
				MainViewModel.Document.IsDirty = true;
			}

			// Successful
			viewModel.Successful = true;

			// DispatchViewModel
			DispatchViewModel(viewModel);

			return viewModel;
		}
	}
}