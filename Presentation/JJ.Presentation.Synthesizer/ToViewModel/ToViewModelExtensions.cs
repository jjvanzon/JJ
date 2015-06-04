﻿using JJ.Business.CanonicalModel;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Managers;
using JJ.Business.Synthesizer.Names;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ViewModels.Keys;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.ToViewModel
{
    internal static class ToViewModelExtensions
    {
        // Full Document

        public static DocumentViewModel ToViewModel(this Document document, RepositoryWrapper repositoryWrapper, EntityPositionManager entityPositionManager)
        {
            if (document == null) throw new NullException(() => document);
            if (repositoryWrapper == null) throw new NullException(() => repositoryWrapper);

            var viewModel = new DocumentViewModel
            {
                ID = document.ID,
                DocumentTree = document.ToTreeViewModel(),
                DocumentProperties = document.ToPropertiesViewModel(),
                AudioFileOutputPropertiesList = document.AudioFileOutputs.ToPropertiesViewModels(repositoryWrapper.AudioFileFormatRepository, repositoryWrapper.SampleDataTypeRepository, repositoryWrapper.SpeakerSetupRepository),
                AudioFileOutputList = document.ToAudioFileOutputListViewModel(),
                InstrumentList = document.Instruments.ToChildDocumentListViewModel(document.ID, ChildDocumentTypeEnum.Instrument),
                InstrumentPropertiesList = document.Instruments.ToChildDocumentPropertiesViewModels(),
                InstrumentDocumentList = document.Instruments.ToChildDocumentViewModels(repositoryWrapper, entityPositionManager),
                EffectList = document.Effects.ToChildDocumentListViewModel(document.ID, ChildDocumentTypeEnum.Effect),
                EffectPropertiesList = document.Effects.ToChildDocumentPropertiesViewModels(),
                EffectDocumentList = document.Effects.ToChildDocumentViewModels(repositoryWrapper, entityPositionManager),
                CurveDetailsList = document.Curves.ToDetailsViewModels(null, repositoryWrapper.NodeTypeRepository),
                CurveList = document.Curves.ToListViewModel(document.ID, null, null),
                PatchDetailsList = document.Patches.ToDetailsViewModels(null, entityPositionManager),
                PatchList = document.Patches.ToListViewModel(document.ID, null, null),
                SampleList = document.Samples.ToListViewModel(document.ID, null, null),
                SamplePropertiesList = document.Samples.ToPropertiesViewModels(null, new SampleRepositories(repositoryWrapper))
            };

            return viewModel;
        }

        // AudioFileOutput

        public static IList<AudioFileOutputPropertiesViewModel> ToPropertiesViewModels(
            this IList<AudioFileOutput> entities,
            IAudioFileFormatRepository audioFileFormatRepository,
            ISampleDataTypeRepository sampleDataTypeRepository,
            ISpeakerSetupRepository speakerSetupRepository)
        {
            if (entities == null) throw new NullException(() => entities);

            entities = entities.OrderBy(x => x.Name).ToArray();

            IList<AudioFileOutputPropertiesViewModel> viewModels = new List<AudioFileOutputPropertiesViewModel>(entities.Count);

            for (int i = 0; i < entities.Count; i++)
            {
                AudioFileOutput entity = entities[i];
                AudioFileOutputPropertiesViewModel viewModel = entity.ToPropertiesViewModel(i, audioFileFormatRepository, sampleDataTypeRepository, speakerSetupRepository);

                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public static AudioFileOutputPropertiesViewModel ToPropertiesViewModel(
            this AudioFileOutput entity,
            int listIndex,
            IAudioFileFormatRepository audioFileFormatRepository,
            ISampleDataTypeRepository sampleDataTypeRepository,
            ISpeakerSetupRepository speakerSetupRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (entity.Document == null) throw new NullException(() => entity.Document);

            var viewModel = new AudioFileOutputPropertiesViewModel
            {
                AudioFileOutput = entity.ToViewModelWithRelatedEntities(listIndex),
                AudioFileFormats = ViewModelHelper.CreateAudioFileFormatLookupViewModel(audioFileFormatRepository),
                SampleDataTypes = ViewModelHelper.CreateSampleDataTypeLookupViewModel(sampleDataTypeRepository),
                SpeakerSetups = ViewModelHelper.CreateSpeakerSetupLookupViewModel(speakerSetupRepository)
            };

            // TODO: Delegate to something in ViewModelHelper_Lookups.cs?
            IList<Outlet> outlets = entity.Document.Patches
                                                   .SelectMany(x => x.Operators)
                                                   .Where(x => String.Equals(x.OperatorTypeName, PropertyNames.PatchOutlet))
                                                   .SelectMany(x => x.Outlets)
                                                   .ToArray();
            // TODO: Sort by something.


            // TODO: This will not cut it, because you only see the operator name, not the patch name.
            viewModel.OutletLookup = outlets.Select(x => x.ToIDAndName()).ToArray();

            return viewModel;
        }

        // Curve

        public static IList<CurveDetailsViewModel> ToDetailsViewModels(
            this IList<Curve> entities,
            int? childDocumentListIndex,
            INodeTypeRepository nodeTypeRepository)
        {
            if (entities == null) throw new NullException(() => entities);

            var viewModels = new List<CurveDetailsViewModel>(entities.Count);

            entities = entities.OrderBy(x => x.Name).ToArray();

            for (int i = 0; i < entities.Count; i++)
            {
                Curve entity = entities[i];
                CurveDetailsViewModel viewModel = entity.ToDetailsViewModel(i, childDocumentListIndex, nodeTypeRepository);
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public static CurveDetailsViewModel ToDetailsViewModel(
            this Curve curve,
            int listIndex,
            int? childDocumentListIndex,
            INodeTypeRepository nodeTypeRepository)
        {
            if (curve == null) throw new NullException(() => curve);

            var viewModel = new CurveDetailsViewModel
            {
                Curve = curve.ToViewModelWithRelatedEntities(listIndex, childDocumentListIndex),
                NodeTypes = ViewModelHelper.CreateNodeTypesLookupViewModel(nodeTypeRepository)
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
                Document = document.ToIDAndName(),
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

        // ChildDocument

        public static IList<ChildDocumentPropertiesViewModel> ToChildDocumentPropertiesViewModels(this IList<Document> childDocuments)
        {
            if (childDocuments == null) throw new NullException(() => childDocuments);

            IList<ChildDocumentPropertiesViewModel> viewModels = new List<ChildDocumentPropertiesViewModel>(childDocuments.Count);

            childDocuments = childDocuments.OrderBy(x => x.Name).ToArray();

            for (int i = 0; i < childDocuments.Count; i++)
            {
                Document entity = childDocuments[i];
                ChildDocumentPropertiesViewModel viewModel = entity.ToChildDocumentPropertiesViewModel(i);
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public static ChildDocumentPropertiesViewModel ToChildDocumentPropertiesViewModel(this Document childDocument, int listIndex)
        {
            if (childDocument == null) throw new NullException(() => childDocument);

            var viewModel = new ChildDocumentPropertiesViewModel
            {
                Name = childDocument.Name,
                ValidationMessages = new List<Message>(),
                Keys = new ChildDocumentKeysViewModel
                {
                    ID = childDocument.ID,
                    ParentDocumentID = ChildDocumentHelper.GetParentDocumentID(childDocument),
                    ChildDocumentTypeEnum = ChildDocumentHelper.GetChildDocumentTypeEnum(childDocument),
                    ListIndex = listIndex,
                }
            };

            return viewModel;
        }

        public static IList<ChildDocumentViewModel> ToChildDocumentViewModels(this IList<Document> childDocuments, RepositoryWrapper repositoryWrapper, EntityPositionManager entityPositionManager)
        {
            if (childDocuments == null) throw new NullException(() => childDocuments);

            IList<ChildDocumentViewModel> viewModels = new List<ChildDocumentViewModel>(childDocuments.Count);

            childDocuments = childDocuments.OrderBy(x => x.Name).ToArray();

            for (int i = 0; i < childDocuments.Count; i++)
            {
                Document entity = childDocuments[i];
                ChildDocumentViewModel viewModel = entity.ToChildDocumentViewModel(i, repositoryWrapper, entityPositionManager);
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public static ChildDocumentViewModel ToChildDocumentViewModel(
            this Document childDocument, 
            int childDocumentListIndex, 
            RepositoryWrapper repositoryWrapper, 
            EntityPositionManager entityPositionManager)
        {
            if (childDocument == null) throw new NullException(() => childDocument);
            if (repositoryWrapper == null) throw new NullException(() => repositoryWrapper);

            int parentDocumentID = ChildDocumentHelper.GetParentDocumentID(childDocument);
            ChildDocumentTypeEnum childDocumentTypeEnum = ChildDocumentHelper.GetChildDocumentTypeEnum(childDocument);

            var viewModel = new ChildDocumentViewModel
            {
                Name = childDocument.Name,
                SampleList = childDocument.Samples.ToListViewModel(parentDocumentID, childDocumentTypeEnum, childDocumentListIndex),
                SamplePropertiesList = childDocument.Samples.ToPropertiesViewModels(childDocumentListIndex, new SampleRepositories(repositoryWrapper)),
                CurveList = childDocument.Curves.ToListViewModel(parentDocumentID, childDocumentTypeEnum, childDocumentListIndex),
                CurveDetailsList = childDocument.Curves.ToDetailsViewModels(childDocumentListIndex, repositoryWrapper.NodeTypeRepository),
                PatchList = childDocument.Patches.ToListViewModel(parentDocumentID, childDocumentTypeEnum, childDocumentListIndex),
                PatchDetailsList = childDocument.Patches.ToDetailsViewModels(childDocumentListIndex, entityPositionManager),
                Keys = new ChildDocumentKeysViewModel
                {
                    ID = childDocument.ID,
                    ParentDocumentID = parentDocumentID,
                    ChildDocumentTypeEnum = childDocumentTypeEnum,
                    ListIndex = childDocumentListIndex
                }
            };

            return viewModel;
        }

        // Sample

        public static IList<SamplePropertiesViewModel> ToPropertiesViewModels(
            this IList<Sample> entities,
            int? childDocumentListIndex,
            SampleRepositories sampleRepositories)
        {
            if (entities == null) throw new NullException(() => entities);

            var viewModels = new List<SamplePropertiesViewModel>(entities.Count);

            entities = entities.OrderBy(x => x.Name).ToArray();

            for (int i = 0; i < entities.Count; i++)
            {
                Sample entity = entities[i];
                SamplePropertiesViewModel viewModel = entity.ToPropertiesViewModel(i, childDocumentListIndex, sampleRepositories);
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public static SamplePropertiesViewModel ToPropertiesViewModel(
            this Sample entity, 
            int listIndex, 
            int? childDocumentListIndex,
            SampleRepositories sampleRepositories)
        {
            if (sampleRepositories == null) throw new NullException(() => sampleRepositories);

            var viewModel = new SamplePropertiesViewModel
            {
                Sample = entity.ToViewModel(listIndex, childDocumentListIndex),
                AudioFileFormats = ViewModelHelper.CreateAudioFileFormatLookupViewModel(sampleRepositories.AudioFileFormatRepository),
                SampleDataTypes = ViewModelHelper.CreateSampleDataTypeLookupViewModel(sampleRepositories.SampleDataTypeRepository),
                SpeakerSetups = ViewModelHelper.CreateSpeakerSetupLookupViewModel(sampleRepositories.SpeakerSetupRepository),
                InterpolationTypes = ViewModelHelper.CreateInterpolationTypesLookupViewModel(sampleRepositories.InterpolationTypeRepository)
            };

            return viewModel;
        }
    }
}