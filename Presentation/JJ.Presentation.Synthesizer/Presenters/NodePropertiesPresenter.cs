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
    internal class NodePropertiesPresenter
    {
        private readonly CurveRepositories _repositories;
        private readonly CurveManager _curveManager;

        public NodePropertiesViewModel ViewModel { get; set; }

        public NodePropertiesPresenter(CurveRepositories repositories)
        {
            if (repositories == null) throw new NullException(() => repositories);

            _repositories = repositories;
            _curveManager = new CurveManager(_repositories);
        }

        public void Show()
        {
            AssertViewModel();

            ViewModel.Visible = true;
        }

        public void Refresh()
        {
            AssertViewModel();

            Node entity = _repositories.NodeRepository.Get(ViewModel.Entity.ID);

            bool visible = ViewModel.Visible;

            ViewModel = entity.ToPropertiesViewModel(_repositories.NodeTypeRepository);

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
            Update();
        }

        private void Update()
        {
            AssertViewModel();

            Node node = ViewModel.ToEntity(_repositories.NodeRepository, _repositories.NodeTypeRepository);

            // TODO: Low priority: I doubt it is wise to validate without parent,
            // because it is incorrect data.
            VoidResult result = _curveManager.ValidateNodeWithoutParent(node);

            ViewModel.Successful = result.Successful;
            ViewModel.ValidationMessages = result.Messages;
        }

        // Helpers

        private void AssertViewModel()
        {
            if (ViewModel == null) throw new NullException(() => ViewModel);
        }
    }
}
