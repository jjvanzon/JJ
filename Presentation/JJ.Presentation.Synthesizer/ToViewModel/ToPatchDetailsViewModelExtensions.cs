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
        public static PatchDetailsViewModel ToDetailsViewModel(this Patch patch, IOperatorTypeRepository operatorTypeRepository, EntityPositionManager entityPositionManager)
        {
            if (patch == null) throw new NullException(() => patch);
            if (entityPositionManager == null) throw new NullException(() => entityPositionManager);

            var viewModel = new PatchDetailsViewModel
            {
                Entity = patch.ToViewModelRecursive(),
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
            EntityPosition entityPosition = entityPositionManager.GetOrCreateOperatorPosition(operatorViewModel.ID);
            operatorViewModel.CenterX = entityPosition.X;
            operatorViewModel.CenterY = entityPosition.Y;
        }

        private static PatchViewModel ToViewModelRecursive(this Patch patch)
        {
            PatchViewModel viewModel = patch.ToViewModel();

            var dictionary = new Dictionary<Operator, OperatorViewModel>();

            viewModel.Operators = patch.Operators.Select(x => x.ToViewModelRecursive(dictionary)).ToList();

            return viewModel;
        }

        private static OperatorViewModel ToViewModelRecursive(this Operator op, IDictionary<Operator, OperatorViewModel> dictionary)
        {
            OperatorViewModel viewModel;
            if (dictionary.TryGetValue(op, out viewModel))
            {
                return viewModel;
            }

            viewModel = op.ToViewModel();

            dictionary.Add(op, viewModel);

            // TODO: When PatchInlets do not have inlets and PatchOutlets do not have PatchOutlets (in the future) these if's are probably not necessary anymore.

            if (op.GetOperatorTypeEnum() != OperatorTypeEnum.PatchInlet)
            {
                viewModel.Inlets = op.Inlets.ToViewModelsRecursive(dictionary);
            }
            else
            {
                viewModel.Inlets = new List<InletViewModel>();
            }

            if (op.GetOperatorTypeEnum() != OperatorTypeEnum.PatchOutlet)
            {
                viewModel.Outlets = op.Outlets.ToViewModelsRecursive(dictionary);
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
        private static OperatorViewModel ToViewModelWithRelatedEntities(this Operator op)
        {
            // Do not reuse this in ToViewModelRecursive, because there you have to do a dictionary.Add there right in the middle of things.
            OperatorViewModel viewModel = op.ToViewModel();

            viewModel.Inlets = op.Inlets.ToViewModels();
            viewModel.Outlets = op.Outlets.ToViewModels();

            return viewModel;
        }

        /// <summary>
        /// Includes its inlets and outlets.
        /// Also includes the inverse property OutletViewModel.Operator.
        /// That view model is one the few with an inverse property.
        /// </summary>
        public static OperatorViewModel ToViewModelWithRelatedEntitiesAndInverseProperties(this Operator op)
        {
            OperatorViewModel operatorViewModel = op.ToViewModel();
            operatorViewModel.Inlets = op.Inlets.ToViewModels();
            operatorViewModel.Outlets = op.Outlets.ToViewModels();

            // This is the inverse property in the view model!
            foreach (OutletViewModel outletViewModel in operatorViewModel.Outlets)
            {
                outletViewModel.Operator = operatorViewModel;
            }

            return operatorViewModel;
        }

        private static IList<InletViewModel> ToViewModelsRecursive(this IList<Inlet> entities, IDictionary<Operator, OperatorViewModel> dictionary)
        {
            var viewModels = new List<InletViewModel>(entities.Count);

            // TODO: Introduce SortOrder property and then sort by it.

            for (int i = 0; i < entities.Count; i++)
            {
                Inlet entity = entities[i];

                InletViewModel viewModel = entity.ToViewModelRecursive(dictionary);

                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        private static InletViewModel ToViewModelRecursive(this Inlet inlet, IDictionary<Operator, OperatorViewModel> dictionary)
        {
            InletViewModel viewModel = inlet.ToViewModel();

            if (inlet.InputOutlet != null)
            {
                viewModel.InputOutlet = inlet.InputOutlet.ToViewModelRecursive(dictionary);
            }

            return viewModel;
        }

        private static IList<OutletViewModel> ToViewModelsRecursive(this IList<Outlet> entities, IDictionary<Operator, OperatorViewModel> dictionary)
        {
            var viewModels = new List<OutletViewModel>(entities.Count);

            // TODO: Introduce SortOrder property and then sort by it.

            for (int i = 0; i < entities.Count; i++)
            {
                Outlet entity = entities[i];
                OutletViewModel viewModel = entity.ToViewModelRecursive(dictionary);
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        private static OutletViewModel ToViewModelRecursive(this Outlet outlet, IDictionary<Operator, OperatorViewModel> dictionary)
        {
            OutletViewModel viewModel = outlet.ToViewModel();

            // Recursive call
            viewModel.Operator = outlet.Operator.ToViewModelRecursive(dictionary);

            return viewModel;
        }
    }
}
