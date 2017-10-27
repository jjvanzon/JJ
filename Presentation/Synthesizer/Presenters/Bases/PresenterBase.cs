﻿using JJ.Framework.Collections;
using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using System;

namespace JJ.Presentation.Synthesizer.Presenters.Bases
{
    internal class PresenterBase<TViewModel>
        where TViewModel : ViewModelBase
    {
        /// <summary>
        /// NOTE: If data read is never edited in this context, we can pretend it is a NonPersistedAction too.
        /// </summary>
        protected virtual void ExecuteNonPersistedAction(TViewModel viewModel, Action action)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (action == null) throw new ArgumentNullException(nameof(action));

            action();

            viewModel.RefreshCounter++;
            viewModel.Successful = true;
        }

        public virtual void CopyNonPersistedProperties(TViewModel sourceViewModel, TViewModel destViewModel)
        {
            if (sourceViewModel == null) throw new NullException(() => sourceViewModel);
            if (destViewModel == null) throw new NullException(() => destViewModel);

            destViewModel.ValidationMessages.AddRange(sourceViewModel.ValidationMessages);
            destViewModel.Visible = sourceViewModel.Visible;
            destViewModel.Successful = sourceViewModel.Successful;
            destViewModel.RefreshCounter = sourceViewModel.RefreshCounter;
        }
    }
}