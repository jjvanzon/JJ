﻿using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.LinkTo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Managers;
using JJ.Business.CanonicalModel;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Presentation.Synthesizer.ToEntity
{
    internal static class ToEntityExtensions
    {
        // Document

        public static Document ToEntityWithRelatedEntities(this MainViewModel userInput, RepositoryWrapper repositoryWrapper)
        {
            if (userInput == null) throw new NullException(() => userInput);
            if (repositoryWrapper == null) throw new NullException(() => repositoryWrapper);

            return userInput.Document.ToEntityWithRelatedEntities(repositoryWrapper);
        }

        public static Document ToEntityWithRelatedEntities(this DocumentViewModel userInput, RepositoryWrapper repositoryWrapper)
        {
            if (userInput == null) throw new NullException(() => userInput);
            if (repositoryWrapper == null) throw new NullException(() => repositoryWrapper);

            Document destDocument = userInput.ToEntity(repositoryWrapper.DocumentRepository);

            ToEntityHelper.ToInstrumentsWithRelatedEntities(userInput.InstrumentDocumentList, destDocument, repositoryWrapper);
            ToEntityHelper.ToEffectsWithRelatedEntities(userInput.EffectDocumentList, destDocument, repositoryWrapper);
            ToEntityHelper.ToSamples(userInput.SamplePropertiesList, destDocument, new SampleRepositories(repositoryWrapper));
            ToEntityHelper.ToCurvesWithRelatedEntities(userInput.CurveDetailsList, destDocument, repositoryWrapper.CurveRepository, repositoryWrapper.NodeRepository, repositoryWrapper.NodeTypeRepository);
            ToEntityHelper.ToPatchesWithRelatedEntities(userInput.PatchDetailsList, destDocument, repositoryWrapper);
            ToEntityHelper.ToAudioFileOutputsWithRelatedEntities(userInput.AudioFileOutputPropertiesList, destDocument, new AudioFileOutputRepositories(repositoryWrapper));

            return destDocument;
        }

        public static Document ToEntity(this DocumentViewModel viewModel, IDocumentRepository documentRepository)
        {
            Document document = documentRepository.TryGet(viewModel.ID);
            if (document == null)
            {
                document = documentRepository.Create();
            }
            document.Name = viewModel.DocumentProperties.Document.Name;
            return document;
        }

        public static Document ToEntity(this DocumentPropertiesViewModel viewModel, IDocumentRepository documentRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            Document document = viewModel.Document.ToDocument(documentRepository);
            return document;
        }

        public static Document ToEntity(this DocumentDetailsViewModel viewModel, IDocumentRepository documentRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            Document document = viewModel.Document.ToDocument(documentRepository);
            return document;
        }

        public static Document ToDocument(this IDAndName idAndName, IDocumentRepository documentRepository)
        {
            if (idAndName == null) throw new NullException(() => idAndName);
            if (documentRepository == null) throw new NullException(() => documentRepository);

            Document document = documentRepository.TryGet(idAndName.ID);
            if (document == null)
            {
                document = documentRepository.Create();
            }

            document.Name = idAndName.Name;

            return document;
        }
 
        // Child Document

        public static Document ToEntity(this ChildDocumentPropertiesViewModel viewModel, IDocumentRepository documentRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (documentRepository == null) throw new NullException(() => documentRepository);

            Document entity = documentRepository.TryGet(viewModel.Keys.ID);
            if (entity == null)
            {
                entity = documentRepository.Create();
            }
            entity.Name = viewModel.Name;

            return entity;
        }

        public static Document ToEntityWithRelatedEntities(this ChildDocumentViewModel sourceViewModel, RepositoryWrapper repositoryWrapper)
        {
            if (sourceViewModel == null) throw new NullException(() => sourceViewModel);
            if (repositoryWrapper == null) throw new NullException(() => repositoryWrapper);

            Document destDocument = sourceViewModel.ToEntity(repositoryWrapper.DocumentRepository);

            ToEntityHelper.ToSamples(sourceViewModel.SamplePropertiesList, destDocument, new SampleRepositories(repositoryWrapper));
            ToEntityHelper.ToCurvesWithRelatedEntities(sourceViewModel.CurveDetailsList, destDocument, repositoryWrapper.CurveRepository, repositoryWrapper.NodeRepository, repositoryWrapper.NodeTypeRepository);
            ToEntityHelper.ToPatchesWithRelatedEntities(sourceViewModel.PatchDetailsList, destDocument, repositoryWrapper);

            return destDocument;
        }

        public static Document ToEntity(this ChildDocumentViewModel viewModel, IDocumentRepository documentRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (documentRepository == null) throw new NullException(() => documentRepository);

            Document childDocument = documentRepository.TryGet(viewModel.Keys.ID);
            if (childDocument == null)
            {
                childDocument = documentRepository.Create();
            }

            childDocument.Name = viewModel.Name;

            return childDocument;
        }

        // Sample

        public static Sample ToEntity(this SamplePropertiesViewModel viewModel, SampleRepositories sampleRepositories)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (sampleRepositories == null) throw new NullException(() => sampleRepositories);

            return viewModel.Entity.ToEntity(sampleRepositories);
        }

        public static Sample ToEntity(this SampleViewModel viewModel, SampleRepositories sampleRepositories)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (sampleRepositories == null) throw new NullException(() => sampleRepositories);

            Sample sample = sampleRepositories.SampleRepository.TryGet(viewModel.Keys.ID);
            if (sample == null)
            {
                sample = sampleRepositories.SampleRepository.Create();
            }
            sample.Name = viewModel.Name;
            sample.Amplifier = viewModel.Amplifier;
            sample.TimeMultiplier = viewModel.TimeMultiplier;
            sample.IsActive = viewModel.IsActive;
            sample.SamplingRate = viewModel.SamplingRate;
            sample.BytesToSkip = viewModel.BytesToSkip;
            sample.Location = viewModel.Location;

            if (viewModel.AudioFileFormat != null)
            {
                sample.AudioFileFormat = sampleRepositories.AudioFileFormatRepository.Get(viewModel.AudioFileFormat.ID);
            }

            if (viewModel.InterpolationType != null)
            {
                sample.InterpolationType = sampleRepositories.InterpolationTypeRepository.Get(viewModel.InterpolationType.ID);
            }

            if (viewModel.SampleDataType != null)
            {
                sample.SampleDataType = sampleRepositories.SampleDataTypeRepository.Get(viewModel.SampleDataType.ID);
            }

            if (viewModel.SpeakerSetup != null)
            {
                sample.SpeakerSetup = sampleRepositories.SpeakerSetupRepository.Get(viewModel.SpeakerSetup.ID);
            }

            // TODO: This is not right, but the outcommented code does not work either.
            //sample.Document = sampleRepositories.DocumentRepository.TryGet(viewModel.Keys.DocumentID);

            //Document document = ChildDocumentHelper.GetRootDocumentOrChildDocument(viewModel.Keys.DocumentID, viewModel.Keys.ChildDocumentTypeEnum, viewModel.Keys.ChildDocumentListIndex, sampleRepositories.DocumentRepository);
            //sample.LinkTo(document);

            return sample;
        }

        // Curve

        public static Curve ToEntityWithRelatedEntities(this CurveDetailsViewModel viewModel, ICurveRepository curveRepository, INodeRepository nodeRepository, INodeTypeRepository nodeTypeRepository)
        {
            return viewModel.Curve.ToEntityWithRelatedEntities(curveRepository, nodeRepository, nodeTypeRepository);
        }

        public static Curve ToEntityWithRelatedEntities(this CurveViewModel viewModel, ICurveRepository curveRepository, INodeRepository nodeRepository, INodeTypeRepository nodeTypeRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (curveRepository == null) throw new NullException(() => curveRepository);
            if (nodeRepository == null) throw new NullException(() => nodeRepository);
            if (nodeTypeRepository == null) throw new NullException(() => nodeTypeRepository);

            Curve curve = viewModel.ToEntity(curveRepository);

            ToEntityHelper.ToNodes(viewModel.Nodes, curve, nodeRepository, nodeTypeRepository);

            return curve;
        }

        public static Curve ToEntity(this CurveViewModel viewModel, ICurveRepository curveRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (curveRepository == null) throw new NullException(() => viewModel);

            Curve curve = curveRepository.TryGet(viewModel.Keys.ID);
            if (curve == null)
            {
                curve = curveRepository.Create();
            }
            curve.Name = viewModel.Name;

            return curve;
        }

        public static Node ToEntity(this NodeViewModel viewModel, INodeRepository nodeRepository, INodeTypeRepository nodeTypeRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (nodeRepository == null) throw new NullException(() => nodeRepository);
            if (nodeTypeRepository == null) throw new NullException(() => nodeTypeRepository);

            Node entity = nodeRepository.TryGet(viewModel.Keys.ID);
            if (entity == null)
            {
                entity = nodeRepository.Create();
            }
            entity.Time = viewModel.Time;
            entity.Value = viewModel.Value;
            entity.Direction = viewModel.Direction;

            if (entity.NodeType != null)
            {
                entity.NodeType = nodeTypeRepository.Get(viewModel.NodeType.ID);
            }

            return entity;
        }

        // AudioFileOutput

        public static AudioFileOutput ToEntityWithRelatedEntities(this AudioFileOutputPropertiesViewModel viewModel, AudioFileOutputRepositories audioFileOutputRepositories)
        {
            return viewModel.Entity.ToEntityWithRelatedEntities(audioFileOutputRepositories);
        }

        public static AudioFileOutput ToEntityWithRelatedEntities(this AudioFileOutputViewModel sourceViewModel, AudioFileOutputRepositories audioFileOutputRepositories)
        {
            if (sourceViewModel == null) throw new NullException(() => sourceViewModel);
            if (audioFileOutputRepositories == null) throw new NullException(() => audioFileOutputRepositories);

            AudioFileOutput destAudioFileOutput = sourceViewModel.ToEntity(audioFileOutputRepositories);

            ToEntityHelper.ToAudioFileOutputChannels(sourceViewModel.Channels, destAudioFileOutput, audioFileOutputRepositories.AudioFileOutputChannelRepository, audioFileOutputRepositories.OutletRepository);

            return destAudioFileOutput;
        }

        public static AudioFileOutput ToEntity(this AudioFileOutputViewModel viewModel, AudioFileOutputRepositories audioFileOutputRepositories)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (audioFileOutputRepositories == null) throw new NullException(() => audioFileOutputRepositories);

            AudioFileOutput audioFileOutput = audioFileOutputRepositories.AudioFileOutputRepository.TryGet(viewModel.Keys.ID);
            if (audioFileOutput == null)
            {
                audioFileOutput = audioFileOutputRepositories.AudioFileOutputRepository.Create();
            }
            audioFileOutput.Name = viewModel.Name;
            audioFileOutput.Amplifier = viewModel.Amplifier;
            audioFileOutput.TimeMultiplier = viewModel.TimeMultiplier;
            audioFileOutput.StartTime = viewModel.StartTime;
            audioFileOutput.Duration = viewModel.Duration;
            audioFileOutput.SamplingRate = viewModel.SamplingRate;
            audioFileOutput.FilePath = viewModel.FilePath;

            if (viewModel.AudioFileFormat != null)
            {
                audioFileOutput.AudioFileFormat = audioFileOutputRepositories.AudioFileFormatRepository.Get(viewModel.AudioFileFormat.ID);
            }

            if (viewModel.SampleDataType != null)
            {
                audioFileOutput.SampleDataType = audioFileOutputRepositories.SampleDataTypeRepository.Get(viewModel.SampleDataType.ID);
            }

            if (viewModel.SpeakerSetup != null)
            {
                audioFileOutput.SpeakerSetup = audioFileOutputRepositories.SpeakerSetupRepository.Get(viewModel.SpeakerSetup.ID);
            }

            //audioFileOutput.Document = audioFileOutputRepositories.DocumentRepository.TryGet(viewModel.Keys.DocumentID);

            return audioFileOutput;
        }

        public static AudioFileOutputChannel ToEntity(this AudioFileOutputChannelViewModel viewModel, IAudioFileOutputChannelRepository audioFileOutputChannelRepository, IOutletRepository outletRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (audioFileOutputChannelRepository == null) throw new NullException(() => audioFileOutputChannelRepository);
            if (outletRepository == null) throw new NullException(() => outletRepository);

            AudioFileOutputChannel entity = audioFileOutputChannelRepository.TryGet(viewModel.Keys.ID);
            if (entity == null)
            {
                entity = audioFileOutputChannelRepository.Create();
            }

            entity.IndexNumber = viewModel.Keys.IndexNumber;

            if (viewModel.Outlet != null)
            {
                entity.Outlet = outletRepository.Get(viewModel.Outlet.ID);
            }

            return entity;
        }

        // Patch

        public static Patch ToEntity(
            this PatchDetailsViewModel viewModel,
            IPatchRepository patchRepository,
            IOperatorRepository operatorRepository,
            IOperatorTypeRepository operatorTypeRepository,
            IInletRepository inletRepository,
            IOutletRepository outletRepository,
            IEntityPositionRepository entityPositionRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            Patch patch = viewModel.Entity.ToEntityWithRelatedEntities(
                patchRepository,
                operatorRepository,
                operatorTypeRepository, 
                inletRepository, 
                outletRepository, 
                entityPositionRepository);

            return patch;
        }

        public static Patch ToEntityWithRelatedEntities(
            this PatchViewModel viewModel,
            IPatchRepository patchRepository,
            IOperatorRepository operatorRepository,
            IOperatorTypeRepository operatorTypeRepository,
            IInletRepository inletRepository,
            IOutletRepository outletRepository,
            IEntityPositionRepository entityPositionRepository)
        {
            Patch patch = viewModel.ToEntity(patchRepository);

            RecursiveViewModelToEntityConverter converter = new RecursiveViewModelToEntityConverter(
                operatorRepository, operatorTypeRepository, inletRepository, outletRepository, entityPositionRepository);

            var convertedOperators = new HashSet<Operator>();

            foreach (OperatorViewModel operatorViewModel in viewModel.Operators)
            {
                Operator op = converter.Convert(operatorViewModel);
                op.LinkTo(patch);

                convertedOperators.Add(op);
            }

            IList<Operator> operatorsToDelete = patch.Operators.Except(convertedOperators).ToArray();
            foreach (Operator op in operatorsToDelete)
            {
                op.UnlinkRelatedEntities();
                op.DeleteRelatedEntities(inletRepository, outletRepository, entityPositionRepository);
                operatorRepository.Delete(op);
            }

            return patch;
        }

        public static Patch ToEntity(this PatchViewModel viewModel, IPatchRepository patchRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            Patch entity = patchRepository.TryGet(viewModel.Keys.ID);
            if (entity == null)
            {
                entity = patchRepository.Create();
            }
            entity.Name = viewModel.Name;

            return entity;
        }

        public static Operator ToEntityWithInletsAndOutlets(
            this OperatorViewModel viewModel, 
            IOperatorRepository operatorRepository, 
            IOperatorTypeRepository operatorTypeRepository,
            IInletRepository inletRepository,
            IOutletRepository outletRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            Operator op = viewModel.ToEntity(operatorRepository, operatorTypeRepository);

            foreach (InletViewModel inletViewModel in viewModel.Inlets)
            {
                Inlet inlet = inletViewModel.ToEntity(inletRepository);
                inlet.LinkTo(op);
            }

            foreach (OutletViewModel outletViewModel in viewModel.Outlets)
            {
                Outlet outlet = outletViewModel.ToEntity(outletRepository);
                outlet.LinkTo(op);
            }

            return op;
        }

        public static Operator ToEntity(this OperatorViewModel viewModel, IOperatorRepository operatorRepository, IOperatorTypeRepository operatorTypeRepository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (operatorRepository == null) throw new NullException(() => operatorRepository);
            if (operatorTypeRepository == null) throw new NullException(() => operatorTypeRepository);

            Operator entity = operatorRepository.TryGet(viewModel.Keys.ID);
            if (entity == null)
            {
                entity = operatorRepository.Create();
            }

            entity.Name = viewModel.Name;
            entity.IndexNumber = viewModel.Keys.OperatorIndexNumber;
            entity.OperatorType = operatorTypeRepository.TryGet(viewModel.OperatorTypeID);

            if (entity.GetOperatorTypeEnum() == OperatorTypeEnum.Value)
            {
                entity.Data = viewModel.Value;
            }

            return entity;
        }

        public static Inlet ToEntity(this InletViewModel viewModel, IInletRepository repository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (repository == null) throw new NullException(() => repository);

            Inlet entity = repository.TryGet(viewModel.Keys.ID);
            if (entity == null)
            {
                entity = repository.Create();
            }
            entity.Name = viewModel.Name;
            return entity;
        }

        public static Outlet ToEntity(this OutletViewModel viewModel, IOutletRepository repository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (repository == null) throw new NullException(() => repository);

            Outlet entity = repository.TryGet(viewModel.Keys.ID);
            if (entity == null)
            {
                entity = repository.Create();
            }
            entity.Name = viewModel.Name;
            return entity;
        }

        public static EntityPosition ToEntityPosition(this OperatorViewModel viewModel, IEntityPositionRepository repository)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            if (repository == null) throw new NullException(() => repository);

            // TODO: Remove outcommented code.
            //// Temporary for debugging (2015-07-05)
            //return new EntityPosition { X = 10, Y = 10 };

            var manager = new EntityPositionManager(repository);
            EntityPosition entityPosition = manager.SetOrCreateOperatorPosition(viewModel.Keys.ID, viewModel.CenterX, viewModel.CenterY);
            return entityPosition;
        }
   }
}