﻿using JJ.Presentation.SaveText.Interface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Presentation.SaveText.Interface.PresenterInterfaces
{
    public interface ISaveTextPresenter
    {
        SaveTextViewModel Show();
        SaveTextViewModel Save(SaveTextViewModel viewModel);
    }
}