//
//  Circle.Controls.Classes.LabelData
//
//      Author: Jan-Joost van Zon
//      Date: 19-06-2011 - 12-07-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Events;
using Circle.Code.Conditions;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// Enabled binding a Label to data.
    /// It can bind the label to any object. 
    /// You can supply a formatting function, 
    /// that determines the text to display.
    /// If Item is a Circle.Events interceptable, then data changes will immediately be displayed.
    /// </summary>
    /// <typeparam name="TItem">Type of the item to display.</typeparam>
    public class LabelDataT<T> : LabelData_New 
    {
        // Constructor

        public LabelDataT(Label label)
            : base(label)
        { }

        // Item

        public T Item
        {
            get { return (T)base.Item; }
            set { base.Item = value; }
        }

        // Fields

        /// <summary>
        /// Fields as opposed to FormattedField was not tested yet.
        /// </summary>
        public ListWithEvents<IHear<T>> Fields
        {
            get { return base.Fields; }
        }

        public Func<T, object> FormattedField
        {
            get { return base.FormattedField; }
            set
            {
                if (Object.Equals(_item, value)) return;
                _formattedField = value;
                GetData();
            }
        }

        private void BindItem()
        {
            if (Item is IHear<object>)
            {
                IHear<object> hear = Item as IHear<object>;
                hear.Changed += (x, y) => GetData();
            }
        }

        private void UnbindItem()
        {
            if (Item is IHear<object>)
            {
                IHear<object> hear = Item as IHear<object>;
                hear.Changed -= (x, y) => GetData();
            }
        }

        public void GetData()
        {
            if (FormattedField != null)
            {
                Label.Text.Value = FormattedField(Item).ToString();
            }
            else
            {
                if (Item != null)
                {
                    if (Item is IHear<object>)
                    {
                        IHear<object> hear = Item as IHear<object>;
                        Label.Text.Value = hear.Value.ToString();
                    }
                    else
                    {
                        Label.Text.Value = Item.ToString();
                    }
                }
            }
        }
    }
}
