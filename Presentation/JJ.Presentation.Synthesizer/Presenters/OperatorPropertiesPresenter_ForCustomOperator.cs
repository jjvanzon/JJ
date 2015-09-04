﻿using JJ.Business.CanonicalModel;
using JJ.Data.Synthesizer;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ToEntity;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Managers;
using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Business;
using JJ.Business.Synthesizer.SideEffects;
using JJ.Presentation.Synthesizer.ToViewModel;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class OperatorPropertiesPresenter_ForCustomOperator
    {
        private PatchRepositories _repositories;
        private PatchManager _patchManager;

        public OperatorPropertiesViewModel_ForCustomOperator ViewModel { get; set; }

        public OperatorPropertiesPresenter_ForCustomOperator(PatchRepositories repositories)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;

            _patchManager = new PatchManager(_repositories);
        }

        public void Show()
        {
            AssertViewModel();

            ViewModel.Visible = true;
        }

        public void Refresh()
        {
            AssertViewModel();

            Operator entity = _repositories.OperatorRepository.Get(ViewModel.ID);
            bool visible = ViewModel.Visible;
            ViewModel = entity.ToPropertiesViewModel_ForCustomOperator(_repositories.DocumentRepository);
            ViewModel.Visible = visible;
        }

        public void Close()
        {
            AssertViewModel();

            Update();

            if (ViewModel.Successful)
            {
                ViewModel.Visible = false;
            }
        }

        public void LoseFocus()
        {
            AssertViewModel();

            Update();
        }

        private void Update()
        {
            Operator entity = ViewModel.ToEntity(
                _repositories.OperatorRepository,
                _repositories.OperatorTypeRepository,
                _repositories.DocumentRepository);

            ISideEffect sideEffect = new Operator_SideEffect_ApplyUnderlyingDocument(
                entity,
                _repositories.InletRepository,
                _repositories.OutletRepository,
                _repositories.DocumentRepository,
                _repositories.OperatorTypeRepository,
                _repositories.IDRepository);
            sideEffect.Execute();

            VoidResult result = _patchManager.ValidateNonRecursive(entity);
            if (!result.Successful)
            {
                ViewModel.Successful = false;
                ViewModel.ValidationMessages = result.Messages;
            }
            else
            {
                ViewModel.Successful = true;
                ViewModel.ValidationMessages = new List<Message>();
            }
        }

        // Helpers

        private void AssertViewModel()
        {
            if (ViewModel == null) throw new NullException(() => ViewModel);
        }
    }
}