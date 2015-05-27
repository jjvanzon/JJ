﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Managers;
using JJ.Business.Synthesizer.Validation;
using JJ.Business.Synthesizer.Warnings;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Presentation.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.ToEntity;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.Extensions;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.Enums;
using JJ.Framework.Common;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using JJ.Business.Synthesizer.Resources;
using JJ.Business.CanonicalModel;

namespace JJ.Presentation.Synthesizer.Presenters
{
    /// <summary>
    /// Panels in the application are so intricately coordinated
    /// that one action in one part of the screen can
    /// affect several other panels on the screen.
    /// So you cannot manage each part of the screen individually.
    /// 
    /// That is why all panels are managed in a single presenter and view model.
    /// Otherwise you would get difficult coordination of application navigation 
    /// in the platform-specific application code, where it does not belong.
    /// 
    /// Also: most non-visible parts of the view model must be kept alive inside the view model,
    /// because the whole document will not be persistent until you hit save,
    /// and until that time, all the data must be kept inside the view model.
    /// </summary>
    public class MainPresenter
    {
        private RepositoryWrapper _repositoryWrapper;

        private AudioFileOutputListPresenter _audioFileOutputListPresenter;
        private AudioFileOutputPropertiesPresenter _audioFileOutputPropertiesPresenter;
        private CurveDetailsPresenter _curveDetailsPresenter;
        private CurveListPresenter _curveListPresenter;
        private DocumentCannotDeletePresenter _documentCannotDeletePresenter;
        private DocumentDeletedPresenter _documentDeletedPresenter;
        private DocumentDeletePresenter _documentDeletePresenter;
        private DocumentDetailsPresenter _documentDetailsPresenter;
        private DocumentListPresenter _documentListPresenter;
        private DocumentPropertiesPresenter _documentPropertiesPresenter;
        private DocumentTreePresenter _documentTreePresenter;
        private EffectListPresenter _effectListPresenter;
        private InstrumentListPresenter _instrumentListPresenter;
        private MenuPresenter _menuPresenter;
        private NotFoundPresenter _notFoundPresenter;
        private PatchDetailsPresenter _patchDetailsPresenter;
        private PatchListPresenter _patchListPresenter;
        private SampleListPresenter _sampleListPresenter;
        private SamplePropertiesPresenter _samplePropertiesPresenter;

        private MainViewModel _viewModel;

        private EntityPositionManager _entityPositionManager;
        private DocumentManager _documentManager;
        private PatchManager _patchManager;
        private CurveManager _curveManager;
        private SampleManager _sampleManager;
        private AudioFileOutputManager _audioFileOutputManager;

        public MainPresenter(RepositoryWrapper repositoryWrapper)
        {
            if (repositoryWrapper == null) throw new NullException(() => repositoryWrapper);

            _repositoryWrapper = repositoryWrapper;

            _audioFileOutputListPresenter = new AudioFileOutputListPresenter(_repositoryWrapper.DocumentRepository);
            _audioFileOutputPropertiesPresenter = CreateAudioFileOutputPropertiesPresenter();
            _curveDetailsPresenter = new CurveDetailsPresenter(_repositoryWrapper.CurveRepository, _repositoryWrapper.NodeTypeRepository);
            _curveListPresenter = new CurveListPresenter(_repositoryWrapper.DocumentRepository);
            _documentCannotDeletePresenter = new DocumentCannotDeletePresenter(_repositoryWrapper.DocumentRepository);
            _documentDeletedPresenter = new DocumentDeletedPresenter();
            _documentDeletePresenter = new DocumentDeletePresenter(_repositoryWrapper);
            _documentDetailsPresenter = new DocumentDetailsPresenter(_repositoryWrapper.DocumentRepository);
            _documentListPresenter = new DocumentListPresenter(_repositoryWrapper.DocumentRepository);
            _documentPropertiesPresenter = new DocumentPropertiesPresenter(_repositoryWrapper.DocumentRepository);
            _documentTreePresenter = new DocumentTreePresenter(_repositoryWrapper.DocumentRepository);
            _effectListPresenter = new EffectListPresenter(_repositoryWrapper);
            _instrumentListPresenter = new InstrumentListPresenter(_repositoryWrapper);
            _menuPresenter = new MenuPresenter();
            _notFoundPresenter = new NotFoundPresenter();
            _patchDetailsPresenter = CreatePatchDetailsPresenter();
            _patchListPresenter = new PatchListPresenter(_repositoryWrapper.DocumentRepository);
            _sampleListPresenter = new SampleListPresenter(_repositoryWrapper.DocumentRepository);
            _samplePropertiesPresenter = new SamplePropertiesPresenter(_repositoryWrapper.SampleRepository, _repositoryWrapper.AudioFileFormatRepository, _repositoryWrapper.SampleDataTypeRepository, repositoryWrapper.SpeakerSetupRepository, _repositoryWrapper.InterpolationTypeRepository);

            _documentManager = new DocumentManager(repositoryWrapper);
            _patchManager = new PatchManager(_repositoryWrapper.PatchRepository, _repositoryWrapper.OperatorRepository, _repositoryWrapper.InletRepository, _repositoryWrapper.OutletRepository, _repositoryWrapper.EntityPositionRepository);
            _curveManager = new CurveManager(_repositoryWrapper.CurveRepository, _repositoryWrapper.NodeRepository);
            _sampleManager = new SampleManager(_repositoryWrapper.SampleRepository, _repositoryWrapper.SampleDataTypeRepository, _repositoryWrapper.SpeakerSetupRepository, _repositoryWrapper.AudioFileFormatRepository, _repositoryWrapper.InterpolationTypeRepository);
            _audioFileOutputManager = new AudioFileOutputManager(_repositoryWrapper.AudioFileOutputRepository, _repositoryWrapper.AudioFileOutputChannelRepository, repositoryWrapper.SampleDataTypeRepository, _repositoryWrapper.SpeakerSetupRepository, _repositoryWrapper.AudioFileFormatRepository);
            _entityPositionManager = new EntityPositionManager(_repositoryWrapper.EntityPositionRepository);

            _dispatchDelegateDictionary = CreateDispatchDelegateDictionary();
        }

        // A lot of times nothing is done with viewModel parameter.
        // This is because this presenter does not concern itself with statelessness yet.
        // If it would, then it would use the incoming view model parameter,
        // instead of the _viewModel field.
        // In this stateful situation, the only time the viewModel parameter is used,
        // is when some incoming data is saved or validated.

        // General

        public MainViewModel Show()
        {
            _viewModel = ViewModelHelper.CreateEmptyMainViewModel();

            MenuViewModel menuViewModel = _menuPresenter.Show();
            DispatchViewModel(menuViewModel);

            DocumentListViewModel documentListViewModel = _documentListPresenter.Show();
            DispatchViewModel(documentListViewModel);

            _viewModel.WindowTitle = Titles.ApplicationName;

            return _viewModel;
        }

        public MainViewModel NotFoundOK(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _notFoundPresenter.OK();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        // Document List

        public MainViewModel DocumentListShow(MainViewModel viewModel, int pageNumber)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            var viewModel2 = _documentListPresenter.Show(pageNumber);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentListClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            var viewModel2 = _documentListPresenter.Close();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentDetailsCreate(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentDetailsPresenter.Create();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentDetailsClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentDetailsPresenter.Close();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentDetailsSave(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentDetailsPresenter.Save(viewModel.DocumentDetails);

            DispatchViewModel(viewModel2);

            RefreshDocumentList();

            return _viewModel;
        }

        public MainViewModel DocumentDelete(MainViewModel viewModel, int id)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentDeletePresenter.Show(id);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentCannotDeleteOK(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            var viewModel2 = _documentCannotDeletePresenter.OK();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentConfirmDelete(MainViewModel viewModel, int id)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentDeletePresenter.Confirm(id);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentCancelDelete(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentDeletePresenter.Cancel();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentDeletedOK(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentDeletedPresenter.OK();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        // Document Actions

        public MainViewModel DocumentOpen(MainViewModel viewModel, int documentID)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            Document document = _repositoryWrapper.DocumentRepository.Get(documentID);

            _viewModel.Document = document.ToViewModel(
                _repositoryWrapper.AudioFileFormatRepository,
                _repositoryWrapper.SampleDataTypeRepository,
                _repositoryWrapper.SpeakerSetupRepository,
                _repositoryWrapper.InterpolationTypeRepository,
                _repositoryWrapper.NodeTypeRepository,
                _entityPositionManager);

            _viewModel.WindowTitle = String.Format("{0} - {1}", document.Name, Titles.ApplicationName);

            _viewModel.DocumentList.Visible = false;
            _viewModel.Document.DocumentTree.Visible = true;

            return _viewModel;
        }

        public MainViewModel DocumentSave(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (viewModel.Document == null) throw new NullException(() => viewModel.Document);

            TemporarilyAssertViewModelField();

            Document document = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);

            IValidator validator = new DocumentValidator_Recursive(document, _repositoryWrapper, alreadyDone: new HashSet<object>());
            _viewModel.ValidationMessages = validator.ValidationMessages.ToCanonical();

            IValidator warningsValidator = new DocumentWarningValidator_Recursive(document);
            _viewModel.WarningMessages = warningsValidator.ValidationMessages.ToCanonical();

            if (!validator.IsValid)
            {
                _repositoryWrapper.Rollback();
            }
            else
            {
                _repositoryWrapper.Commit();
            }

            return _viewModel;
        }

        // Document Tree

        public MainViewModel DocumentTreeShow(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _documentTreePresenter.Show(_viewModel.Document.ID);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentTreeExpandNode(MainViewModel viewModel, int listIndex)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentTreePresenter.ExpandNode(viewModel.Document.DocumentTree, listIndex);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentTreeCollapseNode(MainViewModel viewModel, int listIndex)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentTreePresenter.CollapseNode(viewModel.Document.DocumentTree, listIndex);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentTreeClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentTreePresenter.Close();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        // Document Properties

        public MainViewModel DocumentPropertiesShow(MainViewModel viewModel, int id)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _documentPropertiesPresenter.Show(id);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel DocumentPropertiesClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            DocumentPropertiesViewModel viewModel2 = _documentPropertiesPresenter.Close(_viewModel.Document.DocumentProperties);

            DispatchViewModel(viewModel2);

            bool isValid = !viewModel2.Visible; // TODO: It seems dirty to check success this way.
            if (isValid)
            {
                RefreshDocumentList();
                RefreshDocumentTree();
            }

            //_repositoryWrapper.Rollback();

            return _viewModel;
        }

        public MainViewModel DocumentPropertiesLoseFocus(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            DocumentPropertiesViewModel viewModel2 = _documentPropertiesPresenter.LooseFocus(_viewModel.Document.DocumentProperties);

            DispatchViewModel(viewModel2);

            // TODO: You might only refresh if viewModel2 is valid and if they are visible.

            RefreshDocumentList();
            RefreshDocumentTree();

            //_repositoryWrapper.Rollback();

            return _viewModel;
        }

        // AudioFileOutput Actions

        public MainViewModel AudioFileOutputListShow(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _audioFileOutputListPresenter.Show(viewModel.Document.ID);
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel AudioFileOutputListClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _audioFileOutputListPresenter.Close();
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel AudioFileOutputCreate(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document document = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);

            // Business
            AudioFileOutput audioFileOutput = _audioFileOutputManager.CreateWithRelatedEntities();
            audioFileOutput.LinkTo(document);

            // ToViewModel
            int listIndex = _viewModel.Document.AudioFileOutputPropertiesList.Count;

            AudioFileOutputListItemViewModel listItemViewModel = audioFileOutput.ToListItemViewModel();
            listItemViewModel.ListIndex = listIndex;
            _viewModel.Document.AudioFileOutputList.List.Add(listItemViewModel);

            AudioFileOutputPropertiesViewModel propertiesViewModel = audioFileOutput.ToPropertiesViewModel(_repositoryWrapper.AudioFileFormatRepository, _repositoryWrapper.SampleDataTypeRepository, _repositoryWrapper.SpeakerSetupRepository);
            propertiesViewModel.AudioFileOutput.ListIndex = listIndex;
            _viewModel.Document.AudioFileOutputPropertiesList.Add(propertiesViewModel);

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        public MainViewModel AudioFileOutputDelete(MainViewModel viewModel, int listIndex)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // 'Business' / ToViewModel
            _viewModel.Document.AudioFileOutputPropertiesList.RemoveAt(listIndex);
            _viewModel.Document.AudioFileOutputList.List.RemoveAt(listIndex);

            ListIndexHelper.RenumberListIndexes(_viewModel.Document.AudioFileOutputPropertiesList, listIndex);
            ListIndexHelper.RenumberListIndexes(_viewModel.Document.AudioFileOutputList.List, listIndex);

            // No need to do ToEntity, 
            // because we are not executing any additional business logic or refreshing 
            // that uses the entity models.

            return _viewModel;
        }

        public MainViewModel AudioFileOutputPropertiesEdit(MainViewModel viewModel, int id)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _audioFileOutputPropertiesPresenter.Edit(id);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel AudioFileOutputPropertiesClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _audioFileOutputPropertiesPresenter.Close();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        // Curve Actions

        public MainViewModel CurveListShow(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _curveListPresenter.Show(viewModel.Document.ID);
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel CurveListClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _curveListPresenter.Close();
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel CurveCreate(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document document = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);

            // Business
            Curve curve = _repositoryWrapper.CurveRepository.Create();
            curve.LinkTo(document);

            // ToViewModel
            int listIndex = _viewModel.Document.CurveDetailsList.Count;

            IDNameAndListIndexViewModel listItemViewModel = curve.ToIDNameAndListIndex();
            listItemViewModel.ListIndex = listIndex;
            _viewModel.Document.CurveList.List.Add(listItemViewModel);

            CurveDetailsViewModel detailsViewModel = curve.ToDetailsViewModel(_repositoryWrapper.NodeTypeRepository);
            detailsViewModel.Curve.ListIndex = listIndex;
            _viewModel.Document.CurveDetailsList.Add(detailsViewModel);

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        public MainViewModel CurveDelete(MainViewModel viewModel, int listIndex)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document document = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);
            Curve curve = document.Curves[listIndex];

            // Business
            VoidResult result = _curveManager.DeleteWithRelatedEntities(curve);
            if (result.Successful)
            {
                // ToViewModel
                _viewModel.Document.CurveDetailsList.RemoveAt(listIndex);
                _viewModel.Document.CurveList.List.RemoveAt(listIndex);

                ListIndexHelper.RenumberListIndexes(_viewModel.Document.CurveDetailsList, listIndex);
                ListIndexHelper.RenumberListIndexes(_viewModel.Document.CurveList.List, listIndex);
            }
            else
            {
                // ToViewModel
                _viewModel.PopupMessages = result.Messages;
            }

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        // Effect Actions

        public MainViewModel EffectListShow(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _effectListPresenter.Show(viewModel.Document.ID);
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel EffectListClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _effectListPresenter.Close();
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel EffectCreate(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document parentDocument = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);

            // Business
            Document effect = _documentManager.CreateEffect(parentDocument);

            // ToViewModel
            int listIndex = _viewModel.Document.EffectList.List.Count;

            IDNameAndListIndexViewModel listItemViewModel = effect.ToIDNameAndListIndex();
            listItemViewModel.ListIndex = listIndex;
            _viewModel.Document.EffectList.List.Add(listItemViewModel);

            ChildDocumentPropertiesViewModel propertiesViewModel = effect.ToChildDocumentPropertiesViewModel();
            propertiesViewModel.Document.ListIndex = listIndex;
            _viewModel.Document.EffectPropertiesList.Add(propertiesViewModel);

            ChildDocumentViewModel documentViewModel = effect.ToChildDocumentViewModel(_repositoryWrapper.AudioFileFormatRepository, _repositoryWrapper.SampleDataTypeRepository, _repositoryWrapper.SpeakerSetupRepository, _repositoryWrapper.InterpolationTypeRepository, _repositoryWrapper.NodeTypeRepository, _entityPositionManager);
            documentViewModel.Document.ListIndex = listIndex;
            _viewModel.Document.EffectDocumentList.Add(documentViewModel);

            RefreshDocumentTree();

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        public MainViewModel EffectDelete(MainViewModel viewModel, int listIndex)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // 'Business' / ToViewModel
            _viewModel.Document.EffectList.List.RemoveAt(listIndex);
            _viewModel.Document.EffectPropertiesList.RemoveAt(listIndex);
            _viewModel.Document.EffectDocumentList.RemoveAt(listIndex);

            ListIndexHelper.RenumberListIndexes(_viewModel.Document.EffectList.List, listIndex);
            ListIndexHelper.RenumberListIndexes(_viewModel.Document.EffectPropertiesList, listIndex);
            ListIndexHelper.RenumberListIndexes(_viewModel.Document.EffectDocumentList, listIndex);

            // ToEntity
            Document parentDocument = _repositoryWrapper.DocumentRepository.TryGet(viewModel.Document.ID);
            if (parentDocument == null)
            {
                NotFoundViewModel notFoundViewModel = CreateDocumentNotFoundViewModel();
                DispatchViewModel(notFoundViewModel);
                return _viewModel;
            }
            ToEntityHelper.ToEffectsWithRelatedEntities(_viewModel.Document.EffectDocumentList, parentDocument, _repositoryWrapper);

            RefreshDocumentTree();

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        // Instrument Actions

        public MainViewModel InstrumentListShow(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _instrumentListPresenter.Show(viewModel.Document.ID);
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel InstrumentListClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _instrumentListPresenter.Close();
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel InstrumentCreate(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document parentDocument = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);

            // Business
            Document instrument = _documentManager.CreateInstrument(parentDocument);

            // ToViewModel
            int listIndex = _viewModel.Document.InstrumentList.List.Count;

            IDNameAndListIndexViewModel listItemViewModel = instrument.ToIDNameAndListIndex();
            listItemViewModel.ListIndex = listIndex;
            _viewModel.Document.InstrumentList.List.Add(listItemViewModel);

            ChildDocumentPropertiesViewModel propertiesViewModel = instrument.ToChildDocumentPropertiesViewModel();
            propertiesViewModel.Document.ListIndex = listIndex;
            _viewModel.Document.InstrumentPropertiesList.Add(propertiesViewModel);

            ChildDocumentViewModel documentViewModel = instrument.ToChildDocumentViewModel(_repositoryWrapper.AudioFileFormatRepository, _repositoryWrapper.SampleDataTypeRepository, _repositoryWrapper.SpeakerSetupRepository, _repositoryWrapper.InterpolationTypeRepository, _repositoryWrapper.NodeTypeRepository, _entityPositionManager);
            documentViewModel.Document.ListIndex = listIndex;
            _viewModel.Document.InstrumentDocumentList.Add(documentViewModel);

            RefreshDocumentTree();

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        public MainViewModel InstrumentDelete(MainViewModel viewModel, int listIndex)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // 'Business' / ToViewModel
            _viewModel.Document.InstrumentList.List.RemoveAt(listIndex);
            _viewModel.Document.InstrumentPropertiesList.RemoveAt(listIndex);
            _viewModel.Document.InstrumentDocumentList.RemoveAt(listIndex);

            ListIndexHelper.RenumberListIndexes(_viewModel.Document.InstrumentList.List, listIndex);
            ListIndexHelper.RenumberListIndexes(_viewModel.Document.InstrumentPropertiesList, listIndex);
            ListIndexHelper.RenumberListIndexes(_viewModel.Document.InstrumentDocumentList, listIndex);

            // ToEntity
            Document parentDocument = _repositoryWrapper.DocumentRepository.TryGet(viewModel.Document.ID);
            if (parentDocument == null)
            {
                NotFoundViewModel notFoundViewModel = CreateDocumentNotFoundViewModel();
                DispatchViewModel(notFoundViewModel);
                return _viewModel;
            }
            ToEntityHelper.ToInstrumentsWithRelatedEntities(_viewModel.Document.InstrumentDocumentList, parentDocument, _repositoryWrapper);

            RefreshDocumentTree();

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        // Patch Actions

        public MainViewModel PatchListShow(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _patchListPresenter.Show(viewModel.Document.ID);
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel PatchListClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _patchListPresenter.Close();
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel PatchCreate(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document document = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);

            // Business
            Patch patch = _repositoryWrapper.PatchRepository.Create();
            patch.LinkTo(document);

            // ToViewModel
            int listIndex = _viewModel.Document.PatchDetailsList.Count;

            IDNameAndListIndexViewModel listItemViewModel = patch.ToIDNameAndListIndex();
            listItemViewModel.ListIndex = listIndex;
            _viewModel.Document.PatchList.List.Add(listItemViewModel);

            PatchDetailsViewModel detailsViewModel = patch.ToDetailsViewModel(_entityPositionManager);
            detailsViewModel.Patch.ListIndex = listIndex;
            _viewModel.Document.PatchDetailsList.Add(detailsViewModel);

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        public MainViewModel PatchDelete(MainViewModel viewModel, int listIndex)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document document = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);
            Patch patch = document.Patches[listIndex];

            // Business
            VoidResult result = _patchManager.DeletePatchWithRelatedEntities(patch);
            if (result.Successful)
            {
                // ToViewModel
                _viewModel.Document.PatchDetailsList.RemoveAt(listIndex);
                _viewModel.Document.PatchList.List.RemoveAt(listIndex);

                ListIndexHelper.RenumberListIndexes(_viewModel.Document.PatchDetailsList, listIndex);
                ListIndexHelper.RenumberListIndexes(_viewModel.Document.PatchList.List, listIndex);
            }
            else
            {
                // ToViewModel
                _viewModel.PopupMessages = result.Messages;
            }

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        // Sample List

        public MainViewModel SampleListShow(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _sampleListPresenter.Show(viewModel.Document.ID);
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel SampleListClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            object viewModel2 = _sampleListPresenter.Close();
            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel SampleCreate(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document document = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);

            // Business
            Sample sample = _sampleManager.CreateSample();
            sample.LinkTo(document);

            // ToViewModel
            int listIndex = _viewModel.Document.SamplePropertiesList.Count;

            SampleListItemViewModel listItemViewModel = sample.ToListItemViewModel();
            listItemViewModel.ListIndex = listIndex;
            _viewModel.Document.SampleList.List.Add(listItemViewModel);

            SamplePropertiesViewModel detailsViewModel = sample.ToPropertiesViewModel(
                _repositoryWrapper.AudioFileFormatRepository, 
                _repositoryWrapper.SampleDataTypeRepository, 
                _repositoryWrapper.SpeakerSetupRepository, 
                _repositoryWrapper.InterpolationTypeRepository);
            detailsViewModel.Sample.ListIndex = listIndex;
            _viewModel.Document.SamplePropertiesList.Add(detailsViewModel);

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        public MainViewModel SampleDelete(MainViewModel viewModel, int listIndex)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            TemporarilyAssertViewModelField();

            // ToEntity
            Document document = viewModel.Document.ToEntityWithRelatedEntities(_repositoryWrapper);
            Sample sample = document.Samples[listIndex];

            // Business
            VoidResult result = _sampleManager.DeleteWithRelatedEntities(sample);
            if (result.Successful)
            {
                // ToViewModel
                _viewModel.Document.SamplePropertiesList.RemoveAt(listIndex);
                _viewModel.Document.SampleList.List.RemoveAt(listIndex);

                ListIndexHelper.RenumberListIndexes(_viewModel.Document.SamplePropertiesList, listIndex);
                ListIndexHelper.RenumberListIndexes(_viewModel.Document.SampleList.List, listIndex);
            }
            else
            {
                // ToViewModel
                _viewModel.PopupMessages = result.Messages;
            }

            _repositoryWrapper.Rollback();

            return _viewModel;
        }

        // Temporary Details Actions

        public MainViewModel PatchDetailsEdit(MainViewModel viewModel, int id)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _patchDetailsPresenter.Edit(id);

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        public MainViewModel PatchDetailsClose(MainViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            TemporarilyAssertViewModelField();

            object viewModel2 = _patchDetailsPresenter.Close();

            DispatchViewModel(viewModel2);

            return _viewModel;
        }

        // DispatchViewModel

        private Dictionary<Type, Action<object>> _dispatchDelegateDictionary;

        private Dictionary<Type, Action<object>> CreateDispatchDelegateDictionary()
        {
            var dictionary = new Dictionary<Type, Action<object>>
            {
                 { typeof(AudioFileOutputPropertiesViewModel), DispatchAudioFileOutputPropertiesViewModel },
                 { typeof(AudioFileOutputListViewModel), DispatchAudioFileOutputListViewModel },
                 { typeof(ChildDocumentListViewModel), DispatchChildDocumentListViewModel },
                 //{ typeof(ChildDocumentPropertiesViewModel), DispatchChildDocumentPropertiesViewModel },
                 //{ typeof(CurveDetailsListViewModel), DispatchCurveDetailsViewModel },
                 { typeof(CurveListViewModel), DispatchCurveListViewModel },
                 { typeof(DocumentListViewModel), DispatchDocumentListViewModel },
                 { typeof(DocumentCannotDeleteViewModel), DispatchDocumentCannotDeleteViewModel },
                 { typeof(DocumentDeleteViewModel), DispatchDocumentDeleteViewModel },
                 { typeof(DocumentTreeViewModel), DispatchDocumentTreeViewModel },
                 { typeof(DocumentPropertiesViewModel), DispatchDocumentPropertiesViewModel },
                 { typeof(DocumentDeletedViewModel), DispatchDocumentDeletedViewModel },
                 { typeof(DocumentDetailsViewModel), DispatchDocumentDetailsViewModel },
                 { typeof(MenuViewModel), DispatchMenuViewModel },
                 { typeof(NotFoundViewModel), DispatchNotFoundViewModel },
                 { typeof(PatchListViewModel), DispatchPatchListViewModel },
                 { typeof(PatchDetailsViewModel), DispatchPatchDetailsViewModel },
                 { typeof(SampleListViewModel), DispatchSampleListViewModel },
            };

            return dictionary;
        }

        /// <summary>
        /// Applies a view model from a sub-presenter in the right way
        /// to the main view model.
        /// </summary>
        private void DispatchViewModel(object viewModel2)
        {
            if (viewModel2 == null) throw new NullException(() => viewModel2);

            Type viewModelType = viewModel2.GetType();

            Action<object> dispatchDelegate;
            if (!_dispatchDelegateDictionary.TryGetValue(viewModelType, out dispatchDelegate))
            {
                throw new UnexpectedViewModelTypeException(viewModel2);
            }

            dispatchDelegate(viewModel2);
        }

        private void DispatchChildDocumentListViewModel(object viewModel2)
        {
            var childDocumentListViewModel = (ChildDocumentListViewModel)viewModel2;

            switch (childDocumentListViewModel.ChildDocumentType)
            {
                case ChildDocumentTypeEnum.Instrument:
                    _viewModel.Document.InstrumentList = childDocumentListViewModel;

                    if (_viewModel.Document.InstrumentList.Visible)
                    {
                        HideAllListAndDetailViewModels();
                        _viewModel.Document.InstrumentList.Visible = true;
                    }

                    break;

                case ChildDocumentTypeEnum.Effect:
                    _viewModel.Document.EffectList = childDocumentListViewModel;

                    if (_viewModel.Document.EffectList.Visible)
                    {
                        HideAllListAndDetailViewModels();
                        _viewModel.Document.EffectList.Visible = true;
                    }

                    break;

                default:
                    throw new ValueNotSupportedException(childDocumentListViewModel.ChildDocumentType);
            }
        }

        private void DispatchDocumentDetailsViewModel(object viewModel2)
        {
            _viewModel.DocumentDetails = (DocumentDetailsViewModel)viewModel2;

            if (_viewModel.DocumentDetails.Visible)
            {
                HideAllListAndDetailViewModels();
                _viewModel.DocumentDetails.Visible = true;
            }
        }
        
        private void DispatchMenuViewModel(object viewModel2)
        {
            _viewModel.Menu = (MenuViewModel)viewModel2;
        }

        private void DispatchDocumentListViewModel(object viewModel2)
        {
            _viewModel.DocumentList = (DocumentListViewModel)viewModel2;

            if (_viewModel.DocumentList.Visible)
            {
                HideAllListAndDetailViewModels();
                _viewModel.DocumentList.Visible = true;
            }
        }

        private void DispatchDocumentCannotDeleteViewModel(object viewModel2)
        {
            _viewModel.DocumentCannotDelete = (DocumentCannotDeleteViewModel)viewModel2;
        }

        private void DispatchDocumentDeleteViewModel(object viewModel2)
        {
            _viewModel.DocumentDelete = (DocumentDeleteViewModel)viewModel2;
        }

        private void DispatchDocumentTreeViewModel(object viewModel2)
        {
            _viewModel.Document.DocumentTree = (DocumentTreeViewModel)viewModel2;
        }

        private void DispatchDocumentPropertiesViewModel(object viewModel2)
        {
            _viewModel.Document.DocumentProperties = (DocumentPropertiesViewModel)viewModel2;
        }

        private void DispatchAudioFileOutputListViewModel(object viewModel2)
        {
            _viewModel.Document.AudioFileOutputList = (AudioFileOutputListViewModel)viewModel2;

            if (_viewModel.Document.AudioFileOutputList.Visible)
            {
                HideAllListAndDetailViewModels();
                _viewModel.Document.AudioFileOutputList.Visible = true;
            }
        }

        private void DispatchCurveListViewModel(object viewModel2)
        {
            _viewModel.Document.CurveList = (CurveListViewModel)viewModel2;
        }

        private void DispatchPatchListViewModel(object viewModel2)
        {
            _viewModel.Document.PatchList = (PatchListViewModel)viewModel2;
        }

        private void DispatchSampleListViewModel(object viewModel2)
        {
            _viewModel.Document.SampleList = (SampleListViewModel)viewModel2;
        }

        private void DispatchAudioFileOutputPropertiesViewModel(object viewModel2)
        {
            var audioFileOutputPropertiesViewModel = (AudioFileOutputPropertiesViewModel)viewModel2;

            _viewModel.Document.AudioFileOutputPropertiesList[audioFileOutputPropertiesViewModel.AudioFileOutput.ListIndex] = audioFileOutputPropertiesViewModel;
        }

        private void DispatchPatchDetailsViewModel(object viewModel2)
        {
            _viewModel.TemporaryPatchDetails = (PatchDetailsViewModel)viewModel2;
        }

        private void DispatchDocumentDeletedViewModel(object viewModel2)
        {
            var documentDeletedViewModel = (DocumentDeletedViewModel)viewModel2;

            _viewModel.DocumentDeleted = documentDeletedViewModel;

            _viewModel.DocumentDelete.Visible = false;
            _viewModel.DocumentDetails.Visible = false;

            if (!documentDeletedViewModel.Visible)
            {
                RefreshDocumentList();
            }
        }

        private void DispatchNotFoundViewModel(object viewModel2)
        {
            var notFoundViewModel = (NotFoundViewModel)viewModel2;

            _viewModel.NotFound = notFoundViewModel;

            // HACK: Checking visibility of the NotFound view model
            // prevents refreshing the DocumentList twice:
            // once when showing the NotFound view model,
            // a second time when clicking OK on it.

            // TODO: Low priority: Eventually the NotFoundViewModel will create even more ambiguity,
            // when it is reused for multiple entity types.

            if (notFoundViewModel.Visible)
            {
                RefreshDocumentList();
            }
        }

        private void HideAllListAndDetailViewModels()
        {
            _viewModel.DocumentList.Visible = false;
            _viewModel.DocumentDetails.Visible = false;

            _viewModel.Document.InstrumentList.Visible = false;
            _viewModel.Document.EffectList.Visible = false;
            _viewModel.Document.SampleList.Visible = false;
            _viewModel.Document.CurveList.Visible = false;
            _viewModel.Document.AudioFileOutputList.Visible = false;
            _viewModel.Document.PatchList.Visible = false;

            foreach (CurveDetailsViewModel curveDetailsViewModel in _viewModel.Document.CurveDetailsList)
            {
                curveDetailsViewModel.Visible = false;
            }

            foreach (PatchDetailsViewModel patchDetailsViewModel in _viewModel.Document.PatchDetailsList)
            {
                patchDetailsViewModel.Visible = false;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in _viewModel.Document.InstrumentDocumentList)
            {
                childDocumentViewModel.SampleList.Visible = false;
                childDocumentViewModel.CurveList.Visible = false;
                childDocumentViewModel.PatchList.Visible = false;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in _viewModel.Document.EffectDocumentList)
            {
                childDocumentViewModel.SampleList.Visible = false;
                childDocumentViewModel.CurveList.Visible = false;
                childDocumentViewModel.PatchList.Visible = false;
            }
        }

        // Helpers

        private void RefreshDocumentList()
        {
            _viewModel.DocumentList = _documentListPresenter.Refresh(_viewModel.DocumentList);
        }

        private void RefreshDocumentTree()
        {
            object viewModel2 = _documentTreePresenter.Refresh(_viewModel.Document.DocumentTree);
            DispatchViewModel(viewModel2);
        }

        private void RefreshAudioFileOutputList()
        {
            object viewModel2 = _audioFileOutputListPresenter.Refresh(_viewModel.Document.AudioFileOutputList);
            DispatchViewModel(viewModel2);
        }

        private void TemporarilyAssertViewModelField()
        {
            if (_viewModel == null)
            {
                // TODO: ViewModel should be converted to entities and back to view model again,
                // to work in a stateless environment.
                throw new Exception("_viewModel field is not assigned and code is not adapted to work in a stateless environment.");
            }
        }

        private NotFoundViewModel CreateDocumentNotFoundViewModel()
        {
            NotFoundViewModel viewModel = new NotFoundPresenter().Show(PropertyDisplayNames.Document);
            return viewModel;
        }

        private PatchDetailsPresenter CreatePatchDetailsPresenter()
        {
            var presenter2 = new PatchDetailsPresenter(
                _repositoryWrapper.PatchRepository,
                _repositoryWrapper.OperatorRepository,
                _repositoryWrapper.InletRepository,
                _repositoryWrapper.OutletRepository,
                _repositoryWrapper.EntityPositionRepository,
                _repositoryWrapper.CurveRepository,
                _repositoryWrapper.SampleRepository);

            return presenter2;
        }

        private AudioFileOutputPropertiesPresenter CreateAudioFileOutputPropertiesPresenter()
        {
            var presenter2 = new AudioFileOutputPropertiesPresenter(
                _repositoryWrapper.AudioFileOutputRepository,
                _repositoryWrapper.AudioFileFormatRepository,
                _repositoryWrapper.SampleDataTypeRepository,
                _repositoryWrapper.SpeakerSetupRepository);

            return presenter2;
        }
    }
}
