//
//  Circle.Code.Events.Delegates
//
//      Author: Jan-Joost van Zon
//      Date: 06-06-2011 - 07-06-2011
//
//  -----

namespace Circle.Code.Events
{
    public delegate bool Getting<T>(T value);
    public delegate bool GettingListItem<T>(T item, int index);
    public delegate void Gotten<T>(T value);
    public delegate void GottenListItem<T>(T item, int index);
    public delegate void Assigning<T>(T value);
    public delegate void AssigningListItem<T>(T item, int index);
    public delegate bool Changing<T>(T old, T value);
    public delegate bool ChangingListItem<T>(T oldItem, T item, int index);
    public delegate void Changed<T>(T old, T value);
    public delegate void ChangedListItem<T>(T old, T item, int index);
    public delegate bool Creating<T>(T value);
    public delegate bool CreatingListItem<T>(T item, int index);
    public delegate void Created<T>(T value);
    public delegate void CreatedListItem<T>(T item, int index);
    public delegate bool Annulling<T>(T value);
    public delegate bool AnnullingListItem<T>(T item, int index);
    public delegate void Annulled<T>(T old);
    public delegate void AnnulledListItem<T>(T old, int index);
    public delegate bool Adding<T>(T item, int index);

    public class AddedEventArgs<T> { public T Item; public int Index; }
    public delegate void Added<T>(AddedEventArgs<T> e);

    public delegate bool Removing<T>(T item, int index);
    public delegate void Removed<T>(T item, int index);
    public delegate bool Clearing();
    public delegate void Cleared();
    public delegate void Before();
    public delegate void After();
    public delegate void Before<T>(T args);
    public delegate void After<T>(T args);
}
