﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Framework.Validation;
using System;
using System.Collections.Generic;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Versatile_OperatorValidator : ValidatorBase
    {
        public Versatile_OperatorValidator(Operator op)
        {
            if (op == null) throw new NullException(() => op);

            ExecuteValidator(new Basic_OperatorValidator(op));

            OperatorTypeEnum operatorTypeEnum = op.GetOperatorTypeEnum();

            switch (operatorTypeEnum)
            {
                case OperatorTypeEnum.Undefined:
                    // Handle Undefined
                    return;

                case OperatorTypeEnum.CustomOperator:
                    // Handle CustomOperator
                    ExecuteValidator(new CustomOperator_OperatorValidator(op));
                    return;

                default:
                    // Handle ValidatorTypes in dictionary
                    if (_specializedValidatorTypeDictionary.TryGetValue(operatorTypeEnum, out Type validatorType))
                    {
                        var validator = (IValidator)Activator.CreateInstance(validatorType, op);
                        ExecuteValidator(validator);
                        return;
                    }

                    // Otherwise assume from System Document.
                    ExecuteValidator(new OperatorValidator_FromSystemDocument(op));

                    break;
            }
        }

        private readonly Dictionary<OperatorTypeEnum, Type> _specializedValidatorTypeDictionary = new Dictionary<OperatorTypeEnum, Type>
        {
            { OperatorTypeEnum.AverageFollower, typeof(AverageFollower_OperatorValidator) },
            { OperatorTypeEnum.AverageOverDimension, typeof(AverageOverDimension_OperatorValidator) },
            { OperatorTypeEnum.Cache, typeof(Cache_OperatorValidator) },
            { OperatorTypeEnum.ChangeTrigger, typeof(ChangeTrigger_OperatorValidator) },
            { OperatorTypeEnum.ClosestOverDimension, typeof(ClosestOverDimension_OperatorValidator) },
            { OperatorTypeEnum.ClosestOverDimensionExp, typeof(ClosestOverDimensionExp_OperatorValidator) },
            { OperatorTypeEnum.Curve, typeof(Curve_OperatorValidator) },
            { OperatorTypeEnum.DimensionToOutlets, typeof(DimensionToOutlets_OperatorValidator) },
            { OperatorTypeEnum.Exponent, typeof(Exponent_OperatorValidator) },
            { OperatorTypeEnum.GetDimension, typeof(GetDimension_OperatorValidator) },
            { OperatorTypeEnum.Hold, typeof(Hold_OperatorValidator) },
            { OperatorTypeEnum.InletsToDimension, typeof(InletsToDimension_OperatorValidator) },
            { OperatorTypeEnum.Interpolate, typeof(Interpolate_OperatorValidator) },
            { OperatorTypeEnum.Loop, typeof(OperatorValidator_Loop) },
            { OperatorTypeEnum.MaxFollower, typeof(MaxFollower_OperatorValidator) },
            { OperatorTypeEnum.MaxOverDimension, typeof(MaxOverDimension_OperatorValidator) },
            { OperatorTypeEnum.MinFollower, typeof(MinFollower_OperatorValidator) },
            { OperatorTypeEnum.MinOverDimension, typeof(MinOverDimension_OperatorValidator) },
            { OperatorTypeEnum.Number, typeof(Number_OperatorValidator) },
            { OperatorTypeEnum.PatchInlet, typeof(PatchInlet_OperatorValidator) },
            { OperatorTypeEnum.PatchOutlet, typeof(PatchOutlet_OperatorValidator) },
            { OperatorTypeEnum.PulseTrigger, typeof(PulseTrigger_OperatorValidator) },
            { OperatorTypeEnum.Random, typeof(Random_OperatorValidator) },
            { OperatorTypeEnum.RangeOverDimension, typeof(RangeOverDimension_OperatorValidator) },
            { OperatorTypeEnum.Reset, typeof(Reset_OperatorValidator) },
            { OperatorTypeEnum.Reverse, typeof(Reverse_OperatorValidator) },
            { OperatorTypeEnum.Round, typeof(Round_OperatorValidator) },
            { OperatorTypeEnum.Sample, typeof(Sample_OperatorValidator) },
            { OperatorTypeEnum.Scaler, typeof(Scaler_OperatorValidator) },
            { OperatorTypeEnum.SetDimension, typeof(SetDimension_OperatorValidator) },
            { OperatorTypeEnum.Shift, typeof(Shift_OperatorValidator) },
            { OperatorTypeEnum.SortOverDimension, typeof(SortOverDimension_OperatorValidator) },
            { OperatorTypeEnum.Spectrum, typeof(Spectrum_OperatorValidator) },
            { OperatorTypeEnum.Squash, typeof(Squash_OperatorValidator) },
            { OperatorTypeEnum.Stretch, typeof(Stretch_OperatorValidator) },
            { OperatorTypeEnum.SumFollower, typeof(SumFollower_OperatorValidator) },
            { OperatorTypeEnum.SumOverDimension, typeof(SumOverDimension_OperatorValidator) },
            { OperatorTypeEnum.TimePower, typeof(TimePower_OperatorValidator) },
            { OperatorTypeEnum.ToggleTrigger, typeof(ToggleTrigger_OperatorValidator) },
        };
    }
}