﻿using System.Collections;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class OperatorWrapper_WithUnderlyingPatch_Outlets : IEnumerable<Outlet>
    {
        private readonly Operator _operator;

        internal OperatorWrapper_WithUnderlyingPatch_Outlets(Operator op)
        {
            _operator = op ?? throw new NullException(() => op);
        }

        public Outlet this[string name] => InletOutletSelector.GetOutlet(_operator, name);
        public Outlet TryGet(string name) => InletOutletSelector.TryGetOutlet(_operator, name);
        public IList<Outlet> GetMany(string name) => InletOutletSelector.GetOutlets(_operator, name);

        /// <summary> not fast </summary>
        public Outlet this[int index] => InletOutletSelector.GetOutlet(_operator, index);
        public Outlet TryGet(int index) => InletOutletSelector.TryGetOutlet(_operator, index);
        public IList<Outlet> GetMany(int index) => InletOutletSelector.GetOutlets(_operator, index);

        public Outlet this[DimensionEnum dimensionEnum] => InletOutletSelector.GetOutlet(_operator, dimensionEnum);
        public Outlet TryGet(DimensionEnum dimensionEnum) => InletOutletSelector.TryGetOutlet(_operator, dimensionEnum);
        public IList<Outlet> GetMany(DimensionEnum dimensionEnum) => InletOutletSelector.GetOutlets(_operator, dimensionEnum);

        public int Count => _operator.Outlets.Count;

        public IEnumerator<Outlet> GetEnumerator()
        {
            foreach (Outlet outlet in InletOutletSelector.GetSortedOutlets(_operator))
            {
                yield return outlet;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (Outlet outlet in InletOutletSelector.GetSortedOutlets(_operator))
            {
                yield return outlet;
            }
        }
    }
}