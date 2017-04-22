﻿using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Presentation.Synthesizer.Helpers
{
    internal static class ViewModelSelector
    {
        // AudioFileOutput

        public static AudioFileOutputPropertiesViewModel GetAudioFileOutputPropertiesViewModel(DocumentViewModel documentViewModel, int audioFileOutputID)
        {
            AudioFileOutputPropertiesViewModel viewModel = TryGetAudioFileOutputPropertiesViewModel(documentViewModel, audioFileOutputID);

            if (viewModel == null)
            {
                throw new NotFoundException<AudioFileOutputPropertiesViewModel>(audioFileOutputID);
            }

            return viewModel;
        }

        public static AudioFileOutputPropertiesViewModel TryGetAudioFileOutputPropertiesViewModel(DocumentViewModel documentViewModel, int audioFileOutputID)
        {
            documentViewModel.AudioFileOutputPropertiesDictionary.TryGetValue(audioFileOutputID, out AudioFileOutputPropertiesViewModel viewModel);

            return viewModel;
        }

        // Curve

        public static CurveDetailsViewModel GetCurveDetailsViewModel(DocumentViewModel documentViewModel, int curveID)
        {
            CurveDetailsViewModel viewModel = TryGetCurveDetailsViewModel(documentViewModel, curveID);

            if (viewModel == null)
            {
                throw new NotFoundException<CurveDetailsViewModel>(curveID);
            }

            return viewModel;
        }

        public static CurveDetailsViewModel TryGetCurveDetailsViewModel(DocumentViewModel documentViewModel, int curveID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            documentViewModel.CurveDetailsDictionary.TryGetValue(curveID, out CurveDetailsViewModel viewModel);

            return viewModel;
        }

        public static CurvePropertiesViewModel GetCurvePropertiesViewModel(DocumentViewModel documentViewModel, int curveID)
        {
            CurvePropertiesViewModel propertiesViewModel = TryGetCurvePropertiesViewModel(documentViewModel, curveID);

            if (propertiesViewModel == null)
            {
                throw new NotFoundException<CurvePropertiesViewModel>(curveID);
            }

            return propertiesViewModel;
        }

        public static CurvePropertiesViewModel TryGetCurvePropertiesViewModel(DocumentViewModel documentViewModel, int curveID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            documentViewModel.CurvePropertiesDictionary.TryGetValue(curveID, out CurvePropertiesViewModel viewModel);

            return viewModel;
        }

        // Library

        public static LibraryPropertiesViewModel GetLibraryPropertiesViewModel(DocumentViewModel documentViewModel, int documentReferenceID)
        {
            LibraryPropertiesViewModel propertiesViewModel = TryGetLibraryPropertiesViewModel(documentViewModel, documentReferenceID);

            if (propertiesViewModel == null)
            {
                throw new NotFoundException<LibraryPropertiesViewModel>(documentReferenceID);
            }

            return propertiesViewModel;
        }

        public static LibraryPropertiesViewModel TryGetLibraryPropertiesViewModel(DocumentViewModel documentViewModel, int libraryID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            documentViewModel.LibraryPropertiesDictionary.TryGetValue(libraryID, out LibraryPropertiesViewModel viewModel);

            return viewModel;
        }

        // Node

        public static NodePropertiesViewModel GetNodePropertiesViewModel(DocumentViewModel documentViewModel, int nodeID)
        {
            NodePropertiesViewModel viewModel = TryGetNodePropertiesViewModel(documentViewModel, nodeID);

            if (viewModel == null)
            {
                throw new NotFoundException<NodePropertiesViewModel>(nodeID);
            }

            return viewModel;
        }

        public static NodePropertiesViewModel TryGetNodePropertiesViewModel(DocumentViewModel documentViewModel, int nodeID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            documentViewModel.NodePropertiesDictionary.TryGetValue(nodeID, out NodePropertiesViewModel viewModel);

            return viewModel;
        }

        public static Dictionary<int, NodePropertiesViewModel> GetNodePropertiesViewModelDictionary_ByCurveID(DocumentViewModel documentViewModel, int curveID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            if (documentViewModel.CurveDetailsDictionary.ContainsKey(curveID))
            {
                return documentViewModel.NodePropertiesDictionary;
            }

            throw new NotFoundException<Dictionary<int, NodePropertiesViewModel>>(new { curveID });
        }

        public static Dictionary<int, NodePropertiesViewModel> GetNodePropertiesViewModelDictionary_ByNodeID(DocumentViewModel documentViewModel, int nodeID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            if (documentViewModel.NodePropertiesDictionary.ContainsKey(nodeID))
            {
                return documentViewModel.NodePropertiesDictionary;
            }

            throw new NotFoundException<Dictionary<int, NodePropertiesViewModel>>(new { nodeID });
        }

        // Operator

        public static OperatorViewModel GetOperatorViewModel(DocumentViewModel documentViewModel, int patchID, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            PatchDetailsViewModel patchDetailsViewModel = GetPatchDetailsViewModel(documentViewModel, patchID);

            if (patchDetailsViewModel.Entity.OperatorDictionary.TryGetValue(operatorID, out OperatorViewModel operatorViewModel))
            {
                return operatorViewModel;
            }

            throw new NotFoundException<OperatorViewModel>(new { patchID, operatorID });
        }

        public static OperatorPropertiesViewModel GetOperatorPropertiesViewModel(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel viewModel = TryGetOperatorPropertiesViewModel(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel TryGetOperatorPropertiesViewModel(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel viewModel;

            if (documentViewModel.OperatorPropertiesDictionary.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_ForCache GetOperatorPropertiesViewModel_ForCache(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_ForCache viewModel = TryGetOperatorPropertiesViewModel_ForCache(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_ForCache>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForCache TryGetOperatorPropertiesViewModel_ForCache(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_ForCache viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_ForCaches.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_ForCaches.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_ForCurve GetOperatorPropertiesViewModel_ForCurve(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_ForCurve viewModel = TryGetOperatorPropertiesViewModel_ForCurve(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_ForCurve>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForCurve TryGetOperatorPropertiesViewModel_ForCurve(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_ForCurve viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_ForCurves.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_ForCurves.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForCustomOperator GetOperatorPropertiesViewModel_ForCustomOperator(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_ForCustomOperator viewModel = TryGetOperatorPropertiesViewModel_ForCustomOperator(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_ForCustomOperator>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForCustomOperator TryGetOperatorPropertiesViewModel_ForCustomOperator(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_ForCustomOperator viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_ForCustomOperators.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_ForCustomOperators.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForInletsToDimension GetOperatorPropertiesViewModel_ForInletsToDimension(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_ForInletsToDimension viewModel = TryGetOperatorPropertiesViewModel_ForInletsToDimension(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_ForInletsToDimension>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForInletsToDimension TryGetOperatorPropertiesViewModel_ForInletsToDimension(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_ForInletsToDimension viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_ForInletsToDimension.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_ForInletsToDimension.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_ForNumber GetOperatorPropertiesViewModel_ForNumber(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_ForNumber viewModel = TryGetOperatorPropertiesViewModel_ForNumber(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_ForNumber>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForNumber TryGetOperatorPropertiesViewModel_ForNumber(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_ForNumber viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_ForNumbers.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_ForNumbers.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_ForPatchInlet GetOperatorPropertiesViewModel_ForPatchInlet(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_ForPatchInlet viewModel = TryGetOperatorPropertiesViewModel_ForPatchInlet(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_ForPatchInlet>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForPatchInlet TryGetOperatorPropertiesViewModel_ForPatchInlet(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_ForPatchInlet viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_ForPatchInlets.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_ForPatchInlets.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForPatchOutlet GetOperatorPropertiesViewModel_ForPatchOutlet(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_ForPatchOutlet viewModel = TryGetOperatorPropertiesViewModel_ForPatchOutlet(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_ForPatchOutlet>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForPatchOutlet TryGetOperatorPropertiesViewModel_ForPatchOutlet(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_ForPatchOutlet viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_ForPatchOutlets.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_ForPatchOutlets.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_ForSample GetOperatorPropertiesViewModel_ForSample(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_ForSample viewModel = TryGetOperatorPropertiesViewModel_ForSample(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_ForSample>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForSample TryGetOperatorPropertiesViewModel_ForSample(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_ForSample viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_ForSamples.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_ForSamples.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_WithInterpolation GetOperatorPropertiesViewModel_WithInterpolation(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_WithInterpolation viewModel = TryGetOperatorPropertiesViewModel_WithInterpolation(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_WithInterpolation>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_WithInterpolation TryGetOperatorPropertiesViewModel_WithInterpolation(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_WithInterpolation viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_WithInterpolation.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_WithInterpolation.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_WithCollectionRecalculation GetOperatorPropertiesViewModel_WithCollectionRecalculation(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_WithCollectionRecalculation viewModel = TryGetOperatorPropertiesViewModel_WithCollectionRecalculation(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_WithCollectionRecalculation>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_WithCollectionRecalculation TryGetOperatorPropertiesViewModel_WithCollectionRecalculation(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_WithCollectionRecalculation viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_WithCollectionRecalculation.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_WithCollectionRecalculation.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_WithOutletCount GetOperatorPropertiesViewModel_WithOutletCount(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_WithOutletCount viewModel = TryGetOperatorPropertiesViewModel_WithOutletCount(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_WithOutletCount>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_WithOutletCount TryGetOperatorPropertiesViewModel_WithOutletCount(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_WithOutletCount viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_WithOutletCount.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_WithOutletCount.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return null;
        }

        public static OperatorPropertiesViewModel_WithInletCount GetOperatorPropertiesViewModel_WithInletCount(DocumentViewModel documentViewModel, int operatorID)
        {
            OperatorPropertiesViewModel_WithInletCount viewModel = TryGetOperatorPropertiesViewModel_WithInletCount(documentViewModel, operatorID);
            if (viewModel == null)
            {
                throw new NotFoundException<OperatorPropertiesViewModel_WithInletCount>(operatorID);
            }
            return viewModel;
        }

        public static OperatorPropertiesViewModel_WithInletCount TryGetOperatorPropertiesViewModel_WithInletCount(DocumentViewModel documentViewModel, int operatorID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            OperatorPropertiesViewModel_WithInletCount viewModel;

            if (documentViewModel.OperatorPropertiesDictionary_WithInletCount.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            if (documentViewModel.AutoPatch.OperatorPropertiesDictionary_WithInletCount.TryGetValue(operatorID, out viewModel))
            {
                return viewModel;
            }

            return viewModel;
        }

        // Patch

        public static LibraryPatchPropertiesViewModel GetLibraryPatchPropertiesViewModel(DocumentViewModel documentViewModel, int patchID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            LibraryPatchPropertiesViewModel viewModel = TryGetLibraryPatchPropertiesViewModel(documentViewModel, patchID);

            if (viewModel == null)
            {
                throw new NotFoundException<PatchPropertiesViewModel>(new { patchID });
            }

            return viewModel;
        }

        public static LibraryPatchPropertiesViewModel TryGetLibraryPatchPropertiesViewModel(DocumentViewModel documentViewModel, int patchID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            documentViewModel.LibraryPatchPropertiesDictionary.TryGetValue(patchID, out LibraryPatchPropertiesViewModel viewModel);

            return viewModel;
        }

        public static PatchDetailsViewModel GetPatchDetailsViewModel(DocumentViewModel documentViewModel, int patchID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            if (!documentViewModel.PatchDetailsDictionary.TryGetValue(patchID, out PatchDetailsViewModel viewModel))
            {
                throw new NotFoundException<PatchDetailsViewModel>(new { patchID });
            }

            return viewModel;
        }

        public static PatchDetailsViewModel TryGetPatchDetailsViewModel(DocumentViewModel documentViewModel, int patchID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            if (documentViewModel.PatchDetailsDictionary.TryGetValue(patchID, out PatchDetailsViewModel viewModel))
            {
                return viewModel;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (documentViewModel.AutoPatch.PatchDetails.Entity.ID == patchID)
            {
                return documentViewModel.AutoPatch.PatchDetails;
            }

            return null;
        }

        public static PatchPropertiesViewModel GetPatchPropertiesViewModel(DocumentViewModel documentViewModel, int patchID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            PatchPropertiesViewModel viewModel = TryGetPatchPropertiesViewModel(documentViewModel, patchID);

            if (viewModel == null)
            {
                throw new NotFoundException<PatchPropertiesViewModel>(new { patchID });
            }

            return viewModel;       
        }

        public static PatchPropertiesViewModel TryGetPatchPropertiesViewModel(DocumentViewModel documentViewModel, int patchID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            if (documentViewModel.PatchPropertiesDictionary.TryGetValue(patchID, out PatchPropertiesViewModel viewModel))
            {
                return viewModel;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (documentViewModel.AutoPatch.PatchProperties.ID == patchID)
            {
                return documentViewModel.AutoPatch.PatchProperties;
            }

            return null;
        }

        public static PatchGridViewModel GetPatchGridViewModel(DocumentViewModel documentViewModel, string group)
        {
            PatchGridViewModel viewModel = TryGetPatchGridViewModel(documentViewModel, group);

            if (viewModel == null)
            {
                throw new NotFoundException<PatchGridViewModel>(new { group });
            }

            return viewModel;
        }

        public static PatchGridViewModel TryGetPatchGridViewModel(DocumentViewModel documentViewModel, string group)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            string key = NameHelper.ToCanonical(group);

            documentViewModel.PatchGridDictionary.TryGetValue(key, out PatchGridViewModel viewModel);

            return viewModel;
        }

        // Sample

        public static SamplePropertiesViewModel GetSamplePropertiesViewModel(DocumentViewModel documentViewModel, int sampleID)
        {
            SamplePropertiesViewModel viewModel = TryGetSamplePropertiesViewModel(documentViewModel, sampleID);

            if (viewModel == null)
            {
                throw new NotFoundException<SamplePropertiesViewModel>(sampleID);
            }

            return viewModel;
        }

        public static SamplePropertiesViewModel TryGetSamplePropertiesViewModel(DocumentViewModel documentViewModel, int sampleID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            documentViewModel.SamplePropertiesDictionary.TryGetValue(sampleID, out SamplePropertiesViewModel samplePropertiesViewModel);

            return samplePropertiesViewModel;
        }

        // Scale

        public static ScalePropertiesViewModel GetScalePropertiesViewModel(DocumentViewModel documentViewModel, int scaleID)
        {
            ScalePropertiesViewModel viewModel = TryGetScalePropertiesViewModel(documentViewModel, scaleID);

            if (viewModel == null)
            {
                throw new NotFoundException<ScalePropertiesViewModel>(scaleID);
            }

            return viewModel;
        }

        public static ScalePropertiesViewModel TryGetScalePropertiesViewModel(DocumentViewModel documentViewModel, int scaleID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            documentViewModel.ScalePropertiesDictionary.TryGetValue(scaleID, out ScalePropertiesViewModel viewModel);

            return viewModel;
        }

        // Tone

        public static ToneGridEditViewModel GetToneGridEditViewModel(DocumentViewModel documentViewModel, int scaleID)
        {
            ToneGridEditViewModel viewModel = TryGetToneGridEditViewModel(documentViewModel, scaleID);

            if (viewModel == null)
            {
                throw new NotFoundException<ToneGridEditViewModel>(new { scaleID });
            }

            return viewModel;
        }

        public static ToneGridEditViewModel TryGetToneGridEditViewModel(DocumentViewModel documentViewModel, int scaleID)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            documentViewModel.ToneGridEditDictionary.TryGetValue(scaleID, out ToneGridEditViewModel viewModel);

            return viewModel;
        }
    }
}