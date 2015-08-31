﻿using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.ToViewModel
{
    /// <summary>
    /// Empty view models start out with Visible = false.
    /// </summary>
    internal static partial class ViewModelHelper
    {
        public static NotFoundViewModel CreateNotFoundViewModel(string entityTypeDisplayName)
        {
            var viewModel = new NotFoundViewModel
            {
                Message = CommonMessageFormatter.ObjectNotFound(entityTypeDisplayName)
            };

            return viewModel;
        }

        public static NotFoundViewModel CreateDocumentNotFoundViewModel()
        {
            return CreateNotFoundViewModel<Document>();
        }

        public static NotFoundViewModel CreateNotFoundViewModel<TEntity>()
        {
            string entityTypeName = typeof(TEntity).Name;
            string entityTypeDisplayName = ResourceHelper.GetPropertyDisplayName(entityTypeName);

            NotFoundViewModel viewModel = CreateNotFoundViewModel(entityTypeDisplayName);
            return viewModel;
        }

        public static MenuViewModel CreateMenuViewModel(bool documentIsOpen)
        {
            var viewModel = new MenuViewModel
            {
                DocumentsMenuItem = new MenuItemViewModel { Visible = true },
                DocumentTreeMenuItem = new MenuItemViewModel { Visible = documentIsOpen },
                DocumentCloseMenuItem = new MenuItemViewModel { Visible = documentIsOpen },
                DocumentSaveMenuItem = new MenuItemViewModel { Visible = documentIsOpen }
            };

            return viewModel;
        }

        public static DocumentDeletedViewModel CreateDocumentDeletedViewModel()
        {
            var viewModel = new DocumentDeletedViewModel();

            return viewModel;
        }

        /// <summary>
        /// Is used to be able to update an existing operator view model in-place
        /// without having to re-establish the intricate relations with other operators.
        /// </summary>
        public static void UpdateViewModel_WithInletsAndOutlets_WithoutEntityPosition(
            Operator entity, OperatorViewModel operatorViewModel,
            ISampleRepository sampleRepository, IDocumentRepository documentRepository)
        {
            UpdateViewModel_WithoutEntityPosition(entity, operatorViewModel, sampleRepository, documentRepository);
            UpdateInletViewModels(entity.Inlets, operatorViewModel);
            UpdateOutletViewModels(entity.Outlets, operatorViewModel);
        }

        public static void UpdateInletViewModels(IList<Inlet> sourceInlets, OperatorViewModel destOperatorViewModel)
        {
            if (sourceInlets == null) throw new NullException(() => sourceInlets);
            if (destOperatorViewModel == null) throw new NullException(() => destOperatorViewModel);

            var inletViewModelsToKeep = new List<InletViewModel>(sourceInlets.Count);
            foreach (Inlet inlet in sourceInlets)
            {
                InletViewModel inletViewModel = destOperatorViewModel.Inlets.Where(x => x.ID == inlet.ID).FirstOrDefault();
                if (inletViewModel == null)
                {
                    inletViewModel = inlet.ToViewModel();
                    destOperatorViewModel.Inlets.Add(inletViewModel);
                }

                inletViewModel.Name = inlet.Name;
                inletViewModel.SortOrder = inlet.SortOrder;

                inletViewModelsToKeep.Add(inletViewModel);
            }

            IList<InletViewModel> existingInletViewModels = destOperatorViewModel.Inlets;
            IList<InletViewModel> inletViewModelsToDelete = existingInletViewModels.Except(inletViewModelsToKeep).ToArray();
            foreach (InletViewModel inletViewModelToDelete in inletViewModelsToDelete)
            {
                inletViewModelToDelete.InputOutlet = null;
                destOperatorViewModel.Inlets.Remove(inletViewModelToDelete);
            }

            destOperatorViewModel.Inlets = destOperatorViewModel.Inlets.OrderBy(x => x.SortOrder).ToList();
        }

        public static void UpdateOutletViewModels(IList<Outlet> sourceOutlets, OperatorViewModel destOperatorViewModel)
        {
            if (sourceOutlets == null) throw new NullException(() => sourceOutlets);
            if (destOperatorViewModel == null) throw new NullException(() => destOperatorViewModel);

            var outletViewModelsToKeep = new List<OutletViewModel>(sourceOutlets.Count);
            foreach (Outlet outlet in sourceOutlets)
            {
                OutletViewModel outletViewModel = destOperatorViewModel.Outlets.Where(x => x.ID == outlet.ID).FirstOrDefault();
                if (outletViewModel == null)
                {
                    outletViewModel = outlet.ToViewModel();
                    destOperatorViewModel.Outlets.Add(outletViewModel);

                    // The only inverse property in all the view models.
                    outletViewModel.Operator = destOperatorViewModel;
                }

                outletViewModel.Name = outlet.Name;
                outletViewModel.SortOrder = outlet.SortOrder;

                outletViewModelsToKeep.Add(outletViewModel);
            }

            IList<OutletViewModel> existingOutletViewModels = destOperatorViewModel.Outlets;
            IList<OutletViewModel> outletViewModelsToDelete = existingOutletViewModels.Except(outletViewModelsToKeep).ToArray();
            foreach (OutletViewModel outletViewModelToDelete in outletViewModelsToDelete)
            {
                // The only inverse property in all the view models.
                outletViewModelToDelete.Operator = null;

                destOperatorViewModel.Outlets.Remove(outletViewModelToDelete);
            }

            destOperatorViewModel.Outlets = destOperatorViewModel.Outlets.OrderBy(x => x.SortOrder).ToList();
        }

        /// <summary>
        /// Is used to be able to update an existing operator view model in-place
        /// without having to re-establish the intricate relations with other operators.
        /// </summary>
        public static void UpdateViewModel_WithoutEntityPosition(
            Operator entity, OperatorViewModel viewModel,
            ISampleRepository sampleRepository, IDocumentRepository documentRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (viewModel == null) throw new NullException(() => viewModel);

            viewModel.Name = entity.Name;
            viewModel.ID = entity.ID;
            viewModel.Caption = GetOperatorCaption(entity, sampleRepository, documentRepository);

            if (entity.OperatorType != null)
            {
                viewModel.OperatorType = entity.OperatorType.ToViewModel();
            }
            else
            {
                viewModel.OperatorType = null; // Should never happen.
            }
        }

        private static string GetOperatorCaption(Operator entity, ISampleRepository sampleRepository, IDocumentRepository documentRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (documentRepository == null) throw new NullException(() => documentRepository);

            OperatorTypeEnum operatorTypeEnum = entity.GetOperatorTypeEnum();

            // Value Operator: display name and value or only value.
            if (operatorTypeEnum == OperatorTypeEnum.Value)
            {
                var wrapper = new Value_OperatorWrapper(entity);
                string formattedValue = wrapper.Value.ToString("0.####");

                if (String.IsNullOrWhiteSpace(entity.Name))
                {
                    return formattedValue;
                }
                else
                {
                    return String.Format("{0}: {1}", entity.Name, formattedValue);
                }
            }

            // Prefer Operator's explicit Name.
            if (!String.IsNullOrWhiteSpace(entity.Name))
            {
                return entity.Name;
            }

            // Use Sample Name as fallback.
            if (operatorTypeEnum == OperatorTypeEnum.Sample)
            {
                var wrapper = new Sample_OperatorWrapper(entity, sampleRepository);
                Sample sample = wrapper.Sample;
                if (sample != null)
                {
                    if (!String.IsNullOrWhiteSpace(sample.Name))
                    {
                        return sample.Name;
                    }
                }
            }

            // Use UnderlyingDocument Name as fallback.
            if (operatorTypeEnum == OperatorTypeEnum.CustomOperator)
            {
                var wrapper = new Custom_OperatorWrapper(entity, documentRepository);
                Document underlyingDocument = wrapper.UnderlyingDocument;
                if (underlyingDocument != null)
                {
                    if (!String.IsNullOrWhiteSpace(underlyingDocument.Name))
                    {
                        return underlyingDocument.Name;
                    }
                }
            }

            // Use OperatorType DisplayName as fallback.
            string caption = ResourceHelper.GetOperatorTypeDisplayName(entity.GetOperatorTypeEnum());
            return caption;
        }
    }
}