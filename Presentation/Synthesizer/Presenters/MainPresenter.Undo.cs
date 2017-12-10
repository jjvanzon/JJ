﻿using System.Collections.Generic;
using System.Linq;
using JJ.Business.Canonical;
using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;
using JJ.Framework.Collections;
using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToEntity;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;

namespace JJ.Presentation.Synthesizer.Presenters
{
	public partial class MainPresenter
	{
		public void Undo()
		{
			UndoItemViewModelBase undoItemViewModel = MainViewModel.Document.UndoHistory.PopOrDefault();

			if (undoItemViewModel == null)
			{
				return;
			}

			MainViewModel.Document.RedoFuture.Push(undoItemViewModel);

			switch (undoItemViewModel)
			{
				case UndoCreateViewModel undoInsertViewModel:
					foreach (EntityTypeAndIDViewModel entityTypeAndIDViewModel in undoInsertViewModel.EntityTypesAndIDs)
					{
						ExecuteUndoRedoDeletion(entityTypeAndIDViewModel.EntityTypeEnum, entityTypeAndIDViewModel.EntityID);
					}
					break;

				case UndoUpdateViewModel undoUpdateViewModel:
					ExecuteUndoRedoInsertionOrUpdate(undoUpdateViewModel.OldState);
					break;

				case UndoDeleteViewModel undoDeleteViewModel:
					ExecuteUndoRedoInsertionOrUpdate(undoDeleteViewModel.States);
					break;

				default:
					throw new UnexpectedTypeException(() => undoItemViewModel);
			}
		}

		public void Redo()
		{
			UndoItemViewModelBase undoItemViewModel = MainViewModel.Document.RedoFuture.PopOrDefault();

			if (undoItemViewModel == null)
			{
				return;
			}

			MainViewModel.Document.UndoHistory.Push(undoItemViewModel);

			// ToEntity
			if (MainViewModel.Document.IsOpen)
			{
				MainViewModel.ToEntityWithRelatedEntities(_repositories);
			}

			switch (undoItemViewModel)
			{
				case UndoCreateViewModel undoInsertViewModel:
					ExecuteUndoRedoInsertionOrUpdate(undoInsertViewModel.States);
					break;

				case UndoUpdateViewModel undoUpdateViewModel:
					ExecuteUndoRedoInsertionOrUpdate(undoUpdateViewModel.NewState);
					break;

				case UndoDeleteViewModel undoDeleteViewModel:
					ExecuteUndoRedoDeletion(undoDeleteViewModel.EntityTypeEnum, undoDeleteViewModel.EntityID);
					break;

				default:
					throw new UnexpectedTypeException(() => undoItemViewModel);
			}
		}

		private void ExecuteUndoRedoInsertionOrUpdate(params ViewModelBase[] states) => ExecuteUndoRedoInsertionOrUpdate((IList<ViewModelBase>)states);

		private void ExecuteUndoRedoInsertionOrUpdate(IList<ViewModelBase> states)
		{
			// Action
			foreach (ViewModelBase viewModel in states)
			{
				RestoreUndoState(viewModel);
			}

			// ToEntity
			if (MainViewModel.Document.IsOpen)
			{
				MainViewModel.ToEntityWithRelatedEntities(_repositories);
			}

			// Side-Effects
			if (MustRefreshDocument(states))
			{
				_documentManager.Refresh(MainViewModel.Document.ID);
			}

			// Refresh
			DocumentViewModelRefresh();
		}

		private bool MustRefreshDocument(IList<ViewModelBase> states)
		{
			foreach (ViewModelBase state in states)
			{
				switch (state)
				{
					case OperatorPropertiesViewModel_ForPatchInlet _:
					case OperatorPropertiesViewModel_ForPatchOutlet _:
						return true;
				}
			}

			return false;
		}

		private void RestoreUndoState(ViewModelBase viewModel)
		{
			viewModel.Successful = true;
			viewModel.Visible = true;
			viewModel.RefreshID = RefreshIDProvider.GetRefreshID();

			DispatchViewModel(viewModel);
		}

		private void ExecuteUndoRedoDeletion(EntityTypeEnum entityTypeEnum, int id)
		{
			// ToEntity
			if (MainViewModel.Document.IsOpen)
			{
				MainViewModel.ToEntityWithRelatedEntities(_repositories);
			}

			// Business
			switch (entityTypeEnum)
			{
				case EntityTypeEnum.AudioFileOutput:
					_audioFileOutputManager.Delete(id);
					break;

				case EntityTypeEnum.Node:
					_curveManager.DeleteNode(id);
					break;

				case EntityTypeEnum.Operator:
					_patchManager.DeleteOperatorWithRelatedEntities(id);
					break;

				case EntityTypeEnum.Patch:
				{
					IResult result = _patchManager.DeletePatchWithRelatedEntities(id);
					result.Assert();
					break;
				}

				case EntityTypeEnum.Scale:
					_scaleManager.DeleteWithRelatedEntities(id);
					break;

				case EntityTypeEnum.Tone:
					_scaleManager.DeleteTone(id);
					break;

				case EntityTypeEnum.DocumentReference:
				{
					IResult result = _documentManager.DeleteDocumentReference(id);
					result.Assert();
					break;
				}

				default:
					throw new ValueNotSupportedException(entityTypeEnum);
			}

			// Refresh
			DocumentViewModelRefresh();
		}

		// TODO: Remove outcommented code.
		//private void AppendUndoHistory(UndoItemViewModelBase undoItemViewModel)
		//{
		//	MainViewModel.Document.UndoHistory.Push(undoItemViewModel);

		//	MainViewModel.Document.RedoFuture.Clear();
		//}

		private IList<ViewModelBase> GetAudioFileOutputStates(int id) => new List<ViewModelBase>
		{
			ViewModelSelector.GetAudioFileOutputPropertiesViewModel(MainViewModel.Document, id)
		};

		private IList<ViewModelBase> GetLibraryStates(int documentReferenceID) => new List<ViewModelBase> { ViewModelSelector.GetLibraryPropertiesViewModel(MainViewModel.Document, documentReferenceID) };

		private IList<ViewModelBase> GetNodeStates(int id)
		{
			NodePropertiesViewModel nodePropertiesViewModel = ViewModelSelector.GetNodePropertiesViewModel(MainViewModel.Document, id);
			CurveDetailsViewModel curveDetailsViewModel = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, nodePropertiesViewModel.CurveID);

			return new List<ViewModelBase>
			{
				nodePropertiesViewModel,
				curveDetailsViewModel,
			};
		}

		/// <summary>
		/// NOTE: OperatorViewModel cannot be used with DispatchViewModel, so cannot be part of the undo state,
		/// so instead PatchDetailsViewModel is part of the undo state.
		/// </summary>
		private IList<ViewModelBase> GetOperatorStates(int id)
		{
			OperatorPropertiesViewModelBase operatorPropertiesViewModel = ViewModelSelector.GetOperatorPropertiesViewModelPolymorphic(MainViewModel.Document, id);
			PatchDetailsViewModel patchDetailsViewModel = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, operatorPropertiesViewModel.PatchID);

			var states = new List<ViewModelBase> { patchDetailsViewModel, operatorPropertiesViewModel };

			if (operatorPropertiesViewModel is OperatorPropertiesViewModel_ForCurve castedViewModel)
			{
				CurveDetailsViewModel curveDetailsViewModel = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, castedViewModel.CurveID);
				states.Add(curveDetailsViewModel);
			}

			return states;
		}

		private IList<ViewModelBase> GetPatchStates(int id)
		{
			PatchPropertiesViewModel patchPropertiesViewModel = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, id);
			PatchDetailsViewModel patchDetailsViewModel = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, id);
			IList<ViewModelBase> states = ViewModelSelector.EnumerateAllOperatorPropertiesViewModels(MainViewModel.Document)
			                                               .Where(x => x.PatchID == id)
			                                               .Cast<ViewModelBase>()
			                                               .Union(patchDetailsViewModel)
			                                               .Union(patchPropertiesViewModel)
			                                               .ToArray();
			// Curve and Node view models
			Patch patch = _repositories.PatchRepository.Get(id);
			IEnumerable<int> curveIDs = patch.Operators.Where(x => x.Curve != null).Select(x => x.Curve.ID);
			foreach (int curveID in curveIDs)
			{
				CurveDetailsViewModel curveDetailsViewModel = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);
				states.Add(curveDetailsViewModel);

				IEnumerable<NodePropertiesViewModel> nodePropertiesViewModels = ViewModelSelector.GetNodePropertiesViewModelDictionary_ByCurveID(MainViewModel.Document, curveID).Values;
				states.AddRange(nodePropertiesViewModels);
			}

			return states;
		}

		private IList<ViewModelBase> GetScaleStates(int id) => new List<ViewModelBase>
		{
			ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, id),
			ViewModelSelector.GetScalePropertiesViewModel(MainViewModel.Document, id)
		};

		private IList<ViewModelBase> GetToneStates(int scaleID) => new List<ViewModelBase> { ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID) };
	}
}
