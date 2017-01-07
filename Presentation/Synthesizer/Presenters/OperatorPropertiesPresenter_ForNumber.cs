﻿using JJ.Data.Synthesizer;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToViewModel;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class OperatorPropertiesPresenter_ForNumber
        : OperatorPropertiesPresenterBase<OperatorPropertiesViewModel_ForNumber>
    {
        public OperatorPropertiesPresenter_ForNumber(PatchRepositories repositories)
            : base(repositories)
        { }

        protected override OperatorPropertiesViewModel_ForNumber ToViewModel(Operator op)
        {
            return op.ToPropertiesViewModel_ForNumber();
        }
    }
}