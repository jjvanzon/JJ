﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Data.Synthesizer;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Helpers
{
    internal static class OperatorHelper
    {
        // Sorting

        public static IList<Inlet> GetSortedInlets(Operator op)
        {
            if (op == null) throw new NullException(() => op);

            IList<Inlet> sortedInlets = op.Inlets.OrderBy(x => x.ListIndex).ToArray();
            return sortedInlets;
        }

        internal static Outlet GetInputOutlet(Operator _wrappedOperator, object sPECTRUM_SIGNAL_INDEX)
        {
            throw new NotImplementedException();
        }

        internal static Outlet GetOutlet(Operator _wrappedOperator, object sPECTRUM_RESULT_INDEX)
        {
            throw new NotImplementedException();
        }

        public static IList<Outlet> GetSortedOutlets(Operator op)
        {
            if (op == null) throw new NullException(() => op);

            IList<Outlet> sortedOutlets = op.Outlets.OrderBy(x => x.ListIndex).ToArray();
            return sortedOutlets;
        }

        public static IList<Outlet> GetSortedInputOutlets(Operator op)
        {
            IList<Outlet> outlets = GetSortedInlets(op).Select(x => x.InputOutlet).ToArray();
            return outlets;
        }

        // Get Inlet

        /// <summary> Gets an item out of the sorted _operator.Inlets and verifies that the index is valid in the list. </summary>
        public static Inlet GetInlet(Operator op, int index)
        {
            IList<Inlet> sortedInlets = GetSortedInlets(op);
            if (index >= sortedInlets.Count)
            {
                throw new Exception(String.Format("Sorted inlets does not have index [{0}].", index));
            }
            return sortedInlets[index];
        }

        public static Inlet GetInlet(Operator op, string name)
        {
            Inlet inlet = TryGetInlet(op, name);
            if (inlet == null)
            {
                throw new Exception(String.Format("Inlet '{0}' not found.", name));
            }
            return inlet;
        }

        public static Inlet TryGetInlet(Operator op, string name)
        {
            if (op == null) throw new NullException(() => op);

            IList<Inlet> inlets = op.Inlets.Where(x => String.Equals(x.Name, name)).ToArray();
            switch (inlets.Count)
            {
                case 0:
                    return null;

                case 1:
                    return inlets[0];

                default:
                    throw new Exception(String.Format("Inlet name '{0}' is not unique.", name));
            }
        }

        public static Inlet GetInlet(Operator op, InletTypeEnum inletTypeEnum)
        {
            Inlet inlet = TryGetInlet(op, inletTypeEnum);
            if (inlet == null)
            {
                throw new Exception(String.Format("Inlet with InletTypeEnum '{0}' not found.", inletTypeEnum));
            }
            return inlet;
        }

        public static Inlet TryGetInlet(Operator op, InletTypeEnum inletTypeEnum)
        {
            if (op == null) throw new NullException(() => op);

            IList<Inlet> inlets = GetInlets(op, inletTypeEnum);
            switch (inlets.Count)
            {
                case 0:
                    return null;

                case 1:
                    return inlets[0];

                default:
                    throw new Exception(String.Format("Inlet with InletTypeEnum '{0}' is not unique.", inletTypeEnum));
            }
        }

        public static IList<Inlet> GetInlets(Operator op, InletTypeEnum inletTypeEnum)
        {
            if (op == null) throw new NullException(() => op);

            IList<Inlet> inlets = op.Inlets.Where(x => x.GetInletTypeEnum() == inletTypeEnum).ToArray();
            return inlets;
        }

        // Get Outlet

        /// <summary> Gets an item out of the sorted _operator.Outlets and verifies that the index is valid in the list. </summary>
        public static Outlet GetOutlet(Operator op, int index)
        {
            IList<Outlet> sortedOutlets = GetSortedOutlets(op);
            if (index >= sortedOutlets.Count)
            {
                throw new Exception(String.Format("Sorted outlets does not have index [{0}].", index));
            }
            return sortedOutlets[index];
        }

        public static Outlet GetOutlet(Operator op, string name)
        {
            Outlet outlet = TryGetOutlet(op, name);
            if (outlet == null)
            {
                throw new Exception(String.Format("Outlet '{0}' not found.", name));
            }
            return outlet;
        }

        public static Outlet TryGetOutlet(Operator op, string name)
        {
            if (op == null) throw new NullException(() => op);

            IList<Outlet> outlets = op.Outlets.Where(x => String.Equals(x.Name, name)).ToArray();
            switch (outlets.Count)
            {
                case 0:
                    return null;

                case 1:
                    return outlets[0];

                default:
                    throw new Exception(String.Format("Outlet name '{0}' is not unique.", name));
            }
        }

        public static Outlet GetOutlet(Operator op, OutletTypeEnum outletTypeEnum)
        {
            Outlet outlet = TryGetOutlet(op, outletTypeEnum);
            if (outlet == null)
            {
                throw new Exception(String.Format("Outlet with OutletTypeEnum '{0}' not found.", outletTypeEnum));
            }
            return outlet;
        }

        public static Outlet TryGetOutlet(Operator op, OutletTypeEnum outletTypeEnum)
        {
            if (op == null) throw new NullException(() => op);

            IList<Outlet> outlets = GetOutlets(op, outletTypeEnum);
            switch (outlets.Count)
            {
                case 0:
                    return null;

                case 1:
                    return outlets[0];

                default:
                    throw new Exception(String.Format("Outlet with OutletTypeEnum '{0}' is not unique.", outletTypeEnum));
            }
        }

        public static IList<Outlet> GetOutlets(Operator op, OutletTypeEnum outletTypeEnum)
        {
            if (op == null) throw new NullException(() => op);

            IList<Outlet> outlets = op.Outlets.Where(x => x.GetOutletTypeEnum() == outletTypeEnum).ToArray();
            return outlets;
        }

        // Get InputOutlet

        public static Outlet GetInputOutlet(Operator op, int index)
        {
            Inlet inlet = GetInlet(op, index);
            return inlet.InputOutlet;
        }

        public static Outlet GetInputOutlet(Operator op, string name)
        {
            Inlet inlet = GetInlet(op, name);
            return inlet.InputOutlet;
        }

        public static Outlet GetInputOutlet(Operator op, InletTypeEnum inletTypeEnum)
        {
            Inlet inlet = GetInlet(op, inletTypeEnum);
            return inlet.InputOutlet;
        }

        /// <summary>
        /// Gets the input outlet of a specific inlet.
        /// If it is null, and the inlet has a default value, a fake Number Operator is created and its outlet returned.
        /// If the inlet has no default value either, null is returned.
        /// </summary>
        public static Outlet GetInputOutletOrDefault(Operator op, string name)
        {
            Inlet inlet = GetInlet(op, name);
            return GetInputOutletOrDefault(inlet);
        }

        /// <summary>
        /// Gets the input outlet of a specific inlet.
        /// If it is null, and the inlet has a default value, a fake Number Operator is created and its outlet returned.
        /// If the inlet has no default value either, null is returned.
        /// </summary>
        public static Outlet GetInputOutletOrDefault(Operator op, int index)
        {
            Inlet inlet = GetInlet(op, index);
            return GetInputOutletOrDefault(inlet);
        }

        /// <summary>
        /// Gets the input outlet of a specific inlet.
        /// If it is null, and the inlet has a default value, a fake Number Operator is created and its outlet returned.
        /// If the inlet has no default value either, null is returned.
        /// </summary>
        public static Outlet GetInputOutletOrDefault(Operator op, InletTypeEnum inletTypeEnum)
        {
            Inlet inlet = GetInlet(op, inletTypeEnum);
            return GetInputOutletOrDefault(inlet);
        }

        /// <summary>
        /// Gets the input outlet of a specific inlet.
        /// If it is null, and the inlet has a default value, a fake Number Operator is created and its outlet returned.
        /// If the inlet has no default value either, null is returned.
        /// </summary>
        public static Outlet GetInputOutletOrDefault(Inlet inlet)
        {
            if (inlet == null) throw new NullException(() => inlet);

            if (inlet.InputOutlet != null)
            {
                return inlet.InputOutlet;
            }

            if (inlet.DefaultValue.HasValue)
            {
                Number_OperatorWrapper dummyNumberOperator = CreateDummyNumberOperator(inlet.DefaultValue.Value);
                return dummyNumberOperator.Result;
            }

            return null;
        }

        // Private Methods

        private static Number_OperatorWrapper CreateDummyNumberOperator(double number)
        {
            var operatorType = new OperatorType();
            operatorType.ID = (int)OperatorTypeEnum.Number;
            operatorType.Name = OperatorTypeEnum.Number.ToString();

            var op = new Operator();
            op.LinkTo(operatorType);

            var outlet = new Outlet();
            outlet.LinkTo(op);

            var wrapper = new Number_OperatorWrapper(op);
            wrapper.Number = number;

            return wrapper;
        }
    }
}