﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.SideEffects;
using JJ.Business.Synthesizer.Validation;
using JJ.Framework.Business;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.Synthesizer.Calculation.AudioFileOutputs;

namespace JJ.Business.Synthesizer.Managers
{
    public class AudioFileOutputManager
    {
        private IAudioFileOutputRepository _audioFileOutputRepository;
        private IAudioFileOutputChannelRepository _audioFileOutputChannelRepository;
        private ISampleDataTypeRepository _sampleDataTypeRepository;
        private ISpeakerSetupRepository _speakerSetupRepository;
        private IAudioFileFormatRepository _audioFileFormatRepository;
        private ICurveRepository _curveRepository;
        private ISampleRepository _sampleRepository;
        private IIdentityRepository _identityRepository;

        public AudioFileOutputManager(
            IAudioFileOutputRepository audioFileOutputRepository,
            IAudioFileOutputChannelRepository audioFileOutputChannelRepository,
            ISampleDataTypeRepository sampleDataTypeRepository,
            ISpeakerSetupRepository speakerSetupRepository,
            IAudioFileFormatRepository audioFileFormatRepository,
            ICurveRepository curveRepository,
            ISampleRepository sampleRepository,
            IIdentityRepository identityRepository)
        {
            if (audioFileOutputRepository == null) throw new NullException(() => audioFileOutputRepository);
            if (audioFileOutputChannelRepository == null) throw new NullException(() => audioFileOutputChannelRepository);
            if (sampleDataTypeRepository == null) throw new NullException(() => sampleDataTypeRepository);
            if (speakerSetupRepository == null) throw new NullException(() => speakerSetupRepository);
            if (audioFileFormatRepository == null) throw new NullException(() => audioFileFormatRepository);
            if (curveRepository == null) throw new NullException(() => curveRepository);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (identityRepository == null) throw new NullException(() => identityRepository);

            _audioFileOutputRepository = audioFileOutputRepository;
            _audioFileOutputChannelRepository = audioFileOutputChannelRepository;
            _sampleDataTypeRepository = sampleDataTypeRepository;
            _speakerSetupRepository = speakerSetupRepository;
            _audioFileFormatRepository = audioFileFormatRepository;
            _curveRepository = curveRepository;
            _sampleRepository = sampleRepository;
            _identityRepository = identityRepository;
        }

        /// <summary>
        /// Create an AudioFileOutput and initializes it with defaults
        /// and the necessary child entities.
        /// </summary>
        public AudioFileOutput CreateWithRelatedEntities()
        {
            AudioFileOutput audioFileOutput = _audioFileOutputRepository.Create();
            audioFileOutput.ID = _identityRepository.GenerateID();

            ISideEffect sideEffect1 = new AudioFileOutput_SideEffect_SetDefaults(audioFileOutput, _sampleDataTypeRepository, _speakerSetupRepository, _audioFileFormatRepository);
            sideEffect1.Execute();

            // Adjust channels according to speaker setup.
            SetSpeakerSetup(audioFileOutput, audioFileOutput.SpeakerSetup);

            return audioFileOutput;
        }

        public IValidator Validate(AudioFileOutput audioFileOutput)
        {
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);

            IValidator validator = new AudioFileOutputValidator(audioFileOutput);
            return validator;
        }

        /// <summary>
        /// Sets the speaker setup and adjusts the AudioFileOutputChannels accordingly.
        /// </summary>
        public void SetSpeakerSetup(AudioFileOutput audioFileOutput, SpeakerSetupEnum speakerSetupEnum)
        {
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);
            if (speakerSetupEnum == SpeakerSetupEnum.Undefined) throw new Exception("speakerSetupEnum cannot be 'Undefined'.");

            SpeakerSetup speakerSetup = _speakerSetupRepository.Get((int)speakerSetupEnum);
            SetSpeakerSetup(audioFileOutput, speakerSetup);
        }

        /// <summary>
        /// Sets the speaker setup and adjusts the AudioFileOutputChannels accordingly.
        /// </summary>
        public void SetSpeakerSetup(AudioFileOutput audioFileOutput, SpeakerSetup speakerSetup)
        {
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);
            if (speakerSetup == null) throw new NullException(() => speakerSetup);

            audioFileOutput.LinkTo(speakerSetup);

            IList<AudioFileOutputChannel> entitiesToKeep = new List<AudioFileOutputChannel>(audioFileOutput.AudioFileOutputChannels.Count);

            IList<AudioFileOutputChannel> sortedExistingAudioFileOutputChannels = audioFileOutput.AudioFileOutputChannels.OrderBy(x => x.IndexNumber).ToArray();
            IList<Channel> sortedChannels = speakerSetup.SpeakerSetupChannels.OrderBy(x => x.IndexNumber).Select(x => x.Channel).ToArray();

            for (int i = 0; i < sortedChannels.Count; i++)
            {
                Channel channel = sortedChannels[i];

                AudioFileOutputChannel audioFileOutputChannel = TryGetAudioFileOutputChannel(sortedExistingAudioFileOutputChannels, i);
                if (audioFileOutputChannel == null)
                {
                    audioFileOutputChannel = _audioFileOutputChannelRepository.Create();
                    audioFileOutputChannel.ID = _identityRepository.GenerateID();
                    audioFileOutputChannel.LinkTo(audioFileOutput);
                }
                audioFileOutputChannel.IndexNumber = channel.IndexNumber;

                entitiesToKeep.Add(audioFileOutputChannel);
            }

            IList<AudioFileOutputChannel> entitiesToDelete = sortedExistingAudioFileOutputChannels.Except(entitiesToKeep).ToArray();
            foreach (AudioFileOutputChannel entityToDelete in entitiesToDelete)
            {
                entityToDelete.UnlinkRelatedEntities();
                _audioFileOutputChannelRepository.Delete(entityToDelete);
            }
        }

        private AudioFileOutputChannel TryGetAudioFileOutputChannel(IList<AudioFileOutputChannel> audioFileOutputChannels, int i)
        {
            if (i > audioFileOutputChannels.Count - 1)
            {
                return null;
            }

            return audioFileOutputChannels[i];
        }

        public void Execute(AudioFileOutput audioFileOutput)
        {
            IAudioFileOutputCalculator audioFileOutputCalculator = AudioFileOutputCalculatorFactory.CreateAudioFileOutputCalculator(_curveRepository, _sampleRepository, audioFileOutput);
            audioFileOutputCalculator.Execute();
        }
    }
}