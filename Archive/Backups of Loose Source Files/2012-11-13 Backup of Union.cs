//
//  Circle.Framework.Data.Concepts.Union
//
//      Author: Jan-Joost van Zon
//      Date: 2012-11-08 - 2012-11-08
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.OS.Data.Collections;
using Circle.Framework.Code.Conditions;

namespace Circle.Framework.Data.Concepts
{
    public class Union<T> : ListWithEvents<T>
    {
        public Union(params ListWithEvents[] lists)
        {
            InitializeLists(lists);
        }

        public Union(params T[] items)
        {
        }

        public Union(ListWithEvents[] lists, T[] items)
        {
        }

        private readonly List<ListWithEvents> Lists = new List<ListWithEvents>();

        private void InitializeLists(ListWithEvents[] lists)
        {
            foreach (ListWithEvents list in lists)
            {
                list.ItemChanged += Lists_List_ItemChanged;
                Lists.Add(list);
            }
        }

        private void FinalizeLists()
        {
            foreach (ListWithEvents list in Lists)
            {
                list.ItemChanged -= Lists_List_ItemChanged;
            }
        }

        // TODO: Things can go wrong if another list still contains the item.
        // Gee, the solution would be that each reference is in the union once,
        // but that requires each reference to be present in the union once.
        // they should be references to references.

        private void Lists_List_ItemChanged(object sender, ListItemChangedEventArgs<object> e)
        {
            if (e.Old != null) Result.Remove(e.Old);
            if (e.New != null) Result.Add(e.New);
        }

        public readonly ListWithEvents<T> Result = new ListWithEvents<T>(); // TODO: make sure you cannot add publically.
    }
}
