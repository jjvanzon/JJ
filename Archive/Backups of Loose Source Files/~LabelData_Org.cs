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
    /// If TItem is a Circle.Events interceptable, then data changes will immediately be displayed.
    /// </summary>
    /// <typeparam name="TItem">Type of the item to display.</typeparam>
    public class LabelData_Org<TItem>
    {
        // Constructor

        public LabelData_Org(Label label)
        {
            Condition.NotNull(label,"label");
            Label = label;

            BindFieldsEvents();
        }

        ~LabelData_Org()
        {
            UnbindItem();
        }

        // Label

        private readonly Label Label;

        // Item

        private TItem _item;
        public TItem Item
        {
            get { return _item;}
            set
            {
                if (Object.Equals(_item, value)) return;
                UnbindItem();
                _item = value;
                GetData();
                BindItem();
            }
        }

        // Fields

        /// <summary>
        /// Fields as opposed to FormattedField was not tested yet.
        /// </summary>
        public readonly ListWithEvents<IHear<object>> Fields = new ListWithEvents<IHear<object>>();

        private void BindFieldsEvents()
        {
            Fields.Created += (item, index) =>
            {
                item.Changed += (x, y) => GetData();
            };

            Fields.Annulled += (item, index) =>
            {
                item.Changed -= (x, y) => GetData();
            };
        }

        private Func<TItem, object> _formattedField;
        /// <summary>
        /// A function that returns the field to data-bind to.
        /// </summary>
        public Func<TItem, object> FormattedField
        {
            get { return _formattedField; }
            set
            {
                if (Object.Equals(_item, value)) return;
                _formattedField = value;
                GetData();
            }
        }

        private void BindItem()
        {
            if (Item is IHear<TItem>)
            {
                IHear<TItem> hear = Item as IHear<TItem>;
                hear.Changed += (x, y) => GetData();
            }
        }

        private void UnbindItem()
        {
            if (Item is IHear<TItem>)
            {
                IHear<TItem> hear = Item as IHear<TItem>;
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
                    if (Item is IHear<TItem>)
                    {
                        IHear<TItem> hear = Item as IHear<TItem>;
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
