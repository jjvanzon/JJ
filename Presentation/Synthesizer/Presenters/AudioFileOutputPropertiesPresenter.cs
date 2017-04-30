﻿using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Data.Canonical;
using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Business.Synthesizer;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Collections;
using System;
using JJ.Business.Canonical;
using JJ.Data.Synthesizer.RepositoryInterfaces;
using JJ.Framework.Business;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class AudioFileOutputPropertiesPresenter 
        : PropertiesPresenterBase<AudioFileOutputPropertiesViewModel>
    {
        private readonly IAudioFileOutputRepository _audioFileOutputRepository;
        private readonly AudioFileOutputManager _audioFileOutputManager;

        public AudioFileOutputPropertiesPresenter(AudioFileOutputRepositories repositories)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _audioFileOutputRepository = repositories.AudioFileOutputRepository;
            _audioFileOutputManager = new AudioFileOutputManager(repositories);
        }

        protected override AudioFileOutputPropertiesViewModel CreateViewModel(AudioFileOutputPropertiesViewModel userInput)
        {
            // GetEntity
            AudioFileOutput entity = _audioFileOutputRepository.Get(userInput.Entity.ID);

            // ToViewModel
            AudioFileOutputPropertiesViewModel viewModel = entity.ToPropertiesViewModel();

            return viewModel;
        }

        protected override void UpdateEntity(AudioFileOutputPropertiesViewModel viewModel)
        {
            // ToEntity: was already done by the MainPresenter.

            // GetEntity
            AudioFileOutput entity = _audioFileOutputRepository.Get(viewModel.Entity.ID);

            // Business
            VoidResult result = _audioFileOutputManager.Save(entity);

            // Non-Persisted
            viewModel.ValidationMessages.AddRange(result.Messages.ToCanonical());

            // Successful?
            viewModel.Successful = result.Successful;
        }
    }
}