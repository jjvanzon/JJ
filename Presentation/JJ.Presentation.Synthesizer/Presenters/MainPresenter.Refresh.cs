﻿using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Common;
using JJ.Data.Synthesizer;
using JJ.Business.CanonicalModel;
using JJ.Business.Synthesizer.Enums;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ToViewModel;

namespace JJ.Presentation.Synthesizer.Presenters
{
    public partial class MainPresenter
    {
        private void RefreshAudioFileOutputGrid()
        {
            object viewModel2 = _audioFileOutputGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshChildDocumentGrid(ChildDocumentTypeEnum childDocumentTypeEnum)
        {
            switch (childDocumentTypeEnum)
            {
                case ChildDocumentTypeEnum.Instrument:
                    RefreshInstrumentGrid();
                    break;

                case ChildDocumentTypeEnum.Effect:
                    RefreshEffectGrid();
                    break;

                default:
                    throw new InvalidValueException(childDocumentTypeEnum);
            }
        }

        private void RefreshDocumentGrid()
        {
            _documentGridPresenter.Refresh();
            ViewModel.DocumentGrid = _documentGridPresenter.ViewModel;
        }

        private void RefreshDocumentTree()
        {
            object viewModel2 = _documentTreePresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshInstrumentGrid()
        {
            object viewModel2 = _instrumentGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshEffectGrid()
        {
            object viewModel2 = _effectGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void Refresh_OperatorProperties_ForCustomOperatorViewModels(int underlyingDocumentID)
        {
            foreach (OperatorPropertiesViewModel_ForCustomOperator propertiesViewModel in
                ViewModel.Document.OperatorPropertiesList_ForCustomOperators.Union(
                ViewModel.Document.ChildDocumentList.SelectMany(x => x.OperatorPropertiesList_ForCustomOperators)))
            {
                _operatorPropertiesPresenter_ForCustomOperator.ViewModel = propertiesViewModel;
                _operatorPropertiesPresenter_ForCustomOperator.Refresh();
            }
        }

        private void RefreshPatchGrid(int documentID)
        {
            PatchGridViewModel gridViewModel = ChildDocumentHelper.GetPatchGridViewModel_ByDocumentID(ViewModel.Document, documentID);
            RefreshPatchGrid(gridViewModel);
        }

        private void RefreshPatchGrid(PatchGridViewModel patchGridViewModel)
        {
            _patchGridPresenter.ViewModel = patchGridViewModel;
            object viewModel2 = _patchGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshPatchDetailsOperator(int operatorID)
        {
            OperatorViewModel viewModel = ChildDocumentHelper.GetOperatorViewModel(ViewModel.Document, operatorID);
            RefreshPatchDetailsOperator(viewModel);
        }

        private void RefreshPatchDetailsOperator(OperatorViewModel viewModel)
        {
            Operator entity = _repositories.OperatorRepository.Get(viewModel.ID);
            RefreshPatchDetailsOperator(entity, viewModel);
        }

        private void RefreshPatchDetailsOperator(Operator entity, OperatorViewModel operatorViewModel)
        {
            // TODO: Not sure if I should also have a variation in which I call UpdateViewModel_WithoutEntityPosition instead.
            ViewModelHelper.UpdateViewModel_WithInletsAndOutlets_WithoutEntityPosition(
                entity, operatorViewModel,
                _repositories.SampleRepository, _repositories.CurveRepository, _repositories.DocumentRepository);
        }

        /// <summary>
        /// When an underlying document of a custom operator is changed,
        /// we do not know which PatchDetails OperatorViewModels are affected,
        /// because no OperatorViewModel has as property saying what UnderlyingDocument it is. 
        /// Therefore we refresh all CustomOperators.
        /// 
        /// But also, a custom operator would need to be updated if something connected to it is deleted,
        /// because then the obsolete inlets and outlets might be cleaned up.
        /// </summary>
        private void RefreshOperatorViewModels_OfTypeCustomOperators()
        {
            IList<PatchDetailsViewModel> patchDetailsViewModels =
                ViewModel.Document.PatchDetailsList.Union(
                ViewModel.Document.ChildDocumentList.SelectMany(x => x.PatchDetailsList))
                .ToArray();

            IList<OperatorViewModel> operatorViewModels =
                patchDetailsViewModels.SelectMany(x => x.Entity.Operators)
                                      .Where(x => x.OperatorType.ID == (int)OperatorTypeEnum.CustomOperator)
                                      .ToArray();

            foreach (OperatorViewModel operatorViewModel in operatorViewModels)
            {
                RefreshPatchDetailsOperator(operatorViewModel);
            }
        }

        private void RefreshSampleGrid(SampleGridViewModel sampleGridViewModel)
        {
            _sampleGridPresenter.ViewModel = sampleGridViewModel;
            object viewModel2 = _sampleGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshSampleGridItem(int sampleID)
        {
            SampleGridViewModel gridViewModel = ChildDocumentHelper.GetSampleGridViewModel_BySampleID(ViewModel.Document, sampleID);
            _sampleGridPresenter.ViewModel = gridViewModel;
            _sampleGridPresenter.RefreshListItem(sampleID);
        }

        private void RefreshSampleLookupsItems(int sampleID)
        {
            Sample sample = _repositories.SampleRepository.Get(sampleID);

            // Update sample lookup
            IDAndName idAndName = ViewModel.Document.SampleLookup.Where(x => x.ID == sample.ID).FirstOrDefault();
            if (idAndName != null)
            {
                idAndName.Name = sample.Name;
                ViewModel.Document.SampleLookup = ViewModel.Document.SampleLookup.OrderBy(x => x.Name).ToList();
            }
            foreach (ChildDocumentViewModel childDocumentViewModel in ViewModel.Document.ChildDocumentList)
            {
                IDAndName idAndName2 = childDocumentViewModel.SampleLookup.Where(x => x.ID == sample.ID).FirstOrDefault();
                if (idAndName2 != null)
                {
                    idAndName2.Name = sample.Name;
                    childDocumentViewModel.SampleLookup = childDocumentViewModel.SampleLookup.OrderBy(x => x.Name).ToList();
                }
            }
        }

        private void RefreshScaleGrid()
        {
            object viewModel2 = _scaleGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshUnderylingDocumentLookup()
        {
            Document rootDocument = _repositories.DocumentRepository.Get(ViewModel.Document.ID);
            ViewModel.Document.UnderlyingDocumentLookup = ViewModelHelper.CreateUnderlyingDocumentLookupViewModel(rootDocument.ChildDocuments);
        }
    }
}
