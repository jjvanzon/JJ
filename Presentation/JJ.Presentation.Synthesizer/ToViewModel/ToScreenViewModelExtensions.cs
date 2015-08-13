﻿using JJ.Business.CanonicalModel;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Managers;
using JJ.Business.Synthesizer.Extensions;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.ToViewModel
{
    internal static class ToScreenViewModelExtensions
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
                AudioFileOutputPropertiesList = document.AudioFileOutputs.Select(x => x.ToPropertiesViewModel(
                    repositoryWrapper.AudioFileFormatRepository, 
                    repositoryWrapper.SampleDataTypeRepository, 
                    repositoryWrapper.SpeakerSetupRepository)).ToList(),
                AudioFileOutputGrid = document.ToAudioFileOutputGridViewModel(),
                ChildDocumentPropertiesList = document.ChildDocuments.Select(x => x.ToChildDocumentPropertiesViewModel(repositoryWrapper.ChildDocumentTypeRepository)).ToList(),
                ChildDocumentList = document.ChildDocuments.Select(x => x.ToChildDocumentViewModel(repositoryWrapper, entityPositionManager)).ToList(),
                InstrumentGrid = document.ToChildDocumentGridViewModel((int)ChildDocumentTypeEnum.Instrument),
                EffectGrid = document.ToChildDocumentGridViewModel((int)ChildDocumentTypeEnum.Effect),
                CurveDetailsList = document.Curves.Select(x => x.ToDetailsViewModel(repositoryWrapper.NodeTypeRepository)).ToList(),
                CurveGrid = document.Curves.ToGridViewModel(document.ID),
                OperatorPropertiesList = document.Patches.SelectMany(x => x.Operators).Select(x => x.ToPropertiesViewModel()).ToList(),
                PatchDetailsList = document.Patches.Select(x => x.ToDetailsViewModel(repositoryWrapper.OperatorTypeRepository, entityPositionManager)).ToList(),
                PatchGrid = document.Patches.ToGridViewModel(document.ID),
                SampleGrid = document.Samples.ToGridViewModel(document.ID),
                SamplePropertiesList = document.Samples.Select(x => x.ToPropertiesViewModel(new SampleRepositories(repositoryWrapper))).ToList()
            };

            return viewModel;
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
            // TODO: Sort by something.

            // TODO: This will not cut it, because you only see the operator name on screen, not the patch name.
            viewModel.OutletLookup = outlets.Select(x => x.ToIDAndName()).ToArray();

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
                viewModel.ChildDocumentType = childDocument.ChildDocumentType.ToIDAndName();
            }

            return viewModel;
        }

        public static ChildDocumentViewModel ToChildDocumentViewModel(
            this Document childDocument,
            RepositoryWrapper repositoryWrapper,
            EntityPositionManager entityPositionManager)
        {
            if (childDocument == null) throw new NullException(() => childDocument);
            if (repositoryWrapper == null) throw new NullException(() => repositoryWrapper);

            var viewModel = new ChildDocumentViewModel
            {
                ID = childDocument.ID,
                Name = childDocument.Name,
                SampleGrid = childDocument.Samples.ToGridViewModel(childDocument.ID),
                SamplePropertiesList = childDocument.Samples.Select(x => x.ToPropertiesViewModel(new SampleRepositories(repositoryWrapper))).ToList(),
                CurveGrid = childDocument.Curves.ToGridViewModel(childDocument.ID),
                CurveDetailsList = childDocument.Curves.Select(x => x.ToDetailsViewModel(repositoryWrapper.NodeTypeRepository)).ToList(),
                OperatorPropertiesList = childDocument.Patches.SelectMany(x => x.Operators).Select(x => x.ToPropertiesViewModel()).ToList(),
                PatchGrid = childDocument.Patches.ToGridViewModel(childDocument.ID),
                PatchDetailsList = childDocument.Patches.Select(x => x.ToDetailsViewModel(repositoryWrapper.OperatorTypeRepository, entityPositionManager)).ToList()
            };

            if (childDocument.ChildDocumentType != null)
            {
                viewModel.ChildDocumentType = childDocument.ChildDocumentType.ToIDAndName();
            }

            return viewModel;
        }

        // Curve

        public static CurveDetailsViewModel ToDetailsViewModel(this Curve curve, INodeTypeRepository nodeTypeRepository)
        {
            if (curve == null) throw new NullException(() => curve);

            var viewModel = new CurveDetailsViewModel
            {
                Entity = curve.ToViewModelWithRelatedEntities(),
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

        // Operator

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

        // Sample

        public static SamplePropertiesViewModel ToPropertiesViewModel(this Sample entity, SampleRepositories sampleRepositories)
        {
            if (sampleRepositories == null) throw new NullException(() => sampleRepositories);

            var viewModel = new SamplePropertiesViewModel
            {
                Entity = entity.ToViewModel(),
                AudioFileFormats = ViewModelHelper.CreateAudioFileFormatLookupViewModel(sampleRepositories.AudioFileFormatRepository),
                SampleDataTypes = ViewModelHelper.CreateSampleDataTypeLookupViewModel(sampleRepositories.SampleDataTypeRepository),
                SpeakerSetups = ViewModelHelper.CreateSpeakerSetupLookupViewModel(sampleRepositories.SpeakerSetupRepository),
                InterpolationTypes = ViewModelHelper.CreateInterpolationTypesLookupViewModel(sampleRepositories.InterpolationTypeRepository),
                ValidationMessages = new List<Message>(),
                Successful = true
            };

            return viewModel;
        }
    }
}