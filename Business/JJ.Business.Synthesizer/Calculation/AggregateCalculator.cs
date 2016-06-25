﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.CopiesFromFramework;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation
{
    internal static class AggregateCalculator
    {
        /// <summary> Slowerthan the other overload. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Closest(double input, IList<double> items)
        {
            if (items == null) throw new NullException(() => items);
            if (items.Count < 1) throw new NullException(() => items);

            return Closest(input, items[0], items.Skip(1).ToArray());
        }

        /// <summary> Null-checks omitted for performance. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Closest(double input, double firstItem, double[] remainingItems)
        {
            double smallestDistance = Geometry.AbsoluteDistance(input, firstItem);
            double closestItem = firstItem;

            for (int i = 0; i < remainingItems.Length; i++)
            {
                double item = remainingItems[i];

                double distance = Geometry.AbsoluteDistance(input, item);

                if (smallestDistance > distance)
                {
                    smallestDistance = distance;
                    closestItem = item;
                }
            }

            return closestItem;
        }
    }
}
