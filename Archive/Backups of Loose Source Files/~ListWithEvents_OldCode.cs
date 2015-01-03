//
//  Circle.Code.Events.ListEvents
//
//      Author: Jan-Joost van Zon
//      Date: 12-06-2011 - 12-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Circle.Code.Events
{
    public class ListWithEvents<T> : IEnumerable<T>
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
        public event AnnulledListItem Annulled;

        private IList<T> _list;

        public ListWithEvents()
        {
            _list = new List<T>();
        }

        public ListWithEvents(IList<T> list)
        {
            _list = list;
        }

        public void Add(T item = default(T), int index = -1)
        {
            if (index == -1) index = _list.Count;

            if (Adding != null)
            {
                if (!Adding(item, index))
                {
                    throw new Exception("Add not allowed.");
                }
            }

            _list.Insert(index, item);

            if (Added != null)
                Added(item, index);
        }

        public bool Remove(T item)
        {
            int index = _list.IndexOf(item);

            if (Removing != null)
            {
                if (!Removing(item, index))
                {
                    throw new Exception("Remove not allowed.");
                }
            }

            bool isRemoved = _list.Remove(item);

            if (isRemoved && Removed != null)
            {
                Removed(item, index);
            }

            return isRemoved;
        }

        public void RemoveAt(int index)
        {
            T item = _list[index];

            if (Removing != null)
            {
                if (!Removing(item, index))
                {
                    throw new Exception("Remove not allowed.");
                }
            }

            _list.RemoveAt(index);

            if (Removed != null)
            {
                Removed(item, index);
            }
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

            for (int i = Count - 1; i >= 0; i++)
            {
                RemoveAt(i);
            }

            if (Cleared != null)
            {
                Cleared();
            }
        }

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
                        Annulled(index);
                }
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (Getting != null)
            {
                if (!Getting(default(T), -1))
                {
                    throw new Exception("Get not allowed.");
                }
            }

            _list.CopyTo(array, arrayIndex);

            if (Gotten != null) 
                Gotten(default(T), -1);
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public readonly Events<int> CountEvents = new Events<int>(); // TODO: use this interceptable.
        public int Count
        {
            get { return _list.Count; }
            set { } // TODO: add implementation
        }

        public bool IsReadOnly
        {
            get { return _list.IsReadOnly; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
