﻿using JJ.Business.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class MenuPresenter
    {
        public MenuViewModel ViewModel { get; private set; }

        public MenuViewModel Show(bool documentIsOpen)
        {
            ViewModel = ViewModelHelper.CreateMenuViewModel(documentIsOpen);
            return ViewModel;
        }
    }
}