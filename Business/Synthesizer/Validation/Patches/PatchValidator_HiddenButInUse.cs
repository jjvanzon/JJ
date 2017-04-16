﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer.Entities;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.RepositoryInterfaces;

namespace JJ.Business.Synthesizer.Validation.Patches
{
    internal class PatchValidator_HiddenButInUse : VersatileValidator<Patch>
    {
        private readonly IPatchRepository _patchRepository;

        public PatchValidator_HiddenButInUse(Patch entity, [NotNull] IPatchRepository patchRepository)
            : base(entity, postponeExecute: true)
        {
            _patchRepository = patchRepository ?? throw new ArgumentNullException(nameof(patchRepository));

            // ReSharper disable once VirtualMemberCallInConstructor
            Execute();
        }

        protected override void Execute()
        {
            Patch lowerPatch = Obj;

            // ReSharper disable once InvertIf
            if (lowerPatch.Hidden)
            {
                string lowerPatchIdentifier = ResourceFormatter.Patch + " " + ValidationHelper.GetUserFriendlyIdentifier(lowerPatch);

                IEnumerable<Operator> dependentExternalOperators =
                    lowerPatch.Document
                              .HigherDocumentReferences
                              .SelectMany(x => x.HigherDocument.Patches)
                              .SelectMany(x => x.EnumerateOperatorsOfType(OperatorTypeEnum.CustomOperator))
                              .Select(x => new CustomOperator_OperatorWrapper(x, _patchRepository))
                              .Where(x => x.UnderlyingPatchID == lowerPatch.ID)
                              .Select(x => x.WrappedOperator);

                foreach (Operator op in dependentExternalOperators)
                {
                    Patch higherPatch = op.Patch;
                    string higherDocumentPrefix = ValidationHelper.TryGetHigherDocumentPrefix(lowerPatch, higherPatch);
                    string higherPatchPrefix = ValidationHelper.GetMessagePrefix(op.Patch);
                    string higherOperatorIdentifier = ResourceFormatter.Operator + " " + ValidationHelper.GetUserFriendlyIdentifier_ForCustomOperator(op, _patchRepository);

                    ValidationMessages.Add(
                        nameof(Patch),
                        ResourceFormatter.CannotHide_WithName_AndDependentItem(lowerPatchIdentifier, higherDocumentPrefix + higherPatchPrefix + higherOperatorIdentifier));
                }
            }
        }
    }
}