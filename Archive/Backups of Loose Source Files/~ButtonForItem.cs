//
//  Circle.Controls.Classes.ButtonForItem
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
    /// Button with data-binding. 
    /// </summary>
    /// <typeparam name="TItem">Type of the item to display.</typeparam>
    public class ButtonForItem<TItem> : Button
    {
        public ButtonForItem()
        {
            Data = new LabelData<TItem>(this);
        }

        public readonly LabelData<TItem> Data;
    }
}
