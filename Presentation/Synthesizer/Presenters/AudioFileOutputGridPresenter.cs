﻿using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.RepositoryInterfaces;
using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ToViewModel;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class AudioFileOutputGridPresenter : GridPresenterBase<AudioFileOutputGridViewModel>
    {
        private readonly IDocumentRepository _documentRepository;

        public AudioFileOutputGridPresenter(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository ?? throw new NullException(() => documentRepository);
        }

        protected override AudioFileOutputGridViewModel CreateViewModel(AudioFileOutputGridViewModel userInput)
        {
            // GetEntity
            Document document = _documentRepository.Get(userInput.DocumentID);

            // ToViewModel
            AudioFileOutputGridViewModel viewModel = document.ToAudioFileOutputGridViewModel();

            return viewModel;
        }
    }
}