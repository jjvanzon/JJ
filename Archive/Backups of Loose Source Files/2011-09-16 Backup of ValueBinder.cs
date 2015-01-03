//
//  Circle.Data.Binding.ValueBinder
//
//      Author: Jan-Joost van Zon
//      Date: 08-09-2011 - 12-09-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Events;
using Circle.Code.Apply;
using Circle.Data.Collections;

namespace Circle.Data.Binding
{
    /// <summary>
    /// This class controls binding a value to a control.
    /// This can be done in several ways:
    /// 1) To display a specific value, you can assign the Value property.
    /// 2) You can also assign an IHear interceptable to the Value property, 
    ///    which will cause the display to update automatically when the data changes.
    /// 3) To display a non-interceptable property, you can assign the Get and Set actions. 
    /// 4) Alternatively you can assign your own Bind action.
    /// You can add additional interceptables to the Dependencies
    /// list for automatically updating the display.
    /// When data binding is not triggered automatically by means of interception,
    /// you have to call Bind() manually.
    /// Next to displaying a value, you also should be able to store the value
    /// entered inside the control. For this purpose ValueBinder contains
    /// the OtherBinder member, which is a ValueBinder that goes the other way.
    /// </summary>

    public class ValueBinder<TValue, TOtherValue>
    {
        // Constructor

        public ValueBinder()
        {
            InitializeValue();
            InitializeDerivedGet();
            InitializeDerivedSet();
            InitializeDerivedBind();
            InitializeDependencies();
        }

        // OtherBinder

        /// <summary>
        /// Next to displaying a value, you also should be able to store the value
        /// entered inside a control. For this purpose ValueBinder contains
        /// the OtherBinder member, which is a ValueBinder that goes the other way.
        /// </summary>

        public ValueBinder<TOtherValue, TValue> OtherBinder
        {
            get { return OtherBinderEvents.Value; }
            set { OtherBinderEvents.Value = value; }
        }

        public readonly Events<ValueBinder<TOtherValue, TValue>> OtherBinderEvents = 
                    new Events<ValueBinder<TOtherValue, TValue>>("OtherBinder");

        // Value

        /// <summary>
        /// Assign this property to display a specific value.
        /// You can also assign an IHear interceptable, 
        /// which will cause automatic display of the data when it changes.
        /// </summary>

        public object Value
        {
            get { return ValueEvents.Value; }
            set { ValueEvents.Value = value; }
        }

        public readonly Events<object> ValueEvents = new Events<object>("Value");

        private void InitializeValue()
        {
            ValueEvents.Changed += (e) =>
            {
                if (e.Old != null)
                {
                    if (e.Old is IHear)
                    {
                        var hear = e.Old as IHear;

                        hear.Changed -= (e2) => Bind();
                    }
                }

                if (e.Value != null)
                {
                    if (e.Value is IHear)
                    {
                        var hear = e.Value as IHear;

                        hear.Changed += (e2) => Bind();
                    }
                }
            };
        }

        // Get

        /// <summary> Not nullable. To display a non-interceptable property, you can manually assign the Get and Set actions. </summary>
        
        public Func<TValue> Get
        {
            get { return DerivedGet; }
            set { CustomGet = value; }
        }

        public Events<Func<TValue>> GetEvents
        {
            get { return DerivedGetEvents; }
        }

        // Set

        /// <summary> Not nullable. To display a non-interceptable property, you can manually assign the Get and Set actions. </summary>

        public Action<TOtherValue> Set
        {
            get { return DerivedSet; }
            set { CustomSet = value; }
        }

        public Events<Action<TOtherValue>> SetEvents
        {
            get { return DerivedSetEvents; }
        }

        // Bind

        /// <summary> 
        /// When data binding is not triggered automatically by means of interception,
        /// you have to call Bind() manually.
        /// Alternatively you can assign your own Bind action.
        /// </summary>

        public Action Bind
        {
            get { return DerivedBind; }
            set { CustomBind = value; }
        }

        public Events<Action> BindEvents
        {
            get { return DerivedBindEvents; }
        }

        // Dependencies
        
        /// <summary> When these dependencies change, this will automatically update the control. </summary>

        public readonly ListWithEvents<IHear> Dependencies = new ListWithEvents<IHear>();

        private void InitializeDependencies()
        {
            Dependencies.Changed += (e) =>
            {
                if (e.Old != null)
                {
                    e.Old.Changed -= (e2) => Bind();
                }

                if (e.Item != null)
                {
                    e.Item.Changed += (e2) => Bind();
                }
            };
        }

        // CustomGet

        private Func<TValue> CustomGet
        {
            get { return CustomGetEvents.Value; }
            set { CustomGetEvents.Value = value; }
        }

        private readonly Events<Func<TValue>> CustomGetEvents = new Events<Func<TValue>>("CustomGet");

        // DerivedGet

        /// <summary> Not nullable. </summary>

        private Func<TValue> DerivedGet
        {
            get { return DerivedGetEvents.Value; }
            set { DerivedGetEvents.Value = value; }
        }

        private Events<Func<TValue>> DerivedGetEvents = new Events<Func<TValue>>("DerivedGet");

        private Apply ApplyToDerivedGet = new Apply();

        private void InitializeDerivedGet()
        {
            ApplyToDerivedGet.Mode = ApplyMode.Late;

            ApplyToDerivedGet.ApplyTo.Add
            (
                DerivedGetEvents
            );

            ApplyToDerivedGet.Dependencies.Add
            (
                CustomGetEvents,
                ValueEvents
            );

            ApplyToDerivedGet.Execute = () =>
            {
                if (CustomGet != null)
                {
                    DerivedGet = CustomGet;

                    return;
                }

                if (Value != null && Value is IHear<TValue>)
                {
                    DerivedGet = () =>
                    {
                        var events = Value as IHear<TValue>;

                        return events.Value;
                    };

                    return;
                };

                if (Value != null && Value is TValue)
                {
                    DerivedGet = () =>
                    {
                        return (TValue)(object)Value;
                    };

                    return;
                }

                DerivedGet = () => { return default(TValue); }; // No operation
            };
        }

        // CustomSet

        private Action<TOtherValue> CustomSet
        {
            get { return CustomSetEvents.Value; }
            set { CustomSetEvents.Value = value; }
        }

        private readonly Events<Action<TOtherValue>> CustomSetEvents = new Events<Action<TOtherValue>>("CustomSet");

        // DerivedSet

        /// <summary> Not nullable. </summary>

        private Action<TOtherValue> DerivedSet
        {
            get { return DerivedSetEvents.Value; }
            set { DerivedSetEvents.Value = value; }
        }

        private Events<Action<TOtherValue>> DerivedSetEvents = new Events<Action<TOtherValue>>("DerivedSet");

        private Apply ApplyToDerivedSet = new Apply();

        private void InitializeDerivedSet()
        {
            ApplyToDerivedSet.Mode = ApplyMode.Late;

            ApplyToDerivedSet.ApplyTo.Add
            (
                DerivedSetEvents
            );

            ApplyToDerivedSet.Dependencies.Add
            (
                CustomSetEvents,
                ValueEvents
            );

            ApplyToDerivedSet.Execute = () =>
            {
                if (CustomSet != null)
                {
                    DerivedSet = CustomSet;

                    return;
                }

                if (Value is IHear)
                {
                    DerivedSet = (value) =>
                    {
                        var events = Value as IHear;

                        events.Value = (TValue)Convert.ChangeType(value, typeof(TValue));
                    };

                    return;
                };

                DerivedSet = (value) =>
                {
                    Value = (TValue)Convert.ChangeType(value, typeof(TValue));
                };
            };
        }
        
        // CustomBind

        private Action CustomBind
        {   
            get { return CustomBindEvents.Value; }
            set { CustomBindEvents.Value = value; }
        }

        private readonly Events<Action> CustomBindEvents = new Events<Action>("CustomBind");

        // DerivedBind

        private Action DerivedBind
        {
            get { return DerivedBindEvents.Value; }
            set { DerivedBindEvents.Value = value; }
        }

        private readonly Events<Action> DerivedBindEvents = new Events<Action>("DerivedBind");

        private Apply ApplyToDerivedBind = new Apply();

        private void InitializeDerivedBind()
        {
            ApplyToDerivedBind.Mode = ApplyMode.Immediate;

            ApplyToDerivedBind.ApplyTo.Add
            (
                DerivedBindEvents
            );

            ApplyToDerivedBind.Dependencies.Add
            (
                CustomBindEvents,
                CustomSetEvents,
                OtherBinderEvents
            );

            ApplyToDerivedBind.Execute = () =>
            {
                if (CustomBind != null)
                {
                    DerivedBind = CustomBind;

                    return;
                }

                if (OtherBinder != null)
                {
                    DerivedBind = () =>
                    {
                        OtherBinder.Set(this.Get());
                    };

                    return;
                }

                DerivedBind = () => { }; // No operation
            };

            // Dynamically add and remove OtherValue.Set as a dependency.

            OtherBinderEvents.Changed += (e) =>
            {
                if (e.Old != null)
                {
                    ApplyToDerivedBind.Dependencies.Remove
                    (
                        e.Old.SetEvents
                    );
                }
                if (e.Value != null)
                {
                    ApplyToDerivedBind.Dependencies.Add
                    (
                        e.Value.SetEvents
                    );
                }
            };
        }
    }

    // Generic overloads

    /// <summary>
    /// This class controls binding a value to a control.
    /// This can be done in several ways:
    /// 1) To display a specific value, you can assign the Value property.
    /// 2) You can also assign an IHear interceptable to the Value property, 
    ///    which will cause the display to update automatically when the data changes.
    /// 3) To display a non-interceptable property, you can assign the Get and Set actions. 
    /// 4) Alternatively you can assign your own Bind action.
    /// You can add additional interceptables to the Dependencies
    /// list for automatically updating the display.
    /// When data binding is not triggered automatically by means of interception,
    /// you have to call Bind() manually.
    /// Next to displaying a value, you also should be able to store the value
    /// entered inside the control. For this purpose ValueBinder contains
    /// the OtherBinder member, which is a ValueBinder that goes the other way.
    /// </summary>
    public class ValueBinder<TValue> : ValueBinder<TValue, object>
    { }

    /// <summary>
    /// This class controls binding a value to a control.
    /// This can be done in several ways:
    /// 1) To display a specific value, you can assign the Value property.
    /// 2) You can also assign an IHear interceptable to the Value property, 
    ///    which will cause the display to update automatically when the data changes.
    /// 3) To display a non-interceptable property, you can assign the Get and Set actions. 
    /// 4) Alternatively you can assign your own Bind action.
    /// You can add additional interceptables to the Dependencies
    /// list for automatically updating the display.
    /// When data binding is not triggered automatically by means of interception,
    /// you have to call Bind() manually.
    /// Next to displaying a value, you also should be able to store the value
    /// entered inside the control. For this purpose ValueBinder contains
    /// the OtherBinder member, which is a ValueBinder that goes the other way.
    /// </summary>
    public class ValueBinder : ValueBinder<object, object>
    { }
}
