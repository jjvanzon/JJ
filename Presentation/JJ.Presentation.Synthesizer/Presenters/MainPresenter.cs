﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Framework.Business;
using JJ.Framework.Presentation;
using JJ.Data.Synthesizer;
using JJ.Business.CanonicalModel;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Managers;
using JJ.Business.Synthesizer.Validation;
using JJ.Business.Synthesizer.Warnings;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Resources;
using JJ.Business.Synthesizer.SideEffects;
using JJ.Presentation.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.ToEntity;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ViewModels.Partials;

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
        private ChildDocumentListPresenter _effectListPresenter;
        private ChildDocumentListPresenter _instrumentListPresenter;
        private MenuPresenter _menuPresenter;
        private NotFoundPresenter _notFoundPresenter;
        private PatchDetailsPresenter _patchDetailsPresenter;
        private PatchListPresenter _patchListPresenter;
        private SampleListPresenter _sampleListPresenter;
        private SamplePropertiesPresenter _samplePropertiesPresenter;

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
            _audioFileOutputPropertiesPresenter = new AudioFileOutputPropertiesPresenter(new AudioFileOutputRepositories(_repositoryWrapper));
            _curveDetailsPresenter = new CurveDetailsPresenter(
                _repositoryWrapper.CurveRepository, 
                _repositoryWrapper.NodeRepository, 
                _repositoryWrapper.NodeTypeRepository);
            _curveListPresenter = new CurveListPresenter(_repositoryWrapper.DocumentRepository);
            _documentCannotDeletePresenter = new DocumentCannotDeletePresenter(_repositoryWrapper.DocumentRepository);
            _documentDeletedPresenter = new DocumentDeletedPresenter();
            _documentDeletePresenter = new DocumentDeletePresenter(_repositoryWrapper);
            _documentDetailsPresenter = new DocumentDetailsPresenter(_repositoryWrapper.DocumentRepository, _repositoryWrapper.IDRepository);
            _documentListPresenter = new DocumentListPresenter(_repositoryWrapper.DocumentRepository);
            _documentPropertiesPresenter = new DocumentPropertiesPresenter(_repositoryWrapper.DocumentRepository);
            _documentTreePresenter = new DocumentTreePresenter(_repositoryWrapper.DocumentRepository);
            _effectListPresenter = new ChildDocumentListPresenter(_repositoryWrapper.DocumentRepository);
            _instrumentListPresenter = new ChildDocumentListPresenter(_repositoryWrapper.DocumentRepository);
            _menuPresenter = new MenuPresenter();
            _notFoundPresenter = new NotFoundPresenter();
            _patchDetailsPresenter = _patchDetailsPresenter = new PatchDetailsPresenter(
                _repositoryWrapper.PatchRepository,
                _repositoryWrapper.OperatorRepository,
                _repositoryWrapper.OperatorTypeRepository,
                _repositoryWrapper.InletRepository,
                _repositoryWrapper.OutletRepository,
                _repositoryWrapper.EntityPositionRepository,
                _repositoryWrapper.CurveRepository,
                _repositoryWrapper.SampleRepository,
                _repositoryWrapper.IDRepository);
            _patchListPresenter = new PatchListPresenter(_repositoryWrapper.DocumentRepository);
            _sampleListPresenter = new SampleListPresenter(_repositoryWrapper.DocumentRepository, _repositoryWrapper.SampleRepository);
            _samplePropertiesPresenter = new SamplePropertiesPresenter(new SampleRepositories(_repositoryWrapper));

            _documentManager = new DocumentManager(repositoryWrapper);
            _patchManager = new PatchManager(
                _repositoryWrapper.PatchRepository,
                _repositoryWrapper.OperatorRepository, 
                _repositoryWrapper.InletRepository, 
                _repositoryWrapper.OutletRepository,
                _repositoryWrapper.CurveRepository,
                _repositoryWrapper.SampleRepository,
                _repositoryWrapper.EntityPositionRepository);
            _curveManager = new CurveManager(_repositoryWrapper.CurveRepository, _repositoryWrapper.NodeRepository);
            _sampleManager = new SampleManager(new SampleRepositories(_repositoryWrapper));
            _audioFileOutputManager = new AudioFileOutputManager(
                _repositoryWrapper.AudioFileOutputRepository, 
                _repositoryWrapper.AudioFileOutputChannelRepository, 
                _repositoryWrapper.SampleDataTypeRepository, 
                _repositoryWrapper.SpeakerSetupRepository, 
                _repositoryWrapper.AudioFileFormatRepository,
                _repositoryWrapper.CurveRepository,
                _repositoryWrapper.SampleRepository,
                _repositoryWrapper.IDRepository);
            _entityPositionManager = new EntityPositionManager(_repositoryWrapper.EntityPositionRepository, _repositoryWrapper.IDRepository);

            _dispatchDelegateDictionary = CreateDispatchDelegateDictionary();
        }

        // General

        public MainViewModel ViewModel { get; private set; }

        public void Show()
        {
            try
            {
                ViewModel = ViewModelHelper.CreateEmptyMainViewModel(_repositoryWrapper.OperatorTypeRepository);

                _menuPresenter.Show(documentIsOpen: false);
                DispatchViewModel(_menuPresenter.ViewModel);

                _documentListPresenter.ViewModel = ViewModel.DocumentList;
                _documentListPresenter.Show();
                DispatchViewModel(_documentListPresenter.ViewModel);

                ViewModel.WindowTitle = Titles.ApplicationName;
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void NotFoundOK()
        {
            try
            {
                _notFoundPresenter.OK();

                DispatchViewModel(_notFoundPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PopupMessagesOK()
        {
            try
            {
                ViewModel.PopupMessages = new List<Message> { };
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // Document List

        public void DocumentListShow(int pageNumber)
        {
            try
            {
                _documentListPresenter.ViewModel = ViewModel.DocumentList;
                _documentListPresenter.Show(pageNumber);
                DispatchViewModel(_documentListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentListClose()
        {
            try
            {
                _documentListPresenter.ViewModel = ViewModel.DocumentList;
                _documentListPresenter.Close();
                DispatchViewModel(_documentListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentDetailsCreate()
        {
            try
            {
                _documentDetailsPresenter.Create();
                DispatchViewModel(_documentDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentDetailsClose()
        {
            try
            {
                _documentDetailsPresenter.Close();
                DispatchViewModel(_documentDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentDetailsSave()
        {
            try
            {
                _documentDetailsPresenter.Save();
                DispatchViewModel(_documentDetailsPresenter.ViewModel);

                RefreshDocumentList();
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentDelete(int id)
        {
            try
            {
                object viewModel2 = _documentDeletePresenter.Show(id);
                DispatchViewModel(viewModel2);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentCannotDeleteOK()
        {
            try
            {
                _documentCannotDeletePresenter.OK();
                DispatchViewModel(_documentCannotDeletePresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentConfirmDelete(int id)
        {
            try
            {
                object viewModel2 = _documentDeletePresenter.Confirm(id);

                if (viewModel2 is DocumentDeletedViewModel)
                {
                    _repositoryWrapper.Commit();
                }

                DispatchViewModel(viewModel2);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentCancelDelete()
        {
            try
            {
                _documentDeletePresenter.Cancel();
                DispatchViewModel(_documentDeletePresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentDeletedOK()
        {
            try
            {
                _documentDeletedPresenter.OK();
                DispatchViewModel(_documentDeletedPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // Document Actions

        public void DocumentOpen(int documentID)
        {
            try
            {
                if (ViewModel.Document.IsOpen)
                {
                    // If you do not clear the presenters, they will remember the view model,
                    // so if you open another document and then open the original one,
                    // the original unsaved data will be shown in the presenters,
                    // because they do not think they need to refresh their view models,
                    // since it is the same document.
                    _audioFileOutputListPresenter.ViewModel = null;
                    _audioFileOutputPropertiesPresenter.ViewModel = null;
                    _curveDetailsPresenter.ViewModel = null;
                    _curveListPresenter.ViewModel = null;
                    _documentPropertiesPresenter.ViewModel = null;
                    _documentTreePresenter.ViewModel = null;
                    _effectListPresenter.ViewModel = null;
                    _instrumentListPresenter.ViewModel = null;
                    _patchDetailsPresenter.ViewModel = null;
                    _patchListPresenter.ViewModel = null;
                    _sampleListPresenter.ViewModel = null;
                    _samplePropertiesPresenter.ViewModel = null;
                }

                Document document = _repositoryWrapper.DocumentRepository.Get(documentID);

                ViewModel.Document = document.ToViewModel(_repositoryWrapper, _entityPositionManager);

                ViewModel.WindowTitle = String.Format("{0} - {1}", document.Name, Titles.ApplicationName);
                _menuPresenter.Show(documentIsOpen: true);
                ViewModel.Menu = _menuPresenter.ViewModel;

                ViewModel.DocumentList.Visible = false;
                ViewModel.Document.DocumentTree.Visible = true;

                ViewModel.Document.IsOpen = true;
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentSave()
        {
            if (ViewModel.Document == null) throw new NullException(() => ViewModel.Document);

            try
            {
                Document document = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);

                IValidator validator = new DocumentValidator_Recursive(document, _repositoryWrapper, alreadyDone: new HashSet<object>());
                IValidator warningsValidator = new DocumentWarningValidator_Recursive(document, _repositoryWrapper.SampleRepository, new HashSet<object>());

                if (!validator.IsValid)
                {
                    _repositoryWrapper.Rollback();
                }
                else
                {
                    _repositoryWrapper.Commit();
                }

                ViewModel.ValidationMessages = validator.ValidationMessages.ToCanonical();
                ViewModel.WarningMessages = warningsValidator.ValidationMessages.ToCanonical();
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentClose()
        {
            try
            {
                if (ViewModel.Document.IsOpen)
                {
                    ViewModel.Document = ViewModelHelper.CreateEmptyDocumentViewModel();
                    ViewModel.WindowTitle = Titles.ApplicationName;

                    _menuPresenter.Show(documentIsOpen: false);

                    ViewModel.Menu = _menuPresenter.ViewModel;

                    _audioFileOutputListPresenter.ViewModel = null;
                    _audioFileOutputPropertiesPresenter.ViewModel = null;
                    _curveDetailsPresenter.ViewModel = null;
                    _curveListPresenter.ViewModel = null;
                    _documentPropertiesPresenter.ViewModel = null;
                    _documentTreePresenter.ViewModel = null;
                    _effectListPresenter.ViewModel = null;
                    _instrumentListPresenter.ViewModel = null;
                    _patchDetailsPresenter.ViewModel = null;
                    _patchListPresenter.ViewModel = null;
                    _sampleListPresenter.ViewModel = null;
                    _samplePropertiesPresenter.ViewModel = null;
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentPropertiesShow(int id)
        {
            try
            {
                _documentPropertiesPresenter.ViewModel = ViewModel.Document.DocumentProperties;
                _documentPropertiesPresenter.Show(id);
                DispatchViewModel(_documentPropertiesPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentPropertiesClose()
        {
            try
            {
                _documentPropertiesPresenter.ViewModel = ViewModel.Document.DocumentProperties;
                _documentPropertiesPresenter.Close();
                DispatchViewModel(_documentPropertiesPresenter.ViewModel);

                if (_documentPropertiesPresenter.ViewModel.Successful)
                {
                    RefreshDocumentList();
                    RefreshDocumentTree();
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentPropertiesLoseFocus()
        {

            try
            {
                _documentPropertiesPresenter.ViewModel = ViewModel.Document.DocumentProperties;
                _documentPropertiesPresenter.LoseFocus();
                DispatchViewModel(_documentPropertiesPresenter.ViewModel);

                if (_documentPropertiesPresenter.ViewModel.Successful)
                {
                    RefreshDocumentList();
                    RefreshDocumentTree();
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentTreeShow()
        {
            try
            {
                _documentTreePresenter.ViewModel = ViewModel.Document.DocumentTree;
                _documentTreePresenter.Show();
                DispatchViewModel(_documentTreePresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentTreeExpandNode(int nodeIndex)
        {
            try
            {
                _documentTreePresenter.ViewModel = ViewModel.Document.DocumentTree;
                _documentTreePresenter.ExpandNode(nodeIndex);
                DispatchViewModel(_documentTreePresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentTreeCollapseNode(int nodeIndex)
        {
            try
            {
                _documentTreePresenter.ViewModel = ViewModel.Document.DocumentTree;
                _documentTreePresenter.CollapseNode(nodeIndex);
                DispatchViewModel(_documentTreePresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void DocumentTreeClose()
        {
            try
            {
                _documentTreePresenter.ViewModel = ViewModel.Document.DocumentTree;
                _documentTreePresenter.Close();
                DispatchViewModel(_documentTreePresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // AudioFileOutput Actions

        public void AudioFileOutputListShow()
        {
            try
            {
                _audioFileOutputListPresenter.ViewModel = ViewModel.Document.AudioFileOutputList;
                _audioFileOutputListPresenter.Show();
                DispatchViewModel(_audioFileOutputListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void AudioFileOutputListClose()
        {
            try
            {
                _audioFileOutputListPresenter.ViewModel = ViewModel.Document.AudioFileOutputList;
                _audioFileOutputListPresenter.Close();
                DispatchViewModel(_audioFileOutputListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void AudioFileOutputCreate()
        {
            try
            {
                // ToEntity
                Document document = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);

                // Business
                AudioFileOutput audioFileOutput = _audioFileOutputManager.CreateWithRelatedEntities();
                audioFileOutput.LinkTo(document);

                ISideEffect sideEffect = new AudioFileOutput_SideEffect_GenerateName(audioFileOutput);
                sideEffect.Execute();

                // ToViewModel
                AudioFileOutputListItemViewModel listItemViewModel = audioFileOutput.ToListItemViewModel();
                ViewModel.Document.AudioFileOutputList.List.Add(listItemViewModel);
                ViewModel.Document.AudioFileOutputList.List = ViewModel.Document.AudioFileOutputList.List.OrderBy(x => x.Name).ToList();

                AudioFileOutputPropertiesViewModel propertiesViewModel = audioFileOutput.ToPropertiesViewModel(_repositoryWrapper.AudioFileFormatRepository, _repositoryWrapper.SampleDataTypeRepository, _repositoryWrapper.SpeakerSetupRepository);
                ViewModel.Document.AudioFileOutputPropertiesList.Add(propertiesViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void AudioFileOutputDelete(int id)
        {
            try
            {
                // 'Business' / ToViewModel
                ViewModel.Document.AudioFileOutputPropertiesList.RemoveFirst(x => x.Entity.ID == id);
                ViewModel.Document.AudioFileOutputList.List.RemoveFirst(x => x.ID == id);
                
                // No need to do ToEntity, 
                // because we are not executing any additional business logic or refreshing 
                // that uses the entity models.
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void AudioFileOutputPropertiesShow(int id)
        {
            try
            {
                int listIndex = ViewModel.Document.AudioFileOutputPropertiesList.IndexOf(x => x.Entity.ID == id);

                _audioFileOutputPropertiesPresenter.ViewModel = ViewModel.Document.AudioFileOutputPropertiesList[listIndex];
                _audioFileOutputPropertiesPresenter.Show();

                DispatchViewModel(_audioFileOutputPropertiesPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void AudioFileOutputPropertiesClose()
        {
            try
            {
                // TODO: Can I get away with converting only part of the user input to entities?
                // Do consider that channels reference patch outlets.
                Document document = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);

                _audioFileOutputPropertiesPresenter.Close();

                if (_audioFileOutputPropertiesPresenter.ViewModel.Successful)
                {
                    RefreshAudioFileOutputList();
                }

                DispatchViewModel(_audioFileOutputPropertiesPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void AudioFileOutputPropertiesLoseFocus()
        {
            try
            {
                // TODO: Can I get away with converting only part of the user input to entities?
                // Do consider that channels reference patch outlets.
                Document document = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);

                _audioFileOutputPropertiesPresenter.LoseFocus();

                DispatchViewModel(_audioFileOutputPropertiesPresenter.ViewModel);

                if (_audioFileOutputPropertiesPresenter.ViewModel.Successful)
                {
                    RefreshAudioFileOutputList();
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // Curve Actions

        public void CurveListShow(int? childDocumentID)
        {
            try
            {
                // Needed to create uncommitted child documents.
                if (childDocumentID.HasValue)
                {
                    Document document = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                }

                int documentID = childDocumentID ?? ViewModel.Document.ID;

                CurveListViewModel curveListViewModel = ChildDocumentHelper.GetCurveListViewModel_ByDocumentID(ViewModel.Document, documentID);
                _curveListPresenter.ViewModel = curveListViewModel;
                _curveListPresenter.Show();
                DispatchViewModel(_curveListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void CurveListClose()
        {
            try
            {
                _curveListPresenter.Close();
                DispatchViewModel(_curveListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void CurveCreate(int? childDocumentID)
        {
            try
            {
                // ToEntity
                Document rootDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);

                // Business
                Document document = ChildDocumentHelper.TryGetRootDocumentOrChildDocument(ViewModel.Document.ID, childDocumentID, _repositoryWrapper.DocumentRepository);
                var curve = new Curve();
                curve.ID = _repositoryWrapper.IDRepository.GetID();
                curve.LinkTo(document);

                ISideEffect sideEffect = new Curve_SideEffect_GenerateName(curve);
                sideEffect.Execute();

                _repositoryWrapper.CurveRepository.Insert(curve);

                // ToViewModel
                CurveListViewModel curveListViewModel = ChildDocumentHelper.GetCurveListViewModel_ByDocumentID(ViewModel.Document, document.ID);
                CurveListItemViewModel listItemViewModel = curve.ToListItemViewModel();
                curveListViewModel.List.Add(listItemViewModel);
                curveListViewModel.List = curveListViewModel.List.OrderBy(x => x.Name).ToList();

                IList<CurveDetailsViewModel> curveDetailsViewModels = ChildDocumentHelper.GetCurveDetailsViewModels_ByDocumentID(ViewModel.Document, document.ID);
                CurveDetailsViewModel curveDetailsViewModel = curve.ToDetailsViewModel(_repositoryWrapper.NodeTypeRepository);
                curveDetailsViewModels.Add(curveDetailsViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void CurveDelete(int curveID)
        {
            try
            {
                // ToEntity
                Document rootDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                Curve curve = ChildDocumentHelper.TryGetCurve(rootDocument, curveID);
                if (curve == null)
                {
                    NotFoundViewModel notFoundViewModel = ViewModelHelper.CreateNotFoundViewModel<Curve>();
                    DispatchViewModel(notFoundViewModel);
                    return;
                }
                int documentID = curve.Document.ID;

                // Business
                VoidResult result = _curveManager.DeleteWithRelatedEntities(curve);
                if (result.Successful)
                {
                    // ToViewModel
                    IList<CurveDetailsViewModel> detailsViewModels = ChildDocumentHelper.GetCurveDetailsViewModels_ByDocumentID(ViewModel.Document, documentID);
                    detailsViewModels.RemoveFirst(x => x.Entity.ID == curveID);

                    CurveListViewModel listViewModel = ChildDocumentHelper.GetCurveListViewModel_ByDocumentID(ViewModel.Document, documentID);
                    listViewModel.List.RemoveFirst(x => x.ID == curveID);
                }
                else
                {
                    // ToViewModel
                    ViewModel.PopupMessages = result.Messages;
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void CurveDetailsShow(int curveID)
        {
            try
            {
                ChildDocumentItemAlternativeKey key = ChildDocumentHelper.GetAlternativeCurveKey(ViewModel.Document, curveID);
                CurveDetailsViewModel detailsViewModel = ChildDocumentHelper.GetCurveDetailsViewModel_ByAlternativeKey(ViewModel.Document, key);

                _curveDetailsPresenter.ViewModel = detailsViewModel;
                _curveDetailsPresenter.Show();

                DispatchViewModel(_curveDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void CurveDetailsClose()
        {
            try
            {
                _curveDetailsPresenter.Close();

                DispatchViewModel(_curveDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void CurveDetailsLoseFocus()
        {
            try
            {
                _curveDetailsPresenter.LoseFocus();

                DispatchViewModel(_curveDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // Effect Actions

        public void EffectListShow()
        {
            try
            {
                _effectListPresenter.ViewModel = ViewModel.Document.EffectList;
                _effectListPresenter.Show();
                DispatchViewModel(_effectListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void EffectListClose()
        {
            try
            {
                _effectListPresenter.ViewModel = ViewModel.Document.EffectList;
                _effectListPresenter.Close();
                DispatchViewModel(_effectListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void EffectCreate()
        {
            try
            {
                // ToEntity
                Document parentDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);

                // Business
                var effect = new Document();
                effect.ID = _repositoryWrapper.IDRepository.GetID();
                effect.LinkEffectToDocument(parentDocument);

                ISideEffect sideEffect = new Effect_SideEffect_GenerateName(effect);
                sideEffect.Execute();

                _repositoryWrapper.DocumentRepository.Insert(effect);

                // ToViewModel
                ChildDocumentListItemViewModel listItemViewModel = effect.ToChildDocumentListItemViewModel();
                ViewModel.Document.EffectList.List.Add(listItemViewModel);
                ViewModel.Document.EffectList.List = ViewModel.Document.EffectList.List.OrderBy(x => x.Name).ToList();

                ChildDocumentPropertiesViewModel propertiesViewModel = effect.ToChildDocumentPropertiesViewModel();
                ViewModel.Document.EffectPropertiesList.Add(propertiesViewModel);

                ChildDocumentViewModel documentViewModel = effect.ToChildDocumentViewModel(_repositoryWrapper, _entityPositionManager);
                ViewModel.Document.EffectDocumentList.Add(documentViewModel);

                RefreshDocumentTree();
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void EffectDelete(int effectDocumentID)
        {
            try
            {
                // ToViewModel Only
                ViewModel.Document.EffectList.List.RemoveFirst(x => x.ID == effectDocumentID);
                ViewModel.Document.EffectPropertiesList.RemoveFirst(x => x.ID == effectDocumentID);
                ViewModel.Document.EffectDocumentList.RemoveFirst(x => x.ID == effectDocumentID);
                ViewModel.Document.DocumentTree.Effects.RemoveFirst(x => x.Keys.ID == effectDocumentID);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // Instrument Actions

        public void InstrumentListShow()
        {
            try
            {
                _instrumentListPresenter.ViewModel = ViewModel.Document.InstrumentList;
                _instrumentListPresenter.Show();
                DispatchViewModel(_instrumentListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void InstrumentListClose()
        {
            try
            {
                _instrumentListPresenter.ViewModel = ViewModel.Document.InstrumentList;
                _instrumentListPresenter.Close();
                DispatchViewModel(_instrumentListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void InstrumentCreate()
        {
            try
            {
                // ToEntity
                Document parentDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);

                // Business
                var instrument = new Document();
                instrument.ID = _repositoryWrapper.IDRepository.GetID();
                instrument.LinkInstrumentToDocument(parentDocument);

                ISideEffect sideEffect = new Instrument_SideEffect_GenerateName(instrument);
                sideEffect.Execute();

                _repositoryWrapper.DocumentRepository.Insert(instrument);

                // ToViewModel
                ChildDocumentListItemViewModel listItemViewModel = instrument.ToChildDocumentListItemViewModel();
                ViewModel.Document.InstrumentList.List.Add(listItemViewModel);
                ViewModel.Document.InstrumentList.List = ViewModel.Document.InstrumentList.List.OrderBy(x => x.Name).ToList();

                ChildDocumentPropertiesViewModel propertiesViewModel = instrument.ToChildDocumentPropertiesViewModel();
                ViewModel.Document.InstrumentPropertiesList.Add(propertiesViewModel);

                ChildDocumentViewModel documentViewModel = instrument.ToChildDocumentViewModel(_repositoryWrapper, _entityPositionManager);
                ViewModel.Document.InstrumentDocumentList.Add(documentViewModel);

                RefreshDocumentTree();
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void InstrumentDelete(int instrumentDocumentID)
        {
            try
            {
                // ToViewModel Only
                ViewModel.Document.InstrumentList.List.RemoveFirst(x => x.ID == instrumentDocumentID);
                ViewModel.Document.InstrumentPropertiesList.RemoveFirst(x => x.ID == instrumentDocumentID);
                ViewModel.Document.InstrumentDocumentList.RemoveFirst(x => x.ID == instrumentDocumentID);
                ViewModel.Document.DocumentTree.Instruments.RemoveFirst(x => x.Keys.ID == instrumentDocumentID);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // Patch Actions

        public void PatchListShow(int? childDocumentID)
        {
            try
            {
                // Needed to create uncommitted child documents.
                if (childDocumentID.HasValue)
                {
                    Document document = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                }

                int documentID = childDocumentID ?? ViewModel.Document.ID;
                PatchListViewModel patchListViewModel = ChildDocumentHelper.GetPatchListViewModel_ByDocumentID(ViewModel.Document, documentID);
                _patchListPresenter.ViewModel = patchListViewModel;
                _patchListPresenter.Show();
                DispatchViewModel(_patchListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchListClose()
        {
            try
            {
                _patchListPresenter.Close();
                DispatchViewModel(_patchListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchCreate(int? childDocumentID)
        {
            try
            {
                // ToEntity
                Document rootDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                Document document = ChildDocumentHelper.TryGetRootDocumentOrChildDocument(ViewModel.Document.ID, childDocumentID, _repositoryWrapper.DocumentRepository);

                // Business
                var patch = new Patch();
                patch.ID = _repositoryWrapper.IDRepository.GetID();
                patch.LinkTo(document);

                ISideEffect sideEffect = new Patch_SideEffect_GenerateName(patch);
                sideEffect.Execute();

                _repositoryWrapper.PatchRepository.Insert(patch);

                // ToViewModel
                PatchListViewModel listViewModel = ChildDocumentHelper.GetPatchListViewModel_ByDocumentID(ViewModel.Document, document.ID);
                PatchListItemViewModel listItemViewModel = patch.ToListItemViewModel();
                listViewModel.List.Add(listItemViewModel);
                listViewModel.List = listViewModel.List.OrderBy(x => x.Name).ToList();

                IList<PatchDetailsViewModel> detailsViewModels = ChildDocumentHelper.GetPatchDetailsViewModels_ByDocumentID(ViewModel.Document, document.ID);
                PatchDetailsViewModel detailsViewModel = patch.ToDetailsViewModel(_repositoryWrapper.OperatorTypeRepository, _entityPositionManager);
                detailsViewModels.Add(detailsViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDelete(int patchID)
        {
            try
            {
                // ToEntity
                Document rootDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                Patch patch = ChildDocumentHelper.TryGetPatch(rootDocument, patchID);
                if (patch == null)
                {
                    NotFoundViewModel notFoundViewModel = ViewModelHelper.CreateNotFoundViewModel<Patch>();
                    DispatchViewModel(notFoundViewModel);
                    return;
                }
                int documentID = patch.Document.ID;

                // Business
                VoidResult result = _patchManager.DeleteWithRelatedEntities(patch);
                if (result.Successful)
                {
                    // ToViewModel
                    IList<PatchDetailsViewModel> detailsViewModels = ChildDocumentHelper.GetPatchDetailsViewModels_ByDocumentID(ViewModel.Document, documentID);
                    detailsViewModels.RemoveFirst(x => x.Entity.ID == patchID);

                    PatchListViewModel listViewModel = ChildDocumentHelper.GetPatchListViewModel_ByDocumentID(ViewModel.Document, documentID);
                    listViewModel.List.RemoveFirst(x => x.ID == patchID);
                }
                else
                {
                    // ToViewModel
                    ViewModel.PopupMessages = result.Messages;
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDetailsShow(int patchID)
        {
            try
            {
                ChildDocumentItemAlternativeKey key = ChildDocumentHelper.GetAlternativePatchKey(ViewModel.Document, patchID);
                PatchDetailsViewModel detailsViewModel = ChildDocumentHelper.GetPatchDetailsViewModel_ByAlternativeKey(ViewModel.Document, key);
                _patchDetailsPresenter.ViewModel = detailsViewModel;
                _patchDetailsPresenter.Show();
                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDetailsClose()
        {
            try
            {
                // ToEntity
                int patchID = _patchDetailsPresenter.ViewModel.Entity.ID;
                Document rootDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                Patch patch = ChildDocumentHelper.GetPatch(rootDocument, patchID);
                int documentID = patch.Document.ID;

                // Partial Action
                _patchDetailsPresenter.Close();

                if (_patchDetailsPresenter.ViewModel.Successful)
                {
                    PatchListViewModel listViewModel = ChildDocumentHelper.GetPatchListViewModel_ByDocumentID(ViewModel.Document, documentID);
                    RefreshPatchList(listViewModel);
                }

                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDetailsLoseFocus()
        {
            try
            {
                // ToEntity
                int patchID = _patchDetailsPresenter.ViewModel.Entity.ID;
                Document rootDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                Patch patch = ChildDocumentHelper.GetPatch(rootDocument, patchID);
                int documentID = patch.Document.ID;

                // Partial Action
                _patchDetailsPresenter.LoseFocus();

                if (_patchDetailsPresenter.ViewModel.Successful)
                {
                    PatchListViewModel listViewModel = ChildDocumentHelper.GetPatchListViewModel_ByDocumentID(ViewModel.Document, documentID);
                    RefreshPatchList(listViewModel);
                }

                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDetailsAddOperator(int operatorTypeID)
        {
            try
            {
                _patchDetailsPresenter.AddOperator(operatorTypeID);
                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDetailsMoveOperator(int operatorID, float centerX, float centerY)
        {
            try
            {
                _patchDetailsPresenter.MoveOperator(operatorID, centerX, centerY);
                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDetailsChangeInputOutlet(int inletID, int inputOutletID)
        {
            try
            {
                _patchDetailsPresenter.ChangeInputOutlet(inletID, inputOutletID);
                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDetailsSelectOperator(int operatorID)
        {
            try
            {
                _patchDetailsPresenter.SelectOperator(operatorID);
                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        /// <summary>
        /// Deletes the selected operator. Does not delete anything, if no operator is selected.
        /// </summary>
        public void PatchDetailsDeleteOperator()
        {
            try
            {
                _patchDetailsPresenter.DeleteOperator();
                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchDetailsSetValue(string value)
        {
            try
            {
                _patchDetailsPresenter.SetValue(value);
                DispatchViewModel(_patchDetailsPresenter.ViewModel);

                // Move messages to popup messages, because the default dispatching for PatchDetailsViewModel moves it to the ValidationMessages.
                ViewModel.PopupMessages.AddRange(_patchDetailsPresenter.ViewModel.ValidationMessages);
                _patchDetailsPresenter.ViewModel.ValidationMessages.Clear();

                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void PatchPlay(double duration, string sampleFilePath, string outputFilePath)
        {
            try
            {
                _patchDetailsPresenter.Play(duration, sampleFilePath, outputFilePath, _repositoryWrapper);
                DispatchViewModel(_patchDetailsPresenter.ViewModel);

                // Move messages to popup messages, because the default dispatching for PatchDetailsViewModel moves it to the ValidationMessages.
                ViewModel.PopupMessages.AddRange(_patchDetailsPresenter.ViewModel.ValidationMessages);
                _patchDetailsPresenter.ViewModel.ValidationMessages.Clear();

                ViewModel.Successful = _patchDetailsPresenter.ViewModel.Successful;

                DispatchViewModel(_patchDetailsPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // Sample Actions

        public void SampleListShow(int? childDocumentID)
        {
            try
            {
                // Needed to create uncommitted child documents.
                if (childDocumentID.HasValue)
                {
                    Document document = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                }

                int documentID = childDocumentID ?? ViewModel.Document.ID;
                SampleListViewModel sampleListViewModel = ChildDocumentHelper.GetSampleListViewModel_ByDocumentID(ViewModel.Document, documentID);
                _sampleListPresenter.ViewModel = sampleListViewModel;
                _sampleListPresenter.Show();
                DispatchViewModel(_sampleListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void SampleListClose()
        {
            try
            {
                _sampleListPresenter.Close();
                DispatchViewModel(_sampleListPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void SampleCreate(int? childDocumentID)
        {
            try
            {
                // ToEntity
                Document rootDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                Document document = ChildDocumentHelper.TryGetRootDocumentOrChildDocument(ViewModel.Document.ID, childDocumentID, _repositoryWrapper.DocumentRepository);

                // Business
                Sample sample = _sampleManager.CreateSample();
                sample.LinkTo(document);

                ISideEffect sideEffect = new Sample_SideEffect_GenerateName(sample);
                sideEffect.Execute();

                // ToViewModel
                SampleListViewModel listViewModel = ChildDocumentHelper.GetSampleListViewModel_ByDocumentID(ViewModel.Document, document.ID);
                SampleListItemViewModel listItemViewModel = sample.ToListItemViewModel();
                listViewModel.List.Add(listItemViewModel);
                listViewModel.List = listViewModel.List.OrderBy(x => x.Name).ToList();

                IList<SamplePropertiesViewModel> propertiesViewModels = ChildDocumentHelper.GetSamplePropertiesViewModels_ByDocumentID(ViewModel.Document, document.ID);
                SamplePropertiesViewModel propertiesViewModel = sample.ToPropertiesViewModel(new SampleRepositories(_repositoryWrapper));
                propertiesViewModels.Add(propertiesViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void SampleDelete(int sampleID)
        {
            try
            {
                // ToEntity
                Document rootDocument = ViewModel.ToEntityWithRelatedEntities(_repositoryWrapper);
                Sample sample = ChildDocumentHelper.TryGetSample(rootDocument, sampleID);
                if (sample == null)
                {
                    NotFoundViewModel notFoundViewModel = ViewModelHelper.CreateNotFoundViewModel<Sample>();
                    DispatchViewModel(notFoundViewModel);
                    return;
                }
                int documentID = sample.Document.ID;

                // Business
                VoidResult result = _sampleManager.DeleteWithRelatedEntities(sample);
                if (result.Successful)
                {
                    // ToViewModel
                    IList<SamplePropertiesViewModel> propertiesViewModels = ChildDocumentHelper.GetSamplePropertiesViewModels_ByDocumentID(ViewModel.Document, documentID);
                    propertiesViewModels.RemoveFirst(x => x.Entity.ID == sampleID);

                    SampleListViewModel listViewModel = ChildDocumentHelper.GetSampleListViewModel_ByDocumentID(ViewModel.Document, documentID);
                    listViewModel.List.RemoveFirst(x => x.ID == sampleID);
                }
                else
                {
                    // ToViewModel
                    ViewModel.PopupMessages = result.Messages;
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void SamplePropertiesShow(int sampleID)
        {
            try
            {
                ChildDocumentItemAlternativeKey key = ChildDocumentHelper.GetAlternativeSampleKey(ViewModel.Document, sampleID);
                SamplePropertiesViewModel propertiesViewModel = ChildDocumentHelper.GetSamplePropertiesViewModel_ByAlternativeKey(ViewModel.Document, key);
                _samplePropertiesPresenter.ViewModel = propertiesViewModel;
                _samplePropertiesPresenter.Show();

                DispatchViewModel(_samplePropertiesPresenter.ViewModel);
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void SamplePropertiesClose()
        {
            try
            {
                _samplePropertiesPresenter.Close();
                DispatchViewModel(_samplePropertiesPresenter.ViewModel);

                if (_samplePropertiesPresenter.ViewModel.Successful)
                {
                    // Update list
                    int sampleID = _samplePropertiesPresenter.ViewModel.Entity.ID;
                    Sample sample = _repositoryWrapper.SampleRepository.Get(sampleID);
                    SampleListViewModel listViewModel = ChildDocumentHelper.GetSampleListViewModel_ByDocumentID(ViewModel.Document, sample.Document.ID);
                    _sampleListPresenter.ViewModel = listViewModel;
                    _sampleListPresenter.RefreshListItem(sampleID);
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        public void SamplePropertiesLoseFocus()
        {
            try
            {
                _samplePropertiesPresenter.LoseFocus();
                DispatchViewModel(_samplePropertiesPresenter.ViewModel);

                if (_samplePropertiesPresenter.ViewModel.Successful)
                {
                    // Update list item
                    int sampleID = _samplePropertiesPresenter.ViewModel.Entity.ID;
                    Sample sample = _repositoryWrapper.SampleRepository.Get(sampleID);

                    ChildDocumentItemAlternativeKey key = ChildDocumentHelper.GetAlternativeSampleKey(ViewModel.Document, sampleID);
                    SampleListViewModel listViewModel = ChildDocumentHelper.GetSampleListViewModel_ByAlternativeKey(ViewModel.Document, key);
                    _sampleListPresenter.ViewModel = listViewModel;
                    _sampleListPresenter.RefreshListItem(sampleID);
                }
            }
            finally
            {
                _repositoryWrapper.Rollback();
            }
        }

        // DispatchViewModel

        private Dictionary<Type, Action<object>> _dispatchDelegateDictionary;

        private Dictionary<Type, Action<object>> CreateDispatchDelegateDictionary()
        {
            var dictionary = new Dictionary<Type, Action<object>>
            {
                { typeof(AudioFileOutputListViewModel), DispatchAudioFileOutputListViewModel },
                { typeof(AudioFileOutputPropertiesViewModel), DispatchAudioFileOutputPropertiesViewModel },
                { typeof(ChildDocumentListViewModel), DispatchChildDocumentListViewModel },
                { typeof(ChildDocumentPropertiesViewModel), DispatchChildDocumentPropertiesViewModel },
                { typeof(CurveDetailsViewModel), DispatchCurveDetailsViewModel },
                { typeof(CurveListViewModel), DispatchCurveListViewModel },
                { typeof(DocumentCannotDeleteViewModel), DispatchDocumentCannotDeleteViewModel },
                { typeof(DocumentDeletedViewModel), DispatchDocumentDeletedViewModel },
                { typeof(DocumentDeleteViewModel), DispatchDocumentDeleteViewModel },
                { typeof(DocumentDetailsViewModel), DispatchDocumentDetailsViewModel },
                { typeof(DocumentListViewModel), DispatchDocumentListViewModel },
                { typeof(DocumentPropertiesViewModel), DispatchDocumentPropertiesViewModel },
                { typeof(DocumentTreeViewModel), DispatchDocumentTreeViewModel },
                { typeof(MenuViewModel), DispatchMenuViewModel },
                { typeof(NotFoundViewModel), DispatchNotFoundViewModel },
                { typeof(PatchDetailsViewModel), DispatchPatchDetailsViewModel },
                { typeof(PatchListViewModel), DispatchPatchListViewModel },
                { typeof(SampleListViewModel), DispatchSampleListViewModel },
                { typeof(SamplePropertiesViewModel), DispatchSamplePropertiesViewModel },
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

        private void DispatchAudioFileOutputListViewModel(object viewModel2)
        {
            var audioFileOutputListViewModel = (AudioFileOutputListViewModel)viewModel2;
            
            ViewModel.Document.AudioFileOutputList = (AudioFileOutputListViewModel)viewModel2;

            if (audioFileOutputListViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                audioFileOutputListViewModel.Visible = true;
            }
        }

        private void DispatchAudioFileOutputPropertiesViewModel(object viewModel2)
        {
            var audioFileOutputPropertiesViewModel = (AudioFileOutputPropertiesViewModel)viewModel2;

            if (audioFileOutputPropertiesViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                audioFileOutputPropertiesViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(audioFileOutputPropertiesViewModel.ValidationMessages);
            audioFileOutputPropertiesViewModel.ValidationMessages.Clear();
        }

        private void DispatchChildDocumentListViewModel(object viewModel2)
        {
            var childDocumentListViewModel = (ChildDocumentListViewModel)viewModel2;

            switch (childDocumentListViewModel.Keys.ChildDocumentTypeEnum)
            {
                case ChildDocumentTypeEnum.Instrument:
                    ViewModel.Document.InstrumentList = childDocumentListViewModel;
                    break;

                case ChildDocumentTypeEnum.Effect:
                    ViewModel.Document.EffectList = childDocumentListViewModel;
                    break;

                default:
                    throw new ValueNotSupportedException(childDocumentListViewModel.Keys.ChildDocumentTypeEnum);
            }

            if (childDocumentListViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                childDocumentListViewModel.Visible = true;
            }
        }

        private void DispatchChildDocumentPropertiesViewModel(object viewModel2)
        {
            var childDocumentPropertiesViewModel = (ChildDocumentPropertiesViewModel)viewModel2;

            if (childDocumentPropertiesViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                childDocumentPropertiesViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(childDocumentPropertiesViewModel.ValidationMessages);
            childDocumentPropertiesViewModel.ValidationMessages.Clear();
        }

        private void DispatchCurveDetailsViewModel(object viewModel2)
        {
            var curveDetailsViewModel = (CurveDetailsViewModel)viewModel2;

            if (curveDetailsViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                curveDetailsViewModel.Visible = true;
            }

            ViewModel.ValidationMessages.AddRange(curveDetailsViewModel.ValidationMessages);
            curveDetailsViewModel.ValidationMessages.Clear();
        }

        private void DispatchCurveListViewModel(object viewModel2)
        {
            var curveListViewModel = (CurveListViewModel)viewModel2;

            if (!curveListViewModel.ChildDocumentID.HasValue)
            {
                ViewModel.Document.CurveList = curveListViewModel;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = ChildDocumentHelper.GetChildDocumentViewModel_ByID(ViewModel.Document, curveListViewModel.ChildDocumentID.Value);
                childDocumentViewModel.CurveList = curveListViewModel;
            }

            if (curveListViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                curveListViewModel.Visible = true;
            }
        }

        private void DispatchDocumentCannotDeleteViewModel(object viewModel2)
        {
            var documentCannotDeleteViewModel = (DocumentCannotDeleteViewModel)viewModel2;

            ViewModel.DocumentCannotDelete = documentCannotDeleteViewModel;
        }

        private void DispatchDocumentDeletedViewModel(object viewModel2)
        {
            var documentDeletedViewModel = (DocumentDeletedViewModel)viewModel2;

            ViewModel.DocumentDeleted = documentDeletedViewModel;

            // TODO: This is quite an assumption.
            ViewModel.DocumentDelete.Visible = false;
            ViewModel.DocumentDetails.Visible = false;

            if (!documentDeletedViewModel.Visible)
            {
                // Also: this might better be done in the action method.
                RefreshDocumentList();
            }
        }

        private void DispatchDocumentDeleteViewModel(object viewModel2)
        {
            var documentDeleteViewModel = (DocumentDeleteViewModel)viewModel2;
            ViewModel.DocumentDelete = documentDeleteViewModel;
        }

        private void DispatchDocumentDetailsViewModel(object viewModel2)
        {
            var documentDetailsViewModel = (DocumentDetailsViewModel)viewModel2;
            
            ViewModel.DocumentDetails = documentDetailsViewModel;

            if (documentDetailsViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                documentDetailsViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(documentDetailsViewModel.ValidationMessages);
            documentDetailsViewModel.ValidationMessages.Clear();
        }

        private void DispatchDocumentListViewModel(object viewModel2)
        {
            var documentListViewModel = (DocumentListViewModel)viewModel2;

            ViewModel.DocumentList = documentListViewModel;

            if (documentListViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                documentListViewModel.Visible = true;
            }
        }

        private void DispatchDocumentPropertiesViewModel(object viewModel2)
        {
            var documentPropertiesViewModel = (DocumentPropertiesViewModel)viewModel2;

            ViewModel.Document.DocumentProperties = documentPropertiesViewModel;

            if (documentPropertiesViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                documentPropertiesViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(documentPropertiesViewModel.ValidationMessages);
            documentPropertiesViewModel.ValidationMessages.Clear();
        }

        private void DispatchDocumentTreeViewModel(object viewModel2)
        {
            var documentTreeViewModel = (DocumentTreeViewModel)viewModel2;
            ViewModel.Document.DocumentTree = documentTreeViewModel;
        }

        private void DispatchMenuViewModel(object viewModel2)
        {
            var menuViewModel = (MenuViewModel)viewModel2;
            ViewModel.Menu = menuViewModel;
        }

        private void DispatchNotFoundViewModel(object viewModel2)
        {
            var notFoundViewModel = (NotFoundViewModel)viewModel2;

            ViewModel.NotFound = notFoundViewModel;

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

        private void DispatchPatchDetailsViewModel(object viewModel2)
        {
            var patchDetailsViewModel = (PatchDetailsViewModel)viewModel2;

            if (patchDetailsViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                patchDetailsViewModel.Visible = true;
            }

            ViewModel.ValidationMessages.AddRange(patchDetailsViewModel.ValidationMessages);
            patchDetailsViewModel.ValidationMessages.Clear();
        }

        private void DispatchPatchListViewModel(object viewModel2)
        {
            var patchListViewModel = (PatchListViewModel)viewModel2;

            if (!patchListViewModel.ChildDocumentID.HasValue)
            {
                ViewModel.Document.PatchList = patchListViewModel;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = ChildDocumentHelper.GetChildDocumentViewModel_ByID(ViewModel.Document, patchListViewModel.ChildDocumentID.Value);
                childDocumentViewModel.PatchList = patchListViewModel;
            }

            if (patchListViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                patchListViewModel.Visible = true;
            }
        }

        private void DispatchSamplePropertiesViewModel(object viewModel2)
        {
            var samplePropertiesViewModel = (SamplePropertiesViewModel)viewModel2;

            if (samplePropertiesViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                samplePropertiesViewModel.Visible = true;
            }

            ViewModel.ValidationMessages.AddRange(samplePropertiesViewModel.ValidationMessages);
            samplePropertiesViewModel.ValidationMessages.Clear();
        }

        private void DispatchSampleListViewModel(object viewModel2)
        {
            var sampleListViewModel = (SampleListViewModel)viewModel2;

            if (!sampleListViewModel.ChildDocumentID.HasValue)
            {
                ViewModel.Document.SampleList = sampleListViewModel;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = ChildDocumentHelper.GetChildDocumentViewModel_ByID(ViewModel.Document, sampleListViewModel.ChildDocumentID.Value);
                childDocumentViewModel.SampleList = sampleListViewModel;
            }

            if (sampleListViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                sampleListViewModel.Visible = true;
            }
        }

        // Helpers

        private void HideAllListAndDetailViewModels()
        {
            ViewModel.DocumentList.Visible = false;
            ViewModel.DocumentDetails.Visible = false;

            ViewModel.Document.InstrumentList.Visible = false;
            ViewModel.Document.EffectList.Visible = false;
            ViewModel.Document.SampleList.Visible = false;
            ViewModel.Document.CurveList.Visible = false;
            ViewModel.Document.AudioFileOutputList.Visible = false;
            ViewModel.Document.PatchList.Visible = false;

            foreach (CurveDetailsViewModel curveDetailsViewModel in ViewModel.Document.CurveDetailsList)
            {
                curveDetailsViewModel.Visible = false;
            }

            foreach (PatchDetailsViewModel patchDetailsViewModel in ViewModel.Document.PatchDetailsList)
            {
                patchDetailsViewModel.Visible = false;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in ViewModel.Document.InstrumentDocumentList)
            {
                childDocumentViewModel.SampleList.Visible = false;
                childDocumentViewModel.CurveList.Visible = false;
                childDocumentViewModel.PatchList.Visible = false;

                foreach (PatchDetailsViewModel patchDetailsViewModel in childDocumentViewModel.PatchDetailsList)
                {
                    patchDetailsViewModel.Visible = false;
                }
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in ViewModel.Document.EffectDocumentList)
            {
                childDocumentViewModel.SampleList.Visible = false;
                childDocumentViewModel.CurveList.Visible = false;
                childDocumentViewModel.PatchList.Visible = false;

                foreach (PatchDetailsViewModel patchDetailsViewModel in childDocumentViewModel.PatchDetailsList)
                {
                    patchDetailsViewModel.Visible = false;
                }
            }
        }

        private void HideAllPropertiesViewModels()
        {
            ViewModel.DocumentDetails.Visible = false;
            ViewModel.Document.DocumentProperties.Visible = false;
            ViewModel.Document.AudioFileOutputPropertiesList.ForEach(x => x.Visible = false);
            ViewModel.Document.CurveDetailsList.ForEach(x => x.Visible = false);
            ViewModel.Document.EffectPropertiesList.ForEach(x => x.Visible = false);
            ViewModel.Document.InstrumentPropertiesList.ForEach(x => x.Visible = false);
            ViewModel.Document.SamplePropertiesList.ForEach(x => x.Visible = false);

            // Note that the Samples are the only ones with a Properties view inside the child documents.
            ViewModel.Document.EffectDocumentList.SelectMany(x => x.SamplePropertiesList).ForEach(x => x.Visible = false);
            ViewModel.Document.InstrumentDocumentList.SelectMany(x => x.SamplePropertiesList).ForEach(x => x.Visible = false);
        }

        private void RefreshDocumentList()
        {
            _documentListPresenter.Refresh();
            ViewModel.DocumentList  = _documentListPresenter.ViewModel;
        }

        private void RefreshDocumentTree()
        {
            _documentTreePresenter.ViewModel = ViewModel.Document.DocumentTree;
            object viewModel2 = _documentTreePresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshAudioFileOutputList()
        {
            object viewModel2 = _audioFileOutputListPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshSampleList(SampleListViewModel sampleListViewModel)
        {
            _sampleListPresenter.ViewModel = sampleListViewModel;
            object viewModel2 = _sampleListPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshPatchList(PatchListViewModel patchListViewModel)
        {
            _patchListPresenter.ViewModel = patchListViewModel;
            object viewModel2 = _patchListPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }
    }
}
