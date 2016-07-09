﻿using JJ.Data.Canonical;
using JJ.Business.Synthesizer;
using JJ.Framework.Reflection.Exceptions;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using System.Collections.Generic;
using System.Linq;
using JJ.Presentation.Synthesizer.ToViewModel;
using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Common;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Business.Synthesizer.CopiedCode.FromFramework;

namespace JJ.Presentation.Synthesizer.Converters
{
    internal class RecursiveToViewModelConverter
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly ICurveRepository _curveRepository;
        private readonly IPatchRepository _patchRepository;
        private readonly EntityPositionManager _entityPositionManager;
        private Dictionary<Operator, OperatorViewModel> _dictionary;

        public RecursiveToViewModelConverter(
            ISampleRepository sampleRepository, 
            ICurveRepository curveRepository,
            IPatchRepository patchRepository,
            EntityPositionManager entityPositionManager)
        {
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (curveRepository == null) throw new NullException(() => curveRepository);
            if (patchRepository == null) throw new NullException(() => patchRepository);
            if (entityPositionManager == null) throw new NullException(() => entityPositionManager);

            _sampleRepository = sampleRepository;
            _curveRepository = curveRepository;
            _patchRepository = patchRepository;
            _entityPositionManager = entityPositionManager;
        }

        public PatchDetailsViewModel ConvertToDetailsViewModel(Patch patch)
        {
            if (patch == null) throw new NullException(() => patch);

            _dictionary = new Dictionary<Operator, OperatorViewModel>();

            var viewModel = new PatchDetailsViewModel
            {
                Entity = ConvertToViewModelRecursive(patch),
                ValidationMessages = new List<Message>()
            };

            viewModel.OperatorToolboxItems = ViewModelHelper.GetOperatorTypesViewModel();

            foreach (OperatorViewModel operatorViewModel in viewModel.Entity.Operators)
            {
                SetViewModelPosition(operatorViewModel);
            }

            return viewModel;
        }

        private PatchViewModel ConvertToViewModelRecursive(Patch patch)
        {
            PatchViewModel viewModel = patch.ToViewModel();

            viewModel.Operators = ConvertToViewModelsRecursive(patch.Operators);

            return viewModel;
        }

        /// <summary> Does a ton of specific things regarding the display of dimensions. </summary>
        private IList<OperatorViewModel> ConvertToViewModelsRecursive(IList<Operator> operators)
        {
            var operatorViewModels = new List<OperatorViewModel>(operators.Count);
            var dimensionIDs = new HashSet<int>();

            foreach (Operator op in operators)
            {
                OperatorViewModel operatorViewModel = ConvertToViewModelRecursive(op);

                DimensionEnum dimensionEnum = ViewModelHelper.GetDimensionEnum(op);
                operatorViewModel.Dimension = dimensionEnum.ToIDAndDisplayName();

                dimensionIDs.Add(operatorViewModel.Dimension.ID);

                operatorViewModels.Add(operatorViewModel);
            }

            // Assign style depending on Dimension.
            if (dimensionIDs.Count < 2)
            {
                // If less than 2 dimensions: display neutrally.
                operatorViewModels.ForEach(x => x.StyleGrade = StyleGradeEnum.StyleGrade16);
            }
            else
            {
                IList<OperatorViewModel> operatorViewModelsWithNeutralDimension =
                    operatorViewModels.Where(x => x.Dimension.ID == (int)DimensionEnum.Time ||
                                                  x.Dimension.ID == (int)DimensionEnum.Undefined)
                                      .ToArray();

                // Time should be displayed neutrally.
                operatorViewModelsWithNeutralDimension.ForEach(x => x.StyleGrade = StyleGradeEnum.StyleGrade16);

                // Rest should be displayed in equally spread grades, 
                // sorted by dimension ID (arbitrary, but at least consistent).
                IList<OperatorViewModel> remainingOperatorViewModelModels = operatorViewModels.Except(operatorViewModelsWithNeutralDimension).ToArray();
                IList<int> remainingDimensionIDsSorted = dimensionIDs.Except((int)DimensionEnum.Time)
                                                                     .OrderBy(x => x)
                                                                     .ToArray();

                // Just do not use StyleGrade16 anymore here. That's the easiest.
                int remainingGradeCount = 15;
                int remaingDimensionCount = remainingDimensionIDsSorted.Count;
                double step = (double)remainingGradeCount / (double)remaingDimensionCount;

                var dimensionID_To_StyleGrade_Dictionary = new Dictionary<int, StyleGradeEnum>(remaingDimensionCount);
                for (int i = 0; i < remaingDimensionCount; i++)
                {
                    int dimensionID = remainingDimensionIDsSorted[i];
                    StyleGradeEnum styleGradeEnum = (StyleGradeEnum)(i * step) + 1;

                    dimensionID_To_StyleGrade_Dictionary.Add(dimensionID, styleGradeEnum);
                }

                foreach (OperatorViewModel operatorViewModel in remainingOperatorViewModelModels)
                {
                    int dimensionID = operatorViewModel.Dimension.ID;
                    StyleGradeEnum styleGradeEnum = dimensionID_To_StyleGrade_Dictionary[dimensionID];
                    operatorViewModel.StyleGrade = styleGradeEnum;
                }
            }

            // Do not display text of Time or Undefined dimension
            foreach (OperatorViewModel operatorViewModel in operatorViewModels)
            {
                DimensionEnum dimensionEnum = (DimensionEnum)operatorViewModel.Dimension.ID;

                if (dimensionEnum == DimensionEnum.Undefined ||
                    dimensionEnum == DimensionEnum.Time)
                {
                    operatorViewModel.Dimension = ViewModelHelper.CreateEmptyIDAndName();
                }
            }

            return operatorViewModels;
        }

        private OperatorViewModel ConvertToViewModelRecursive(Operator op)
        {
            OperatorViewModel viewModel;
            if (_dictionary.TryGetValue(op, out viewModel))
            {
                return viewModel;
            }

            viewModel = op.ToViewModel(_sampleRepository, _curveRepository, _patchRepository, _entityPositionManager);

            _dictionary.Add(op, viewModel);

            viewModel.Inlets = ConvertToViewModelsRecursive(op.Inlets);
            viewModel.Outlets = ConvertToViewModelsRecursive(op.Outlets);

            return viewModel;
        }

        private IList<InletViewModel> ConvertToViewModelsRecursive(IList<Inlet> entities)
        {
            IList<InletViewModel> viewModels = entities.Select(x => ConvertToViewModelRecursive(x))
                                                       .OrderBy(x => x.ListIndex)
                                                       .ToList();
            return viewModels;
        }

        private InletViewModel ConvertToViewModelRecursive(Inlet inlet)
        {
            InletViewModel viewModel = inlet.ToViewModel(_curveRepository, _sampleRepository, _patchRepository);

            if (inlet.InputOutlet != null)
            {
                viewModel.InputOutlet = ConvertToViewModelRecursive(inlet.InputOutlet);
            }

            return viewModel;
        }

        private IList<OutletViewModel> ConvertToViewModelsRecursive(IList<Outlet> entities)
        {
            IList<OutletViewModel> viewModels = entities.Select(x => ConvertToViewModelRecursive(x))
                                                        .OrderBy(x => x.ListIndex)
                                                        .ToList();
            return viewModels;
        }

        private OutletViewModel ConvertToViewModelRecursive(Outlet outlet)
        {
            OutletViewModel viewModel = outlet.ToViewModel(_curveRepository, _sampleRepository, _patchRepository);

            _entityPositionManager.GetOrCreateOperatorPosition(outlet.Operator);

            // Recursive call
            viewModel.Operator = ConvertToViewModelRecursive(outlet.Operator);

            return viewModel;
        }

        private void SetViewModelPosition(OperatorViewModel operatorViewModel)
        {
            EntityPosition entityPosition = _entityPositionManager.GetOrCreateOperatorPosition(operatorViewModel.ID);
            operatorViewModel.EntityPositionID = entityPosition.ID;
            operatorViewModel.CenterX = entityPosition.X;
            operatorViewModel.CenterY = entityPosition.Y;
        }
    }
}