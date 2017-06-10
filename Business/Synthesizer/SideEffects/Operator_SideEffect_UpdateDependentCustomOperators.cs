﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.SideEffects
{
    /// <summary> Only effective if the Operator is PatchInlet or PatchOutlet. </summary>
    internal class Operator_SideEffect_UpdateDependentCustomOperators : ISideEffect
    {
        private readonly Operator _operator;
        private readonly RepositoryWrapper _repositories;

        public Operator_SideEffect_UpdateDependentCustomOperators(Operator op, RepositoryWrapper repositories)
        {
            _operator = op ?? throw new NullException(() => op);
            _repositories = repositories ?? throw new NullException(() => repositories);
        }

        public void Execute()
        {
            // ReSharper disable once InvertIf
            if (MustExecute())
            {
                new Patch_SideEffect_UpdateDependentCustomOperators(_operator.Patch, _repositories).Execute();
            }
        }

        private bool MustExecute()
        {
            OperatorTypeEnum operatorTypeEnum = _operator.GetOperatorTypeEnum();

            bool mustExecute = operatorTypeEnum == OperatorTypeEnum.PatchInlet ||
                               operatorTypeEnum == OperatorTypeEnum.PatchOutlet;

            return mustExecute;
        }
    }
}