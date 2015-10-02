﻿using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Common;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using System;
using System.Collections.Generic;

namespace JJ.Presentation.Synthesizer.Presenters
{
    public partial class MainPresenter
    {
        private Dictionary<Type, Action<object>> _dispatchDelegateDictionary;

        private Dictionary<Type, Action<object>> CreateDispatchDelegateDictionary()
        {
            var dictionary = new Dictionary<Type, Action<object>>
            {
                { typeof(AudioFileOutputGridViewModel), DispatchAudioFileOutputGridViewModel },
                { typeof(AudioFileOutputPropertiesViewModel), DispatchAudioFileOutputPropertiesViewModel },
                { typeof(ChildDocumentGridViewModel), DispatchChildDocumentGridViewModel },
                { typeof(ChildDocumentPropertiesViewModel), DispatchChildDocumentPropertiesViewModel },
                { typeof(CurveDetailsViewModel), DispatchCurveDetailsViewModel },
                { typeof(CurveGridViewModel), DispatchCurveGridViewModel },
                { typeof(DocumentCannotDeleteViewModel), DispatchDocumentCannotDeleteViewModel },
                { typeof(DocumentDeletedViewModel), DispatchDocumentDeletedViewModel },
                { typeof(DocumentDeleteViewModel), DispatchDocumentDeleteViewModel },
                { typeof(DocumentDetailsViewModel), DispatchDocumentDetailsViewModel },
                { typeof(DocumentGridViewModel), DispatchDocumentGridViewModel },
                { typeof(DocumentPropertiesViewModel), DispatchDocumentPropertiesViewModel },
                { typeof(DocumentTreeViewModel), DispatchDocumentTreeViewModel },
                { typeof(MenuViewModel), DispatchMenuViewModel },
                { typeof(NotFoundViewModel), DispatchNotFoundViewModel },
                { typeof(OperatorPropertiesViewModel), DispatchOperatorPropertiesViewModel },
                { typeof(OperatorPropertiesViewModel_ForCustomOperator), DispatchOperatorPropertiesViewModel_ForCustomOperator },
                { typeof(OperatorPropertiesViewModel_ForPatchInlet), DispatchOperatorPropertiesViewModel_ForPatchInlet },
                { typeof(OperatorPropertiesViewModel_ForPatchOutlet), DispatchOperatorPropertiesViewModel_ForPatchOutlet },
                { typeof(OperatorPropertiesViewModel_ForSample), DispatchOperatorPropertiesViewModel_ForSample },
                { typeof(OperatorPropertiesViewModel_ForNumber), DispatchOperatorPropertiesViewModel_ForNumber },
                { typeof(PatchDetailsViewModel), DispatchPatchDetailsViewModel },
                { typeof(PatchGridViewModel), DispatchPatchGridViewModel },
                { typeof(SampleGridViewModel), DispatchSampleGridViewModel },
                { typeof(SamplePropertiesViewModel), DispatchSamplePropertiesViewModel },
                { typeof(ToneGridEditViewModel), DispatchToneGridEditViewModel },
                { typeof(ScaleGridViewModel), DispatchScaleGridViewModel },
                { typeof(ScalePropertiesViewModel), DispatchScalePropertiesViewModel },
            };

            return dictionary;
        }

        /// <summary> 
        /// Applies a ViewModel from a partial presenter in the right way to the MainViewModel. 
        /// This can mean assigning a partial ViewModel to a property of the MainViewModel,
        /// but also for instance also yielding over the validation message from a partial
        /// ViewModel to the MainViewModel.
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

        private void DispatchAudioFileOutputGridViewModel(object viewModel2)
        {
            var castedViewModel = (AudioFileOutputGridViewModel)viewModel2;

            ViewModel.Document.AudioFileOutputGrid = (AudioFileOutputGridViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }
        }

        private void DispatchAudioFileOutputPropertiesViewModel(object viewModel2)
        {
            var castedViewModel = (AudioFileOutputPropertiesViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchChildDocumentGridViewModel(object viewModel2)
        {
            var castedViewModel = (ChildDocumentGridViewModel)viewModel2;

            ChildDocumentTypeEnum childDocumentTypeEnum = (ChildDocumentTypeEnum)castedViewModel.ChildDocumentTypeID;

            switch (childDocumentTypeEnum)
            {
                case ChildDocumentTypeEnum.Instrument:
                    ViewModel.Document.InstrumentGrid = castedViewModel;
                    break;

                case ChildDocumentTypeEnum.Effect:
                    ViewModel.Document.EffectGrid = castedViewModel;
                    break;

                default:
                    throw new ValueNotSupportedException(childDocumentTypeEnum);
            }

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }
        }

        private void DispatchChildDocumentPropertiesViewModel(object viewModel2)
        {
            var castedViewModel = (ChildDocumentPropertiesViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchCurveDetailsViewModel(object viewModel2)
        {
            var castedViewModel = (CurveDetailsViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.ValidationMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchCurveGridViewModel(object viewModel2)
        {
            var castedViewModel = (CurveGridViewModel)viewModel2;

            bool isRootDocument = ViewModel.Document.ID == castedViewModel.DocumentID;
            if (isRootDocument)
            {
                ViewModel.Document.CurveGrid = castedViewModel;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = ChildDocumentHelper.GetChildDocumentViewModel(ViewModel.Document, castedViewModel.DocumentID);
                childDocumentViewModel.CurveGrid = castedViewModel;
            }

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }
        }

        private void DispatchDocumentCannotDeleteViewModel(object viewModel2)
        {
            var castedViewModel = (DocumentCannotDeleteViewModel)viewModel2;

            ViewModel.DocumentCannotDelete = castedViewModel;
        }

        private void DispatchDocumentDeletedViewModel(object viewModel2)
        {
            var castedViewModel = (DocumentDeletedViewModel)viewModel2;

            ViewModel.DocumentDeleted = castedViewModel;

            // TODO: This is quite an assumption.
            ViewModel.DocumentDelete.Visible = false;
            ViewModel.DocumentDetails.Visible = false;

            if (!castedViewModel.Visible)
            {
                // Also: this might better be done in the action method.
                RefreshDocumentGrid();
            }
        }

        private void DispatchDocumentDeleteViewModel(object viewModel2)
        {
            var castedViewModel = (DocumentDeleteViewModel)viewModel2;
            ViewModel.DocumentDelete = castedViewModel;
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

        private void DispatchDocumentGridViewModel(object viewModel2)
        {
            var castedViewModel = (DocumentGridViewModel)viewModel2;

            ViewModel.DocumentGrid = castedViewModel;

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }
        }

        private void DispatchDocumentPropertiesViewModel(object viewModel2)
        {
            var castedViewModel = (DocumentPropertiesViewModel)viewModel2;

            ViewModel.Document.DocumentProperties = castedViewModel;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchDocumentTreeViewModel(object viewModel2)
        {
            var castedViewModel = (DocumentTreeViewModel)viewModel2;
            ViewModel.Document.DocumentTree = castedViewModel;
        }

        private void DispatchMenuViewModel(object viewModel2)
        {
            var castedViewModel = (MenuViewModel)viewModel2;
            ViewModel.Menu = castedViewModel;
        }

        private void DispatchNotFoundViewModel(object viewModel2)
        {
            var castedViewModel = (NotFoundViewModel)viewModel2;

            ViewModel.NotFound = castedViewModel;

            // HACK: Checking visibility of the NotFound view model
            // prevents refreshing the DocumentList twice:
            // once when showing the NotFound view model,
            // a second time when clicking OK on it.

            // TODO: Low priority: Eventually the NotFoundViewModel will create even more ambiguity,
            // when it is reused for multiple entity types.

            if (castedViewModel.Visible)
            {
                RefreshDocumentGrid();
            }
        }

        private void DispatchOperatorPropertiesViewModel(object viewModel2)
        {
            var castedViewModel = (OperatorPropertiesViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchOperatorPropertiesViewModel_ForCustomOperator(object viewModel2)
        {
            var castedViewModel = (OperatorPropertiesViewModel_ForCustomOperator)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchOperatorPropertiesViewModel_ForPatchInlet(object viewModel2)
        {
            var castedViewModel = (OperatorPropertiesViewModel_ForPatchInlet)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchOperatorPropertiesViewModel_ForPatchOutlet(object viewModel2)
        {
            var castedViewModel = (OperatorPropertiesViewModel_ForPatchOutlet)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchOperatorPropertiesViewModel_ForSample(object viewModel2)
        {
            var castedViewModel = (OperatorPropertiesViewModel_ForSample)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchOperatorPropertiesViewModel_ForNumber(object viewModel2)
        {
            var castedViewModel = (OperatorPropertiesViewModel_ForNumber)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchPatchDetailsViewModel(object viewModel2)
        {
            var castedViewModel = (PatchDetailsViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.ValidationMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchPatchGridViewModel(object viewModel2)
        {
            var castedViewModel = (PatchGridViewModel)viewModel2;

            bool isRootDocument = ViewModel.Document.ID == castedViewModel.DocumentID;
            if (isRootDocument)
            {
                ViewModel.Document.PatchGrid = castedViewModel;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = ChildDocumentHelper.GetChildDocumentViewModel(ViewModel.Document, castedViewModel.DocumentID);
                childDocumentViewModel.PatchGrid = castedViewModel;
            }

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }
        }

        private void DispatchSamplePropertiesViewModel(object viewModel2)
        {
            var castedViewModel = (SamplePropertiesViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.ValidationMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchSampleGridViewModel(object viewModel2)
        {
            var castedViewModel = (SampleGridViewModel)viewModel2;

            bool isRootDocument = ViewModel.Document.ID == castedViewModel.DocumentID;
            if (isRootDocument)
            {
                ViewModel.Document.SampleGrid = castedViewModel;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = ChildDocumentHelper.GetChildDocumentViewModel(ViewModel.Document, castedViewModel.DocumentID);
                childDocumentViewModel.SampleGrid = castedViewModel;
            }

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }
        }

        private void DispatchScaleGridViewModel(object viewModel2)
        {
            var castedViewModel = (ScaleGridViewModel)viewModel2;

            ViewModel.Document.ScaleGrid = (ScaleGridViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }
        }

        private void DispatchToneGridEditViewModel(object viewModel2)
        {
            var castedViewModel = (ToneGridEditViewModel)viewModel2;

            int? index = ViewModel.Document.ToneGridEditList.TryGetIndexOf(x => x.ScaleID == castedViewModel.ScaleID);
            if (index.HasValue)
            {
                ViewModel.Document.ToneGridEditList[index.Value] = castedViewModel;
            }
            else
            {
                ViewModel.Document.ToneGridEditList.Add(castedViewModel);
            }

            if (castedViewModel.Visible)
            {
                HideAllListAndDetailViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }

        private void DispatchScalePropertiesViewModel(object viewModel2)
        {
            var castedViewModel = (ScalePropertiesViewModel)viewModel2;

            if (castedViewModel.Visible)
            {
                HideAllPropertiesViewModels();
                castedViewModel.Visible = true;
            }

            ViewModel.PopupMessages.AddRange(castedViewModel.ValidationMessages);
            castedViewModel.ValidationMessages.Clear();
        }
    }
}
