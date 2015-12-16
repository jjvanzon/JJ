﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Reflection.Exceptions;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Managers
{
    public partial class PatchManager
    {
        /// <summary>
        /// Use the Patch property after calling this method.
        /// Do a rollback after calling this method to prevent saving the new patch.
        /// Tries to produce a new patch by tying together existing patches,
        /// trying to match PatchInlet and PatchOutlet operators by:
        /// 1) InletType.Name and OutletType.Name
        /// 2) PatchInlet Operator.Name and PatchOutlet Operator.Name.
        /// The non-matched inlets and outlets will become inlets and outlets of the new patch.
        /// If there is overlap in type or name, they will merge to a single inlet or outlet.
        /// </summary>
        public void AutoPatch(IList<Patch> underlyingPatches)
        {
            if (underlyingPatches == null) throw new NullException(() => underlyingPatches);

            CreatePatch();

            var customOperators = new List<Operator>(underlyingPatches.Count);

            foreach (Patch underlyingPatch in underlyingPatches)
            {
                var customOperatorWrapper = CustomOperator(underlyingPatch);
                customOperators.Add(customOperatorWrapper);
            }

            var matchedOutlets = new List<Outlet>();
            var matchedInlets = new List<Inlet>();

            for (int i = 0; i < customOperators.Count; i++)
            {
                for (int j = i + 1; j < customOperators.Count; j++)
                {
                    Operator customOperator1 = customOperators[i];
                    Operator customOperator2 = customOperators[j];

                    foreach (Outlet outlet in customOperator1.Outlets)
                    {
                        foreach (Inlet inlet in customOperator2.Inlets)
                        {
                            if (AreMatch(outlet, inlet))
                            {
                                inlet.InputOutlet = outlet;

                                matchedOutlets.Add(outlet);
                                matchedInlets.Add(inlet);
                            }
                        }
                    }
                }
            }

            // Unmatched inlets of the custom operators become inlets of the new patch.
            IEnumerable<Inlet> unmatchedInlets = customOperators.SelectMany(x => x.Inlets)
                                                                .Except(matchedInlets);
            foreach (Inlet unmatchedInlet in unmatchedInlets)
            {
                var patchInlet = PatchInlet();
                patchInlet.Name = unmatchedInlet.Name;
                patchInlet.ListIndex = unmatchedInlet.ListIndex;
                patchInlet.InletTypeEnum = unmatchedInlet.GetInletTypeEnum();
                patchInlet.DefaultValue = unmatchedInlet.DefaultValue;

                unmatchedInlet.InputOutlet = patchInlet;
            }

            // Unmatched outlets of the custom operators become outlets of the new patch.
            IEnumerable<Outlet> unmatchedOutlets = customOperators.SelectMany(x => x.Outlets)
                                                                  .Except(matchedOutlets);
            foreach (Outlet unmatchedOutlet in unmatchedOutlets)
            {
                var patchOutlet = PatchOutlet();
                patchOutlet.Name = unmatchedOutlet.Name;
                patchOutlet.ListIndex = unmatchedOutlet.ListIndex;
                patchOutlet.OutletTypeEnum = unmatchedOutlet.GetOutletTypeEnum();

                patchOutlet.Input = unmatchedOutlet;
            }

            // TODO: If there is overlap in type or name, they will merge to a single inlet or outlet.
        }

        private bool AreMatch(Outlet outlet, Inlet inlet)
        {
            if (outlet == null)
            {
                return false;
            }

            if (inlet == null)
            {
                return false;
            }

            // First match by OutletType / InletType.
            OutletTypeEnum outletTypeEnum = outlet.GetOutletTypeEnum();
            if (outletTypeEnum != OutletTypeEnum.Undefined)
            {
                InletTypeEnum inletTypeEnum = inlet.GetInletTypeEnum();
                if (inletTypeEnum != InletTypeEnum.Undefined)
                {
                    string outletTypeString = outletTypeEnum.ToString();
                    string inletTypeString = inletTypeEnum.ToString();

                    if (String.Equals(outletTypeString, inletTypeString))
                    {
                        return true;
                    }
                }
            }

            // Then match by name
            if (String.Equals(outlet.Name, inlet.Name))
            {
                return true;
            }

            // Do not match by list index, because that would result in something arbitrary.

            return false;
        }

        /// <summary> Will return null if no Frequency inlet or Signal outlet is found. </summary>
        public Outlet TryAutoPatch_WithTone(Tone tone, IList<Patch> underlyingPatches)
        {
            if (tone == null) throw new NullException(() => tone);
            if (underlyingPatches == null) throw new NullException(() => underlyingPatches);

            // Create a new patch out of the other patches.
            CustomOperator_OperatorWrapper tempCustomOperator = AutoPatch_ToCustomOperator(underlyingPatches);

            // TODO: InletTypes and OutletTypes do not have to be unique and in that case this method crashes.
            Inlet inlet = OperatorHelper.TryGetInlet(tempCustomOperator, InletTypeEnum.Frequency);
            if (inlet != null)
            {
                double frequency = tone.GetFrequency();
                inlet.InputOutlet = Number(frequency);

                Outlet outlet = OperatorHelper.TryGetOutlet(tempCustomOperator, OutletTypeEnum.Signal);
                return outlet;
            }

            return null;
        }

        public CustomOperator_OperatorWrapper AutoPatch_ToCustomOperator(IList<Patch> underlyingPatches)
        {
            if (underlyingPatches == null) throw new NullException(() => underlyingPatches);

            AutoPatch(underlyingPatches);
            Patch tempUnderlyingPatch = Patch;

            // Use new patch as custom operator.
            CreatePatch();
            var customOperator = CustomOperator(tempUnderlyingPatch);

            return customOperator;
        }
    }
}
