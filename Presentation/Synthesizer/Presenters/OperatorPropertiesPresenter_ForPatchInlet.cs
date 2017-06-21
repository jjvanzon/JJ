﻿using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Framework.Validation;
using JJ.Presentation.Synthesizer.Validators;
using JJ.Business.Canonical;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class OperatorPropertiesPresenter_ForPatchInlet
        : OperatorPropertiesPresenterBase<OperatorPropertiesViewModel_ForPatchInlet>
    {
        public OperatorPropertiesPresenter_ForPatchInlet(RepositoryWrapper repositories)
            : base(repositories)
        { }

        protected override OperatorPropertiesViewModel_ForPatchInlet ToViewModel(Operator op)
        {
            return op.ToPropertiesViewModel_ForPatchInlet();
        }

        protected override Operator GetEntity(OperatorPropertiesViewModel_ForPatchInlet userInput)
        {
            return _repositories.OperatorRepository.Get(userInput.ID);
        }

        protected override IResult SaveWithUserInput(Operator entity, OperatorPropertiesViewModel_ForPatchInlet userInput)
        {
            IValidator validator = new OperatorPropertiesViewModel_ForPatchInlet_Validator(userInput);
            if (!validator.IsValid)
            {
                return validator.ToResult();
            }
            
            return _patchManager.SaveOperator(entity);
        }
    }
}