﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Canonical;
using JJ.Business.Synthesizer;
using JJ.Business.Synthesizer.Api;
using JJ.Business.Synthesizer.Calculation;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Data.Canonical;
using JJ.Data.Synthesizer;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.ToEntity;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.Validators;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using JJ.Presentation.Synthesizer.ViewModels.Partials;

namespace JJ.Presentation.Synthesizer.Presenters
{
    public partial class MainPresenter
    {
        // General Actions

        public void Show()
        {
            // Create ViewModel
            MainViewModel = ViewModelHelper.CreateEmptyMainViewModel();

            // Partial Actions
            MenuViewModel menuViewModel = _menuPresenter.Show(documentIsOpen: false);
            DocumentGridViewModel documentGridViewModel = _documentGridPresenter.Show(MainViewModel.DocumentGrid);
            string titleBar = _titleBarPresenter.Show();

            // DispatchViewModel
            MainViewModel.TitleBar = titleBar;
            DispatchViewModel(menuViewModel);
            DispatchViewModel(documentGridViewModel);
        }

        public void PopupMessagesOK()
        {
            MainViewModel.PopupMessages = new List<Message> { };
        }

        // AudioFileOutput

        public void AudioFileOutputGridShow()
        {
            // GetViewModel
            AudioFileOutputGridViewModel userInput = MainViewModel.Document.AudioFileOutputGrid;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _audioFileOutputGridPresenter.Show(userInput));
        }

        public void AudioFileOutputGridClose()
        {
            // GetViewModel
            AudioFileOutputGridViewModel userInput = MainViewModel.Document.AudioFileOutputGrid;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _audioFileOutputGridPresenter.Close(userInput));
        }

        public void AudioFileOutputCreate()
        {
            // GetViewModel
            AudioFileOutputGridViewModel userInput = MainViewModel.Document.AudioFileOutputGrid;

            // Template Method
            AudioFileOutput audioFileOutput = null;
            AudioFileOutputGridViewModel gridViewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Document document = _repositories.DocumentRepository.Get(MainViewModel.Document.ID);

                // Business
                audioFileOutput = _audioFileOutputManager.Create(document, mustGenerateName: true);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            if (gridViewModel.Successful)
            {
                // ToViewModel
                AudioFileOutputPropertiesViewModel propertiesViewModel = audioFileOutput.ToPropertiesViewModel();

                // DispatchViewModel
                DispatchViewModel(propertiesViewModel);

                // Refresh
                DocumentTreeRefresh();
                AudioFileOutputGridRefresh();
            }
        }

        public void AudioFileOutputDelete(int id)
        {
            // GetViewModel
            AudioFileOutputGridViewModel userInput = MainViewModel.Document.AudioFileOutputGrid;

            // Template Method
            AudioFileOutputGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // Business
                _audioFileOutputManager.Delete(id);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

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

        public void AudioFileOutputPropertiesShow(int id)
        {
            // GetViewModel
            AudioFileOutputPropertiesViewModel userInput = ViewModelSelector.GetAudioFileOutputPropertiesViewModel(MainViewModel.Document, id);

            // TemplateMethod
            AudioFileOutputPropertiesViewModel viewModel = TemplateActionMethod(userInput, () => _audioFileOutputPropertiesPresenter.Show(userInput));
        }

        public void AudioFileOutputPropertiesClose(int id)
        {
            AudioFileOutputPropertiesViewModel viewModel = AudioFileOutputPropertiesCloseOrLoseFocus(_audioFileOutputPropertiesPresenter.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleAudioFileOutputProperties?.Entity.ID == id)
                {
                    MainViewModel.Document.VisibleAudioFileOutputProperties = null;
                }
            }
        }

        public void AudioFileOutputPropertiesLoseFocus(int id)
        {
            AudioFileOutputPropertiesCloseOrLoseFocus(_audioFileOutputPropertiesPresenter.LoseFocus, id);
        }

        private AudioFileOutputPropertiesViewModel AudioFileOutputPropertiesCloseOrLoseFocus(Func<AudioFileOutputPropertiesViewModel, AudioFileOutputPropertiesViewModel> partialAction, int id)
        {
            // GetViewModel
            AudioFileOutputPropertiesViewModel userInput = ViewModelSelector.GetAudioFileOutputPropertiesViewModel(MainViewModel.Document, id);

            // TemplateMethod
            AudioFileOutputPropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                AudioFileOutputGridRefresh();
            }

            return viewModel;
        }

        // AudioOutput

        public void AudioOutputPropertiesShow()
        {
            // GetViewModel
            AudioOutputPropertiesViewModel userInput = MainViewModel.Document.AudioOutputProperties;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _audioOutputPropertiesPresenter.Show(userInput));
        }

        public void AudioOutputPropertiesClose()
        {
            AudioOutputPropertiesCloseOrLoseFocus(_audioOutputPropertiesPresenter.Close);
        }

        public void AudioOutputPropertiesLoseFocus()
        {
            AudioOutputPropertiesCloseOrLoseFocus(_audioOutputPropertiesPresenter.LoseFocus);
        }

        private void AudioOutputPropertiesCloseOrLoseFocus(Func<AudioOutputPropertiesViewModel, AudioOutputPropertiesViewModel> partialAction)
        {
            // GetViewModel
            AudioOutputPropertiesViewModel userInput = MainViewModel.Document.AudioOutputProperties;

            // TemplateMethod
            AudioOutputPropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));
        }

        // CurrentPatches

        public void CurrentPatchesShow()
        {
            // GetViewModel
            CurrentPatchesViewModel userInput = MainViewModel.Document.CurrentPatches;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _currentPatchesPresenter.Show(userInput));
        }

        public void CurrentPatchesClose()
        {
            // GetViewModel
            CurrentPatchesViewModel userInput = MainViewModel.Document.CurrentPatches;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _currentPatchesPresenter.Close(userInput));
        }

        public void CurrentPatchAdd(int patchID)
        {
            // GetViewModel
            CurrentPatchesViewModel userInput = MainViewModel.Document.CurrentPatches;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _currentPatchesPresenter.Add(userInput, patchID));
        }

        public void CurrentPatchRemove(int patchID)
        {
            // GetViewModel
            CurrentPatchesViewModel userInput = MainViewModel.Document.CurrentPatches;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _currentPatchesPresenter.Remove(userInput, patchID));
        }

        public void CurrentPatchMove(int patchID, int newPosition)
        {
            // GetViewModel
            CurrentPatchesViewModel userInput = MainViewModel.Document.CurrentPatches;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _currentPatchesPresenter.Move(userInput, patchID, newPosition));
        }

        public void CurrentPatchesShowAutoPatchPolyphonic()
        {
            // NOTE: Almost a copy of CurrentPatchesShowAutoPatch, except for the business method call.

            // GetViewModel
            CurrentPatchesViewModel currentPatchesViewModel = MainViewModel.Document.CurrentPatches;

            // RefreshCounter
            currentPatchesViewModel.RefreshCounter++;

            // Set !Successful
            currentPatchesViewModel.Successful = false;

            // ToEntity
            Document document = MainViewModel.ToEntityWithRelatedEntities(_repositories);

            // Get Entities
            IList<Patch> underlyingPatches = currentPatchesViewModel.ToEntities(_repositories.PatchRepository);

            // Business
            var patchManager = new PatchManager(_patchRepositories);
            patchManager.AutoPatchPolyphonic(underlyingPatches, 2);

            // Business
            IResult validationResult = _documentManager.Save(document);
            if (!validationResult.Successful)
            {
                // Non-Persisted
                currentPatchesViewModel.ValidationMessages.AddRange(validationResult.Messages);

                // DispatchViewModel
                DispatchViewModel(currentPatchesViewModel);

                return;
            }

            // ToViewModel
            PatchDetailsViewModel detailsViewModel = patchManager.Patch.ToDetailsViewModel(
                _repositories.SampleRepository,
                _repositories.CurveRepository,
                _repositories.PatchRepository,
                _entityPositionManager);

            // Non-Persisted
            detailsViewModel.Visible = true;

            // Successful
            currentPatchesViewModel.Successful = true;
            detailsViewModel.Successful = true;

            // DispatchViewModel
            DispatchAutoPatchDetailsViewModel(detailsViewModel);
        }

        public void CurrentPatchesShowAutoPatch()
        {
            // NOTE: Almost a copy of CurrentPatchesShowAutoPatchPolyphonic, except for the business method call.

            // GetViewModel
            CurrentPatchesViewModel currentPatchesViewModel = MainViewModel.Document.CurrentPatches;

            // RefreshCounter
            currentPatchesViewModel.RefreshCounter++;

            // Set !Successful
            currentPatchesViewModel.Successful = false;

            // ToEntity
            Document document = MainViewModel.ToEntityWithRelatedEntities(_repositories);

            // Get Entities
            IList<Patch> underlyingPatches = currentPatchesViewModel.ToEntities(_repositories.PatchRepository);

            // Business
            var patchManager = new PatchManager(_patchRepositories);
            patchManager.AutoPatch(underlyingPatches);

            // Business
            IResult validationResult = _documentManager.Save(document);
            if (!validationResult.Successful)
            {
                // Non-Persisted
                currentPatchesViewModel.ValidationMessages.AddRange(validationResult.Messages);

                // DispatchViewModel
                DispatchViewModel(currentPatchesViewModel);

                return;
            }

            // ToViewModel
            PatchDetailsViewModel detailsViewModel = patchManager.Patch.ToDetailsViewModel(
                _repositories.SampleRepository,
                _repositories.CurveRepository,
                _repositories.PatchRepository,
                _entityPositionManager);

            // Non-Persisted
            detailsViewModel.Visible = true;

            // Successful
            currentPatchesViewModel.Successful = true;
            detailsViewModel.Successful = true;

            // DispatchViewModel
            DispatchAutoPatchDetailsViewModel(detailsViewModel);
        }

        public void AutoPatchDetailsClose()
        {
            MainViewModel.Document.AutoPatchDetails.Visible = false;
        }

        // Curve

        public void CurveGridShow()
        {
            // GetViewModel
            CurveGridViewModel userInput = MainViewModel.Document.CurveGrid;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _curveGridPresenter.Show(userInput));
        }

        public void CurveGridClose()
        {
            // GetViewModel
            CurveGridViewModel userInput = MainViewModel.Document.CurveGrid;

            // TemplateMethod
            TemplateActionMethod(userInput, () => _curveGridPresenter.Close(userInput));
        }

        public void CurveCreate()
        {
            // GetViewModel
            CurveGridViewModel userInput = MainViewModel.Document.CurveGrid;

            // Template Method
            CurveGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Document document = _repositories.DocumentRepository.Get(MainViewModel.Document.ID);

                // Business
                Curve curve = _curveManager.Create(document, mustGenerateName: true);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void CurveDelete(int id)
        {
            // GetViewModel
            CurveGridViewModel userInput = MainViewModel.Document.CurveGrid;

            // Template Method
            CurveGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Curve curve = _repositories.CurveRepository.Get(id);

                // Business
                IResult result = _curveManager.DeleteWithRelatedEntities(curve);

                // Non-Persisted
                userInput.ValidationMessages.AddRange(result.Messages);

                // Successful?
                userInput.Successful = result.Successful;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void CurveDetailsShow(int id)
        {
            // GetViewModel
            CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, id);

            // TemplateMethod
            CurveDetailsViewModel viewModel = TemplateActionMethod(userInput, () => _curveDetailsPresenter.Show(userInput));
        }

        public void CurveDetailsClose(int id)
        {
            // GetViewModel
            CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, id);

            // TemplateMethod
            CurveDetailsViewModel viewModel = TemplateActionMethod(userInput, () => _curveDetailsPresenter.Close(userInput));

            if (viewModel.Successful)
            {
                // ToViewModel
                if (MainViewModel.Document.VisibleCurveDetails?.CurveID == id)
                {
                    MainViewModel.Document.VisibleCurveDetails = null;
                }
            }
        }

        public void CurveDetailsLoseFocus(int id)
        {
            // GetViewModel
            CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, id);

            // TemplateMethod
            TemplateActionMethod(userInput, () => _curveDetailsPresenter.LoseFocus(userInput));
        }

        public void CurvePropertiesShow(int id)
        {
            // GetViewModel
            CurvePropertiesViewModel userInput = ViewModelSelector.GetCurvePropertiesViewModel(MainViewModel.Document, id);

            // TemplateMethod
            TemplateActionMethod(userInput, () => _curvePropertiesPresenter.Show(userInput));
        }

        public void CurvePropertiesClose(int id)
        {
            CurvePropertiesViewModel viewModel = CurvePropertiesCloseOrLoseFocus(_curvePropertiesPresenter.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleCurveProperties?.ID == id)
                {
                    MainViewModel.Document.VisibleCurveProperties = null;
                }
            }
        }

        public void CurvePropertiesLoseFocus(int id)
        {
            CurvePropertiesCloseOrLoseFocus(_curvePropertiesPresenter.LoseFocus, id);
        }

        private CurvePropertiesViewModel CurvePropertiesCloseOrLoseFocus(Func<CurvePropertiesViewModel, CurvePropertiesViewModel> partialAction, int id)
        {
            // GetViewModel
            CurvePropertiesViewModel userInput = ViewModelSelector.GetCurvePropertiesViewModel(MainViewModel.Document, id);

            // TemplateMethod
            CurvePropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }

            return viewModel;
        }

        // Document Grid

        public void DocumentGridShow()
        {
            // GetViewModel
            DocumentGridViewModel userInput = MainViewModel.DocumentGrid;

            // Partial Action
            DocumentGridViewModel viewModel = _documentGridPresenter.Show(userInput);

            // DispatchViewModel
            DispatchViewModel(viewModel);
        }

        public void DocumentGridClose()
        {
            // GetViewModel
            DocumentGridViewModel userInput = MainViewModel.DocumentGrid;

            // Partial Action
            DocumentGridViewModel viewModel = _documentGridPresenter.Close(userInput);

            // DispatchViewModel
            DispatchViewModel(viewModel);
        }

        public void DocumentDetailsCreate()
        {
            // GetViewModel
            DocumentGridViewModel gridViewModel = MainViewModel.DocumentGrid;

            // RefreshCounter
            gridViewModel.RefreshCounter++;

            // Set !Successful
            gridViewModel.Successful = false;

            // Partial Action
            DocumentDetailsViewModel viewModel = _documentDetailsPresenter.Create();

            // Successful
            viewModel.Successful = true;

            // DispatchViewModel
            DispatchViewModel(viewModel);
        }

        public void DocumentDetailsClose()
        {
            // GetViewModel
            DocumentDetailsViewModel userInput = MainViewModel.DocumentDetails;

            // Partial Action
            DocumentDetailsViewModel viewModel = _documentDetailsPresenter.Close(userInput);

            // DispatchViewModel
            DispatchViewModel(viewModel);
        }

        public void DocumentDetailsSave()
        {
            // GetViewModel
            DocumentDetailsViewModel userInput = MainViewModel.DocumentDetails;

            // Partial Action
            DocumentDetailsViewModel viewModel = _documentDetailsPresenter.Save(userInput);

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

        public void DocumentDeleteShow(int id)
        {
            // GetViewModel
            DocumentGridViewModel gridViewModel = MainViewModel.DocumentGrid;

            // RefreshCounter
            gridViewModel.RefreshCounter++;

            // Set !Successful
            gridViewModel.Successful = false;

            // Partial Action
            object viewModel2 = _documentDeletePresenter.Show(id);

            // Successful
            gridViewModel.Successful = true;

            // DispatchViewModel
            DispatchViewModel(viewModel2);
        }

        public void DocumentCannotDeleteOK()
        {
            // GetViewModel
            DocumentCannotDeleteViewModel userInput = MainViewModel.DocumentCannotDelete;

            // Partial Action
            DocumentCannotDeleteViewModel viewModel = _documentCannotDeletePresenter.OK(userInput);

            // DispatchViewModel
            DispatchViewModel(viewModel);
        }

        public void DocumentDeleteConfirm(int id)
        {
            // GetViewModel
            DocumentDeleteViewModel viewModel = MainViewModel.DocumentDelete;

            // Partial Action
            object viewModel2 = _documentDeletePresenter.Confirm(viewModel);

            // RefreshCounter
            viewModel.RefreshCounter++;

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

        public void DocumentDeleteCancel()
        {
            // GetViewModel
            DocumentDeleteViewModel userInput = MainViewModel.DocumentDelete;

            // Partial Action
            DocumentDeleteViewModel viewModel = _documentDeletePresenter.Cancel(MainViewModel.DocumentDelete);

            // DispatchViewModel
            DispatchViewModel(viewModel);
        }

        public void DocumentDeletedOK()
        {
            // GetViewModel
            DocumentDeletedViewModel userInput = MainViewModel.DocumentDeleted;

            // Partial Action
            DocumentDeletedViewModel viewModel = _documentDeletedPresenter.OK(userInput);

            // DispatchViewModel
            DispatchViewModel(viewModel);
        }

        // Document

        public void DocumentOpen(int id)
        {
            // GetEntity
            Document document = _repositories.DocumentRepository.GetComplete(id);

            // Business
            var patchManager = new PatchManager(_patchRepositories);
            IList<Patch> grouplessPatches = patchManager.GetGrouplessPatches(document.Patches);
            IList<PatchGroupDto> patchGroupDtos = patchManager.GetPatchGroupDtos(document.Patches);
            IList<UsedInDto<Curve>> curveUsedInDtos = _documentManager.GetUsedIn(document.Curves);
            IList<UsedInDto<Sample>> sampleUsedInDtos = _documentManager.GetUsedIn(document.Samples);

            // TODO: Ugly code.
            IList<UsedInDto<Patch>> grouplessPatchUsedInDtos = _documentManager.GetUsedIn(grouplessPatches);
            IList<PatchGroupDto_WithUsedIn> patchGroupDtos_WithUsedIn = patchGroupDtos.Select(x => new PatchGroupDto_WithUsedIn
            {
                GroupName = x.GroupName,
                PatchUsedInDtos = _documentManager.GetUsedIn(x.Patches)
            }).ToArray();

            // ToViewModel
            DocumentViewModel viewModel = document.ToViewModel(
                grouplessPatchUsedInDtos,
                patchGroupDtos_WithUsedIn,
                curveUsedInDtos,
                sampleUsedInDtos,
                _repositories,
                _entityPositionManager);

            // Non-Persisted
            viewModel.DocumentTree.Visible = true;
            viewModel.IsOpen = true;

            // Set everything to successful.
            viewModel.AudioFileOutputGrid.Successful = true;
            viewModel.AudioFileOutputPropertiesDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.AudioOutputProperties.Successful = true;
            viewModel.AutoPatchDetails.Successful = true;
            viewModel.CurrentPatches.Successful = true;
            viewModel.CurveDetailsDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.CurveGrid.Successful = true;
            viewModel.CurvePropertiesDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.DocumentProperties.Successful = true;
            viewModel.DocumentTree.Successful = true;
            viewModel.NodePropertiesDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForBundles.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForCaches.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForCurves.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForCustomOperators.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForMakeContinuous.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForNumbers.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForPatchInlets.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForPatchOutlets.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_ForSamples.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_WithCollectionRecalculation.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_WithInterpolation.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_WithOutletCount.Values.ForEach(x => x.Successful = true);
            viewModel.OperatorPropertiesDictionary_WithInletCount.Values.ForEach(x => x.Successful = true);
            viewModel.PatchDetailsDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.PatchGridDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.PatchPropertiesDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.SampleGrid.Successful = true;
            viewModel.SamplePropertiesDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.SamplePropertiesDictionary.Values.Select(x => x.Successful = true);
            viewModel.ScaleGrid.Successful = true;
            viewModel.ScalePropertiesDictionary.Values.ForEach(x => x.Successful = true);
            viewModel.ToneGridEditDictionary.Values.ForEach(x => x.Successful = true);

            string titleBar = _titleBarPresenter.Show(document);
            MenuViewModel menuViewModel = _menuPresenter.Show(documentIsOpen: true);

            // DispatchViewModel
            MainViewModel.Document = viewModel;
            MainViewModel.TitleBar = titleBar;
            MainViewModel.Menu = menuViewModel;

            // Redirections
            MainViewModel.DocumentGrid.Visible = false;
            CurrentPatchesShow();
            if (document.Patches.Count == 1)
            {
                PatchDetailsShow(document.Patches[0].ID);
            }
        }

        public void DocumentSave()
        {
            // ToEntity
            Document document = MainViewModel.ToEntityWithRelatedEntities(_repositories);

            // Business
            IResult validationResult = _documentManager.Save(document);
            IResult warningsResult = _documentManager.GetWarningsRecursive(document);

            // Commit
            if (validationResult.Successful)
            {
                _repositories.Commit();
            }

            // ToViewModel
            MainViewModel.ValidationMessages = validationResult.Messages;
            MainViewModel.WarningMessages = warningsResult.Messages;
        }

        public void DocumentClose()
        {
            if (MainViewModel.Document.IsOpen)
            {
                // Partial Actions
                string titleBar = _titleBarPresenter.Show();
                MenuViewModel menuViewModel = _menuPresenter.Show(documentIsOpen: false);
                DocumentViewModel documentViewModel = ViewModelHelper.CreateEmptyDocumentViewModel();

                // DispatchViewModel
                MainViewModel.TitleBar = titleBar;
                MainViewModel.Menu = menuViewModel;
                MainViewModel.Document = documentViewModel;

                // Redirections
                DocumentGridShow();
            }
        }

        public void DocumentPropertiesShow()
        {
            // GetViewModel
            DocumentPropertiesViewModel userInput = MainViewModel.Document.DocumentProperties;

            // Template Method
            TemplateActionMethod(userInput, () => _documentPropertiesPresenter.Show(userInput));
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
            DocumentPropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                TitleBarRefresh();
                DocumentGridRefresh();
                DocumentTreeRefresh();
            }
        }

        public void DocumentTreeShow()
        {
            // GetViewModel
            DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

            // Template Method
            TemplateActionMethod(userInput, () => _documentTreePresenter.Show(userInput));
        }

        public void DocumentTreeClose()
        {
            // GetViewModel
            DocumentTreeViewModel userInput = MainViewModel.Document.DocumentTree;

            // Template Method
            TemplateActionMethod(userInput, () => _documentTreePresenter.Close(userInput));
        }

        // Node

        public void NodePropertiesShow(int id)
        {
            // GetViewModel
            NodePropertiesViewModel userInput = ViewModelSelector.GetNodePropertiesViewModel(MainViewModel.Document, id);

            // Template Method
            TemplateActionMethod(userInput, () => _nodePropertiesPresenter.Show(userInput));
        }

        public void NodePropertiesClose(int id)
        {
            NodePropertiesViewModel viewModel = NodePropertiesCloseOrLoseFocus(_nodePropertiesPresenter.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleNodeProperties?.Entity.ID == id)
                {
                    MainViewModel.Document.VisibleNodeProperties = null;
                }
            }
        }

        public void NodePropertiesLoseFocus(int id)
        {
            NodePropertiesCloseOrLoseFocus(_nodePropertiesPresenter.LoseFocus, id);
        }

        public NodePropertiesViewModel NodePropertiesCloseOrLoseFocus(Func<NodePropertiesViewModel, NodePropertiesViewModel> partialAction, int id)
        {
            // GetViewModel
            NodePropertiesViewModel userInput = ViewModelSelector.GetNodePropertiesViewModel(MainViewModel.Document, id);

            // TemplateMethod
            NodePropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                Node node = _repositories.NodeRepository.Get(id);
                CurveDetailsNodeRefresh(node.Curve.ID, id);
            }

            return viewModel;
        }

        public void NodeSelect(int curveID, int nodeID)
        {
            // GetViewModel
            CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

            // TemplateMethod
            TemplateActionMethod(userInput, () => _curveDetailsPresenter.SelectNode(userInput, nodeID));
        }

        public void NodeCreate(int curveID)
        {
            // GetViewModel
            CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

            // Template Method
            CurveDetailsViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Curve curve = _repositories.CurveRepository.Get(userInput.CurveID);

                // Business
                // TODO: This kind of stuff belongs in the business layer.
                Node afterNode;
                if (userInput.SelectedNodeID.HasValue)
                {
                    afterNode = _repositories.NodeRepository.Get(userInput.SelectedNodeID.Value);
                }
                else
                {
                    // Insert after last node if none selected.
                    afterNode = curve.Nodes.OrderBy(x => x.X).Last();
                }

                // Business
                Node node = _curveManager.CreateNode(curve, afterNode);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void NodeDeleteSelected(int curveID)
        {
            // GetViewModel
            CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

            // RefreshCounter
            userInput.RefreshCounter++;

            // Set !Successful
            userInput.Successful = false;

            // ViewModel Validation
            if (!userInput.SelectedNodeID.HasValue)
            {
                MainViewModel.ValidationMessages.Add(new Message
                {
                    PropertyKey = PresentationPropertyNames.SelectedNodeID,
                    Text = PresentationMessages.SelectANodeFirst
                });
                return;
            }

            // Template Method
            CurveDetailsViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                int nodeID = userInput.SelectedNodeID.Value;
                Node node = _repositories.NodeRepository.Get(nodeID);

                // Business
                IResult result = _curveManager.DeleteNode(node);

                // Non-Persisted
                userInput.ValidationMessages = result.Messages;

                // Successful?
                userInput.Successful = result.Successful;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void NodeMove(int curveID, int nodeID, double x, double y)
        {
            // Opted to not use the TemplateActionMethod,
            // because this is faster but less robust.
            // Because it is not nice when moving nodes is slow.
            // When you work in-memory backed with zipped XML,
            // you might use the more robust method again.
            // The overhead is mainly in the database queries.

            // GetViewModel
            CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

            // RefreshCounter
            userInput.RefreshCounter++;

            // Set !Successful
            userInput.Successful = false;

            // ToEntity
            userInput.ToEntityWithNodes(_curveRepositories);
            Node node = _repositories.NodeRepository.Get(nodeID);

            // Business
            node.X = x;
            node.Y = y;

            // Successful
            userInput.Successful = true;

            // Refresh
            CurveDetailsNodeRefresh(curveID, nodeID);
            NodePropertiesRefresh(nodeID);
        }

        /// <summary>
        /// Rotates between node types for the selected node.
        /// If no node is selected, nothing happens.
        /// </summary>
        public void NodeChangeSelectedNodeType(int curveID)
        {
            // GetViewModel
            CurveDetailsViewModel userInput = ViewModelSelector.GetCurveDetailsViewModel(MainViewModel.Document, curveID);

            // RefreshCounter
            userInput.RefreshCounter++;

            // Set !Successful
            userInput.Successful = false;

            // ViewModel Validation
            if (!userInput.SelectedNodeID.HasValue)
            {
                return;
            }
            int nodeID = userInput.SelectedNodeID.Value;

            // Template Method
            CurveDetailsViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Node node = _repositories.NodeRepository.Get(nodeID);

                // Business
                _curveManager.RotateNodeType(node);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                CurveDetailsNodeRefresh(curveID, nodeID);
                NodePropertiesRefresh(nodeID);
            }
        }

        // Operator

        public void OperatorPropertiesShow(int id)
        {
            // GetViewModel & Partial Action
            {
                OperatorPropertiesViewModel userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForBundle userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForBundle(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForBundle.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForCache userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForCache(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForCache.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForCurve userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForCurve(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForCurve.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForCustomOperator userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForCustomOperator(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForCustomOperator.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForMakeContinuous userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForMakeContinuous(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForMakeContinuous.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForNumber userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForNumber(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForNumber.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForPatchInlet userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForPatchInlet(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForPatchInlet.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForPatchOutlet userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForPatchOutlet(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForPatchOutlet.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_ForSample userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_ForSample(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_ForSample.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_WithInterpolation userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_WithInterpolation(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_WithInterpolation.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_WithCollectionRecalculation userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_WithCollectionRecalculation(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_WithCollectionRecalculation.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_WithOutletCount userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_WithOutletCount(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_WithOutletCount.Show(userInput));
                    return;
                }
            }
            {
                OperatorPropertiesViewModel_WithInletCount userInput = ViewModelSelector.TryGetOperatorPropertiesViewModel_WithInletCount(MainViewModel.Document, id);
                if (userInput != null)
                {
                    TemplateActionMethod(userInput, () => _operatorPropertiesPresenter_WithInletCount.Show(userInput));
                    return;
                }
            }

            throw new Exception(String.Format("Properties ViewModel not found for Operator with ID '{0}'.", id));
        }

        public void OperatorPropertiesClose(int id)
        {
            OperatorPropertiesViewModel viewModel = OperatorPropertiesCloseOrLoseFocus(_operatorPropertiesPresenter.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForBundle(int id)
        {
            OperatorPropertiesViewModel_ForBundle viewModel = OperatorPropertiesCloseOrLoseFocus_ForBundle(_operatorPropertiesPresenter_ForBundle.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForBundle?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForBundle = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForCache(int id)
        {
            OperatorPropertiesViewModel_ForCache viewModel = OperatorPropertiesCloseOrLoseFocus_ForCache(_operatorPropertiesPresenter_ForCache.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForCache?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForCache = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForCurve(int id)
        {
            OperatorPropertiesViewModel_ForCurve viewModel = OperatorPropertiesCloseOrLoseFocus_ForCurve(_operatorPropertiesPresenter_ForCurve.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForCurve?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForCurve = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForCustomOperator(int id)
        {
            OperatorPropertiesViewModel_ForCustomOperator viewModel = OperatorPropertiesCloseOrLoseFocus_ForCustomOperator(_operatorPropertiesPresenter_ForCustomOperator.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForCustomOperator?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForCustomOperator = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForMakeContinuous(int id)
        {
            OperatorPropertiesViewModel_ForMakeContinuous viewModel = OperatorPropertiesCloseOrLoseFocus_ForMakeContinuous(_operatorPropertiesPresenter_ForMakeContinuous.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForMakeContinuous?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForMakeContinuous = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForNumber(int id)
        {
            OperatorPropertiesViewModel_ForNumber viewModel = OperatorPropertiesCloseOrLoseFocus_ForNumber(_operatorPropertiesPresenter_ForNumber.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForNumber?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForNumber = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForPatchInlet(int id)
        {
            OperatorPropertiesViewModel_ForPatchInlet viewModel = OperatorPropertiesCloseOrLoseFocus_ForPatchInlet(_operatorPropertiesPresenter_ForPatchInlet.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForPatchInlet?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForPatchInlet = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForPatchOutlet(int id)
        {
            OperatorPropertiesViewModel_ForPatchOutlet viewModel = OperatorPropertiesCloseOrLoseFocus_ForPatchOutlet(_operatorPropertiesPresenter_ForPatchOutlet.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForPatchOutlet?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForPatchOutlet = null;
                }
            }
        }

        public void OperatorPropertiesClose_ForSample(int id)
        {
            OperatorPropertiesViewModel_ForSample viewModel = OperatorPropertiesCloseOrLoseFocus_ForSample(_operatorPropertiesPresenter_ForSample.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_ForSample?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_ForSample = null;
                }
            }
        }

        public void OperatorPropertiesClose_WithInterpolation(int id)
        {
            OperatorPropertiesViewModel_WithInterpolation viewModel = OperatorPropertiesCloseOrLoseFocus_WithInterpolation(_operatorPropertiesPresenter_WithInterpolation.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_WithInterpolation?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_WithInterpolation = null;
                }
            }
        }

        public void OperatorPropertiesClose_WithCollectionRecalculation(int id)
        {
            OperatorPropertiesViewModel_WithCollectionRecalculation viewModel = OperatorPropertiesCloseOrLoseFocus_WithCollectionRecalculation(_operatorPropertiesPresenter_WithCollectionRecalculation.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_WithCollectionRecalculation?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_WithCollectionRecalculation = null;
                }
            }
        }

        public void OperatorPropertiesClose_WithInletCount(int id)
        {
            OperatorPropertiesViewModel_WithInletCount viewModel = OperatorPropertiesCloseOrLoseFocus_WithInletCount(_operatorPropertiesPresenter_WithInletCount.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_WithInletCount?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_WithInletCount = null;
                }
            }
        }

        public void OperatorPropertiesClose_WithOutletCount(int id)
        {
            OperatorPropertiesViewModel_WithOutletCount viewModel = OperatorPropertiesCloseOrLoseFocus_WithOutletCount(_operatorPropertiesPresenter_WithOutletCount.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleOperatorProperties_WithOutletCount?.ID == id)
                {
                    MainViewModel.Document.VisibleOperatorProperties_WithOutletCount = null;
                }
            }
        }

        public void OperatorPropertiesLoseFocus(int id)
        {
            OperatorPropertiesCloseOrLoseFocus(_operatorPropertiesPresenter.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForBundle(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForBundle(_operatorPropertiesPresenter_ForBundle.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForCache(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForCache(_operatorPropertiesPresenter_ForCache.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForCurve(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForCurve(_operatorPropertiesPresenter_ForCurve.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForCustomOperator(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForCustomOperator(_operatorPropertiesPresenter_ForCustomOperator.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForMakeContinuous(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForMakeContinuous(_operatorPropertiesPresenter_ForMakeContinuous.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForNumber(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForNumber(_operatorPropertiesPresenter_ForNumber.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForPatchInlet(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForPatchInlet(_operatorPropertiesPresenter_ForPatchInlet.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForPatchOutlet(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForPatchOutlet(_operatorPropertiesPresenter_ForPatchOutlet.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_ForSample(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_ForSample(_operatorPropertiesPresenter_ForSample.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_WithInterpolation(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_WithInterpolation(_operatorPropertiesPresenter_WithInterpolation.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_WithCollectionRecalculation(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_WithCollectionRecalculation(_operatorPropertiesPresenter_WithCollectionRecalculation.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_WithOutletCount(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_WithOutletCount(_operatorPropertiesPresenter_WithOutletCount.LoseFocus, id);
        }

        public void OperatorPropertiesLoseFocus_WithInletCount(int id)
        {
            OperatorPropertiesCloseOrLoseFocus_WithInletCount(_operatorPropertiesPresenter_WithInletCount.LoseFocus, id);
        }

        private OperatorPropertiesViewModel OperatorPropertiesCloseOrLoseFocus(Func<OperatorPropertiesViewModel, OperatorPropertiesViewModel> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel userInput = ViewModelSelector.GetOperatorPropertiesViewModel(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForBundle OperatorPropertiesCloseOrLoseFocus_ForBundle(Func<OperatorPropertiesViewModel_ForBundle, OperatorPropertiesViewModel_ForBundle> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForBundle userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForBundle(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForBundle viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForCache OperatorPropertiesCloseOrLoseFocus_ForCache(Func<OperatorPropertiesViewModel_ForCache, OperatorPropertiesViewModel_ForCache> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForCache userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForCache(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForCache viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForCurve OperatorPropertiesCloseOrLoseFocus_ForCurve(Func<OperatorPropertiesViewModel_ForCurve, OperatorPropertiesViewModel_ForCurve> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForCurve userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForCurve(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForCurve viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForCustomOperator OperatorPropertiesCloseOrLoseFocus_ForCustomOperator(Func<OperatorPropertiesViewModel_ForCustomOperator, OperatorPropertiesViewModel_ForCustomOperator> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForCustomOperator userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForCustomOperator(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForCustomOperator viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForMakeContinuous OperatorPropertiesCloseOrLoseFocus_ForMakeContinuous(Func<OperatorPropertiesViewModel_ForMakeContinuous, OperatorPropertiesViewModel_ForMakeContinuous> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForMakeContinuous userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForMakeContinuous(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForMakeContinuous viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForNumber OperatorPropertiesCloseOrLoseFocus_ForNumber(Func<OperatorPropertiesViewModel_ForNumber, OperatorPropertiesViewModel_ForNumber> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForNumber userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForNumber(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForNumber viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForPatchInlet OperatorPropertiesCloseOrLoseFocus_ForPatchInlet(Func<OperatorPropertiesViewModel_ForPatchInlet, OperatorPropertiesViewModel_ForPatchInlet> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForPatchInlet userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForPatchInlet(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForPatchInlet viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
                OperatorViewModels_OfType_Refresh(OperatorTypeEnum.CustomOperator); // Refresh Dependencies
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForPatchOutlet OperatorPropertiesCloseOrLoseFocus_ForPatchOutlet(Func<OperatorPropertiesViewModel_ForPatchOutlet, OperatorPropertiesViewModel_ForPatchOutlet> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForPatchOutlet userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForPatchOutlet(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForPatchOutlet viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
                OperatorViewModels_OfType_Refresh(OperatorTypeEnum.CustomOperator); // Refresh Dependent Things
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_ForSample OperatorPropertiesCloseOrLoseFocus_ForSample(Func<OperatorPropertiesViewModel_ForSample, OperatorPropertiesViewModel_ForSample> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_ForSample userInput = ViewModelSelector.GetOperatorPropertiesViewModel_ForSample(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_ForSample viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_WithInterpolation OperatorPropertiesCloseOrLoseFocus_WithInterpolation(Func<OperatorPropertiesViewModel_WithInterpolation, OperatorPropertiesViewModel_WithInterpolation> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_WithInterpolation userInput = ViewModelSelector.GetOperatorPropertiesViewModel_WithInterpolation(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_WithInterpolation viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_WithCollectionRecalculation OperatorPropertiesCloseOrLoseFocus_WithCollectionRecalculation(Func<OperatorPropertiesViewModel_WithCollectionRecalculation, OperatorPropertiesViewModel_WithCollectionRecalculation> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_WithCollectionRecalculation userInput = ViewModelSelector.GetOperatorPropertiesViewModel_WithCollectionRecalculation(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_WithCollectionRecalculation viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_WithOutletCount OperatorPropertiesCloseOrLoseFocus_WithOutletCount(Func<OperatorPropertiesViewModel_WithOutletCount, OperatorPropertiesViewModel_WithOutletCount> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_WithOutletCount userInput = ViewModelSelector.GetOperatorPropertiesViewModel_WithOutletCount(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_WithOutletCount viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        private OperatorPropertiesViewModel_WithInletCount OperatorPropertiesCloseOrLoseFocus_WithInletCount(Func<OperatorPropertiesViewModel_WithInletCount, OperatorPropertiesViewModel_WithInletCount> partialAction, int id)
        {
            // GetViewModel
            OperatorPropertiesViewModel_WithInletCount userInput = ViewModelSelector.GetOperatorPropertiesViewModel_WithInletCount(MainViewModel.Document, id);

            // TemplateMethod
            OperatorPropertiesViewModel_WithInletCount viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                PatchDetails_RefreshOperator(userInput.ID);
            }

            return viewModel;
        }

        public void OperatorCreate(int patchID, int operatorTypeID)
        {
            // GetViewModel
            PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

            // TemplateMethod
            PatchDetailsViewModel viewModel = TemplateActionMethod(userInput, () => _patchDetailsPresenter.CreateOperator(userInput, operatorTypeID));

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void OperatorDeleteSelected(int patchID)
        {
            // GetViewModel
            PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

            // Template Method
            PatchDetailsViewModel viewModel = TemplateActionMethod(userInput, () => _patchDetailsPresenter.DeleteOperator(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void OperatorMove(int patchID, int operatorID, float centerX, float centerY)
        {
            // GetViewModel
            PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

            // TemplateMethod
            TemplateActionMethod(userInput, () => _patchDetailsPresenter.MoveOperator(userInput, operatorID, centerX, centerY));
        }

        public void OperatorChangeInputOutlet(int patchID, int inletID, int inputOutletID)
        {
            // GetViewModel
            PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

            // TemplateMethod
            TemplateActionMethod(userInput, () => _patchDetailsPresenter.ChangeInputOutlet(userInput, inletID, inputOutletID));
        }

        public void OperatorSelect(int patchID, int operatorID)
        {
            // GetViewModel
            PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, patchID);

            // Partial Action
            TemplateActionMethod(userInput, () => _patchDetailsPresenter.SelectOperator(userInput, operatorID));
        }

        // Patch

        public void PatchDetailsShow(int id)
        {
            // GetViewModel
            PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, id);

            // TemplateMethod
            TemplateActionMethod(userInput, () => _patchDetailsPresenter.Show(userInput));
        }

        public void PatchDetailsClose(int id)
        {
            PatchDetailsViewModel viewModel = PatchDetailsCloseOrLoseFocus(_patchDetailsPresenter.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisiblePatchDetails?.Entity.ID == id)
                {
                    MainViewModel.Document.VisiblePatchDetails = null;
                }
            }
        }

        public void PatchDetailsLoseFocus(int id)
        {
            PatchDetailsCloseOrLoseFocus(_patchDetailsPresenter.LoseFocus, id);
        }

        private PatchDetailsViewModel PatchDetailsCloseOrLoseFocus(Func<PatchDetailsViewModel, PatchDetailsViewModel> partialAction, int id)
        {
            // GetViewModel
            PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, id);

            // TemplateMethod
            return TemplateActionMethod(userInput, () => partialAction(userInput));
        }

        public void PatchGridShow(string group)
        {
            // GetViewModel
            PatchGridViewModel userInput = ViewModelSelector.GetPatchGridViewModel_ByGroup(MainViewModel.Document, group);

            // Template Method
            TemplateActionMethod(userInput, () => _patchGridPresenter.Show(userInput));
        }

        public void PatchGridClose(string group)
        {
            // GetViewModel
            PatchGridViewModel userInput = ViewModelSelector.GetPatchGridViewModel_ByGroup(MainViewModel.Document, group);

            // Template Method
            PatchGridViewModel viewModel = TemplateActionMethod(userInput, () => _patchGridPresenter.Close(userInput));

            if (viewModel.Successful)
            {
                if (String.Equals(MainViewModel.Document.VisiblePatchGrid?.Group?.ToLower() ?? "", group?.ToLower() ?? ""))
                {
                    MainViewModel.Document.VisiblePatchGrid = null;
                }
            }
        }

        /// <summary> Returns output file path if ViewModel.Successful. <summary>
        public string PatchPlay(int id)
        {
            // GetViewModel
            PatchDetailsViewModel userInput = ViewModelSelector.GetPatchDetailsViewModel(MainViewModel.Document, id);

            // TemplateMethod
            string outputFilePath = null;
            TemplateActionMethod(userInput, () =>
            {
                outputFilePath = _patchDetailsPresenter.Play(userInput, _repositories);
                return userInput;
            });

            // Non-Persisted
            // Move messages to popup messages, because the default
            // dispatching for PatchDetailsViewModel moves it to the ValidationMessages.
            MainViewModel.PopupMessages.AddRange(userInput.ValidationMessages);
            userInput.ValidationMessages.Clear();

            return outputFilePath;
        }

        public void PatchPropertiesShow(int id)
        {
            // GetViewModel
            PatchPropertiesViewModel userInput = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, id);

            // Template Method
            TemplateActionMethod(userInput, () => _patchPropertiesPresenter.Show(userInput));
        }

        public void PatchPropertiesClose(int id)
        {
            PatchPropertiesViewModel viewModel = PatchPropertiesCloseOrLoseFocus(_patchPropertiesPresenter.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisiblePatchProperties?.ID == id)
                {
                    MainViewModel.Document.VisiblePatchProperties = null;
                }
            }

        }

        public void PatchPropertiesLoseFocus(int id)
        {
            PatchPropertiesCloseOrLoseFocus(_patchPropertiesPresenter.LoseFocus, id);
        }

        private PatchPropertiesViewModel PatchPropertiesCloseOrLoseFocus(Func<PatchPropertiesViewModel, PatchPropertiesViewModel> partialAction, int patchID)
        {
            // GetViewModel
            PatchPropertiesViewModel userInput = ViewModelSelector.GetPatchPropertiesViewModel(MainViewModel.Document, patchID);

            // Template Method
            PatchPropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }

            return viewModel;
        }

        /// <param name="group">nullable</param>
        public void PatchCreate(string group)
        {
            // GetViewModel
            PatchGridViewModel userInput = ViewModelSelector.GetPatchGridViewModel_ByGroup(MainViewModel.Document, group);

            // Template Method
            PatchGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Document document = _repositories.DocumentRepository.Get(MainViewModel.Document.ID);

                // Business
                var patchManager = new PatchManager(_patchRepositories);
                patchManager.CreatePatch(document, mustGenerateName: true);
                Patch patch = patchManager.Patch;
                patch.GroupName = group;

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void PatchDelete(string group, int patchID)
        {
            // GetViewModel
            PatchGridViewModel userInput = ViewModelSelector.GetPatchGridViewModel(MainViewModel.Document, group);

            // Template Method
            PatchGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Patch patch = _repositories.PatchRepository.Get(patchID);

                // Businesss
                var patchManager = new PatchManager(_patchRepositories);
                patchManager.PatchID = patchID;
                IResult result = patchManager.DeletePatchWithRelatedEntities();

                // Non-Persisted
                userInput.ValidationMessages.AddRange(result.Messages);

                // Successful?
                userInput.Successful = result.Successful;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        // Sample

        public void SampleGridShow()
        {
            // GetViewModel
            SampleGridViewModel userInput = MainViewModel.Document.SampleGrid;

            // Template Method
            TemplateActionMethod(userInput, () => _sampleGridPresenter.Show(userInput));
        }

        public void SampleGridClose()
        {
            // GetViewModel
            SampleGridViewModel userInput = MainViewModel.Document.SampleGrid;

            // Template Method
            TemplateActionMethod(userInput, () => _sampleGridPresenter.Close(userInput));
        }

        public void SampleCreate()
        {
            // GetViewModel
            SampleGridViewModel userInput = MainViewModel.Document.SampleGrid;

            // Template Method
            SampleGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Document document = _repositories.DocumentRepository.Get(MainViewModel.Document.ID);

                // Business
                Sample sample = _sampleManager.CreateSample(document, mustGenerateName: true);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void SampleDelete(int sampleID)
        {
            // GetViewModel
            SampleGridViewModel userInput = MainViewModel.Document.SampleGrid;

            // Template Method
            SampleGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // GetEntity
                Sample sample = _repositories.SampleRepository.Get(sampleID);

                // Business
                IResult result = _sampleManager.Delete(sample);

                // Non-Persisted
                userInput.ValidationMessages = result.Messages;

                // Successful?
                userInput.Successful = result.Successful;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void SamplePropertiesShow(int id)
        {
            // GetViewModel
            SamplePropertiesViewModel userInput = ViewModelSelector.GetSamplePropertiesViewModel(MainViewModel.Document, id);

            // Template Method
            TemplateActionMethod(userInput, () => _samplePropertiesPresenter.Show(userInput));
        }

        public void SamplePropertiesClose(int id)
        {
            SamplePropertiesViewModel viewModel = SamplePropertiesCloseOrLoseFocus(_samplePropertiesPresenter.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleSampleProperties?.Entity.ID == id)
                {
                    MainViewModel.Document.VisibleSampleProperties = null;
                }
            }
        }

        public void SamplePropertiesLoseFocus(int id)
        {
            SamplePropertiesCloseOrLoseFocus(_samplePropertiesPresenter.LoseFocus, id);
        }

        private SamplePropertiesViewModel SamplePropertiesCloseOrLoseFocus(Func<SamplePropertiesViewModel, SamplePropertiesViewModel> partialAction, int sampleID)
        {
            // GetViewModel
            SamplePropertiesViewModel userInput = ViewModelSelector.GetSamplePropertiesViewModel(MainViewModel.Document, sampleID);

            // TemplateMethod
            SamplePropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                Sample sample = _repositories.SampleRepository.Get(sampleID);
                SampleGridRefresh();
                SampleLookupRefresh();
                OperatorViewModels_OfType_Refresh(OperatorTypeEnum.Sample);
            }

            return viewModel;
        }

        // Scale

        public void ScaleGridShow()
        {
            // GetViewModel
            ScaleGridViewModel userInput = MainViewModel.Document.ScaleGrid;

            // Template Method
            TemplateActionMethod(userInput, () => _scaleGridPresenter.Show(userInput));
        }

        public void ScaleGridClose()
        {
            // GetViewModel
            ScaleGridViewModel userInput = MainViewModel.Document.ScaleGrid;

            // Template Method
            TemplateActionMethod(userInput, () => _scaleGridPresenter.Close(userInput));
        }

        public void ScaleCreate()
        {
            // GetViewModel
            ScaleGridViewModel userInput = MainViewModel.Document.ScaleGrid;

            // Template Method
            ScaleGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Document document = _repositories.DocumentRepository.Get(userInput.DocumentID);

                // Business
                Scale scale = _scaleManager.Create(document, mustSetDefaults: true, mustGenerateName: true);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            // Refresh
            if (viewModel.Successful)
            {
                DocumentViewModelRefresh();
            }
        }

        public void ScaleDelete(int id)
        {
            // GetViewModel
            ScaleGridViewModel userInput = MainViewModel.Document.ScaleGrid;

            // Template Method
            ScaleGridViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Scale scale = _repositories.ScaleRepository.Get(id);

                // Business
                _scaleManager.DeleteWithRelatedEntities(id);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            if (viewModel.Successful)
            {
                // ToViewModel
                MainViewModel.Document.ScaleGrid.Dictionary.Remove(id);
                MainViewModel.Document.ToneGridEditDictionary.Remove(id);
                MainViewModel.Document.ScalePropertiesDictionary.Remove(id);
                if (MainViewModel.Document.VisibleScaleProperties?.Entity.ID == id)
                {
                    MainViewModel.Document.VisibleScaleProperties = null;
                }
                if (MainViewModel.Document.VisibleToneGridEdit?.ScaleID == id)
                {
                    MainViewModel.Document.VisibleToneGridEdit = null;
                }

                // Refresh
                DocumentTreeRefresh();
            }
        }

        public void ScaleShow(int id)
        {
            // GetViewModel
            ScalePropertiesViewModel userInput1 = ViewModelSelector.GetScalePropertiesViewModel(MainViewModel.Document, id);
            ToneGridEditViewModel userInput2 = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID: id);

            // RefreshCounter
            userInput1.RefreshCounter++;
            userInput2.RefreshCounter++;

            // Set !Successful
            userInput1.Successful = false;
            userInput2.Successful = false;

            // ToEntity
            Document document = MainViewModel.ToEntityWithRelatedEntities(_repositories);

            // Partial Actions
            ScalePropertiesViewModel scalePropertiesViewModel = _scalePropertiesPresenter.Show(userInput1);
            if (!scalePropertiesViewModel.Successful)
            {
                DispatchViewModel(scalePropertiesViewModel);
                return;
            }
            scalePropertiesViewModel.Successful = false;

            ToneGridEditViewModel toneGridEditViewModel = _toneGridEditPresenter.Show(userInput2);
            if (!toneGridEditViewModel.Successful)
            {
                DispatchViewModel(toneGridEditViewModel);
                return;
            }
            toneGridEditViewModel.Successful = false;

            // Business
            IResult validationResult = _documentManager.Save(document);
            if (!validationResult.Successful)
            {
                scalePropertiesViewModel.ValidationMessages.AddRange(validationResult.Messages);
                DispatchViewModel(scalePropertiesViewModel);
                DispatchViewModel(toneGridEditViewModel);
                return;
            }

            // Set Successful
            scalePropertiesViewModel.Successful = true;
            toneGridEditViewModel.Successful = true;

            // DispatchViewModel
            DispatchViewModel(scalePropertiesViewModel);
            DispatchViewModel(toneGridEditViewModel);
        }

        public void ScalePropertiesClose(int id)
        {
            ScalePropertiesViewModel viewModel = ScalePropertiesCloseOrLoseFocus(_scalePropertiesPresenter.Close, id);

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleScaleProperties?.Entity.ID == id)
                {
                    MainViewModel.Document.VisibleScaleProperties = null;
                }
            }
        }

        public void ScalePropertiesLoseFocus(int id)
        {
            ScalePropertiesCloseOrLoseFocus(_scalePropertiesPresenter.LoseFocus, id);
        }

        private ScalePropertiesViewModel ScalePropertiesCloseOrLoseFocus(Func<ScalePropertiesViewModel, ScalePropertiesViewModel> partialAction, int id)
        {
            // Get ViewModel
            ScalePropertiesViewModel userInput = ViewModelSelector.GetScalePropertiesViewModel(MainViewModel.Document, id);

            // Template Method
            ScalePropertiesViewModel viewModel = TemplateActionMethod(userInput, () => partialAction(userInput));

            // Refresh
            if (viewModel.Successful)
            {
                ToneGridEditRefresh(userInput.Entity.ID);
                ScaleGridRefresh();
            }

            return viewModel;
        }

        // Tone

        public void ToneCreate(int scaleID)
        {
            // GetViewModel
            ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

            // RefreshCounter
            userInput.RefreshCounter++;

            // Set !Successful
            userInput.Successful = false;

            // ViewModelValidator
            IValidator viewModelValidator = new ToneGridEditViewModelValidator(userInput);
            if (!viewModelValidator.IsValid)
            {
                userInput.ValidationMessages = viewModelValidator.ValidationMessages.ToCanonical();
                DispatchViewModel(userInput);
                return;
            }

            // TemplateMethod
            Tone tone = null;
            ToneGridEditViewModel viewModel = TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // Get Entity
                Scale scale = _repositories.ScaleRepository.Get(scaleID);

                // Business
                tone = _scaleManager.CreateTone(scale);

                // Successful
                userInput.Successful = true;

                return userInput;
            });

            if (viewModel.Successful)
            {
                // ToViewModel
                ToneViewModel toneViewModel = tone.ToViewModel();
                userInput.Tones.Add(toneViewModel);
                // Do not sort grid, so that the new item appears at the bottom.
            }
        }

        public void ToneDelete(int scaleID, int toneID)
        {
            // GetViewModel
            ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

            // RefreshCounter
            userInput.RefreshCounter++;

            // Set !Successful
            userInput.Successful = false;

            // ViewModelValidator
            IValidator viewModelValidator = new ToneGridEditViewModelValidator(userInput);
            if (!viewModelValidator.IsValid)
            {
                userInput.ValidationMessages = viewModelValidator.ValidationMessages.ToCanonical();
                DispatchViewModel(userInput);
                return;
            }

            // Template Method
            TemplateActionMethod(userInput, () =>
            {
                // RefreshCounter
                userInput.RefreshCounter++;

                // Set !Successful
                userInput.Successful = false;

                // GetEntity
                Tone tone = _repositories.ToneRepository.Get(toneID);
                Scale scale = tone.Scale;

                // Business
                _scaleManager.DeleteTone(tone);

                // ToViewModel
                ToneGridEditViewModel viewModel = scale.ToToneGridEditViewModel();

                // Non-Persisted
                viewModel.Visible = userInput.Visible;

                // Successful
                viewModel.Successful = true;

                return viewModel;
            });
        }

        public void ToneGridEditClose(int scaleID)
        {
            // GetViewModel
            ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

            // Template Method
            ToneGridEditViewModel viewModel = TemplateActionMethod(userInput, () => _toneGridEditPresenter.Close(userInput));

            if (viewModel.Successful)
            {
                if (MainViewModel.Document.VisibleToneGridEdit?.ScaleID == scaleID)
                {
                    MainViewModel.Document.VisibleToneGridEdit = null;
                }
            }
        }

        public void ToneGridEditLoseFocus(int scaleID)
        {
            // GetViewModel
            ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

            // Template Method
            TemplateActionMethod(userInput, () => _toneGridEditPresenter.LoseFocus(userInput));
        }

        public void ToneGridEditEdit(int scaleID)
        {
            // GetViewModel
            ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

            // Template Method
            TemplateActionMethod(userInput, () => _toneGridEditPresenter.Edit(userInput));
        }

        /// <summary>
        /// Writes a sine sound with the pitch of the tone to an audio file with a configurable duration.
        /// Returns the output file path if successful.
        /// TODO: This action is too dependent on infrastructure, because the AudioFileOutput business logic is.
        /// Instead of writing to a file it had better write to a stream.
        /// </summary>
        public string TonePlay(int scaleID, int toneID)
        {
            // NOTE:
            // Cannot use partial presenter, because this action uses both
            // ToneGridEditViewModel and CurrentPatches view model.

            // GetEntity
            ToneGridEditViewModel userInput = ViewModelSelector.GetToneGridEditViewModel(MainViewModel.Document, scaleID);

            // RefreshCounter
            userInput.RefreshCounter++;

            // Set !Successful
            userInput.Successful = false;

            // ViewModel Validator
            IValidator viewModelValidator = new ToneGridEditViewModelValidator(userInput);
            if (!viewModelValidator.IsValid)
            {
                userInput.ValidationMessages = viewModelValidator.ValidationMessages.ToCanonical();
                DispatchViewModel(userInput);
                return null;
            }

            // ToEntity
            Document document = MainViewModel.ToEntityWithRelatedEntities(_repositories);
            Tone tone = _repositories.ToneRepository.Get(toneID);
            AudioOutput audioOutput = document.AudioOutput;

            var underlyingPatches = new List<Patch>(MainViewModel.Document.CurrentPatches.List.Count);
            foreach (IDAndName itemViewModel in MainViewModel.Document.CurrentPatches.List)
            {
                Patch underlyingPatch = _repositories.PatchRepository.Get(itemViewModel.ID);
                underlyingPatches.Add(underlyingPatch);
            }

            // Business
            var patchManager = new PatchManager(_patchRepositories);

            Outlet outlet = null;
            if (underlyingPatches.Count != 0)
            {
                outlet = patchManager.TryAutoPatch_WithTone(tone, underlyingPatches);
            }

            if (outlet == null) // Fallback to Sine
            {
                var x = new PatchApi();
                double frequency = tone.GetFrequency();
                outlet = x.Sine(x.PatchInlet(DimensionEnum.Frequency, frequency));
            }

            IResult validationResult = _documentManager.Save(document);
            if (!validationResult.Successful)
            {
                userInput.Successful = false;
                userInput.ValidationMessages = validationResult.Messages;
                return null;
            }

            patchManager.Patch = outlet.Operator.Patch;
            IPatchCalculator patchCalculator = patchManager.CreateCalculator(
                outlet,
                audioOutput.SamplingRate,
                audioOutput.GetChannelCount(), 
                DEFAULT_CHANNEL_INDEX, 
                new CalculatorCache());

            // Infrastructure
            AudioFileOutput audioFileOutput = _audioFileOutputManager.Create();
            audioFileOutput.LinkTo(audioOutput.SpeakerSetup);
            audioFileOutput.SamplingRate = audioOutput.SamplingRate;
            audioFileOutput.FilePath = _playOutputFilePath;
            audioFileOutput.Duration = DEFAULT_DURATION;
            audioFileOutput.LinkTo(outlet);
            _audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);

            // Successful
            userInput.Successful = true;

            return _playOutputFilePath;
        }

        // Helpers

        /// <summary>
        /// A template method for a MainPresenter action method.
        /// Works for most actions. Less suitable for specialized cases.
        /// In particular the ones that are not about the open document.
        ///
        /// Executes a sub-presenter's action and surrounds it with
        /// converting the full document view model to entity,
        /// doing a full document validation and
        /// managing view model transactionality.
        /// All you need to do is provide the right sub-viewmodel,
        /// provide a delegate to the sub-presenter's action method
        /// and possibly do some refreshing of other view models afterwards.
        /// </summary>
        private TViewModel TemplateActionMethod<TViewModel>(
            TViewModel userInput,
            Func<TViewModel> partialAction)
            where TViewModel : ViewModelBase
        {
            if (userInput == null) throw new NullException(() => userInput);

            // Set !Successful
            userInput.Successful = false;

            // ToEntity
            Document document = MainViewModel.ToEntityWithRelatedEntities(_repositories);

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

            // Business
            IResult validationResult = _documentManager.Save(document);
            if (!validationResult.Successful)
            {
                // Non-Persisted
                viewModel.ValidationMessages.AddRange(validationResult.Messages);

                // DispatchViewModel
                DispatchViewModel(viewModel);
                return viewModel;
            }

            // Successful
            viewModel.Successful = true;

            // DispatchViewModel
            DispatchViewModel(viewModel);

            return viewModel;
        }
    }
}
