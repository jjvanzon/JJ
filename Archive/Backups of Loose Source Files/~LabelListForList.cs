//
//  Circle.Controls.Classes.LabelListForList
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 20-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Events;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// LabelList with data-binding to an IEnumerable.
    /// If TList and TItem are Circle.Events interceptables
    /// then data changes will immediately be displayed.
    /// </summary>
    /// <typeparam name="TList">IEnumerable</typeparam>
    /// <typeparam name="TItem">Type of the items in the IEnumerable.</typeparam>
    public class LabelListForList<TList, TItem> : LabelList<LabelForItem<TItem>>
        where TList: IEnumerable<TItem>
    {
        public LabelListForList()
        {
            Data = new LabelListData<TList, TItem, LabelForItem<TItem>>(this);
        }

        /// <summary> Read-only </summary>
        public readonly LabelListData<TList, TItem, LabelForItem<TItem>> Data;
    }
}
