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

        // PatchDocument

        public static PatchGridViewModel GetPatchGridViewModel_ByGroup(DocumentViewModel documentViewModel, string group)
        {
            PatchGridViewModel viewModel = TryGetPatchGridViewModel_ByGroup(documentViewModel, group);

            if (viewModel == null)
            {
                throw new NotFoundException<PatchGridViewModel>(new { group });
            }

            return viewModel;
        }

        public static PatchGridViewModel TryGetPatchGridViewModel_ByGroup(DocumentViewModel documentViewModel, string group)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            string key = NameHelper.ToCanonical(group);

            documentViewModel.PatchGridDictionary.TryGetValue(key, out PatchGridViewModel viewModel);

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

            documentViewModel.OperatorPropertiesDictionary.TryGetValue(operatorID, out OperatorPropertiesViewModel viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_ForCaches.TryGetValue(operatorID, out OperatorPropertiesViewModel_ForCache viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_ForCurves.TryGetValue(operatorID, out OperatorPropertiesViewModel_ForCurve viewModel);

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

            documentViewModel.OperatorPropertiesDictionary_ForCustomOperators.TryGetValue(operatorID, out OperatorPropertiesViewModel_ForCustomOperator viewModel);

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

            documentViewModel.OperatorPropertiesDictionary_ForInletsToDimension.TryGetValue(operatorID, out OperatorPropertiesViewModel_ForInletsToDimension viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_ForNumbers.TryGetValue(operatorID, out OperatorPropertiesViewModel_ForNumber viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_ForPatchInlets.TryGetValue(operatorID, out OperatorPropertiesViewModel_ForPatchInlet viewModel);

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

            documentViewModel.OperatorPropertiesDictionary_ForPatchOutlets.TryGetValue(operatorID, out OperatorPropertiesViewModel_ForPatchOutlet viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_ForSamples.TryGetValue(operatorID, out OperatorPropertiesViewModel_ForSample viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_WithInterpolation.TryGetValue(operatorID, out OperatorPropertiesViewModel_WithInterpolation viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_WithCollectionRecalculation.TryGetValue(operatorID, out OperatorPropertiesViewModel_WithCollectionRecalculation viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_WithOutletCount.TryGetValue(operatorID, out OperatorPropertiesViewModel_WithOutletCount viewModel);

            return viewModel;
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

            documentViewModel.OperatorPropertiesDictionary_WithInletCount.TryGetValue(operatorID, out OperatorPropertiesViewModel_WithInletCount viewModel);

            return viewModel;
        }

        // Patch

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

            documentViewModel.PatchDetailsDictionary.TryGetValue(patchID, out PatchDetailsViewModel viewModel);

            return viewModel;
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

            documentViewModel.PatchPropertiesDictionary.TryGetValue(patchID, out PatchPropertiesViewModel viewModel);

            return viewModel;
        }

        public static PatchGridViewModel GetPatchGridViewModel(DocumentViewModel documentViewModel, string group)
        {
            if (documentViewModel == null) throw new NullException(() => documentViewModel);

            string key = NameHelper.ToCanonical(group);

            if (documentViewModel.PatchGridDictionary.TryGetValue(key, out PatchGridViewModel viewModel))
            {
                return viewModel;
            }

            throw new NotFoundException<PatchGridViewModel>(new { group });
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