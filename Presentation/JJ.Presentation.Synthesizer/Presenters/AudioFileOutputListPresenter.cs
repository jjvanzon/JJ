﻿using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Configuration;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ToViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Presentation;

namespace JJ.Presentation.Synthesizer.Presenters
{
    public class AudioFileOutputListPresenter
    {
        private static int _pageSize;

        private IAudioFileOutputRepository _audioFileOutputRepository;

        private AudioFileOutputListViewModel _viewModel;

        public AudioFileOutputListPresenter(IAudioFileOutputRepository audioFileOutputRepository)
        {
            if (audioFileOutputRepository == null) throw new NullException(() => audioFileOutputRepository);

            _audioFileOutputRepository = audioFileOutputRepository;

            ConfigurationSection config = ConfigurationHelper.GetSection<ConfigurationSection>();
            _pageSize = config.PageSize;
        }

        public AudioFileOutputListViewModel Show(int pageNumber)
        {
            int pageIndex = pageNumber - 1;

            IList<AudioFileOutput> audioFileOutputs = _audioFileOutputRepository.GetPage(pageIndex * _pageSize, _pageSize);
            int totalCount = _audioFileOutputRepository.Count();

            _viewModel = audioFileOutputs.ToListViewModel(pageIndex, _pageSize, totalCount);

            return _viewModel;
        }

        public AudioFileOutputListViewModel Close()
        {
            if (_viewModel == null)
            {
                _viewModel = ViewModelHelper.CreateEmptyAudioFileOutputListViewModel();
            }

            _viewModel.Visible = false;

            return _viewModel;
        }
    }
}
