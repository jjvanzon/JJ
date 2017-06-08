﻿using JJ.Business.Canonical;
using JJ.Framework.Exceptions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Business.Synthesizer;
using JJ.Data.Canonical;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class SamplePropertiesPresenter : PropertiesPresenterBase<SamplePropertiesViewModel>
    {
        private readonly RepositoryWrapper _repositories;
        private readonly SampleRepositories _sampleRepositories;
        private readonly SampleManager _sampleManager;

        public SamplePropertiesPresenter(RepositoryWrapper repositories)
        {
            _repositories = repositories ?? throw new NullException(() => repositories);
            _sampleRepositories = new SampleRepositories(repositories);
            _sampleManager = new SampleManager(_sampleRepositories);
        }

        protected override SamplePropertiesViewModel CreateViewModel(SamplePropertiesViewModel userInput)
        {
            // GetEntity
            Sample sample = _repositories.SampleRepository.Get(userInput.Entity.ID);

            // ToViewModel
            SamplePropertiesViewModel viewModel = sample.ToPropertiesViewModel(_sampleRepositories);

            return viewModel;
        }

        protected override void UpdateEntity(SamplePropertiesViewModel viewModel)
        {
            // GetEntity
            Sample entity = _repositories.SampleRepository.Get(viewModel.Entity.ID);

            // Business
            VoidResultDto result = _sampleManager.Save(entity);

            // Non-Persisted
            viewModel.ValidationMessages = result.Messages;

            // Successful?
            viewModel.Successful = result.Successful;
        }

        public SamplePropertiesViewModel Play(SamplePropertiesViewModel userInput)
        {
            return TemplateMethod(
                userInput,
                viewModel =>
                {
                    // GetEntity
                    Sample entity = _repositories.SampleRepository.Get(userInput.Entity.ID);

                    // Business
                    var x = new PatchManager(_repositories);
                    x.CreatePatch();
                    Outlet outlet = x.Sample(entity);
                    VoidResultDto result = x.SavePatch();

                    // Non-Persisted
                    viewModel.OutletIDToPlay = outlet?.ID;
                    viewModel.ValidationMessages = result.Messages;

                    // Successful?
                    viewModel.Successful = result.Successful;
                });
        }

        public SamplePropertiesViewModel Delete(SamplePropertiesViewModel userInput)
        {
            return TemplateMethod(
                userInput,
                viewModel =>
                {
                    // Business
                    IResult result = _sampleManager.Delete(userInput.Entity.ID);

                    // Non-Persisted
                    viewModel.ValidationMessages = result.Messages.ToCanonical();

                    // Successful?
                    viewModel.Successful = result.Successful;
                });
        }
    }
}
