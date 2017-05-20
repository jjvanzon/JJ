﻿using JJ.Business.Canonical;
using JJ.Framework.Exceptions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Business.Synthesizer;
using JJ.Data.Canonical;
using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.RepositoryInterfaces;
using JJ.Framework.Business;
using JJ.Framework.Collections;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class CurvePropertiesPresenter
        : PropertiesPresenterBase<CurvePropertiesViewModel>
    {
        private readonly ICurveRepository _curveRepository;
        private readonly CurveManager _curveManager;

        public CurvePropertiesPresenter(CurveRepositories repositories)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _curveRepository = repositories.CurveRepository;
            _curveManager = new CurveManager(repositories);
        }

        protected override CurvePropertiesViewModel CreateViewModel(CurvePropertiesViewModel userInput)
        {
            // GetEntity
            Curve entity = _curveRepository.Get(userInput.ID);

            // ToViewModel
            CurvePropertiesViewModel viewModel = entity.ToPropertiesViewModel();

            return viewModel;
        }

        protected override void UpdateEntity(CurvePropertiesViewModel viewModel)
        {
            // ToEntity: was already done by the MainPresenter.

            // GetEntity
            Curve entity = _curveRepository.Get(viewModel.ID);

            // Business
            VoidResultDto result = _curveManager.SaveCurveWithRelatedEntities(entity);

            // Non-Persisted
            viewModel.ValidationMessages.AddRange(result.Messages);

            // Successful?
            viewModel.Successful = result.Successful;
        }

        public CurvePropertiesViewModel Delete(CurvePropertiesViewModel userInput)
        {
            return TemplateMethod(
                userInput,
                viewModel =>
                {
                    // Business
                    IResult result = _curveManager.DeleteWithRelatedEntities(userInput.ID);

                    // Non-Persisted
                    viewModel.ValidationMessages.AddRange(result.Messages.ToCanonical());

                    // Successful?
                    viewModel.Successful = result.Successful;
                });
        }
    }
}