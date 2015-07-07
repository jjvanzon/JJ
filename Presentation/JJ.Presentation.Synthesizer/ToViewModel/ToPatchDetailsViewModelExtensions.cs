﻿using JJ.Business.CanonicalModel;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Managers;
using JJ.Business.Synthesizer.Names;
using JJ.Business.Synthesizer.Extensions;
using JJ.Framework.Reflection.Exceptions;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Presentation.Synthesizer.ToViewModel
{
    internal static class ToPatchDetailsViewModelExtensions
    {
        public static IList<PatchDetailsViewModel> ToDetailsViewModels(
            this IList<Patch> entities,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            IOperatorTypeRepository operatorTypeRepository,
            EntityPositionManager entityPositionManager)
        {
            if (entities == null) throw new NullException(() => entities);

            entities = entities.OrderBy(x => x.Name).ToArray();

            var viewModels = new List<PatchDetailsViewModel>(entities.Count);

            for (int i = 0; i < entities.Count; i++)
            {
                Patch entity = entities[i];

                PatchDetailsViewModel viewModel = entity.ToDetailsViewModel(
                    rootDocumentID, 
                    childDocumentTypeEnum, 
                    childDocumentListIndex, 
                    i, 
                    operatorTypeRepository, 
                    entityPositionManager);

                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public static PatchDetailsViewModel ToDetailsViewModel(
            this Patch patch,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex,
            IOperatorTypeRepository operatorTypeRepository,
            EntityPositionManager entityPositionManager)
        {
            if (patch == null) throw new NullException(() => patch);
            if (entityPositionManager == null) throw new NullException(() => entityPositionManager);

            var viewModel = new PatchDetailsViewModel
            {
                Entity = patch.ToViewModelRecursive(rootDocumentID, childDocumentTypeEnum, childDocumentListIndex, patchListIndex),
                ValidationMessages = new List<Message>()
            };

            viewModel.OperatorToolboxItems = ViewModelHelper.CreateOperatorTypesViewModel(operatorTypeRepository);

            foreach (OperatorViewModel operatorViewModel in viewModel.Entity.Operators)
            {
                SetViewModelPosition(operatorViewModel, entityPositionManager);
            }

            return viewModel;
        }

        private static void SetViewModelPosition(OperatorViewModel operatorViewModel, EntityPositionManager entityPositionManager)
        {
            // TODO: Remove outcommented code.
            //// Temporary for debugging (2015-07-05)
            //return;

            EntityPosition entityPosition = entityPositionManager.GetOrCreateOperatorPosition(operatorViewModel.Keys.ID);
            operatorViewModel.CenterX = entityPosition.X;
            operatorViewModel.CenterY = entityPosition.Y;
        }

        private static PatchViewModel ToViewModelRecursive(
            this Patch patch,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex)
        {
            PatchViewModel viewModel = patch.ToViewModel(rootDocumentID, childDocumentTypeEnum, childDocumentListIndex, patchListIndex);

            var dictionary = new Dictionary<Operator, OperatorViewModel>();

            viewModel.Operators = patch.Operators.Select(x => x.ToViewModelRecursive
            (
                dictionary, 
                rootDocumentID,
                childDocumentTypeEnum,
                childDocumentListIndex,
                patchListIndex
            )).ToList();

            return viewModel;
        }

        private static OperatorViewModel ToViewModelRecursive(
            this Operator op, 
            IDictionary<Operator, OperatorViewModel> dictionary,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex)
        {
            OperatorViewModel viewModel;
            if (dictionary.TryGetValue(op, out viewModel))
            {
                return viewModel;
            }

            viewModel = op.ToViewModel(rootDocumentID, childDocumentTypeEnum, childDocumentListIndex, patchListIndex);

            dictionary.Add(op, viewModel);

            // TODO: When PatchInlets do not have inlets and PatchOutlets do not have PatchOutlets (in the future) these if's are probably not necessary anymore.

            if (op.GetOperatorTypeEnum() != OperatorTypeEnum.PatchInlet)
            {
                viewModel.Inlets = op.Inlets.ToViewModelsRecursive(
                    dictionary, 
                    rootDocumentID, 
                    childDocumentTypeEnum, 
                    childDocumentListIndex, 
                    patchListIndex, 
                    op.IndexNumber);
            }
            else
            {
                viewModel.Inlets = new List<InletViewModel>();
            }

            if (op.GetOperatorTypeEnum() != OperatorTypeEnum.PatchOutlet)
            {
                viewModel.Outlets = op.Outlets.ToViewModelsRecursive(
                    dictionary,
                    rootDocumentID,
                    childDocumentTypeEnum,
                    childDocumentListIndex,
                    patchListIndex,
                    op.IndexNumber);
            }
            else
            {
                viewModel.Outlets = new List<OutletViewModel>();
            }

            return viewModel;
        }

        /// <summary>
        /// Includes its inlets and outlets.
        /// But does not include the inverse properties OutletViewModel.Operator and InletViewModel.Operator.
        /// These view models are one of the few with inverse properties.
        /// </summary>
        private static OperatorViewModel ToViewModelWithRelatedEntities(
            this Operator op,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex)
        {
            // Do not reuse this in ToViewModelRecursive, because there you have to do a dictionary.Add there right in the middle of things.
            OperatorViewModel viewModel = op.ToViewModel(rootDocumentID, childDocumentTypeEnum, childDocumentListIndex, patchListIndex);

            viewModel.Inlets = op.Inlets.ToViewModels(
                rootDocumentID, 
                childDocumentTypeEnum, 
                childDocumentListIndex, 
                patchListIndex, 
                op.IndexNumber);

            viewModel.Outlets = op.Outlets.ToViewModels(
                rootDocumentID,
                childDocumentTypeEnum,
                childDocumentListIndex,
                patchListIndex,
                op.IndexNumber);

            return viewModel;
        }

        /// <summary>
        /// Includes its inlets and outlets.
        /// Also includes the inverse property OutletViewModel.Operator.
        /// That view model is one the few with an inverse property.
        /// </summary>
        public static OperatorViewModel ToViewModelWithRelatedEntitiesAndInverseProperties(
            this Operator op,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex)
        {
            OperatorViewModel operatorViewModel = op.ToViewModel(
                rootDocumentID,
                childDocumentTypeEnum, 
                childDocumentListIndex, 
                patchListIndex);

            operatorViewModel.Inlets = op.Inlets.ToViewModels(
                rootDocumentID,
                childDocumentTypeEnum, 
                childDocumentListIndex, 
                patchListIndex, 
                op.IndexNumber);

            operatorViewModel.Outlets = op.Outlets.ToViewModels(
                rootDocumentID,
                childDocumentTypeEnum, 
                childDocumentListIndex, 
                patchListIndex, 
                op.IndexNumber);

            // This is the inverse property in the view model!
            foreach (OutletViewModel outletViewModel in operatorViewModel.Outlets)
            {
                outletViewModel.Operator = operatorViewModel;
            }

            return operatorViewModel;
        }

        private static IList<InletViewModel> ToViewModelsRecursive(
            this IList<Inlet> entities, 
            IDictionary<Operator, OperatorViewModel> dictionary,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex,
            int operatorIndexNumber)
        {
            var viewModels = new List<InletViewModel>(entities.Count);

            // TODO: Introduce SortOrder property and then sort by it.

            for (int i = 0; i < entities.Count; i++)
            {
                Inlet entity = entities[i];

                InletViewModel viewModel = entity.ToViewModelRecursive(
                    dictionary, 
                    rootDocumentID, 
                    childDocumentTypeEnum, 
                    childDocumentListIndex, 
                    patchListIndex, 
                    operatorIndexNumber, 
                    i);

                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        private static InletViewModel ToViewModelRecursive(
            this Inlet inlet, 
            IDictionary<Operator, OperatorViewModel> dictionary,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex,
            int operatorIndexNumber,
            int inletListIndex)
        {
            InletViewModel viewModel = inlet.ToViewModel(
                rootDocumentID, 
                childDocumentTypeEnum, 
                childDocumentListIndex, 
                patchListIndex, 
                operatorIndexNumber, 
                inletListIndex);

            if (inlet.InputOutlet != null)
            {
                int outletOperatorIndexNumber = inlet.InputOutlet.Operator.IndexNumber;
                int outletListIndex = inlet.InputOutlet.Operator.Outlets.IndexOf(inlet.InputOutlet);

                viewModel.InputOutlet = inlet.InputOutlet.ToViewModelRecursive(
                    dictionary,
                    rootDocumentID,
                    childDocumentTypeEnum,
                    childDocumentListIndex,
                    patchListIndex,
                    outletOperatorIndexNumber,
                    outletListIndex);
            }

            return viewModel;
        }

        private static IList<OutletViewModel> ToViewModelsRecursive(
            this IList<Outlet> entities,
            IDictionary<Operator, OperatorViewModel> dictionary,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex,
            int operatorIndexNumber)
        {
            var viewModels = new List<OutletViewModel>(entities.Count);

            // TODO: Introduce SortOrder property and then sort by it.

            for (int i = 0; i < entities.Count; i++)
            {
                Outlet entity = entities[i];
                OutletViewModel viewModel = entity.ToViewModelRecursive(dictionary, rootDocumentID, childDocumentTypeEnum, childDocumentListIndex, patchListIndex, operatorIndexNumber, i);
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        private static OutletViewModel ToViewModelRecursive(
            this Outlet outlet, 
            IDictionary<Operator, OperatorViewModel> dictionary,
            int rootDocumentID,
            ChildDocumentTypeEnum? childDocumentTypeEnum,
            int? childDocumentListIndex,
            int patchListIndex,
            int operatorIndexNumber,
            int outletListIndex)
        {
            OutletViewModel viewModel = outlet.ToViewModel(rootDocumentID, childDocumentTypeEnum, childDocumentListIndex, patchListIndex, operatorIndexNumber, outletListIndex);

            // Recursive call
            viewModel.Operator = outlet.Operator.ToViewModelRecursive(dictionary, rootDocumentID, childDocumentTypeEnum, childDocumentListIndex, patchListIndex);

            return viewModel;
        }
    }
}
