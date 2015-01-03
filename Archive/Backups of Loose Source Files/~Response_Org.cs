//
//  Circle.Code.Concepts.Response
//
//      Author: Jan-Joost van Zon
//      Date: 24-06-2011 - 29-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Events;
using System.Reflection;
using Circle.Code.Conditions;
using Circle.StringFunctions;
using System.Text.RegularExpressions;

namespace Circle.Code.Concepts
{
    // TODO: consider using new Circle.Data collections.
    // TODO: consider renaming the horrible terms PointCut and JoinPoint.
    // TODO: make a better solution for circularities.
    // TODO: the event procedures are repeated in EventsT. Code smell.

    /// <summary>
    /// Using reflection,
    /// go through the sub-objects of Roots,
    /// detect Events&lt;&gt;, see if they match the criteria,
    /// and set their event response so that they delegate to Response's own events.
    /// You should take circularities into account.
    /// </summary>
    public class Response_Org
    {
        // Constructor

        ~Response_Org()
        {
            Annull();
        }

        // Events

        public event Getting<object> Getting;
        public event Gotten<object> Gotten;
        public event Assigning<object> Assigning;
        public event Changing<object> Changing;
        public event Changed<object> Changed;
        public event Creating<object> Creating;
        public event Created<object> Created;
        public event Annulling<object> Annulling;
        public event Annulled<object> Annulled;

        // Roots

        public readonly ListWithEvents<object> Roots = new ListWithEvents<object>(); 

        public object Root
        {
            get
            {
                Condition.AboveZero(Roots.Count, "Roots.Count");
                return Roots[0];
            }
            set
            {
                if (Roots.Count == 0) Roots.Add(value);
                else Roots[0] = value;
            }
        }

        // Types

        public readonly ListWithEvents<Type> Types = new ListWithEvents<Type>(); 

        public Type Type
        {
            get
            {
                Condition.AboveZero(Types.Count, "Types.Count");
                return Types[0];
            }
            set
            {
                if (Types.Count == 0) Types.Add(value);
                else Types[0] = value;
            }
        }

        // Excluded Types

        public readonly ListWithEvents<Type> ExcludedTypes = new ListWithEvents<Type>(); 

        // Members

        public readonly ListWithEvents<string> Members = new ListWithEvents<string>(); 

        public string Member
        {
            get
            {
                Condition.AboveZero(Members.Count, "Members.Count");
                return Members[0];
            }
            set
            {
                if (Members.Count == 0) Members.Add(value);
                else Members[0] = value;
            }
        }

        // Excluded Members

        public readonly ListWithEvents<string> ExcludedMembers = new ListWithEvents<string>(); 

        // Joint Points

        public readonly ListWithEvents<Events.Events> JoinPoints = new ListWithEvents<Events.Events>(); 

        private int MaxLevels = 10;

        // Set

        public void Set()
        {
            // Collect join points
            foreach (var root in Roots)
            {
                SetFieldsAndProperties(root, level: 0);
            }
        }

        private void SetFieldsAndProperties(object parent, int level)
        {
            if (level >= MaxLevels) return;

            // Skip nulls
            if (parent == null) return;

            // Values cannot be parents
            if (parent is ValueType) return;

            // Traverse fields
            foreach (var field in parent.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                SetField(parent, field, level);
            }

            // Traverse properties
            foreach (var property in parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                SetProperty(parent, property, level);
            }
        }

        private void SetField(object parent, FieldInfo field, int level)
        {
            // Get value
            object value = field.GetValue(parent);

            // Mutual base method
            SetFieldOrProperty(value, field.Name, level);
        }

        private void SetProperty(object parent, PropertyInfo property, int level)
        {
            // Skip indexers
            if (property.GetIndexParameters().Length > 0) return;

            // Get value
            object value = property.GetValue(parent, null);

            // Mutual base method
            SetFieldOrProperty(value, property.Name, level);
        }

        private void SetFieldOrProperty(object value, string name, int level)
        {
            // Skip nulls
            if (value == null) return;

            // Match and bind
            MatchAndBind(value, value.GetType(), name, level);

            // Recursive call
            SetFieldsAndProperties(value, level + 1);
        }

        private void MatchAndBind(object value, Type type, string name, int level)
        {
            // Is Events<T>?
            if (!IsEventsT(type)) return;

            // Get generic type argument
            Type genericTypeArgument = type.GetGenericArguments()[0];

            // Match
            if (!IsTypeMatch(genericTypeArgument)) return;
            if (IsTypeExcluded(genericTypeArgument)) return;
            if (!IsMemberMatch(name)) return;
            if (IsMemberExcluded(name)) return;

            // Bind

            Events.Events joinPoint = value as Events.Events;

            if (!JoinPoints.Contains(joinPoint))
            {
                JoinPoints.Add(joinPoint);
                joinPoint.Getting += JoinPoint_Getting;
                joinPoint.Gotten += JoinPoint_Gotten;
                joinPoint.Assigning += JoinPoint_Assigning;
                joinPoint.Changing += JoinPoint_Changing;
                joinPoint.Changed += JoinPoint_Changed;
                joinPoint.Creating += JoinPoint_Creating;
                joinPoint.Created += JoinPoint_Created;
                joinPoint.Annulling += JoinPoint_Annulling;
                joinPoint.Annulled += JoinPoint_Annulled;
                // Also bind to changed event in order to bind new join points.
                joinPoint.Changed += new Changed<object>(JoinPoint_Live);
            }
        }

        private bool IsEventsT(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Events<>);
        }

        private bool IsTypeMatch(Type type)
        {
            foreach (Type t in Types)
            {
                if (type == t)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsTypeExcluded(Type type)
        {
            foreach (Type excludedType in ExcludedTypes)
            {
                if (type == excludedType)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMemberMatch(string name)
        {
            foreach (string member in Members)
            {
                if (name.Match(member))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMemberExcluded(string name)
        {
            foreach (string excludedProperty in ExcludedMembers)
            {
                if (name.Match(excludedProperty))
                {
                    return true;
                }
            }

            return false;
        }

        //  Annull

        public void Annull()
        {
            foreach (Events.Events joinPoint in JoinPoints)
            {
                joinPoint.Getting -= JoinPoint_Getting;
                joinPoint.Gotten -= JoinPoint_Gotten;
                joinPoint.Assigning -= JoinPoint_Assigning;
                joinPoint.Changing -= JoinPoint_Changing;
                joinPoint.Changed -= JoinPoint_Changed;
                joinPoint.Creating -= JoinPoint_Creating;
                joinPoint.Created -= JoinPoint_Created;
                joinPoint.Annulling -= JoinPoint_Annulling;
                joinPoint.Annulled -= JoinPoint_Annulled;
            }

            JoinPoints.Clear();
        }

        private void AnnullFieldsAndProperties(object parent, int level)
        {
            if (level >= MaxLevels) return;

            // Skip nulls
            if (parent == null) return;

            // Values cannot be parents
            if (parent is ValueType) return;

            // Traverse fields
            foreach (var field in parent.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                AnnullField(parent, field, level);
            }

            // Traverse properties
            foreach (var property in parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                AnnullProperty(parent, property, level);
            }
        }

        private void AnnullField(object parent, FieldInfo field, int level)
        {
            // Get value
            object value = field.GetValue(parent);

            // Mutual base method
            AnnullFieldOrProperty(value, level);
        }

        private void AnnullProperty(object parent, PropertyInfo property, int level)
        {
            // Skip indexers
            if (property.GetIndexParameters().Length > 0) return;

            // Get value
            object value = property.GetValue(parent, null);

            // Mutual base method
            AnnullFieldOrProperty(value, level);
        }

        private void AnnullFieldOrProperty(object value, int level)
        {
            // Skip nulls
            if (value == null) return;

            // Unbind and continue
            Unbind(value, level);

            // Recursive call
            AnnullFieldsAndProperties(value, level + 1);
        }

        private void Unbind(object value, int level)
        {
            // Is Events?
            if (!(value is Events.Events)) return;

            // Unbind join points
            Events.Events joinPoint = value as Events.Events;

            if (JoinPoints.Contains(joinPoint))
            {
                JoinPoints.Remove(joinPoint);
                joinPoint.Getting -= JoinPoint_Getting;
                joinPoint.Gotten -= JoinPoint_Gotten;
                joinPoint.Assigning -= JoinPoint_Assigning;
                joinPoint.Changing -= JoinPoint_Changing;
                joinPoint.Changed -= JoinPoint_Changed;
                joinPoint.Creating -= JoinPoint_Creating;
                joinPoint.Created -= JoinPoint_Created;
                joinPoint.Annulling -= JoinPoint_Annulling;
                joinPoint.Annulled -= JoinPoint_Annulled;
                // Also unbind to changed event bound in order to bind new join points.
                joinPoint.Changed -= new Changed<object>(JoinPoint_Live);
            }
        }

        // Create new join points

        private void JoinPoint_Live(object old, object value)
        {
            // The max levels thing will fail here

            if (old != null)
            {
                AnnullFieldsAndProperties(old, level: 0);
            }
            if (value != null)
            {
                SetFieldsAndProperties(value, level: 0);
            }
        }

        // Event procedures

        private bool JoinPoint_Getting(object value)
        {
            if (Getting != null) return Getting(value);
            return true;
        }

        private void JoinPoint_Gotten(object value)
        {
            if (Gotten != null) Gotten(value);
        }

        private void JoinPoint_Assigning(object value)
        {
            if (Assigning != null) Assigning(value);
        }

        private bool JoinPoint_Changing(object old, object value)
        {
            if (Changing != null) return Changing(old, value);
            return true;
        }

        private void JoinPoint_Changed(object old, object value)
        {
            if (Changed != null) Changed(old, value);
        }

        private bool JoinPoint_Creating(object value)
        {
            if (Creating != null) return Creating(value);
            return true;
        }

        private void JoinPoint_Created(object value)
        {
            if (Created != null) Created(value);
        }

        private bool JoinPoint_Annulling(object value)
        {
            if (Annulling != null) return Annulling(value);
            return true;
        }

        private void JoinPoint_Annulled(object old)
        {
            if (Annulled != null) Annulled(old);
        }
    }
}
