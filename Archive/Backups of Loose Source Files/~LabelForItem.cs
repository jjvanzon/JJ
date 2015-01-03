//
//  Circle.Controls.Classes.LabelForItem
//
//      Author: Jan-Joost van Zon
//      Date: 19-06-2011 - 19-06-2011
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
    /// Label with data-binding. 
    /// </summary>
    /// <typeparam name="TItem">Type of the item to display.</typeparam>
    public class LabelForItem<TItem> : Label
    {
        public LabelForItem()
        {
            Data = new LabelData<TItem>(this);
        }

        /// <summary> Read-only </summary>
        public readonly LabelData<TItem> Data;
    }
}
