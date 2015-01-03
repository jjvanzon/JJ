//
//  Circle.Controls.Classes.ButtonListForList
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
    /// ButtonList with data-binding to an IEnumerable.
    /// If TList and TItem are Circle.Events interceptables
    /// then data changes will immediately be displayed.
    /// </summary>
    /// <typeparam name="TList">IEnumerable</typeparam>
    /// <typeparam name="TItem">Type of the items in the IEnumerable.</typeparam>
    public class ButtonListForList<TList, TItem> : ButtonList<Label<TItem>>
        where TList : IEnumerable<TItem>
        where TItem : new()
    {
        private TList _list;
        public TList List
        {
            get { return _list; }
            set
            {
                if (Object.ReferenceEquals(_list, value)) return;

                UnbindList();

                _list = value;

                GetData();
                BindList();
            }
        }

        ~ButtonListForList()
        {
            UnbindList();
        }

        // TODO: Distinction between Fields and FormattedField is not implemented yet.
        // It would require some tricks to get the right IHear to the right button, though.

        private Func<TItem, object> _field;
        public Func<TItem, object> Field
        {
            get { return _field; }
            set { _field = value; }
        }

        private void BindList()
        {
            var listWithEvents = List as ListWithEvents<TItem>;

            if (listWithEvents != null)
            {
                listWithEvents.Added += _list_Added;
                listWithEvents.Removed += _list_Removed;
                listWithEvents.Changed += _list_Changed;
            }
        }

        private void UnbindList()
        {
            var listWithEvents = List as ListWithEvents<TItem>;

            if (listWithEvents != null)
            {
                listWithEvents.Added -= _list_Added;
                listWithEvents.Removed -= _list_Removed;
                listWithEvents.Changed -= _list_Changed;
            }
        }

        private void _list_Added(AddedEventArgs<TItem> e)
        {
            AddButton(e.Item, e.Index);
        }

        private void _list_Removed(TItem item, int index)
        {
            Buttons.Remove(index);
        }

        private void _list_Changed(TItem old, TItem value, int index)
        {
            Buttons[index].Data.Item = value;
        }

        public void GetData()
        {
            base.Buttons.Clear();

            foreach (TItem item in List)
            {
                AddButton(item);
            }
        }

        private void AddButton(TItem item, int index = -1)
        {
            Label<TItem> button = Buttons.Add(index);
            button.Data.Item = item;
            button.Data.FormattedField = Field;
        }
    }
}
