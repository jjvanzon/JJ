﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.CanonicalModel;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Managers;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Common;
using JJ.Framework.Presentation;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Converters;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ViewModels.Partials;

namespace JJ.Presentation.Synthesizer.ToViewModel
{
    internal static class ToScreenViewModelExtensions
    {
        private static int _maxVisiblePageNumbers = GetMaxVisiblePageNumbers();

        private static int GetMaxVisiblePageNumbers()
        {
            ConfigurationSection config = ConfigurationHelper.GetSection<ConfigurationSection>();
            return config.MaxVisiblePageNumbers;
        }

        // AudioFileOutput

        public static AudioFileOutputPropertiesViewModel ToPropertiesViewModel(
            this AudioFileOutput entity,
            IAudioFileFormatRepository audioFileFormatRepository,
            ISampleDataTypeRepository sampleDataTypeRepository,
            ISpeakerSetupRepository speakerSetupRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (entity.Document == null) throw new NullException(() => entity.Document);

            var viewModel = new AudioFileOutputPropertiesViewModel
            {
                Entity = entity.ToViewModelWithRelatedEntities(),
                AudioFileFormats = ViewModelHelper.CreateAudioFileFormatLookupViewModel(audioFileFormatRepository),
                SampleDataTypes = ViewModelHelper.CreateSampleDataTypeLookupViewModel(sampleDataTypeRepository),
                SpeakerSetups = ViewModelHelper.CreateSpeakerSetupLookupViewModel(speakerSetupRepository),
                ValidationMessages = new List<Message>(),
                Successful = true
            };

            // TODO: Delegate to something in ViewModelHelper_Lookups.cs?
            IList<Outlet> outlets = entity.Document.Patches
                                                   .SelectMany(x => x.Operators)
                                                   .Where(x => x.GetOperatorTypeEnum() != OperatorTypeEnum.PatchOutlet)
                                                   .SelectMany(x => x.Outlets)
                                                   .ToArray();

            // TODO: This will not cut it, because you only see the operator name on screen, not the patch name.
            viewModel.OutletLookup = outlets.Select(x => x.ToIDAndName())
                                            .OrderBy(x => x.Name)
                                            .ToArray();
            return viewModel;
        }

        public static AudioFileOutputGridViewModel ToAudioFileOutputGridViewModel(this Document document)
        {
            if (document == null) throw new NullException(() => document);

            IList<AudioFileOutput> sortedEntities = document.AudioFileOutputs.OrderBy(x => x.Name).ToList();

            var viewModel = new AudioFileOutputGridViewModel
            {
                List = sortedEntities.ToListItemViewModels(),
                DocumentID = document.ID
            };

            return viewModel;
        }

        // ChildDocument

        public static ChildDocumentPropertiesViewModel ToChildDocumentPropertiesViewModel(this Document childDocument, IChildDocumentTypeRepository childDocumentTypeRepository)
        {
            if (childDocument == null) throw new NullException(() => childDocument);

            var viewModel = new ChildDocumentPropertiesViewModel
            {
                ID = childDocument.ID,
                Name = childDocument.Name,
                ChildDocumentTypeLookup = ViewModelHelper.CreateChildDocumentTypeLookupViewModel(childDocumentTypeRepository),
                ValidationMessages = new List<Message>(),
                Successful = true
            };

            if (childDocument.ChildDocumentType != null)
            {
                viewModel.ChildDocumentType = childDocument.ChildDocumentType.ToIDAndDisplayName();
            }

            if (childDocument.MainPatch != null)
            {
                viewModel.MainPatch = childDocument.MainPatch.ToIDAndName();
            }

            viewModel.MainPatchLookup = ViewModelHelper.CreateMainPatchLookupViewModel(childDocument);

            return viewModel;
        }

        public static ChildDocumentGridViewModel ToChildDocumentGridViewModel(this Document rootDocument, int childDocumentTypeID)
        {
            if (rootDocument == null) throw new NullException(() => rootDocument);

            IList<Document> childDocuments = rootDocument.ChildDocuments
                                                         .Where(x => x.ChildDocumentType != null &&
                                                                     x.ChildDocumentType.ID == childDocumentTypeID)
                                                         .ToList();

            ChildDocumentGridViewModel viewModel = childDocuments.ToChildDocumentGridViewModel(rootDocument.ID, childDocumentTypeID);

            return viewModel;
        }

        private static ChildDocumentGridViewModel ToChildDocumentGridViewModel(
            this IList<Document> entities,
            int rootDocumentID,
            int childDocumentTypeID)
        {
            if (entities == null) throw new NullException(() => entities);

            var viewModel = new ChildDocumentGridViewModel
            {
                List = entities.OrderBy(x => x.Name)
                               .Select(x => x.ToIDAndName())
                               .ToList(),
                RootDocumentID = rootDocumentID,
                ChildDocumentTypeID = childDocumentTypeID
            };

            return viewModel;
        }

        // Curve

        public static CurveDetailsViewModel ToDetailsViewModel(this Curve curve, INodeTypeRepository nodeTypeRepository)
        {
            if (curve == null) throw new NullException(() => curve);

            var viewModel = new CurveDetailsViewModel
            {
                Entity = curve.ToViewModelWithRelatedEntities(),
                NodeTypeLookup = ViewModelHelper.CreateNodeTypeLookupViewModel(nodeTypeRepository),
                ValidationMessages = new List<Message>()
            };

            return viewModel;
        }

        public static CurveGridViewModel ToGridViewModel(this IList<Curve> entities, int documentID)
        {
            if (entities == null) throw new NullException(() => entities);

            var viewModel = new CurveGridViewModel
            {
                DocumentID = documentID,
                List = entities.OrderBy(x => x.Name)
                               .Select(x => x.ToIDAndName())
                               .ToList()
            };

            return viewModel;
        }

        public static CurvePropertiesViewModel ToPropertiesViewModel(this Curve entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new CurvePropertiesViewModel
            {
                Entity = entity.ToIDAndName(),
                ValidationMessages = new List<Message>(),
                Successful = true
            };

            return viewModel;
        }

        public static NodePropertiesViewModel ToPropertiesViewModel(this Node entity, INodeTypeRepository nodeTypeRepository)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new NodePropertiesViewModel
            {
                Entity = entity.ToViewModel(),
                ValidationMessages = new List<Message>(),
                NodeTypeLookup = ViewModelHelper.CreateNodeTypeLookupViewModel(nodeTypeRepository),
                Successful = true
            };

            return viewModel;
        }

        // Document

        public static DocumentDetailsViewModel ToDetailsViewModel(this Document document)
        {
            var viewModel = new DocumentDetailsViewModel
            {
                Document = document.ToIDAndName(),
                ValidationMessages = new List<Message>()
            };

            return viewModel;
        }

        public static DocumentPropertiesViewModel ToPropertiesViewModel(this Document document)
        {
            var viewModel = new DocumentPropertiesViewModel
            {
                Entity = document.ToIDAndName(),
                ValidationMessages = new List<Message>(),
                Successful = true
            };

            return viewModel;
        }

        public static DocumentDeleteViewModel ToDeleteViewModel(this Document entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new DocumentDeleteViewModel
            {
                Document = new IDAndName
                {
                    ID = entity.ID,
                    Name = entity.Name,
                }
            };

            return viewModel;
        }

        public static DocumentCannotDeleteViewModel ToCannotDeleteViewModel(this Document entity, IList<Message> messages)
        {
            if (messages == null) throw new NullException(() => messages);

            var viewModel = new DocumentCannotDeleteViewModel
            {
                Document = entity.ToIDAndName(),
                Messages = messages
            };

            return viewModel;
        }

        public static DocumentTreeViewModel ToTreeViewModel(this Document document)
        {
            if (document == null) throw new NullException(() => document);

            var viewModel = new DocumentTreeViewModel
            {
                ID = document.ID,
                Name = document.Name,
                CurvesNode = new DummyViewModel(),
                SamplesNode = new DummyViewModel(),
                AudioFileOutputsNode = new DummyViewModel(),
                PatchesNode = new DummyViewModel(),
                Instruments = new List<ChildDocumentTreeNodeViewModel>(),
                Effects = new List<ChildDocumentTreeNodeViewModel>(),
                ReferencedDocuments = new ReferencedDocumentsTreeNodeViewModel
                {
                    List = new List<ReferencedDocumentViewModel>()
                }
            };

            viewModel.ReferencedDocuments.List = document.DependentOnDocuments.Select(x => x.DependentOnDocument)
                                                                              .Select(x => x.ToReferencedDocumentViewModelWithRelatedEntities())
                                                                              .OrderBy(x => x.Name)
                                                                              .ToList();
            viewModel.Instruments = document.ChildDocuments.Where(x => x.GetChildDocumentTypeEnum() == ChildDocumentTypeEnum.Instrument)
                                                           .OrderBy(x => x.Name)
                                                           .Select(x => x.ToChildDocumentTreeNodeViewModel())
                                                           .ToList();

            viewModel.Effects = document.ChildDocuments.Where(x => x.GetChildDocumentTypeEnum() == ChildDocumentTypeEnum.Effect)
                                                       .OrderBy(x => x.Name)
                                                       .Select(x => x.ToChildDocumentTreeNodeViewModel())
                                                       .ToList();
            return viewModel;
        }

        public static ChildDocumentTreeNodeViewModel ToChildDocumentTreeNodeViewModel(this Document document)
        {
            if (document == null) throw new NullException(() => document);

            var viewModel = new ChildDocumentTreeNodeViewModel
            {
                Name = document.Name,
                CurvesNode = new DummyViewModel(),
                SamplesNode = new DummyViewModel(),
                PatchesNode = new DummyViewModel(),
                ChildDocumentID = document.ID
            };

            return viewModel;
        }

        public static DocumentGridViewModel ToGridViewModel(this IList<Document> pageOfEntities, int pageIndex, int pageSize, int totalCount)
        {
            if (pageOfEntities == null) throw new NullException(() => pageOfEntities);

            var viewModel = new DocumentGridViewModel
            {
                List = pageOfEntities.Select(x => x.ToIDAndName()).ToList(),
                Pager = PagerViewModelFactory.Create(pageIndex, pageSize, totalCount, _maxVisiblePageNumbers)
            };

            return viewModel;
        }

        // Operator

        /// <summary> Converts to properties view models, the operators that do not have a specialized properties view. </summary>
        public static IList<OperatorPropertiesViewModel> ToOperatorPropertiesViewModelList(this Patch patch)
        {
            if (patch == null) throw new NullException(() => patch);

            var viewModels = new List<OperatorPropertiesViewModel>();

            foreach (Operator op in patch.Operators)
            {
                OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();

                if (operatorTypeEnum != OperatorTypeEnum.Curve &&
                    operatorTypeEnum != OperatorTypeEnum.CustomOperator &&
                    operatorTypeEnum != OperatorTypeEnum.PatchInlet &&
                    operatorTypeEnum != OperatorTypeEnum.PatchOutlet &&
                    operatorTypeEnum != OperatorTypeEnum.Number &&
                    operatorTypeEnum != OperatorTypeEnum.Sample)
                {
                    OperatorPropertiesViewModel viewModel = op.ToPropertiesViewModel();
                    viewModels.Add(viewModel);
                }
            }

            return viewModels;
        }

        public static IList<OperatorPropertiesViewModel_ForCurve> ToOperatorPropertiesViewModelList_ForCurves(this Patch patch, ICurveRepository curveRepository)
        {
            if (patch == null) throw new NullException(() => patch);

            return patch.GetOperatorsOfType(OperatorTypeEnum.Curve)
                        .Select(x => x.ToOperatorPropertiesViewModel_ForCurve(curveRepository))
                        .ToList();
        }

        public static IList<OperatorPropertiesViewModel_ForCustomOperator> ToOperatorPropertiesViewModelList_ForCustomOperators(this Patch patch, IDocumentRepository documentRepository)
        {
            if (patch == null) throw new NullException(() => patch);

            return patch.GetOperatorsOfType(OperatorTypeEnum.CustomOperator)
                        .Select(x => x.ToPropertiesViewModel_ForCustomOperator(documentRepository))
                        .ToList();
        }

        public static IList<OperatorPropertiesViewModel_ForPatchInlet> ToPropertiesViewModelList_ForPatchInlets(this Patch patch)
        {
            if (patch == null) throw new NullException(() => patch);

            return patch.GetOperatorsOfType(OperatorTypeEnum.PatchInlet)
                        .Select(x => x.ToPropertiesViewModel_ForPatchInlet())
                        .ToList();
        }

        public static IList<OperatorPropertiesViewModel_ForPatchOutlet> ToPropertiesViewModelList_ForPatchOutlets(this Patch patch)
        {
            if (patch == null) throw new NullException(() => patch);

            return patch.GetOperatorsOfType(OperatorTypeEnum.PatchOutlet)
                        .Select(x => x.ToPropertiesViewModel_ForPatchOutlet())
                        .ToList();
        }

        public static IList<OperatorPropertiesViewModel_ForSample> ToOperatorPropertiesViewModelList_ForSamples(this Patch patch, ISampleRepository sampleRepository)
        {
            if (patch == null) throw new NullException(() => patch);

            return patch.GetOperatorsOfType(OperatorTypeEnum.Sample)
                        .Select(x => x.ToOperatorPropertiesViewModel_ForSample(sampleRepository))
                        .ToList();
        }

        public static IList<OperatorPropertiesViewModel_ForNumber> ToPropertiesViewModelList_ForNumbers(this Patch patch)
        {
            if (patch == null) throw new NullException(() => patch);

            return patch.GetOperatorsOfType(OperatorTypeEnum.Number)
                        .Select(x => x.ToPropertiesViewModel_ForNumber())
                        .ToList();
        }

        public static OperatorPropertiesViewModel ToPropertiesViewModel(this Operator entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new OperatorPropertiesViewModel
            {
                ID = entity.ID,
                Name = entity.Name,
                Successful = true,
                ValidationMessages = new List<Message>()
            };

            if (entity.OperatorType != null)
            {
                viewModel.OperatorType = entity.OperatorType.ToViewModel();
            }

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForCurve ToOperatorPropertiesViewModel_ForCurve(this Operator entity, ICurveRepository curveRepository)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new OperatorPropertiesViewModel_ForCurve
            {
                ID = entity.ID,
                Name = entity.Name,
                Successful = true,
                ValidationMessages = new List<Message>()
            };

            var wrapper = new OperatorWrapper_Curve(entity, curveRepository);

            Curve curve = wrapper.Curve;
            if (curve != null)
            {
                viewModel.Curve = curve.ToIDAndName();
            }

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForCustomOperator ToPropertiesViewModel_ForCustomOperator(this Operator entity, IDocumentRepository documentRepository)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new OperatorPropertiesViewModel_ForCustomOperator
            {
                ID = entity.ID,
                Name = entity.Name,
                Successful = true,
                ValidationMessages = new List<Message>()
            };

            var wrapper = new OperatorWrapper_CustomOperator(entity, documentRepository);

            Document underlyingDocument = wrapper.UnderlyingDocument;
            if (underlyingDocument != null)
            {
                viewModel.UnderlyingDocument = underlyingDocument.ToIDAndName();
            }

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForPatchInlet ToPropertiesViewModel_ForPatchInlet(this Operator entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var wrapper = new OperatorWrapper_PatchInlet(entity);

            var viewModel = new OperatorPropertiesViewModel_ForPatchInlet
            {
                ID = entity.ID,
                Name = entity.Name,
                SortOrder = wrapper.SortOrder,
                Successful = true,
                ValidationMessages = new List<Message>()
            };

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForPatchOutlet ToPropertiesViewModel_ForPatchOutlet(this Operator entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var wrapper = new OperatorWrapper_PatchOutlet(entity);

            var viewModel = new OperatorPropertiesViewModel_ForPatchOutlet
            {
                ID = entity.ID,
                Name = entity.Name,
                SortOrder = wrapper.SortOrder,
                Successful = true,
                ValidationMessages = new List<Message>()
            };

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForSample ToOperatorPropertiesViewModel_ForSample(this Operator entity, ISampleRepository sampleRepository)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new OperatorPropertiesViewModel_ForSample
            {
                ID = entity.ID,
                Name = entity.Name,
                Successful = true,
                ValidationMessages = new List<Message>()
            };

            var wrapper = new OperatorWrapper_Sample(entity, sampleRepository);

            Sample sample = wrapper.Sample;
            if (sample != null)
            {
                viewModel.Sample = sample.ToIDAndName();
            }

            return viewModel;
        }

        public static OperatorPropertiesViewModel_ForNumber ToPropertiesViewModel_ForNumber(this Operator entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var wrapper = new OperatorWrapper_Number(entity);

            var viewModel = new OperatorPropertiesViewModel_ForNumber
            {
                ID = entity.ID,
                Name = entity.Name,
                Number = wrapper.Number.ToString(),
                Successful = true,
                ValidationMessages = new List<Message>()
            };

            return viewModel;
        }

        // Patch

        public static PatchDetailsViewModel ToDetailsViewModel(
            this Patch patch,
            IOperatorTypeRepository operatorTypeRepository, 
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository,
            IDocumentRepository documentRepository,
            EntityPositionManager entityPositionManager)
        {
            var converter = new RecursiveToViewModelConverter(
                operatorTypeRepository, sampleRepository, curveRepository, documentRepository, entityPositionManager);

            return converter.ConvertToDetailsViewModel(patch);
        }

        public static PatchGridViewModel ToGridViewModel(this IList<Patch> entities, int documentID)
        {
            if (entities == null) throw new NullException(() => entities);

            var viewModel = new PatchGridViewModel
            {
                DocumentID = documentID,
                List = entities.OrderBy(x => x.Name)
                               .Select(x => x.ToIDAndName())
                               .ToList()
            };

            return viewModel;
        }

        // Sample

        public static SamplePropertiesViewModel ToPropertiesViewModel(this Sample entity, SampleRepositories repositories)
        {
            if (entity == null) throw new NullException(() => entity);
            if (repositories == null) throw new NullException(() => repositories);

            var viewModel = new SamplePropertiesViewModel
            {
                AudioFileFormats = ViewModelHelper.CreateAudioFileFormatLookupViewModel(repositories.AudioFileFormatRepository),
                SampleDataTypes = ViewModelHelper.CreateSampleDataTypeLookupViewModel(repositories.SampleDataTypeRepository),
                SpeakerSetups = ViewModelHelper.CreateSpeakerSetupLookupViewModel(repositories.SpeakerSetupRepository),
                InterpolationTypes = ViewModelHelper.CreateInterpolationTypesLookupViewModel(repositories.InterpolationTypeRepository),
                ValidationMessages = new List<Message>(),
                Successful = true
            };

            byte[] bytes = repositories.SampleRepository.TryGetBytes(entity.ID);
            viewModel.Entity = entity.ToViewModel(bytes);

            return viewModel;
        }

        public static SampleGridViewModel ToGridViewModel(this IList<Sample> entities, int documentID)
        {
            if (entities == null) throw new NullException(() => entities);

            var viewModel = new SampleGridViewModel
            {
                DocumentID = documentID,
                List = entities.ToListItemViewModels()
            };

            return viewModel;
        }

        // Scale

        public static ToneGridEditViewModel ToDetailsViewModel(this Scale entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var viewModel = new ToneGridEditViewModel
            {
                ScaleID = entity.ID,
                NumberTitle =  ViewModelHelper.GetToneGridEditNumberTitle(entity),
                Tones = entity.Tones.ToToneViewModels(),
                ValidationMessages = new List<Message>(),
                Successful = true
            };

            return viewModel;
        }

        public static IList<ToneViewModel> ToToneViewModels(this IList<Tone> entities)
        {
            if (entities == null) throw new NullException(() => entities);

            IList<ToneViewModel> viewModels = entities.OrderBy(x => x.Octave)
                                                      .ThenBy(x => x.Number)
                                                      .Select(x => x.ToViewModel())
                                                      .ToList();
            return viewModels;
        }

        public static ScalePropertiesViewModel ToPropertiesViewModel(this Scale entity, IScaleTypeRepository scaleTypeRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (scaleTypeRepository == null) throw new NullException(() => scaleTypeRepository);

            var viewModel = new ScalePropertiesViewModel
            {
                Entity = entity.ToViewModel(),
                ScaleTypeLookup = ViewModelHelper.CreateScaleTypeLookupViewModel(scaleTypeRepository),
                ValidationMessages = new List<Message>(),
                Successful = true
            };

            return viewModel;
        }

        public static ScaleGridViewModel ToGridViewModel(this IList<Scale> entities, int documentID)
        {
            if (entities == null) throw new NullException(() => entities);

            var viewModel = new ScaleGridViewModel
            {
                DocumentID = documentID,
                List = entities.OrderBy(x => x.Name)
                               .Select(x => x.ToIDAndName())
                               .ToList()
            };

            return viewModel;
        }
    }
}