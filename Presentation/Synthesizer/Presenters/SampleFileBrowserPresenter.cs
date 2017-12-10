﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JJ.Business.Synthesizer;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Collections;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.Presenters.Bases;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.ViewModels;

namespace JJ.Presentation.Synthesizer.Presenters
{
	internal class SampleFileBrowserPresenter : PresenterBase<SampleFileBrowserViewModel>
	{
		private const float ESTIMATED_OPERATOR_WIDTH = 50f;
		private const float OPERATOR_HEIGHT = 30f;

		private readonly RepositoryWrapper _repositories;
		private readonly AutoPatcher _autoPatcher;
		private readonly EntityPositionManager _entityPositionManager;

		public SampleFileBrowserPresenter(AutoPatcher autoPatcher, EntityPositionManager entityPositionManager, RepositoryWrapper repositories)
		{
			_autoPatcher = autoPatcher ?? throw new ArgumentNullException(nameof(autoPatcher));
			_entityPositionManager = entityPositionManager ?? throw new ArgumentNullException(nameof(entityPositionManager));
			_repositories = repositories ?? throw new ArgumentNullException(nameof(repositories));
		}

		public void Cancel(SampleFileBrowserViewModel userInput)
		{
			ExecuteNonPersistedAction(userInput, () =>
			{
				userInput.Visible = false;
				userInput.Bytes = new byte[0];
			});
		}

		public SampleFileBrowserViewModel OK(SampleFileBrowserViewModel userInput)
		{
			if (userInput == null) throw new ArgumentNullException(nameof(userInput));

			// RefreshCounter
			userInput.RefreshID = RefreshIDProvider.GetRefreshID();

			// Set !Successful
			userInput.Successful = false;

			// GetEntities
			Patch patch = _repositories.PatchRepository.Get(userInput.DestPatchID);

			// Business
			var operatorFactory = new OperatorFactory(patch, _repositories);
			Operator op = operatorFactory.Sample(userInput.Bytes);

			string fileName = Path.GetFileName(userInput.FilePath);
			op.Name = fileName;
			op.Sample.Name = fileName;

			IList<Operator> autoCreatedNumberOperators = _autoPatcher.CreateNumbersForEmptyInletsWithDefaultValues(op, ESTIMATED_OPERATOR_WIDTH, OPERATOR_HEIGHT, _entityPositionManager);

			// ToViewModel
			SampleFileBrowserViewModel viewModel = ToViewModelHelper.CreateEmptySampleFileBrowserViewModel();

			// Non-Persited
			CopyNonPersistedProperties(userInput, viewModel);
			// Put main operator last so it is dispatched last upon redo and put on top.
			viewModel.CreatedOperatorIDs = autoCreatedNumberOperators.Union(op).Select(x => x.ID).ToList(); 
			viewModel.Visible = false;

			// Successful
			viewModel.Successful = true;

			return viewModel;
		}

		public override void CopyNonPersistedProperties(SampleFileBrowserViewModel sourceViewModel, SampleFileBrowserViewModel destViewModel)
		{
			base.CopyNonPersistedProperties(sourceViewModel, destViewModel);

			destViewModel.DestPatchID = sourceViewModel.DestPatchID;
			destViewModel.Bytes = sourceViewModel.Bytes;
			destViewModel.FilePath = sourceViewModel.FilePath;
			destViewModel.CreatedOperatorIDs = sourceViewModel.CreatedOperatorIDs;
		}
	}
}
