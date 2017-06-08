﻿using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.SideEffects;
using JJ.Business.Synthesizer.Validation;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Calculation.AudioFileOutputs;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Canonical;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;

namespace JJ.Business.Synthesizer
{
    public class AudioFileOutputManager
    {
        private readonly AudioFileOutputRepositories _repositories;

        public AudioFileOutputManager(AudioFileOutputRepositories repositories)
        {
            _repositories = repositories ?? throw new NullException(() => repositories);
        }

        /// <summary> Create an AudioFileOutput and initializes it with defaults. </summary>
        public AudioFileOutput Create(Document document = null)
        {
            var audioFileOutput = new AudioFileOutput { ID = _repositories.IDRepository.GetID() };
            audioFileOutput.LinkTo(document);
            _repositories.AudioFileOutputRepository.Insert(audioFileOutput);

            new AudioFileOutput_SideEffect_GenerateName(audioFileOutput).Execute();
            new AudioFileOutput_SideEffect_SetDefaults(
                audioFileOutput,
                _repositories.SampleDataTypeRepository, _repositories.SpeakerSetupRepository, _repositories.AudioFileFormatRepository)
                .Execute();


            return audioFileOutput;
        }

        public void Delete(int id)
        {
            AudioFileOutput audioFileOutput = _repositories.AudioFileOutputRepository.Get(id);
            Delete(audioFileOutput);
        }

        public void Delete(AudioFileOutput entity)
        {
            if (entity == null) throw new NullException(() => entity);

            entity.UnlinkRelatedEntities();
            _repositories.AudioFileOutputRepository.Delete(entity);
        }

        public VoidResult Save(AudioFileOutput entity)
        {
            if (entity == null) throw new NullException(() => entity);

            var validators = new List<IValidator>
            {
                new AudioFileOutputValidator(entity),
                new AudioFileOutputValidator_UniqueName(entity)
            };

            if (entity.Document != null)
            {
                validators.Add(new AudioFileOutputValidator_InDocument(entity));
            }

            return validators.ToResult();
        }

        /// <summary>
        /// This overload taking PatchCalculator can save you the overhead of re-initializing the patch calculation every time you write a file.
        /// </summary>
        public void WriteFile(AudioFileOutput audioFileOutput, params IPatchCalculator[] patchCalculators)
        {
            IAudioFileOutputCalculator audioFileOutputCalculator =
                AudioFileOutputCalculatorFactory.CreateAudioFileOutputCalculator(audioFileOutput, patchCalculators);

            audioFileOutputCalculator.WriteFile(audioFileOutput);
        }
    }
}