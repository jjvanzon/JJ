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

namespace JJ.Presentation.Synthesizer.ToViewModel
{
    /// <summary> Empty view models start out with Visible = false. </summary>
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
                DocumentSaveMenuItem = new MenuItemViewModel { Visible = documentIsOpen },
                CurrentPatches = new MenuItemViewModel { Visible = documentIsOpen }
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
        public static void RefreshViewModel_WithoutEntityPosition(
            Operator entity, OperatorViewModel viewModel,
            ISampleRepository sampleRepository, ICurveRepository curveRepository, IPatchRepository patchRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (viewModel == null) throw new NullException(() => viewModel);

            viewModel.Name = entity.Name;
            viewModel.ID = entity.ID;
            viewModel.Caption = GetOperatorCaption(entity, sampleRepository, curveRepository, patchRepository);

            if (entity.OperatorType != null)
            {
                viewModel.OperatorType = entity.OperatorType.ToViewModel();
            }
            else
            {
                viewModel.OperatorType = null; // Should never happen.
            }

            viewModel.IsOwned = GetOperatorIsOwned(entity);
        }

        /// <summary>
        /// A Number Operator can be considered 'owned' by another operator if
        /// it is the only operator it is connected to.
        /// In that case it is convenient that the Number Operator moves along
        /// with the operator it is connected to.
        /// In the Vector Graphics we accomplish this by making the Number Operator Rectangle a child of the owning Operator's Rectangle. 
        /// But also in the MoveOperator action we must move the owned operators along with their owner.
        /// </summary>
        public static bool GetOperatorIsOwned(Operator entity)
        {
            if (entity.Outlets.Count > 0)
            {
                bool isOwned = entity.GetOperatorTypeEnum() == OperatorTypeEnum.Number &&
                               // Make sure the connected inlets are all of the same operator.
                               entity.Outlets[0].ConnectedInlets.Select(x => x.Operator).Distinct().Count() == 1;

                return isOwned;
            }

            return false;
        }

        /// <summary>
        /// Is used to be able to update an existing operator view model in-place
        /// without having to re-establish the intricate relations with other operators.
        /// </summary>
        public static void RefreshViewModel_WithInletsAndOutlets_WithoutEntityPosition(
            Operator entity, OperatorViewModel operatorViewModel,
            ISampleRepository sampleRepository, ICurveRepository curveRepository, IPatchRepository patchRepository)
        {
            RefreshViewModel_WithoutEntityPosition(entity, operatorViewModel, sampleRepository, curveRepository, patchRepository);
            RefreshInletViewModels(entity.Inlets, operatorViewModel);
            RefreshOutletViewModels(entity.Outlets, operatorViewModel);
        }

        /// <summary>
        /// Is used to be able to update an existing operator view model in-place
        /// without having to re-establish the intricate relations with other operators.
        /// </summary>
        public static void RefreshInletViewModels(IList<Inlet> sourceInlets, OperatorViewModel destOperatorViewModel)
        {
            if (sourceInlets == null) throw new NullException(() => sourceInlets);
            if (destOperatorViewModel == null) throw new NullException(() => destOperatorViewModel);

            var inletViewModelsToKeep = new List<InletViewModel>(sourceInlets.Count);
            foreach (Inlet inlet in sourceInlets)
            {
                if (!MustConvertToInletViewModel(inlet))
                {
                    continue;
                }

                InletViewModel inletViewModel = destOperatorViewModel.Inlets.Where(x => x.ID == inlet.ID).FirstOrDefault();
                if (inletViewModel == null)
                {
                    inletViewModel = new InletViewModel();
                    destOperatorViewModel.Inlets.Add(inletViewModel);
                }

                inlet.ToViewModel(inletViewModel);

                inletViewModelsToKeep.Add(inletViewModel);
            }

            IList<InletViewModel> existingInletViewModels = destOperatorViewModel.Inlets;
            IList<InletViewModel> inletViewModelsToDelete = existingInletViewModels.Except(inletViewModelsToKeep).ToArray();
            foreach (InletViewModel inletViewModelToDelete in inletViewModelsToDelete)
            {
                inletViewModelToDelete.InputOutlet = null;
                destOperatorViewModel.Inlets.Remove(inletViewModelToDelete);
            }

            destOperatorViewModel.Inlets = destOperatorViewModel.Inlets.OrderBy(x => x.ListIndex).ToList();
        }

        /// <summary>
        /// Is used to be able to update an existing operator view model in-place
        /// without having to re-establish the intricate relations with other operators.
        /// </summary>
        public static void RefreshOutletViewModels(IList<Outlet> sourceOutlets, OperatorViewModel destOperatorViewModel)
        {
            if (sourceOutlets == null) throw new NullException(() => sourceOutlets);
            if (destOperatorViewModel == null) throw new NullException(() => destOperatorViewModel);

            var outletViewModelsToKeep = new List<OutletViewModel>(sourceOutlets.Count);
            foreach (Outlet outlet in sourceOutlets)
            {
                if (!MustConvertToOutletViewModel(outlet))
                {
                    continue;
                }

                OutletViewModel outletViewModel = destOperatorViewModel.Outlets.Where(x => x.ID == outlet.ID).FirstOrDefault();
                if (outletViewModel == null)
                {
                    outletViewModel = new OutletViewModel();
                    destOperatorViewModel.Outlets.Add(outletViewModel);

                    // The only inverse property in all the view models.
                    outletViewModel.Operator = destOperatorViewModel;
                }

                outlet.ToViewModel(outletViewModel);

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

            destOperatorViewModel.Outlets = destOperatorViewModel.Outlets.OrderBy(x => x.ListIndex).ToList();
        }

        /// <summary> The inlet of a PatchInlet operator is never converted to view model. </summary>
        public static bool MustConvertToInletViewModel(Inlet inlet)
        {
            if (inlet == null) throw new NullException(() => inlet);

            bool mustConvert = inlet.Operator.GetOperatorTypeEnum() != OperatorTypeEnum.PatchInlet;
            return mustConvert;
        }

        /// <summary> The outlet of a PatchOutlet operator is never converted to view model. </summary>
        public static bool MustConvertToOutletViewModel(Outlet outlet)
        {
            if (outlet == null) throw new NullException(() => outlet);

            bool mustConvert = outlet.Operator.GetOperatorTypeEnum() != OperatorTypeEnum.PatchOutlet;
            return mustConvert;
        }

        public static string GetOperatorCaption(
            Operator entity, ISampleRepository sampleRepository, ICurveRepository curveRepository, IPatchRepository patchRepository)
        {
            if (entity == null) throw new NullException(() => entity);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (curveRepository == null) throw new NullException(() => curveRepository);
            if (patchRepository == null) throw new NullException(() => patchRepository);

            OperatorTypeEnum operatorTypeEnum = entity.GetOperatorTypeEnum();

            // Value Operator: display name and value or only value.
            if (operatorTypeEnum == OperatorTypeEnum.Number)
            {
                var wrapper = new Number_OperatorWrapper(entity);
                string formattedValue = wrapper.Number.ToString("0.####");

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
                Sample underlyingEntity = wrapper.Sample;
                if (underlyingEntity != null)
                {
                    if (!String.IsNullOrWhiteSpace(underlyingEntity.Name))
                    {
                        return underlyingEntity.Name;
                    }
                }
            }

            // Use Curve Name as fallback.
            if (operatorTypeEnum == OperatorTypeEnum.Curve)
            {
                var wrapper = new Curve_OperatorWrapper(entity, curveRepository);
                Curve underlyingEntity = wrapper.Curve;
                if (underlyingEntity != null)
                {
                    if (!String.IsNullOrWhiteSpace(underlyingEntity.Name))
                    {
                        return underlyingEntity.Name;
                    }
                }
            }

            // Use UnderlyingPatch Name as fallback.
            if (operatorTypeEnum == OperatorTypeEnum.CustomOperator)
            {
                var wrapper = new CustomOperator_OperatorWrapper(entity, patchRepository);
                Patch underlyingPatch = wrapper.UnderlyingPatch;
                if (underlyingPatch != null)
                {
                    if (!String.IsNullOrWhiteSpace(underlyingPatch.Name))
                    {
                        return underlyingPatch.Name;
                    }
                }
            }

            // Use OperatorType DisplayName as fallback.
            string caption = ResourceHelper.GetOperatorTypeDisplayName(entity.GetOperatorTypeEnum());
            return caption;
        }

        public static string GetToneGridEditNumberTitle(Scale entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return ResourceHelper.GetScaleTypeDisplayNameSingular(entity);
        }
    }
}