//
//  Circle.Code.Events.Events
//
//      Author: Jan-Joost van Zon
//      Date: 02-06-2011 - 02-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Helpers;
using System.ComponentModel;

namespace Circle.Code.Events
{
    [DefaultProperty("Value")]
    public class Events_Org<T> : IHear<T>
    {
        public event Getting<T> Getting;
        public event Gotten<T> Gotten;
        public event Assigning<T> Assigning;
        public event Changing<T> Changing;
        public event Changed<T> Changed;
        public event Creating<T> Creating;
        public event Created<T> Created;
        public event Annulling<T> Annulling;
        public event Annulled<T> Annulled;

        private T _value;

        public T Value // override
        {
            get
            {
                if (Getting != null)
                {
                    if (!Getting(_value))
                    {
                        throw new Exception("Get not allowed.");
                    }
                }

                if (Gotten != null) 
                    Gotten(_value);

                return _value;
            }
            set
            {
                if (Assigning != null)
                    Assigning(value);

                if (Object.Equals(_value, value)) return;

                if (Changing != null)
                {
                    if (!Changing(_value, value))
                    {
                        throw new Exception("Not allowed to change.");
                    }
                }

                if (_value == null && value != null)
                {
                    if (Creating!= null)
                    {
                        if (!Creating(value))
                        {
                            throw new Exception("Creation not allowed");
                        }
                    }
                }

                if (_value != null && value == null)
                {
                    if (Annulling != null)
                    {
                        if (!Annulling(_value))
                        {
                            throw new Exception("Annullment not allowed");
                        }
                    }
                }

                T old = _value;

                _value = value;

                if (Changed != null) 
                    Changed(old, _value);

                if (old == null && _value != null)
                {
                    if (Created != null)
                        Created(_value);
                }

                if (old != null && _value == null)
                {
                    if (Annulled != null)
                        Annulled(old);
                }
            }
        }

        public bool IsCreated
        {
            get { return _value != null; }
        }
    }
}
