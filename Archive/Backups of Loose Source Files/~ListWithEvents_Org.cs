//
//  Circle.Code.Events.ListEvents
//
//      Author: Jan-Joost van Zon
//      Date: 12-06-2011 - 20-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Circle.Code.Conditions;

namespace Circle.Code.Events
{
    public class ListWithEvents_Org<T> : IEnumerable<T>
    {
        public event Adding<T> Adding;
        public event Added<T> Added;
        public event Removing<T> Removing;
        public event Removed<T> Removed;
        public event Clearing Clearing;
        public event Cleared Cleared;
        public event GettingListItem<T> Getting;
        public event GottenListItem<T> Gotten;
        public event AssigningListItem<T> Assigning;
        public event ChangingListItem<T> Changing;
        public event ChangedListItem<T> Changed;
        public event CreatingListItem<T> Creating;
        public event CreatedListItem<T> Created;
        public event AnnullingListItem<T> Annulling;
        public event AnnulledListItem<T> Annulled;

        private IList<T> _list;

        public ListWithEvents_Org()
        {
            _list = new List<T>();
        }

        public ListWithEvents_Org(IList<T> list)
        {
            _list = list;
        }

        // Count

        public readonly Events<int> CountEvents = new Events<int>();
        public int Count
        {
            get { return CountEvents.Value; }
            set 
            {
                for (int i = _list.Count; i < value; i++)
                {
                    Add(i);
                }

                for (int i = _list.Count - 1 ; i >= value; i--)
                {
                    Remove(i);
                }

                CountEvents.Value = value;
            }
        }

        public void Add(params T[] items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public T Add(T item = default(T), int index = -1)
        {
            if (index == -1) index = _list.Count;

            if (Adding != null)
            {
                if (!Adding(item, index))
                {
                    throw new Exception("Add not allowed.");
                }
            }

            _list.Insert(index, default(T));

            Count = _list.Count;

            this[index] = item;

            if (Added != null)
                Added(item, index);

            return _list[index];
        }

        public T Add(int index)
        {
            return Add(default(T), index);
        }

        public void Remove(T item)
        {
            Condition.AboveZero(_list.Count, "Count");

            int index = _list.IndexOf(item);

            Remove(index);
        }

        public T Remove(int index = -1)
        {
            Condition.AboveZero(_list.Count, "Count");

            if (index == -1) index = _list.Count - 1;

            T item = _list[index];

            if (Removing != null)
            {
                if (!Removing(item, index))
                {
                    throw new Exception("Remove not allowed.");
                }
            }

            this[index] = default(T);

            _list.RemoveAt(index);

            Count = _list.Count;

            if (Removed != null)
            {
                Removed(item, index);
            }

            return item;
        }

        public void Clear()
        {
            if (Clearing != null)
            {
                if (!Clearing())
                {
                    throw new Exception("Clear not allowed.");
                }
            }

            for (int i = Count - 1; i >= 0; i--)
            {
                Remove(i);
            }

            if (Cleared != null)
            {
                Cleared();
            }
        }

        // Get & Set

        public T this[int index]
        {
            get
            {
                T item = _list[index];

                if (Getting != null)
                {
                    if (!Getting(item, index))
                    {
                        throw new Exception("Get not allowed.");
                    }
                }

                if (Gotten != null) 
                    Gotten(item, index);

                return item;
            }
            set
            {
                if (Assigning != null)
                    Assigning(value, index);

                if (Object.Equals(_list[index], value)) return;

                if (Changing != null)
                {
                    if (!Changing(_list[index], value, index))
                    {
                        throw new Exception("Not allowed to change.");
                    }
                }

                if (_list[index] == null && value != null)
                {
                    if (Creating!= null)
                    {
                        if (!Creating(value, index))
                        {
                            throw new Exception("Creation not allowed");
                        }
                    }
                }

                if (_list[index] != null && value == null)
                {
                    if (Annulling != null)
                    {
                        if (!Annulling(_list[index], index))
                        {
                            throw new Exception("Annullment not allowed");
                        }
                    }
                }

                T old = _list[index];

                _list[index] = value;

                if (Changed != null) 
                    Changed(old, _list[index], index);

                if (old == null && _list[index] != null)
                {
                    if (Created != null)
                        Created(_list[index], index);
                }

                if (old != null && _list[index] == null)
                {
                    if (Annulled != null)
                        Annulled(old, index);
                }
            }
        }

        // Enumerate

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                yield return this[i];
            }
        }

        // Lookup

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }
    }
}
