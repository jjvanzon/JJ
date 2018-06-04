﻿using JJ.Demos.ReturnActions.NoViewMapping.ViewModels;
using JJ.Demos.ReturnActions.Helpers;

// ReSharper disable MemberCanBeMadeStatic.Global

namespace JJ.Demos.ReturnActions.NoViewMapping.Presenters
{
    public class DetailsPresenter
    {
        public DetailsViewModel Show(int id) => new DetailsViewModel { Entity = MockViewModelFactory.CreateEntityViewModel(id) };

        public object Edit(int id, string authenticatedUserName)
        {
            var presenter2 = new EditPresenter(authenticatedUserName);
            return presenter2.Show(id, returnAction: $"Details/Show/{id}");
        }
    }
}