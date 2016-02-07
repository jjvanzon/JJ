﻿using JJ.Data.Synthesizer;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ToEntity;
using System.Collections.Generic;
using JJ.Data.Canonical;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class ScalePropertiesPresenter
    {
        private ScaleRepositories _repositories;
        private ScaleManager _scaleManager;

        public ScalePropertiesPresenter(ScaleRepositories repositories)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;
            _scaleManager = new ScaleManager(_repositories);
        }

        public ScalePropertiesViewModel Show(ScalePropertiesViewModel userInput)
        {
            if (userInput == null) throw new NullException(() => userInput);

            // Set !Successful
            userInput.Successful = false;

            // GetEntity
            Scale scale = _repositories.ScaleRepository.Get(userInput.Entity.ID);

            // ToViewModel
            ScalePropertiesViewModel viewModel = scale.ToPropertiesViewModel(_repositories.ScaleTypeRepository);

            // Non-Persisted
            viewModel.Visible = true;

            // Successful
            viewModel.Successful = true;

            return viewModel;
        }

        public ScalePropertiesViewModel Refresh(ScalePropertiesViewModel userInput)
        {
            if (userInput == null) throw new NullException(() => userInput);

            // !Successful
            userInput.Successful = false;

            // GetEntity
            Scale entity = _repositories.ScaleRepository.Get(userInput.Entity.ID);

            // ToViewModel
            ScalePropertiesViewModel viewModel = entity.ToPropertiesViewModel(_repositories.ScaleTypeRepository);

            // Non-Persisted
            viewModel.Visible = userInput.Visible;

            // Successful
            viewModel.Successful = true;

            return viewModel;
        }

        public ScalePropertiesViewModel Close(ScalePropertiesViewModel userInput)
        {
            ScalePropertiesViewModel viewModel = Update(userInput);

            if (viewModel.Successful)
            {
                viewModel.Visible = false;
            }

            return viewModel;
        }

        public ScalePropertiesViewModel LoseFocus(ScalePropertiesViewModel userInput)
        {
            ScalePropertiesViewModel viewModel = Update(userInput);
            return viewModel;
        }

        private ScalePropertiesViewModel Update(ScalePropertiesViewModel userInput)
        {
            if (userInput == null) throw new NullException(() => userInput);

            // Set !Successful
            userInput.Successful = false;

            // GetEntity
            Scale entity = _repositories.ScaleRepository.Get(userInput.Entity.ID);

            // Business
            VoidResult result = _scaleManager.ValidateWithoutTones(entity);

            // ToViewModel
            ScalePropertiesViewModel viewModel = entity.ToPropertiesViewModel(_repositories.ScaleTypeRepository);

            // Non-Persisted
            viewModel.ValidationMessages = result.Messages;
            viewModel.Visible = userInput.Visible;

            // Successful
            viewModel.Successful = result.Successful;

            return viewModel;
        }
    }
}
