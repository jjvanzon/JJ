﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Framework.Common;

namespace JJ.Presentation.Synthesizer.Presenters
{
    internal class PatchListPresenter
    {
        private IDocumentRepository _documentRepository;
        private PatchListViewModel _viewModel;

        public PatchListPresenter(IDocumentRepository documentRepository)
        {
            if (documentRepository == null) throw new NullException(() => documentRepository);

            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Can return PatchListViewModel or NotFoundViewModel.
        /// </summary>
        public object Show(int documentID, ChildDocumentTypeEnum? childDocumentTypeEnum, int? childDocumentListIndex)
        {
            bool mustCreateViewModel = _viewModel == null ||
                                       _viewModel.Keys.DocumentID != documentID ||
                                       _viewModel.Keys.ChildDocumentTypeEnum != childDocumentTypeEnum ||
                                       _viewModel.Keys.ChildDocumentListIndex != childDocumentListIndex;

            if (mustCreateViewModel)
            {
                Document document = ChildDocumentHelper.TryGetRootDocumentOrChildDocument(documentID, childDocumentTypeEnum, childDocumentListIndex, _documentRepository);

                if (document == null)
                {
                    return CreateDocumentNotFoundViewModel();
                }

                _viewModel = document.Patches.ToListViewModel(documentID, childDocumentTypeEnum, childDocumentListIndex);
            }

            _viewModel.Visible = true;

            return _viewModel;
        }

        /// <summary>
        /// Can return PatchListViewModel or NotFoundViewModel.
        /// </summary>
        public object Refresh(PatchListViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            Document document = ChildDocumentHelper.TryGetRootDocumentOrChildDocument(viewModel.Keys.DocumentID, viewModel.Keys.ChildDocumentTypeEnum, viewModel.Keys.ChildDocumentListIndex, _documentRepository); 
            
            if (document == null)
            {
                return CreateDocumentNotFoundViewModel();
            }

            _viewModel = document.Patches.ToListViewModel(viewModel.Keys.DocumentID, viewModel.Keys.ChildDocumentTypeEnum, viewModel.Keys.ChildDocumentListIndex);

            _viewModel.Visible = viewModel.Visible;

            return _viewModel;
        }

        public PatchListViewModel Close()
        {
            if (_viewModel == null)
            {
                _viewModel = ViewModelHelper.CreateEmptyPatchListViewModel();
            }

            _viewModel.Visible = false;

            return _viewModel;
        }

        public void Clear()
        {
            _viewModel = null;
        }

        // Helpers

        private NotFoundViewModel CreateDocumentNotFoundViewModel()
        {
            NotFoundViewModel viewModel = new NotFoundPresenter().Show(PropertyDisplayNames.Document);
            return viewModel;
        }
    }
}
