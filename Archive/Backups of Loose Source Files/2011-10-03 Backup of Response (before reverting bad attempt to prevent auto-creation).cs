//
//  Circle.Code.Concepts.Response
//
//      Author: Jan-Joost van Zon
//      Date: 24-06-2011 - 20-07-2011
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
using System.Diagnostics;
using Circle.Data.Collections;

namespace Circle.Code.Concepts
{
    // TODO: consider using new Circle.Data collections.
    // TODO: the event procedures are repeated in EventsT. Code smell.

    /// <summary>
    /// This class allows you to bind a reaction 
    /// to a multiple interceptables at once by means of a query.
    /// Using reflection
    /// this class goes through the sub-objects of Roots,
    /// detects Events objects, sees if they match the criteria,
    /// and then sets their event response so that they delegate to Response's own events.
    /// Also, when a sub-object is changed,
    /// the old object's events are unbound and the new object's events are bound.
    /// Circularity is taken into account, so that no stack overflow exceptions occur.
    /// </summary>
    public class Response
    {
        // TODO: Response can not work with lists yet.

        // Constructor

        public Response()
        {
            InitializeMembers();
            InitializeExcludedMembers();
            InitializeJoinPoints();
        }

        ~Response()
        {
            Annull();
        }

        // Events

        public event Getting<object> Getting;
        public event Gotten<object> Gotten;
        public event Assigning<object> Assigning;
        public event Changing<object> Changing;
        public event Changed<object> Changed;

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

        /// <summary>
        /// Also add the roots' types to this list.
        /// </summary>

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

        private readonly List<Regex> MemberRegexs = new List<Regex>();

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

        private void InitializeMembers()
        {
            Members.Added += (s, e) => MemberRegexs.Insert(e.Index, null);
            Members.Removed += (s, e) => MemberRegexs.RemoveAt(e.Index);
            Members.Changed += (s, e) => MemberRegexs[e.Index] = CreateRegex(e.Item);
        }

        // Excluded Members

        public readonly ListWithEvents<string> ExcludedMembers = new ListWithEvents<string>();

        private readonly List<Regex> ExcludedMemberRegexs = new List<Regex>();

        private void InitializeExcludedMembers()
        {
            ExcludedMembers.Added += (s, e) => ExcludedMemberRegexs.Insert(e.Index, null);
            ExcludedMembers.Removed += (s, e) => ExcludedMemberRegexs.RemoveAt(e.Index);
            ExcludedMembers.Changed += (s, e) => ExcludedMemberRegexs[e.Index] = CreateRegex(e.Item);
        }

        // TypesToFollow

        public readonly ListWithEvents<Type> TypesToFollow = new ListWithEvents<Type>(); // TODO: This may also require live events.

        // Regex

        private Regex CreateRegex(string format)
        {
            format = format.Replace("*", ".*");
            format = format.Replace("?", ".?");
            return new Regex(format); // RegexOptions.Compiled was slower
        }

        // Joint Points

        public readonly ListWithEvents<Events.Events> JoinPoints = new ListWithEvents<Events.Events>();

        private void InitializeJoinPoints()
        {
            JoinPoints.Changed += (s, e) =>
            {
                if (e.Old != null)
                {
                    Unbind(e.Old);
                }

                if (e.Item != null)
                {
                    Bind(e.Item);
                }
            };
        }

        private void Bind(Events.Events events)
        {
            events.Getting += OnGetting;
            events.Gotten += OnGotten;
            events.Assigning += OnAssigning;
            events.Changed += OnChanged;
            events.Changing += OnChanging;

            if (IsTypeToFollow(events.GetType()))
            {
                events.Changed += OnLive;
            }
        }

        private void Unbind(Events.Events events)
        {
            events.Getting -= OnGetting;
            events.Gotten -= OnGotten;
            events.Assigning -= OnAssigning;
            events.Changed -= OnChanged;
            events.Changing -= OnChanging;
            //if (IsTypeToFollow(events.GetType()))
            //{
                events.Changed -= OnLive; // TODO: consider this has to be done for types NOT to follow.
            //}
        }

        // Set

        /// <summary>
        /// Binds the events of all matching sub-objects so that they raise Response's own events.
        /// </summary>
        public void Set()
        {
            Annull();

            // Collect join points
            foreach (var root in Roots)
            {
                SetFieldsAndProperties(root, done: new HashSet<object>());
            }
        }

        private void SetFieldsAndProperties(object parent, HashSet<object> done)
        {
            // Match parent
            if (!MatchParent(parent, done)) return;
            Type parentType = parent.GetType();

            // Traverse fields
            foreach (var field in ReflectionCache.GetFields(parentType))
            {
                SetField(parent, field, done);
            }

            // Traverse properties
            foreach (var property in ReflectionCache.GetProperties(parentType))
            {
                SetProperty(parent, property, done);
            }
        }

        private void SetField(object parent, FieldInfo field, HashSet<object> done)
        {
            // Mutual base method
            SetFieldOrProperty(parent, field, done);
        }

        private void SetProperty(object parent, PropertyInfo property, HashSet<object> done)
        {
            // Skip indexers
            if (property.GetIndexParameters().Length > 0) return;

            // Check existence of getter.
            if (!property.CanRead) return;

            // Mutual base method
            SetFieldOrProperty(parent, property, done);
        }

        private void SetFieldOrProperty(object parent, MemberInfo member, HashSet<object> done)
        {
            // Match
            object value = MatchFieldOrProperty(parent, member);
            
            // Add
            if (value != null) JoinPoints.Add(value);

            // Recursive call
            if (IsTypeToFollow(parent.GetType()))
            {
                if (value == null) value = GetValue(parent, member); // TODO: there must be better ways to do this.
                
                SetFieldsAndProperties(value, done);
            }
        }

        // Annull

        public void Annull()
        {
            JoinPoints.Clear();
        }

        private void AnnullFieldsAndProperties(object parent, HashSet<object> done)
        {
            // Match parent
            if (!MatchParent(parent, done)) return;

            // Traverse fields
            foreach (var field in parent.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                AnnullField(parent, field, done);
            }

            // Traverse properties
            foreach (var property in parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                AnnullProperty(parent, property, done);
            }
        }

        private void AnnullField(object parent, FieldInfo field, HashSet<object> done)
        {
            // Mutual base method
            AnnullFieldOrProperty(parent, field, done);
        }

        private void AnnullProperty(object parent, PropertyInfo property, HashSet<object> done)
        {
            // Skip indexers
            if (property.GetIndexParameters().Length > 0) return;

            // Check existence of getter.
            if (!property.CanRead) return;

            // Mutual base method
            AnnullFieldOrProperty(parent, property, done);
        }

        private void AnnullFieldOrProperty(object parent, MemberInfo member, HashSet<object> done)
        {
            // Match
            object value = MatchFieldOrProperty(parent, member);

            // Remove
            if (value != null) JoinPoints.Remove(value);

            // Recursive call
            if (IsTypeToFollow(parent.GetType()))
            {
                if (value == null) value = GetValue(parent, member); // TODO: there must be better ways to do this.
                
                AnnullFieldsAndProperties(value, done);
            }
        }

        // Shared by Set and Annull

        private bool MatchParent(object parent, HashSet<object> done)
        {
            // Skip nulls
            if (parent == null) return false; 

            // Values cannot be parents
            if (parent is ValueType) return false; 

            // Handle circularity
            if (done.Contains(parent)) return false;
            done.Add(parent);

            // Match parent
            Type parentType = parent.GetType();
            bool isParentMatch = IsTypeMatch(parentType) && !IsTypeExcluded(parentType);
            if (!isParentMatch) return false;

            return true;
        }

        private Events.Events MatchFieldOrProperty(object parent, MemberInfo member)
        {
            // Match member name
            if (!IsMemberMatch(member.Name)) return null;
            if (IsMemberExcluded(member.Name)) return null;

            // Get Value
            object value = GetValue(parent, member);
            if (value == null) return null;

            // Never check IsCreated to prevent auto-instantiation,
            // because even when it is not created, you still have to unbind OnLive.

            // Is Events?
            var events = value as Events.Events;
            if (events == null) return null;

            return events;
        }

        private object GetValue(object parent, MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    FieldInfo field = member as FieldInfo;
                    return field.GetValue(parent);

                case MemberTypes.Property:
                    PropertyInfo property = member as PropertyInfo;
                    try { return property.GetValue(parent, null); }
                    catch { return null; }
            }

            return null;
        }

        // Helpers

        private bool IsTypeMatch(Type type)
        {
            if (Types.Contains(type))
            {
                return true;
            }

            foreach (var x in Types)
            {
                if (type.IsSubclassOf(x)) return true;
            }

            return false;
        }

        private bool IsTypeExcluded(Type type)
        {
            if (ExcludedTypes.Contains(type))
            {
                return true;
            }

            foreach (var x in ExcludedTypes)
            {
                if (type.IsSubclassOf(x)) return true;
            }

            return false;
        }

        private bool IsTypeToFollow(Type type)
        {
            if (TypesToFollow.Contains(type))
            {
                return true;
            }

            foreach (var x in TypesToFollow)
            {
                if (type.IsSubclassOf(x)) return true;
            }

            return false;
        }

        private bool IsMemberMatch(string name)
        {
            foreach (var member in MemberRegexs)
            {
                if (member.IsMatch(name))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMemberExcluded(string name)
        {
            foreach (var excludedMember in ExcludedMemberRegexs)
            {
                if (excludedMember.IsMatch(name))
                {
                    return true;
                }
            }

            return false;
        }

        // Live creation of new join points

        private void OnLive(object sender, ChangedEventArgs<object> e)
        {
            if (e.Old != null)
            {
                AnnullFieldsAndProperties(e.Old, done: new HashSet<object>());
            }

            if (e.Value != null)
            {
                SetFieldsAndProperties(e.Value, done: new HashSet<object>());
            }
        }

        // Event procedures

        [DebuggerHidden]
        private void OnGetting(object sender, GettingEventArgs<object> e)
        {
            if (Getting != null) Getting(sender, e);
        }

        [DebuggerHidden]
        private void OnGotten(object sender, GottenEventArgs<object> e)
        {
            if (Gotten != null) Gotten(sender, e);
        }

        [DebuggerHidden]
        private void OnAssigning(object sender, AssigningEventArgs<object> e)
        {
            if (Assigning != null) Assigning(sender, e);
        }

        [DebuggerHidden]
        private void OnChanging(object sender, ChangingEventArgs<object> e)
        {
            if (Changing != null) Changing(sender, e);
        }

        [DebuggerHidden]
        private void OnChanged(object sender, ChangedEventArgs<object> e)
        {
            if (Changed != null) Changed(sender, e);
        }
    }
}
