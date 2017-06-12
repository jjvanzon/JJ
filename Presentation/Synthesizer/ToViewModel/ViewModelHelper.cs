﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.Synthesizer;
using JJ.Business.Synthesizer.Dto;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Canonical;
using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.RepositoryInterfaces;
using JJ.Framework.Common;
using JJ.Framework.Mathematics;
using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using JJ.Framework.Collections;
using JJ.Framework.Configuration;

namespace JJ.Presentation.Synthesizer.ToViewModel
{
    /// <summary> Empty view models start out with Visible = false. </summary>
    internal static partial class ViewModelHelper
    {
        public const string DIMENSION_KEY_EMPTY = "";
        public const string STANDARD_DIMENSION_KEY_PREFIX = "0C26ADA8-0BFC-484C-BF80-774D055DAA3F-StandardDimension-";
        public const string CUSTOM_DIMENSION_KEY_PREFIX = "5133584A-BA76-42DB-BD0E-42801FCB96DF-CustomDimension-";

        private const int STRETCH_AND_SQUASH_ORIGIN_LIST_INDEX = 2;
        private const int RANGE_OVER_OUTLETS_FROM_LIST_INDEX = 0;
        private const int RANGE_OVER_OUTLETS_STEP_LIST_INDEX = 1;

        private static readonly bool _idsVisible = CustomConfigurationManager.GetSection<ConfigurationSection>().IDsVisible;

        // OperatorTypeEnum HashSets

        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithCollectionRecalculationPropertyViews { get; } =
            new HashSet<OperatorTypeEnum>
            {
                OperatorTypeEnum.AverageOverDimension,
                OperatorTypeEnum.ClosestOverDimension,
                OperatorTypeEnum.ClosestOverDimensionExp,
                OperatorTypeEnum.MaxOverDimension,
                OperatorTypeEnum.MinOverDimension,
                OperatorTypeEnum.SortOverDimension,
                OperatorTypeEnum.SumOverDimension
            };

        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithInletCountPropertyViews { get; } =
            new HashSet<OperatorTypeEnum>
            {
                OperatorTypeEnum.Add,
                OperatorTypeEnum.AverageOverInlets,
                OperatorTypeEnum.ClosestOverInlets,
                OperatorTypeEnum.ClosestOverInletsExp,
                OperatorTypeEnum.MaxOverInlets,
                OperatorTypeEnum.MinOverInlets,
                OperatorTypeEnum.Multiply,
                OperatorTypeEnum.SortOverInlets
            };

        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithInterpolationPropertyViews { get; } =
            new HashSet<OperatorTypeEnum>
            {
                OperatorTypeEnum.Random,
                OperatorTypeEnum.Interpolate
            };

        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithOutletCountPropertyViews { get; } =
            new HashSet<OperatorTypeEnum>
            {
                OperatorTypeEnum.DimensionToOutlets,
                OperatorTypeEnum.RangeOverOutlets
            };

        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithTheirOwnPropertyViews { get; } =
            new HashSet<OperatorTypeEnum>
            {
                OperatorTypeEnum.Cache,
                OperatorTypeEnum.Curve,
                OperatorTypeEnum.InletsToDimension,
                OperatorTypeEnum.Number,
                OperatorTypeEnum.PatchInlet,
                OperatorTypeEnum.PatchOutlet,
                OperatorTypeEnum.Sample
            };

        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithoutAlternativePropertiesView_WithUnderlyingPatch { get; } =
            new HashSet<OperatorTypeEnum>
            {
                OperatorTypeEnum.Absolute,
                OperatorTypeEnum.And,
                OperatorTypeEnum.CustomOperator
            };

        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithoutAlternativePropertiesView_WithoutUnderlyingPatch { get; } =
            EnumHelper.GetValues<OperatorTypeEnum>().Except(OperatorTypeEnums_WithTheirOwnPropertyViews)
                      .Except(OperatorTypeEnums_WithInterpolationPropertyViews)
                      .Except(OperatorTypeEnums_WithCollectionRecalculationPropertyViews)
                      .Except(OperatorTypeEnums_WithOutletCountPropertyViews)
                      .Except(OperatorTypeEnums_WithInletCountPropertyViews)
                      .Except(OperatorTypeEnums_WithoutAlternativePropertiesView_WithUnderlyingPatch)
                      .ToHashSet();

        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithoutAlternativePropertiesView { get; } =
            // ReSharper disable once InvokeAsExtensionMethod
            Enumerable.Union(
                OperatorTypeEnums_WithoutAlternativePropertiesView_WithUnderlyingPatch,
                OperatorTypeEnums_WithoutAlternativePropertiesView_WithoutUnderlyingPatch).ToHashSet();

        [Obsolete("Will be replaced with Inlet.NameOrDimensionHidden, "+
                  "but only after all standard operators have been bootstrapped " +
                  "into the System Document.")]
        public static HashSet<OperatorTypeEnum> OperatorTypeEnums_WithHiddenInletNames { get; } =
            new HashSet<OperatorTypeEnum>
            {
                OperatorTypeEnum.Add,
                OperatorTypeEnum.PatchInlet,
                OperatorTypeEnum.PatchOutlet,
                OperatorTypeEnum.Subtract,
                OperatorTypeEnum.Number,
                OperatorTypeEnum.Curve,
                OperatorTypeEnum.Noise,
                OperatorTypeEnum.Equal,
                OperatorTypeEnum.NotEqual,
                OperatorTypeEnum.LessThan,
                OperatorTypeEnum.GreaterThan,
                OperatorTypeEnum.LessThanOrEqual,
                OperatorTypeEnum.GreaterThanOrEqual,
                OperatorTypeEnum.And,
                OperatorTypeEnum.Or,
                OperatorTypeEnum.Not,
                OperatorTypeEnum.Absolute,
                OperatorTypeEnum.Negative,
                OperatorTypeEnum.OneOverX,
                OperatorTypeEnum.GetDimension,
                OperatorTypeEnum.Hold,
                OperatorTypeEnum.DimensionToOutlets,
                OperatorTypeEnum.InletsToDimension,
                OperatorTypeEnum.MaxOverInlets,
                OperatorTypeEnum.MinOverInlets,
                OperatorTypeEnum.AverageOverInlets,
                OperatorTypeEnum.Multiply,
                OperatorTypeEnum.SortOverInlets,
            };

        [Obsolete("Will be replaced with Outlet.NameOrDimensionHidden, " +
                  "but only after all standard operators have been bootstrapped " +
                  "into the System Document.")]
        // A list until it will have more items. Then it might be made a HashSet for performance.
        public static IList<OperatorTypeEnum> OperatorTypeEnums_WithVisibleOutletNames { get; } =
            new List<OperatorTypeEnum>
            {
                OperatorTypeEnum.ChangeTrigger,
                OperatorTypeEnum.CustomOperator,
                OperatorTypeEnum.PulseTrigger,
                OperatorTypeEnum.Reset,
                OperatorTypeEnum.SetDimension,
                OperatorTypeEnum.ToggleTrigger,
            };

        // CurrentInstrument

        public static CurrentInstrumentViewModel CreateCurrentInstrumentViewModel(IList<Patch> patches, Document higherDocument)
        {
            if (patches == null) throw new NullException(() => patches);
            if (higherDocument == null) throw new NullException(() => higherDocument);

            Dictionary<int?, DocumentReference> documentReferenceDictionary = higherDocument.LowerDocumentReferences.ToDictionary(x => x.LowerDocument?.ID);

            var viewModel = new CurrentInstrumentViewModel
            {
                DocumentID = higherDocument.ID,
                List = patches.Select(x => toIDAndName(x)).ToList(),
                ValidationMessages = new List<MessageDto>()
            };

            IDAndName toIDAndName(Patch entity)
            {
                if (entity.Document?.ID == higherDocument.ID)
                {
                    return entity.ToIDAndName();
                }

                int? lowerDocumentID = entity.Document?.ID;
                DocumentReference documentReference = documentReferenceDictionary[lowerDocumentID];

                var idAndName = new IDAndName
                {
                    ID = entity.ID,
                    Name = $"{documentReference.GetAliasOrName()} | {entity.Name}"
                };

                return idAndName;
            }

            return viewModel;
        }

        public static CurrentInstrumentViewModel CreateCurrentInstrumentViewModelWithEmptyList(Document higherDocument)
        {
            if (higherDocument == null) throw new NullException(() => higherDocument);

            var viewModel = new CurrentInstrumentViewModel
            {
                DocumentID = higherDocument.ID,
                List = new List<IDAndName>(),
                ValidationMessages = new List<MessageDto>()
            };

            return viewModel;
        }

        // Dimensions

        public static string GetDimensionKey(Operator op)
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!string.IsNullOrEmpty(op.CustomDimensionName))
            {
                return $"{CUSTOM_DIMENSION_KEY_PREFIX}{op.CustomDimensionName}";
            }
            else
            {
                return GetDimensionKey(op.GetStandardDimensionEnum());
            }
        }

        public static string GetDimensionKey(DimensionEnum standardDimensionEnum)
        {
            if (standardDimensionEnum != DimensionEnum.Undefined)
            {
                return $"{STANDARD_DIMENSION_KEY_PREFIX}{standardDimensionEnum}";
            }

            return DIMENSION_KEY_EMPTY;
        }

        public static string TryGetDimensionName(Operator op)
        {
            if (!string.IsNullOrEmpty(op.CustomDimensionName))
            {
                return op.CustomDimensionName;
            }

            DimensionEnum standardDimensionEnum = op.GetStandardDimensionEnum();
            if (standardDimensionEnum != DimensionEnum.Undefined)
            {
                return ResourceFormatter.GetDisplayName(standardDimensionEnum);
            }

            return null;
        }

        private static bool MustStyleDimension(Operator entity)
        {
            if (entity.OperatorType == null)
            {
                return false;
            }

            switch (entity.GetOperatorTypeEnum())
            {
                case OperatorTypeEnum.GetDimension:
                case OperatorTypeEnum.SetDimension:
                    return false;
            }

            return entity.OperatorType.HasDimension;
        }

        // Document

        public static DocumentDeletedViewModel CreateDocumentDeletedViewModel()
        {
            var viewModel = new DocumentDeletedViewModel
            {
                ValidationMessages = new List<MessageDto>()
            };

            return viewModel;
        }

        public static DocumentOrPatchNotFoundPopupViewModel CreateDocumentOrPatchNotFoundPopupViewModel(string message = null)
        {
            DocumentOrPatchNotFoundPopupViewModel viewModel = CreateEmptyDocumentOrPatchNotFoundPopupViewModel();

            viewModel.NotFoundMessage = message;

            return viewModel;
        }

        public static bool GetCanPlay(DocumentTreeNodeTypeEnum selectedNodeType)
        {
            switch (selectedNodeType)
            {
                case DocumentTreeNodeTypeEnum.AudioOutput:
                case DocumentTreeNodeTypeEnum.Library:
                case DocumentTreeNodeTypeEnum.LibraryPatch:
                case DocumentTreeNodeTypeEnum.LibraryPatchGroup:
                case DocumentTreeNodeTypeEnum.Patch:
                case DocumentTreeNodeTypeEnum.PatchGroup:
                case DocumentTreeNodeTypeEnum.Samples:
                case DocumentTreeNodeTypeEnum.Libraries:
                    return true;

                case DocumentTreeNodeTypeEnum.AudioFileOutputList:
                case DocumentTreeNodeTypeEnum.Curves:
                case DocumentTreeNodeTypeEnum.Scales:
                default:
                    return false;
            }
        }

        public static bool GetCanOpenExternally(DocumentTreeNodeTypeEnum selectedNodeType)
        {
            switch (selectedNodeType)
            {
                case DocumentTreeNodeTypeEnum.Library:
                case DocumentTreeNodeTypeEnum.LibraryPatch:
                    return true;

                case DocumentTreeNodeTypeEnum.AudioFileOutputList:
                case DocumentTreeNodeTypeEnum.AudioOutput:
                case DocumentTreeNodeTypeEnum.Curves:
                case DocumentTreeNodeTypeEnum.LibraryPatchGroup:
                case DocumentTreeNodeTypeEnum.Libraries:
                case DocumentTreeNodeTypeEnum.Patch:
                case DocumentTreeNodeTypeEnum.PatchGroup:
                case DocumentTreeNodeTypeEnum.Samples:
                case DocumentTreeNodeTypeEnum.Scales:
                default:
                    return false;
            }
        }

        // Menu

        public static MenuViewModel CreateMenuViewModel(bool documentIsOpen)
        {
            var viewModel = new MenuViewModel
            {
                DocumentList = new MenuItemViewModel { Visible = true },
                DocumentTree = new MenuItemViewModel { Visible = documentIsOpen },
                DocumentClose = new MenuItemViewModel { Visible = documentIsOpen },
                CurrentInstrument = new MenuItemViewModel { Visible = documentIsOpen },
                DocumentProperties = new MenuItemViewModel { Visible = documentIsOpen },
                ValidationMessages = new List<MessageDto>()
            };

            return viewModel;
        }

        // Node

        public static string GetNodeCaption(Node entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return $"{entity.X:0.####}, {entity.Y:0.####}";
        }

        // Tone

        public static string GetToneGridEditNumberTitle(Scale entity)
        {
            if (entity == null) throw new NullException(() => entity);

            return ResourceFormatter.GetScaleTypeDisplayNameSingular(entity);
        }

        // Patch-Related

        public static Dictionary<string, PatchGridViewModel> CreatePatchGridViewModelDictionary(
            IList<UsedInDto<Patch>> grouplessPatchUsedInDtos,
            IList<PatchGroupDto_WithUsedIn> patchGroupDtos,
            int rootDocumentID)
        {
            if (grouplessPatchUsedInDtos == null) throw new NullException(() => grouplessPatchUsedInDtos);
            if (patchGroupDtos == null) throw new NullException(() => patchGroupDtos);

            // ReSharper disable once UseObjectOrCollectionInitializer
            var list = new List<PatchGridViewModel>();

            list.Add(grouplessPatchUsedInDtos.ToGridViewModel(rootDocumentID, null));
            list.AddRange(patchGroupDtos.Select(x => x.PatchUsedInDtos.ToGridViewModel(rootDocumentID, x.GroupName)));

            return list.ToDictionary(x => NameHelper.ToCanonical(x.Group));
        }

        public static bool GetInletVisible(Inlet inlet)
        {
            if (inlet == null) throw new NullException(() => inlet);

            if (inlet.InputOutlet != null)
            {
                return true;
            }

            Operator op = inlet.Operator;

            OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();

            switch (operatorTypeEnum)
            {
                case OperatorTypeEnum.PatchInlet:
                    return false;

                case OperatorTypeEnum.Stretch:
                case OperatorTypeEnum.Squash:
                    if (inlet.ListIndex == STRETCH_AND_SQUASH_ORIGIN_LIST_INDEX)
                    {
                        if (op.GetStandardDimensionEnum() == DimensionEnum.Time)
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        public static bool GetOutletVisible(Outlet outlet)
        {
            if (outlet == null) throw new NullException(() => outlet);

            OperatorTypeEnum operatorTypeEnum = outlet.Operator.GetOperatorTypeEnum();

            if (operatorTypeEnum == OperatorTypeEnum.PatchOutlet)
            {
                return false;
            }

            return true;
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
            // ReSharper disable once InvertIf
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
        public static void RefreshInletViewModels(
            IList<Inlet> sourceInlets,
            OperatorViewModel destOperatorViewModel,
            ICurveRepository curveRepository,
            ISampleRepository sampleRepository,
            EntityPositionManager entityPositionManager)
        {
            if (sourceInlets == null) throw new NullException(() => sourceInlets);
            if (destOperatorViewModel == null) throw new NullException(() => destOperatorViewModel);

            var inletViewModelsToKeep = new List<InletViewModel>(sourceInlets.Count);
            foreach (Inlet inlet in sourceInlets)
            {
                InletViewModel inletViewModel = destOperatorViewModel.Inlets.Where(x => x.ID == inlet.ID).FirstOrDefault();
                if (inletViewModel == null)
                {
                    inletViewModel = new InletViewModel();
                    destOperatorViewModel.Inlets.Add(inletViewModel);
                }

                inlet.ToViewModel(inletViewModel, curveRepository, sampleRepository, entityPositionManager);

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
        public static void RefreshOutletViewModels(
            IList<Outlet> sourceOutlets,
            OperatorViewModel destOperatorViewModel,
            ICurveRepository curveRepository,
            ISampleRepository sampleRepository,
            EntityPositionManager entityPositionManager)
        {
            if (sourceOutlets == null) throw new NullException(() => sourceOutlets);
            if (destOperatorViewModel == null) throw new NullException(() => destOperatorViewModel);

            var outletViewModelsToKeep = new List<OutletViewModel>(sourceOutlets.Count);
            foreach (Outlet outlet in sourceOutlets)
            {
                OutletViewModel outletViewModel = destOperatorViewModel.Outlets.Where(x => x.ID == outlet.ID).FirstOrDefault();
                if (outletViewModel == null)
                {
                    outletViewModel = new OutletViewModel();
                    destOperatorViewModel.Outlets.Add(outletViewModel);

                    // The only inverse property in all the view models.
                    outletViewModel.Operator = destOperatorViewModel;
                }

                outlet.ToViewModel(outletViewModel, curveRepository, sampleRepository, entityPositionManager);

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

        /// <summary>
        /// Is used to be able to update an existing operator view model in-place
        /// without having to re-establish the intricate relations with other operators.
        /// </summary>
        public static void RefreshViewModel_WithInletsAndOutlets(
            Operator entity,
            OperatorViewModel operatorViewModel,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository,
            EntityPositionManager entityPositionManager)
        {
            RefreshViewModel(entity, operatorViewModel, sampleRepository, curveRepository, entityPositionManager);
            RefreshInletViewModels(entity.Inlets, operatorViewModel, curveRepository, sampleRepository, entityPositionManager);
            RefreshOutletViewModels(entity.Outlets, operatorViewModel, curveRepository, sampleRepository, entityPositionManager);
        }

        /// <summary>
        /// Is used to be able to update an existing operator view model in-place
        /// without having to re-establish the intricate relations with other operators.
        /// </summary>
        public static void RefreshViewModel(
            Operator entity,
            OperatorViewModel viewModel,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository,
            EntityPositionManager entityPositionManager)
        {
            if (entity == null) throw new NullException(() => entity);
            if (viewModel == null) throw new NullException(() => viewModel);

            viewModel.ID = entity.ID;
            viewModel.StyleGrade = StyleGradeEnum.StyleGradeNeutral;
            viewModel.Caption = GetOperatorCaption(entity, sampleRepository, curveRepository);
            viewModel.IsOwned = GetOperatorIsOwned(entity);

            if (entity.OperatorType != null)
            {
                viewModel.OperatorType = entity.OperatorType.ToIDAndDisplayName();
            }
            else
            {
                viewModel.OperatorType = CreateEmptyIDAndName();
            }

            EntityPosition entityPosition = entityPositionManager.GetOrCreateOperatorPosition(entity.ID);
            viewModel.EntityPositionID = entityPosition.ID;
            viewModel.CenterX = entityPosition.X;
            viewModel.CenterY = entityPosition.Y;
            viewModel.Dimension = entity.ToDimensionViewModel();
        }

        public static DimensionViewModel ToDimensionViewModel(this Operator entity)
        {
            var viewModel = new DimensionViewModel
            {
                Key = GetDimensionKey(entity),
                Name = TryGetDimensionName(entity),
                Visible = MustStyleDimension(entity)
            };

            return viewModel;
        }

        public static string GetOperatorCaption(
            Operator op,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository)
        {
            if (op == null) throw new NullException(() => op);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (curveRepository == null) throw new NullException(() => curveRepository);

            OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();

            string operatorCaption;

            switch (operatorTypeEnum)
            {
                case OperatorTypeEnum.AverageOverDimension:
                case OperatorTypeEnum.AverageOverInlets:
                    operatorCaption = ResourceFormatter.Average;
                    break;

                case OperatorTypeEnum.ClosestOverDimension:
                case OperatorTypeEnum.ClosestOverInlets:
                    operatorCaption = ResourceFormatter.Closest;
                    break;

                case OperatorTypeEnum.ClosestOverDimensionExp:
                case OperatorTypeEnum.ClosestOverInletsExp:
                    operatorCaption = ResourceFormatter.ClosestExp;
                    break;

                case OperatorTypeEnum.Curve:
                    operatorCaption = GetOperatorCaption_ForCurve(op, curveRepository);
                    break;

                case OperatorTypeEnum.CustomOperator:
                    operatorCaption = GetOperatorCaption_ForCustomOperator(op);
                    break;

                case OperatorTypeEnum.GetDimension:
                    operatorCaption = GetOperatorCaption_ForGetDimension(op);
                    break;

                case OperatorTypeEnum.MaxOverDimension:
                case OperatorTypeEnum.MaxOverInlets:
                    operatorCaption = ResourceFormatter.Max;
                    break;

                case OperatorTypeEnum.MinOverDimension:
                case OperatorTypeEnum.MinOverInlets:
                    operatorCaption = ResourceFormatter.Min;
                    break;

                case OperatorTypeEnum.Number:
                    operatorCaption = GetOperatorCaption_ForNumber(op);
                    break;

                case OperatorTypeEnum.PatchInlet:
                    operatorCaption = GetOperatorCaption_ForPatchInlet(op);
                    break;

                case OperatorTypeEnum.PatchOutlet:
                    operatorCaption = GetOperatorCaption_ForPatchOutlet(op);
                    break;

                case OperatorTypeEnum.RangeOverDimension:
                case OperatorTypeEnum.RangeOverOutlets:
                    operatorCaption = ResourceFormatter.Range;
                    break;

                case OperatorTypeEnum.Sample:
                    operatorCaption = GetOperatorCaption_ForSample(op, sampleRepository);
                    break;

                case OperatorTypeEnum.SetDimension:
                    operatorCaption = GetOperatorCaption_ForSetDimension(op);
                    break;

                case OperatorTypeEnum.SortOverDimension:
                case OperatorTypeEnum.SortOverInlets:
                    operatorCaption = ResourceFormatter.Sort;
                    break;

                case OperatorTypeEnum.SumOverDimension:
                    operatorCaption = ResourceFormatter.Sum;
                    break;

                default:
                    operatorCaption = GetOperatorCaption_ForOtherOperators(op);
                    break;
            }

            if (_idsVisible)
            {
                operatorCaption += $" ({op.ID})";
            }

            return operatorCaption;
        }

        private static string GetOperatorCaption_ForCurve(Operator op, ICurveRepository curveRepository)
        {
            string operatorTypeDisplayName = ResourceFormatter.Curve;

            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return $"{operatorTypeDisplayName}: {op.Name}";
            }

            // Use Curve.Name
            var wrapper = new Curve_OperatorWrapper(op, curveRepository);
            Curve underlyingEntity = wrapper.Curve;
            if (!string.IsNullOrWhiteSpace(underlyingEntity?.Name))
            {
                return $"{operatorTypeDisplayName}: {underlyingEntity.Name}";
            }

            // Use OperatorTypeDisplayName
            string caption = operatorTypeDisplayName;
            return caption;
        }

        private static string GetOperatorCaption_ForCustomOperator(Operator op)
        {
            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return op.Name;
            }

            // Use UnderlyingPatch.Name
            Patch underlyingPatch = op.UnderlyingPatch;
            if (!string.IsNullOrWhiteSpace(underlyingPatch?.Name))
            {
                return underlyingPatch.Name;
            }

            // Use OperatorTypeDisplayName
            string caption = ResourceFormatter.GetDisplayName(op.GetOperatorTypeEnum());
            return caption;
        }

        /// <summary> Value Operator: display name and value or only value. </summary>
        private static string GetOperatorCaption_ForNumber(Operator op)
        {
            var wrapper = new Number_OperatorWrapper(op);
            string formattedValue = wrapper.Number.ToString("0.####");

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (string.IsNullOrWhiteSpace(op.Name))
            {
                return formattedValue;
            }
            else
            {
                return $"{op.Name}: {formattedValue}";
            }
        }

        private static string GetOperatorCaption_ForPatchInlet(Operator op)
        {
            var sb = new StringBuilder();

            var wrapper = new PatchInlet_OperatorWrapper(op);
            DimensionEnum dimensionEnum = wrapper.Inlet.GetDimensionEnum();

            // Use OperatorType DisplayName
            sb.Append(ResourceFormatter.Inlet);

            // Try Use Operator Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                sb.Append($": {op.Name}");
            }
            // Try Use Dimension
            else if (dimensionEnum != DimensionEnum.Undefined)
            {
                sb.Append($": {ResourceFormatter.GetDisplayName(dimensionEnum)}");
            }
            // Try Use List Index
            else
            {
                sb.Append($" {wrapper.Inlet.ListIndex + 1}");
            }

            // Try Use DefaultValue
            double? defaultValue = wrapper.Inlet.DefaultValue;
            if (defaultValue.HasValue)
            {
                sb.Append($" = {defaultValue.Value}");
            }

            return sb.ToString();
        }

        private static string GetOperatorCaption_ForPatchOutlet(Operator op)
        {
            var sb = new StringBuilder();

            var wrapper = new PatchOutlet_OperatorWrapper(op);
            Outlet outlet = wrapper.Outlet;
            DimensionEnum dimensionEnum = outlet.GetDimensionEnum();

            // Use OperatorType DisplayName
            sb.Append(ResourceFormatter.Outlet);

            // Try Use Operator Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                sb.AppendFormat(": {0}", op.Name);
            }
            // Try Use Dimension
            else if (dimensionEnum != DimensionEnum.Undefined)
            {
                sb.AppendFormat(": {0}", ResourceFormatter.GetDisplayName(dimensionEnum));
            }
            // Try Use List Index
            else
            {
                sb.AppendFormat(" {0}", wrapper.Outlet.ListIndex + 1);
            }

            return sb.ToString();
        }

        private static string GetOperatorCaption_ForSample(Operator op, ISampleRepository sampleRepository)
        {
            string operatorTypeDisplayName = ResourceFormatter.Sample;

            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return $"{operatorTypeDisplayName}: {op.Name}";
            }

            // Use Sample.Name
            var wrapper = new Sample_OperatorWrapper(op, sampleRepository);
            Sample underlyingEntity = wrapper.Sample;
            if (!string.IsNullOrWhiteSpace(underlyingEntity?.Name))
            {
                return $"{operatorTypeDisplayName}: {underlyingEntity.Name}";
            }

            // Use OperatorType DisplayName
            string caption = operatorTypeDisplayName;
            return caption;
        }

        private static string GetOperatorCaption_ForGetDimension(Operator op)
        {
            return GetOperatorCaption_WithDimensionPlaceholder(
                op,
                ResourceFormatter.GetDimensionWithPlaceholder("{0}")); // HACK: Method delegated to will replace placeholder.
        }

        private static string GetOperatorCaption_ForSetDimension(Operator op)
        {
            return GetOperatorCaption_WithDimensionPlaceholder(
                op,
                ResourceFormatter.SetDimensionWithPlaceholder("{0}")); // HACK: Method delegated to will replace placeholder.
        }

        private static string GetOperatorCaption_WithDimensionPlaceholder(Operator op, string operatorTypeDisplayNameWithPlaceholder)
        {
            DimensionEnum dimensionEnum = op.GetStandardDimensionEnum();
            string formattedOperatorTypeDisplayName;
            if (dimensionEnum != DimensionEnum.Undefined)
            {
                string dimensionDisplayName = ResourceFormatter.GetDisplayName(dimensionEnum);
                formattedOperatorTypeDisplayName = string.Format(operatorTypeDisplayNameWithPlaceholder, dimensionDisplayName);
            }
            else
            {
                formattedOperatorTypeDisplayName = ResourceFormatter.GetOperatorTypeDisplayName(op);
            }

            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return $"{formattedOperatorTypeDisplayName}: {op.Name}";
            }
            // Use OperatorType DisplayName only.
            else
            {
                return formattedOperatorTypeDisplayName;
            }
        }

        private static string GetOperatorCaption_ForOtherOperators(Operator op)
        {
            string operatorTypeDisplayName = ResourceFormatter.GetDisplayName(op.GetOperatorTypeEnum());

            // Use Operator.Name
            if (!string.IsNullOrWhiteSpace(op.Name))
            {
                return $"{operatorTypeDisplayName}: {op.Name}";
            }

            // Use OperatorType DisplayName
            string caption = operatorTypeDisplayName;
            return caption;
        }

        public static string GetInletCaption(
            Inlet inlet,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository)
        {
            var sb = new StringBuilder();

            OperatorTypeEnum operatorTypeEnum = inlet.Operator.GetOperatorTypeEnum();
            bool nameOrDimensionHidden = inlet.NameOrDimensionHidden ||
                                         OperatorTypeEnums_WithHiddenInletNames.Contains(operatorTypeEnum);
            if (!nameOrDimensionHidden)
            {
                OperatorWrapperBase wrapper = OperatorWrapperFactory.CreateOperatorWrapper(
                    inlet.Operator,
                    curveRepository,
                    sampleRepository);
                string inletDisplayName = wrapper.GetInletDisplayName(inlet);
                sb.Append(inletDisplayName);
            }

            if (inlet.InputOutlet == null)
            {
                if (inlet.DefaultValue.HasValue)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(' ');
                    }
                    sb.AppendFormat("= {0:0.####}", inlet.DefaultValue.Value);
                }
            }

            if (inlet.IsObsolete)
            {
                AppendObsoleteFlag(sb);
            }

            if (_idsVisible)
            {
                sb.Append($" ({inlet.ID})");
            }

            return sb.ToString();
        }

        public static string GetOutletCaption(
            Outlet outlet,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository)
        {
            if (outlet == null) throw new NullException(() => outlet);

            switch (outlet.Operator.GetOperatorTypeEnum())
            {
                case OperatorTypeEnum.RangeOverOutlets:
                    return GetOutletCaption_ForRangeOverOutlets(outlet);

                default:
                    return GetOutletCaption_ForOtherOperatorType(outlet, sampleRepository, curveRepository);
            }
        }

        private static string GetOutletCaption_ForRangeOverOutlets(Outlet outlet)
        {
            var sb = new StringBuilder();

            double? from = outlet.Operator.Inlets
                                 .Where(x => x.ListIndex == RANGE_OVER_OUTLETS_FROM_LIST_INDEX)
                                 .Select(x => x.TryGetConstantNumber())
                                 .FirstOrDefault();
            if (from.HasValue)
            {
                double? step = outlet.Operator.Inlets
                                     .Where(x => x.ListIndex == RANGE_OVER_OUTLETS_STEP_LIST_INDEX)
                                     .Select(x => x.TryGetConstantNumber())
                                     .FirstOrDefault();
                if (step.HasValue)
                {
                    int listIndex = outlet.Operator.Outlets.IndexOf(outlet);

                    double value = from.Value + step.Value * listIndex;

                    sb.Append(value.ToString("0.####"));
                }
            }

            if (outlet.IsObsolete)
            {
                AppendObsoleteFlag(sb);
            }

            if (_idsVisible)
            {
                sb.Append($" ({outlet.ID})");
            }

            return sb.ToString();
        }

        private static string GetOutletCaption_ForOtherOperatorType(
            Outlet outlet,
            ISampleRepository sampleRepository,
            ICurveRepository curveRepository)
        {
            var sb = new StringBuilder();

            OperatorTypeEnum operatorTypeEnum = outlet.Operator.GetOperatorTypeEnum();

            // TODO: Use outlet.NameOrDimensionHidden once all system operators are boostrapped.
            //bool nameOrDimensionHidden = outlet.NameOrDimensionHidden;
            //if (OperatorTypeEnums_WithVisibleOutletNames.Contains(operatorTypeEnum))
            //{
            //    nameOrDimensionHidden = false;
            //}

            bool nameOrDimensionHidden = !OperatorTypeEnums_WithVisibleOutletNames.Contains(operatorTypeEnum);
            if (!nameOrDimensionHidden)
            {
                OperatorWrapperBase wrapper = OperatorWrapperFactory.CreateOperatorWrapper(
                    outlet.Operator,
                    curveRepository,
                    sampleRepository);

                string inletDisplayName = wrapper.GetOutletDisplayName(outlet);
                sb.Append(inletDisplayName);
            }

            if (outlet.IsObsolete)
            {
                AppendObsoleteFlag(sb);
            }

            if (_idsVisible)
            {
                sb.Append($" ({outlet.ID})");
            }

            return sb.ToString();
        }

        private static void AppendObsoleteFlag(StringBuilder sb)
        {
            if (sb.Length != 0)
            {
                sb.Append(' ');
            }

            sb.AppendFormat("({0})", ResourceFormatter.IsObsolete);
        }

        public static float? TryGetConnectionDistance(Inlet entity, EntityPositionManager entityPositionManager)
        {
            if (entity == null) throw new NullException(() => entity);

            if (entity.InputOutlet == null)
            {
                return null;
            }

            Operator operator1 = entity.Operator;
            Operator operator2 = entity.InputOutlet.Operator;

            EntityPosition entityPosition1 = entityPositionManager.GetOrCreateOperatorPosition(operator1.ID);
            EntityPosition entityPosition2 = entityPositionManager.GetOrCreateOperatorPosition(operator2.ID);

            float distance = Geometry.AbsoluteDistance(
                entityPosition1.X,
                entityPosition1.Y,
                entityPosition2.X,
                entityPosition2.Y);

            return distance;
        }

        internal static float? TryGetAverageConnectionDistance(Outlet entity, EntityPositionManager entityPositionManager)
        {
            if (entity == null) throw new NullException(() => entity);

            int connectedInletCount = entity.ConnectedInlets.Count;

            if (connectedInletCount == 0)
            {
                return null;
            }

            Operator operator1 = entity.Operator;

            float aggregate = 0f;

            foreach (Inlet connectedInlet in entity.ConnectedInlets)
            {
                Operator operator2 = connectedInlet.Operator;

                EntityPosition entityPosition1 = entityPositionManager.GetOrCreateOperatorPosition(operator1.ID);
                EntityPosition entityPosition2 = entityPositionManager.GetOrCreateOperatorPosition(operator2.ID);

                float distance = Geometry.AbsoluteDistance(
                    entityPosition1.X,
                    entityPosition1.Y,
                    entityPosition2.X,
                    entityPosition2.Y);

                aggregate += distance;
            }

            aggregate /= connectedInletCount;

            return aggregate;
        }

        // UsedIn

        public static string FormatUsedInDto(UsedInDto<Curve> dto)
        {
            if (dto == null) throw new NullException(() => dto);

            var sb = new StringBuilder();

            sb.Append(dto.Entity.Name);

            // ReSharper disable once InvertIf
            if (dto.UsedInIDAndNames.Count > 0)
            {
                string formattedUsedInList = FormatUsedInList(dto.UsedInIDAndNames);
                sb.AppendFormat(" ({0}: {1})", ResourceFormatter.UsedIn, formattedUsedInList);
            }

            return sb.ToString();
        }

        public static string FormatUsedInDto(UsedInDto<Sample> dto)
        {
            if (dto == null) throw new NullException(() => dto);

            var sb = new StringBuilder();

            sb.Append(dto.Entity.Name);

            // ReSharper disable once InvertIf
            if (dto.UsedInIDAndNames.Count > 0)
            {
                string formattedUsedInList = FormatUsedInList(dto.UsedInIDAndNames);
                sb.Append($" ({ResourceFormatter.UsedIn}: {formattedUsedInList})");
            }

            return sb.ToString();
        }

        public static string FormatUsedInList(IList<IDAndName> idAndNames)
        {
            if (idAndNames == null) throw new NullException(() => idAndNames);

            string concatinatedUsedIn = string.Join(", ", idAndNames.Select(x => x.Name).OrderBy(x => x));

            return concatinatedUsedIn;
        }
    }
}