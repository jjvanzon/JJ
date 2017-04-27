﻿using System;
using JetBrains.Annotations;
using JJ.Business.Synthesizer;
using JJ.Business.Synthesizer.Calculation;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Data.Canonical;
using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.RepositoryInterfaces;
using JJ.Framework.Collections;
using JJ.Framework.Configuration;
using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.ViewModels;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class LibraryPatchPropertiesPresenter : PropertiesPresenterBase<LibraryPatchPropertiesViewModel>
    {
        private readonly IPatchRepository _patchRepository;
        private readonly IDocumentReferenceRepository _documentReferenceRepository;

        public LibraryPatchPropertiesPresenter([NotNull] IPatchRepository patchRepository, [NotNull] IDocumentReferenceRepository documentReferenceRepository)
        {
            _documentReferenceRepository = documentReferenceRepository ?? throw new NullException(() => documentReferenceRepository);
            _patchRepository = patchRepository ?? throw new NullException(() => patchRepository);
        }

        protected override LibraryPatchPropertiesViewModel CreateViewModel(LibraryPatchPropertiesViewModel userInput)
        {
            // Get Entities
            Patch patch = _patchRepository.Get(userInput.PatchID);
            DocumentReference documentReference = _documentReferenceRepository.Get(userInput.DocumentReferenceID);

            // ToViewModel
            LibraryPatchPropertiesViewModel viewModel = patch.ToLibraryPatchPropertiesViewModel(documentReference);

            return viewModel;
        }

        /// <summary> This view is read-only, so just recreate the view model. </summary>
        protected override LibraryPatchPropertiesViewModel UpdateEntity(LibraryPatchPropertiesViewModel userInput)
        {
            return TemplateMethod(userInput, viewModel => CreateViewModel(userInput));
        }

        public LibraryPatchPropertiesViewModel Play(LibraryPatchPropertiesViewModel userInput, [NotNull] RepositoryWrapper repositories)
        {
            if (repositories == null) throw new NullException(() => repositories);

            return TemplateMethod(userInput, viewModel =>
            {
                // GetEntity
                Patch patch = repositories.PatchRepository.Get(userInput.PatchID);

                // Business
                var patchManager = new PatchManager(patch, new PatchRepositories(repositories));
                Result<Outlet> result = patchManager.AutoPatch_TryCombineSignals(patch);
                Outlet outlet = result.Data;

                // Non-Persisted
                viewModel.OutletIDToPlay = outlet?.ID;
                userInput.ValidationMessages.AddRange(result.Messages);
                userInput.Successful = true;
            });
        }
    }
}
