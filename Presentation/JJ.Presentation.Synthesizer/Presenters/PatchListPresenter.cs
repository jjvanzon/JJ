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
    public class PatchListPresenter
    {
        private IPatchRepository _patchRepository;

        private PatchListViewModel _viewModel;

        private static int _pageSize;

        public PatchListPresenter(IPatchRepository patchRepository)
        {
            if (patchRepository == null) throw new NullException(() => patchRepository);

            _patchRepository = patchRepository;

            ConfigurationSection config = ConfigurationHelper.GetSection<ConfigurationSection>();
            _pageSize = config.PageSize;
        }

        public PatchListViewModel Show(int pageNumber)
        {
            int pageIndex = pageNumber - 1;

            IList<Patch> patches = _patchRepository.GetPage(pageIndex * _pageSize, _pageSize);
            int totalCount = _patchRepository.Count();

            _viewModel = patches.ToListViewModel(pageIndex, _pageSize, totalCount);

            return _viewModel;
        }

        public PatchListViewModel Close()
        {
            if (_viewModel == null)
            {
                _viewModel = ViewModelHelper.CreateEmptyPatchListViewModel();
            }

            _viewModel.Visible = false;

            return _viewModel;
        }
    }
}
