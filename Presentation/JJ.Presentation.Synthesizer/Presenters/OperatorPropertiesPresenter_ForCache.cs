﻿using JJ.Data.Synthesizer;
using JJ.Presentation.Synthesizer.ViewModels;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToViewModel;
using System;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class OperatorPropertiesPresenter_ForCache
        : OperatorPropertiesPresenterBase<OperatorPropertiesViewModel_ForCache>
    {
        public OperatorPropertiesPresenter_ForCache(PatchRepositories repositories)
            : base(repositories)
        { }

        protected override OperatorPropertiesViewModel_ForCache ToViewModel(Operator op)
        {
            return op.ToPropertiesViewModel_ForCache(_repositories.InterpolationTypeRepository);
        }
    }
}