﻿using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Presentation.Synthesizer.Helpers
{
    internal static class ChildDocumentHelper
    {
        // ChildDocument

        public static ChildDocumentViewModel GetChildDocumentViewModel(DocumentViewModel rootDocumentViewModel, int childDocumentID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            ChildDocumentViewModel childDocumentViewModel = rootDocumentViewModel.ChildDocumentList
                                                                                 .Where(x => x.ID == childDocumentID)
                                                                                 .FirstOrDefault(); // First for performance.
            if (childDocumentViewModel == null)
            {
                throw new Exception(String.Format("ChildDocumentViewModel with ID '{0}' not found in documentViewModel.ChildDocumentList.", childDocumentID));
            }

            return childDocumentViewModel;
        }

        public static ChildDocumentPropertiesViewModel TryGetChildDocumentPropertiesViewModel(DocumentViewModel rootDocumentViewModel, int childDocumentID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            return rootDocumentViewModel.ChildDocumentPropertiesList.Where(x => x.ID == childDocumentID).FirstOrDefault();
        }

        // Curve

        public static CurveDetailsViewModel GetCurveDetailsViewModel(DocumentViewModel rootDocumentViewModel, int curveID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            CurveDetailsViewModel detailsViewModel = ChildDocumentHelper.EnumerateCurveDetailsViewModels(rootDocumentViewModel)
                                                                        .FirstOrDefault(x => x.Entity.ID == curveID); // First for performance.
            if (detailsViewModel == null)
            {
                throw new Exception(String.Format("CurveDetailsViewModel with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", curveID));
            }

            return detailsViewModel;
        }

        public static CurveGridViewModel GetCurveGridViewModel_ByDocumentID(DocumentViewModel rootDocumentViewModel, int documentID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.ID == documentID)
            {
                return rootDocumentViewModel.CurveGrid;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = GetChildDocumentViewModel(rootDocumentViewModel, documentID);
                return childDocumentViewModel.CurveGrid;
            }
        }

        public static IList<CurveDetailsViewModel> GetCurveDetailsViewModels_ByDocumentID(DocumentViewModel rootDocumentViewModel, int documentID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.ID == documentID)
            {
                return rootDocumentViewModel.CurveDetailsList;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = GetChildDocumentViewModel(rootDocumentViewModel, documentID);
                return childDocumentViewModel.CurveDetailsList;
            }
        }

        private static IEnumerable<CurveDetailsViewModel> EnumerateCurveDetailsViewModels(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (CurveDetailsViewModel curveDetailsViewModel in rootDocumentViewModel.CurveDetailsList)
            {
                yield return curveDetailsViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (CurveDetailsViewModel curveDetailsViewModel in childDocumentViewModel.CurveDetailsList)
                {
                    yield return curveDetailsViewModel;
                }
            }
        }

        // Operator

        public static OperatorViewModel GetOperatorViewModel(DocumentViewModel rootDocumentViewModel, int operatorID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            OperatorViewModel viewModel = ChildDocumentHelper.EnumerateOperatorViewModels(rootDocumentViewModel)
                                                             .FirstOrDefault(x => x.ID == operatorID); // First for performance.
            if (viewModel == null)
            {
                throw new Exception(String.Format("OperatorViewModel with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", operatorID));
            }

            return viewModel;
        }

        private static IEnumerable<OperatorViewModel> EnumerateOperatorViewModels(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (OperatorViewModel operatorViewModel in rootDocumentViewModel.PatchDetailsList.SelectMany(x => x.Entity.Operators))
            {
                yield return operatorViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (OperatorViewModel operatorViewModel in childDocumentViewModel.PatchDetailsList.SelectMany(x => x.Entity.Operators))
                {
                    yield return operatorViewModel;
                }
            }
        }

        public static OperatorPropertiesViewModel TryGetOperatorPropertiesViewModel(DocumentViewModel rootDocumentViewModel, int operatorID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            OperatorPropertiesViewModel viewModel = ChildDocumentHelper.EnumerateOperatorPropertiesViewModels(rootDocumentViewModel)
                                                                       .FirstOrDefault(x => x.ID == operatorID); // First for performance.
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForCustomOperator TryGetOperatorPropertiesViewModel_ForCustomOperator(DocumentViewModel rootDocumentViewModel, int operatorID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            OperatorPropertiesViewModel_ForCustomOperator viewModel = ChildDocumentHelper.EnumerateOperatorPropertiesViewModels_ForCustomOperators(rootDocumentViewModel)
                                                                                         .FirstOrDefault(x => x.ID == operatorID); // First for performance.
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForPatchInlet TryGetOperatorPropertiesViewModel_ForPatchInlet(DocumentViewModel rootDocumentViewModel, int operatorID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            OperatorPropertiesViewModel_ForPatchInlet viewModel = ChildDocumentHelper.EnumerateOperatorPropertiesViewModels_ForPatchInlets(rootDocumentViewModel)
                                                                                     .FirstOrDefault(x => x.ID == operatorID); // First for performance.
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForPatchOutlet TryGetOperatorPropertiesViewModel_ForPatchOutlet(DocumentViewModel rootDocumentViewModel, int operatorID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            OperatorPropertiesViewModel_ForPatchOutlet viewModel = ChildDocumentHelper.EnumerateOperatorPropertiesViewModels_ForPatchOutlets(rootDocumentViewModel)
                                                                                      .FirstOrDefault(x => x.ID == operatorID); // First for performance.
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForSample TryGetOperatorPropertiesViewModel_ForSample(DocumentViewModel rootDocumentViewModel, int operatorID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            OperatorPropertiesViewModel_ForSample viewModel = ChildDocumentHelper.EnumerateOperatorPropertiesViewModels_ForSamples(rootDocumentViewModel)
                                                                                  .FirstOrDefault(x => x.ID == operatorID); // First for performance.
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForNumber TryGetOperatorPropertiesViewModel_ForNumber(DocumentViewModel rootDocumentViewModel, int operatorID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            OperatorPropertiesViewModel_ForNumber viewModel = ChildDocumentHelper.EnumerateOperatorPropertiesViewModels_ForNumbers(rootDocumentViewModel)
                                                                                      .FirstOrDefault(x => x.ID == operatorID); // First for performance.
            return viewModel;
        }

        private static IEnumerable<OperatorPropertiesViewModel> EnumerateOperatorPropertiesViewModels(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (OperatorPropertiesViewModel propertiesViewModel in rootDocumentViewModel.OperatorPropertiesList)
            {
                yield return propertiesViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (OperatorPropertiesViewModel propertiesViewModel in childDocumentViewModel.OperatorPropertiesList)
                {
                    yield return propertiesViewModel;
                }
            }
        }

        private static IEnumerable<OperatorPropertiesViewModel_ForCustomOperator> EnumerateOperatorPropertiesViewModels_ForCustomOperators(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (OperatorPropertiesViewModel_ForCustomOperator propertiesViewModel in rootDocumentViewModel.OperatorPropertiesList_ForCustomOperators)
            {
                yield return propertiesViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (OperatorPropertiesViewModel_ForCustomOperator propertiesViewModel in childDocumentViewModel.OperatorPropertiesList_ForCustomOperators)
                {
                    yield return propertiesViewModel;
                }
            }
        }

        private static IEnumerable<OperatorPropertiesViewModel_ForPatchInlet> EnumerateOperatorPropertiesViewModels_ForPatchInlets(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (OperatorPropertiesViewModel_ForPatchInlet propertiesViewModel in rootDocumentViewModel.OperatorPropertiesList_ForPatchInlets)
            {
                yield return propertiesViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (OperatorPropertiesViewModel_ForPatchInlet propertiesViewModel in childDocumentViewModel.OperatorPropertiesList_ForPatchInlets)
                {
                    yield return propertiesViewModel;
                }
            }
        }

        private static IEnumerable<OperatorPropertiesViewModel_ForPatchOutlet> EnumerateOperatorPropertiesViewModels_ForPatchOutlets(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (OperatorPropertiesViewModel_ForPatchOutlet propertiesViewModel in rootDocumentViewModel.OperatorPropertiesList_ForPatchOutlets)
            {
                yield return propertiesViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (OperatorPropertiesViewModel_ForPatchOutlet propertiesViewModel in childDocumentViewModel.OperatorPropertiesList_ForPatchOutlets)
                {
                    yield return propertiesViewModel;
                }
            }
        }

        private static IEnumerable<OperatorPropertiesViewModel_ForSample> EnumerateOperatorPropertiesViewModels_ForSamples(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (OperatorPropertiesViewModel_ForSample propertiesViewModel in rootDocumentViewModel.OperatorPropertiesList_ForSamples)
            {
                yield return propertiesViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (OperatorPropertiesViewModel_ForSample propertiesViewModel in childDocumentViewModel.OperatorPropertiesList_ForSamples)
                {
                    yield return propertiesViewModel;
                }
            }
        }

        private static IEnumerable<OperatorPropertiesViewModel_ForNumber> EnumerateOperatorPropertiesViewModels_ForNumbers(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (OperatorPropertiesViewModel_ForNumber propertiesViewModel in rootDocumentViewModel.OperatorPropertiesList_ForNumbers)
            {
                yield return propertiesViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (OperatorPropertiesViewModel_ForNumber propertiesViewModel in childDocumentViewModel.OperatorPropertiesList_ForNumbers)
                {
                    yield return propertiesViewModel;
                }
            }
        }

        public static IList<OperatorPropertiesViewModel> GetOperatorPropertiesViewModelList_ByPatchID(DocumentViewModel rootDocumentViewModel, int patchID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
            {
                return rootDocumentViewModel.OperatorPropertiesList;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                if (childDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
                {
                    return childDocumentViewModel.OperatorPropertiesList;
                }
            }

            throw new Exception(String.Format("OperatorPropertiesViewModel with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", patchID));
        }

        public static IList<OperatorPropertiesViewModel_ForCustomOperator> GetOperatorPropertiesViewModelList_ForCustomOperators_ByPatchID(DocumentViewModel rootDocumentViewModel, int patchID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
            {
                return rootDocumentViewModel.OperatorPropertiesList_ForCustomOperators;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                if (childDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
                {
                    return childDocumentViewModel.OperatorPropertiesList_ForCustomOperators;
                }
            }

            throw new Exception(String.Format("OperatorPropertiesViewModel_ForCustomOperator with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", patchID));
        }

        public static IList<OperatorPropertiesViewModel_ForPatchInlet> GetOperatorPropertiesViewModelList_ForPatchInlets_ByPatchID(DocumentViewModel rootDocumentViewModel, int patchID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
            {
                return rootDocumentViewModel.OperatorPropertiesList_ForPatchInlets;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                if (childDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
                {
                    return childDocumentViewModel.OperatorPropertiesList_ForPatchInlets;
                }
            }

            throw new Exception(String.Format("OperatorPropertiesViewModel_ForPatchInlet with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", patchID));
        }

        public static IList<OperatorPropertiesViewModel_ForPatchOutlet> GetOperatorPropertiesViewModelList_ForPatchOutlets_ByPatchID(DocumentViewModel rootDocumentViewModel, int patchID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
            {
                return rootDocumentViewModel.OperatorPropertiesList_ForPatchOutlets;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                if (childDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
                {
                    return childDocumentViewModel.OperatorPropertiesList_ForPatchOutlets;
                }
            }

            throw new Exception(String.Format("OperatorPropertiesViewModel_ForPatchOutlet with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", patchID));
        }

        public static IList<OperatorPropertiesViewModel_ForSample> GetOperatorPropertiesViewModelList_ForSamples_ByPatchID(DocumentViewModel rootDocumentViewModel, int patchID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
            {
                return rootDocumentViewModel.OperatorPropertiesList_ForSamples;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                if (childDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
                {
                    return childDocumentViewModel.OperatorPropertiesList_ForSamples;
                }
            }

            throw new Exception(String.Format("OperatorPropertiesViewModel_ForSample with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", patchID));
        }

        public static IList<OperatorPropertiesViewModel_ForNumber> GetOperatorPropertiesViewModelList_ForNumbers_ByPatchID(DocumentViewModel rootDocumentViewModel, int patchID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
            {
                return rootDocumentViewModel.OperatorPropertiesList_ForNumbers;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                if (childDocumentViewModel.PatchDetailsList.Any(x => x.Entity.ID == patchID))
                {
                    return childDocumentViewModel.OperatorPropertiesList_ForNumbers;
                }
            }

            throw new Exception(String.Format("OperatorPropertiesViewModel_ForNumber with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", patchID));
        }

        // Patch

        public static PatchDetailsViewModel GetPatchDetailsViewModel(DocumentViewModel rootDocumentViewModel, int patchID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            PatchDetailsViewModel detailsViewModel = ChildDocumentHelper.EnumeratePatchDetailsViewModels(rootDocumentViewModel)
                                                                        .FirstOrDefault(x => x.Entity.ID == patchID); // First for performance.
            if (detailsViewModel == null)
            {
                throw new Exception(String.Format("PatchDetailsViewModel with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", patchID));
            }

            return detailsViewModel;
        }

        public static PatchGridViewModel GetPatchGridViewModel_ByDocumentID(DocumentViewModel rootDocumentViewModel, int documentID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.ID == documentID)
            {
                return rootDocumentViewModel.PatchGrid;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = GetChildDocumentViewModel(rootDocumentViewModel, documentID);
                return childDocumentViewModel.PatchGrid;
            }
        }

        public static IList<PatchDetailsViewModel> GetPatchDetailsViewModels_ByDocumentID(DocumentViewModel rootDocumentViewModel, int documentID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.ID == documentID)
            {
                return rootDocumentViewModel.PatchDetailsList;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = GetChildDocumentViewModel(rootDocumentViewModel, documentID);
                return childDocumentViewModel.PatchDetailsList;
            }
        }

        public static PatchDetailsViewModel GetPatchDetailsViewModel_ByOperatorID(DocumentViewModel rootDocumentViewModel, int operatorID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            PatchDetailsViewModel detailsViewModel = ChildDocumentHelper.EnumeratePatchDetailsViewModels(rootDocumentViewModel)
                                                                        .Where(x => x.Entity.Operators.Any(y => y.ID == operatorID))
                                                                        .First();
            return detailsViewModel;
        }

        private static IEnumerable<PatchDetailsViewModel> EnumeratePatchDetailsViewModels(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (PatchDetailsViewModel patchDetailsViewModel in rootDocumentViewModel.PatchDetailsList)
            {
                yield return patchDetailsViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (PatchDetailsViewModel patchDetailsViewModel in childDocumentViewModel.PatchDetailsList)
                {
                    yield return patchDetailsViewModel;
                }
            }
        }

        // Sample

        public static SamplePropertiesViewModel GetSamplePropertiesViewModel(DocumentViewModel rootDocumentViewModel, int sampleID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            SamplePropertiesViewModel propertiesViewModel = ChildDocumentHelper.EnumerateSamplePropertiesViewModels(rootDocumentViewModel)
                                                                               .FirstOrDefault(x => x.Entity.ID == sampleID); // First for performance.
            if (propertiesViewModel == null)
            {
                throw new Exception(String.Format("SamplePropertiesViewModel with ID '{0}' not found in rootDocumentViewModel nor its ChildDocumentViewModels.", sampleID));
            }

            return propertiesViewModel;
        }

        public static IList<SamplePropertiesViewModel> GetSamplePropertiesViewModels_ByDocumentID(DocumentViewModel rootDocumentViewModel, int documentID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.ID == documentID)
            {
                return rootDocumentViewModel.SamplePropertiesList;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = GetChildDocumentViewModel(rootDocumentViewModel, documentID);
                return childDocumentViewModel.SamplePropertiesList;
            }
        }

        public static SampleGridViewModel GetSampleGridViewModel_BySampleID(DocumentViewModel rootDocumentViewModel, int sampleID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            SampleGridViewModel gridViewModel = ChildDocumentHelper.EnumerateSampleGridViewModels(rootDocumentViewModel)
                                                                   .Where(x => x.List.Any(y => y.ID == sampleID))
                                                                   .First();
            return gridViewModel;
        }

        public static SampleGridViewModel GetSampleGridViewModel_ByDocumentID(DocumentViewModel rootDocumentViewModel, int documentID)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            if (rootDocumentViewModel.ID == documentID)
            {
                return rootDocumentViewModel.SampleGrid;
            }
            else
            {
                ChildDocumentViewModel childDocumentViewModel = GetChildDocumentViewModel(rootDocumentViewModel, documentID);
                return childDocumentViewModel.SampleGrid;
            }
        }

        private static IEnumerable<SampleGridViewModel> EnumerateSampleGridViewModels(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            yield return rootDocumentViewModel.SampleGrid;

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                yield return childDocumentViewModel.SampleGrid;
            }
        }

        private static IEnumerable<SamplePropertiesViewModel> EnumerateSamplePropertiesViewModels(DocumentViewModel rootDocumentViewModel)
        {
            if (rootDocumentViewModel == null) throw new NullException(() => rootDocumentViewModel);

            foreach (SamplePropertiesViewModel samplePropertiesViewModel in rootDocumentViewModel.SamplePropertiesList)
            {
                yield return samplePropertiesViewModel;
            }

            foreach (ChildDocumentViewModel childDocumentViewModel in rootDocumentViewModel.ChildDocumentList)
            {
                foreach (SamplePropertiesViewModel samplePropertiesViewModel in childDocumentViewModel.SamplePropertiesList)
                {
                    yield return samplePropertiesViewModel;
                }
            }
        }
    }
}