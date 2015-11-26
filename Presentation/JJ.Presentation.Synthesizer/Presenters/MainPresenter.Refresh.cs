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
using System;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Presentation.Synthesizer.Presenters
{
    public partial class MainPresenter
    {
        private void RefreshAudioFileOutputGrid()
        {
            object viewModel2 = _audioFileOutputGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshCurveGrid(CurveGridViewModel curveGridViewModel)
        {
            _curveGridPresenter.ViewModel = curveGridViewModel;
            object viewModel2 = _curveGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshCurveGridItem(int curveID)
        {
            CurveGridViewModel gridViewModel = DocumentViewModelHelper.GetCurveGridViewModel_ByCurveID(ViewModel.Document, curveID);
            _curveGridPresenter.ViewModel = gridViewModel;
            _curveGridPresenter.RefreshListItem(curveID);
        }

        private void RefreshCurveLookupsItems(int curveID)
        {
            Curve curve = _repositories.CurveRepository.Get(curveID);

            // Update curve lookup
            IDAndName idAndName = ViewModel.Document.CurveLookup.Where(x => x.ID == curve.ID).FirstOrDefault();
            if (idAndName != null)
            {
                idAndName.Name = curve.Name;
                ViewModel.Document.CurveLookup = ViewModel.Document.CurveLookup.OrderBy(x => x.Name).ToList();
            }
            foreach (PatchDocumentViewModel patchDocumentViewModel in ViewModel.Document.PatchDocumentList)
            {
                IDAndName idAndName2 = patchDocumentViewModel.CurveLookup.Where(x => x.ID == curve.ID).FirstOrDefault();
                if (idAndName2 != null)
                {
                    idAndName2.Name = curve.Name;
                    patchDocumentViewModel.CurveLookup = patchDocumentViewModel.CurveLookup.OrderBy(x => x.Name).ToList();
                }
            }
        }

        private void RefreshCurveDetailsNode(int nodeID)
        {
            // TODO: This is not very fast.
            CurveDetailsViewModel detailsViewModel = DocumentViewModelHelper.GetCurveDetailsViewModel_ByNodeID(ViewModel.Document, nodeID);

            // Remove original node
            detailsViewModel.Entity.Nodes.RemoveFirst(x => x.ID == nodeID);

            // Add new version of the node
            Node node = _repositories.NodeRepository.Get(nodeID);
            NodeViewModel nodeViewModel = node.ToViewModel();
            detailsViewModel.Entity.Nodes.Add(nodeViewModel);
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

        private void Refresh_OperatorProperties_ForCustomOperatorViewModels(int underlyingDocumentID)
        {
            foreach (OperatorPropertiesViewModel_ForCustomOperator propertiesViewModel in
                ViewModel.Document.PatchDocumentList.SelectMany(x => x.OperatorPropertiesList_ForCustomOperators))
            {
                _operatorPropertiesPresenter_ForCustomOperator.ViewModel = propertiesViewModel;
                _operatorPropertiesPresenter_ForCustomOperator.Refresh();
            }
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
            IList<PatchDetailsViewModel> patchDetailsViewModels = ViewModel.Document.PatchDocumentList.Select(x => x.PatchDetails).ToArray();

            IList<OperatorViewModel> operatorViewModels =
                patchDetailsViewModels.SelectMany(x => x.Entity.Operators)
                                      .Where(x => x.OperatorType.ID == (int)OperatorTypeEnum.CustomOperator)
                                      .ToArray();

            foreach (OperatorViewModel operatorViewModel in operatorViewModels)
            {
                RefreshPatchDetailsOperator(operatorViewModel);
            }
        }

        private void RefreshPatchDetailsOperator(int operatorID)
        {
            OperatorViewModel viewModel = DocumentViewModelHelper.GetOperatorViewModel(ViewModel.Document, operatorID);
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
            ViewModelHelper.RefreshViewModel_WithInletsAndOutlets_WithoutEntityPosition(
                entity, operatorViewModel,
                _repositories.SampleRepository, _repositories.CurveRepository, _repositories.DocumentRepository);
        }

        private void RefreshPatchGrid(string group)
        {
            PatchGridViewModel viewModel2 = DocumentViewModelHelper.GetPatchGridViewModel_ByGroup(ViewModel.Document, group);

            RefreshPatchGrid(viewModel2);
        }

        private void RefreshPatchGrids()
        {
            foreach (PatchGridViewModel viewModel in ViewModel.Document.PatchGridList.ToArray())
            {
                RefreshPatchGrid(viewModel);
            }
        }

        private void RefreshPatchGrid(PatchGridViewModel patchGridViewModel)
        {
            if (patchGridViewModel == null) throw new NullException(() => patchGridViewModel);
            _patchGridPresenter.ViewModel = patchGridViewModel;
            _patchGridPresenter.Refresh();

            DispatchViewModel(patchGridViewModel);
        }

        private void RefreshSampleGrid(SampleGridViewModel sampleGridViewModel)
        {
            _sampleGridPresenter.ViewModel = sampleGridViewModel;
            object viewModel2 = _sampleGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshSampleGridItem(int sampleID)
        {
            SampleGridViewModel gridViewModel = DocumentViewModelHelper.GetSampleGridViewModel_BySampleID(ViewModel.Document, sampleID);
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
            foreach (PatchDocumentViewModel patchDocumentViewModel in ViewModel.Document.PatchDocumentList)
            {
                IDAndName idAndName2 = patchDocumentViewModel.SampleLookup.Where(x => x.ID == sample.ID).FirstOrDefault();
                if (idAndName2 != null)
                {
                    idAndName2.Name = sample.Name;
                    patchDocumentViewModel.SampleLookup = patchDocumentViewModel.SampleLookup.OrderBy(x => x.Name).ToList();
                }
            }
        }

        private void RefreshScaleGrid()
        {
            object viewModel2 = _scaleGridPresenter.Refresh();
            DispatchViewModel(viewModel2);
        }

        private void RefreshToneGridEdit(int scaleID)
        {
            ToneGridEditViewModel viewModel = DocumentViewModelHelper.GetToneGridEditViewModel(ViewModel.Document, scaleID);
            RefreshToneGridEdit(viewModel);
        }

        private void RefreshToneGridEdit(ToneGridEditViewModel viewModel)
        {
            _toneGridEditPresenter.ViewModel = viewModel;
            _toneGridEditPresenter.Refresh();
            DispatchViewModel(_toneGridEditPresenter.ViewModel);
        }

        private void RefreshUnderylingDocumentLookup()
        {
            Document rootDocument = _repositories.DocumentRepository.Get(ViewModel.Document.ID);
            ViewModel.Document.UnderlyingDocumentLookup = ViewModelHelper.CreateUnderlyingDocumentLookupViewModel(rootDocument.ChildDocuments);
        }
    }
}
