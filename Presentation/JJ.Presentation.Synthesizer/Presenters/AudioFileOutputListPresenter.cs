﻿using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.ToEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Presentation;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class AudioFileOutputListPresenter
    {
        private IDocumentRepository _documentRepository;

        public AudioFileOutputListViewModel ViewModel { get; set; }

        public AudioFileOutputListPresenter(IDocumentRepository documentRepository)
        {
            if (documentRepository == null) throw new NullException(() => documentRepository);

            _documentRepository = documentRepository;
        }

        public void Show()
        {
            ViewModel.Visible = true;
        }

        /// <summary>
        /// Can return AudioFileOutputListViewModel or NotFoundViewModel.
        /// </summary>
        public object Refresh()
        {
            Document document = _documentRepository.TryGet(ViewModel.DocumentID);
            if (document == null)
            {
                return CreateDocumentNotFoundViewModel();
            }

            bool visible = ViewModel.Visible;
            ViewModel = document.ToAudioFileOutputListViewModel();
            ViewModel.Visible = visible;
            return ViewModel;
        }

        public void Close()
        {
            ViewModel.Visible = false;
        }

        // Helpers

        private NotFoundViewModel CreateDocumentNotFoundViewModel()
        {
            NotFoundViewModel viewModel = new NotFoundPresenter().Show(PropertyDisplayNames.Document);
            return viewModel;
        }
    }
}
