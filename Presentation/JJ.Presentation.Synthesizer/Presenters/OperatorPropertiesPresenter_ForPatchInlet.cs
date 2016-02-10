﻿using System.Collections.Generic;
using JJ.Data.Canonical;
using JJ.Data.Synthesizer;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ToEntity;
using JJ.Business.Synthesizer;
using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Framework.Validation;
using JJ.Presentation.Synthesizer.Validators;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Business.Canonical;
using JJ.Framework.Common;
using System;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class OperatorPropertiesPresenter_ForPatchInlet
        : OperatorPropertiesPresenterBase<OperatorPropertiesViewModel_ForPatchInlet>
    {
        public OperatorPropertiesPresenter_ForPatchInlet(PatchRepositories repositories)
            : base(repositories)
        { }

        protected override OperatorPropertiesViewModel_ForPatchInlet ToViewModel(Operator op)
        {
            return op.ToPropertiesViewModel_ForPatchInlet(_repositories.InletTypeRepository);
        }

        protected override OperatorPropertiesViewModel_ForPatchInlet Update(OperatorPropertiesViewModel_ForPatchInlet userInput)
        {
            if (userInput == null) throw new NullException(() => userInput);

            // Set !Successful
            userInput.Successful = false;

            // ViewModel Validator
            IValidator validator = new OperatorPropertiesViewModel_ForPatchInlet_Validator(userInput);
            if (!validator.IsValid)
            {
                userInput.Successful = validator.IsValid;
                userInput.ValidationMessages = validator.ValidationMessages.ToCanonical();
                return userInput;
            }

            // GetEntity
            Operator entity = _repositories.OperatorRepository.Get(userInput.ID);

            // Business
            PatchManager patchManager = new PatchManager(entity.Patch, _repositories);
            VoidResult result = patchManager.SaveOperator(entity);

            // ToViewModel
            OperatorPropertiesViewModel_ForPatchInlet viewModel = entity.ToPropertiesViewModel_ForPatchInlet(_repositories.InletTypeRepository);

            // Non-Persisted
            CopyNonPersistedProperties(userInput, viewModel);
            viewModel.ValidationMessages.AddRange(result.Messages);

            // Successful?
            viewModel.Successful = result.Successful;

            return viewModel;
        }
    }
}