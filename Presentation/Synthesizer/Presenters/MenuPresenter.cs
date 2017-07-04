﻿using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.ViewModels.Partials;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class MenuPresenter
    {
        public MenuViewModel Show(bool documentIsOpen)
        {
            MenuViewModel viewModel = ToViewModelHelper.CreateMenuViewModel(documentIsOpen);
            return viewModel;
        }
    }
}