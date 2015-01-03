//
//  Circle.Code.Concepts.Response
//
//      Author: Jan-Joost van Zon
//      Date: 24-06-2011 - 03-07-2011
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

namespace Circle.Code.Concepts
{
    // TODO: consider using new Circle.Data collections.
    // TODO: the event procedures are repeated in EventsT. Code smell.

    /// <summary>
    /// Using reflection,
    /// go through the sub-objects of Roots,
    /// detect Events&lt;&gt;, see if they match the criteria,
    /// and set their event response so that they delegate to Response's own events.
    /// You should take circularities into account.
    /// </summary>
    public class Response
    {
        // Constructor

        public Response()
        {
            InitializeMembers();
            InitializeExcludedMembers();
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
            Members.Added += (e) => MemberRegexs.Insert(e.Index, null);
            Members.Removed += (item, index) => MemberRegexs.RemoveAt(index);
            Members.Changed += (old, item, index) => MemberRegexs[index] = CreateRegex(item);
        }

        // Excluded Members

        public readonly ListWithEvents<string> ExcludedMembers = new ListWithEvents<string>();
        private readonly List<Regex> ExcludedMemberRegexs = new List<Regex>();

        private void InitializeExcludedMembers()
        {
            ExcludedMembers.Added += (e) => ExcludedMemberRegexs.Insert(e.Index, null);
            ExcludedMembers.Removed += (item, index) => ExcludedMemberRegexs.RemoveAt(index);
            ExcludedMembers.Changed += (old, item, index) => ExcludedMemberRegexs[index] = CreateRegex(item);
        }

        // Regex

        private Regex CreateRegex(string format)
        {
            format = format.Replace("*", ".*");
            format = format.Replace("?", ".?");
            return new Regex(format); // RegexOptions.Compiled was slower
        }

        // Joint Points

        // TODO: these lists should not be just public.
        public readonly ListWithEvents<IHear<object>> IHearJoinPoints = new ListWithEvents<IHear<object>>(); 
        public readonly ListWithEvents<Events.Events> EventsJoinPoints = new ListWithEvents<Events.Events>(); 

        public int MaxLevels = 10;

        // Set

        public void Set()
        {
            Annull();

            // Collect join points
            foreach (var root in Roots)
            {
                SetFieldsAndProperties(root, level: 0, done: new HashSet<object>());
            }
        }

        private void SetFieldsAndProperties(object parent, int level, HashSet<object> done)
        {
            // Conditions
            if (level >= MaxLevels) return; // Check max levels
            if (parent == null) return; // Skip nulls
            if (parent is ValueType) return; // Values cannot be parents

            // Handle doubles
            if (done.Contains(parent)) return;
            done.Add(parent);

            // Traverse fields
            foreach (var field in ReflectionCache.GetFields(parent.GetType()))
            {
                SetField(parent, field, level, done);
            }

            // Traverse properties
            foreach (var property in ReflectionCache.GetProperties(parent.GetType()))
            {
                SetProperty(parent, property, level, done);
            }
        }

        private void SetField(object parent, FieldInfo field, int level, HashSet<object> done)
        {
            // Get value
            object value = field.GetValue(parent);

            // Mutual base method
            SetFieldOrProperty(parent, value, field.Name, level, done);
        }

        private void SetProperty(object parent, PropertyInfo property, int level, HashSet<object> done)
        {
            // Skip indexers
            if (property.GetIndexParameters().Length > 0) return;

            // Get value
            object value = property.GetValue(parent, null);

            // Mutual base method
            SetFieldOrProperty(parent, value, property.Name, level, done);
        }

        private void SetFieldOrProperty(object parent, object value, string name, int level, HashSet<object> done)
        {
            // Skip nulls
            if (value == null) return;
            
            // Match parent
            Type parentType = parent.GetType();
            bool isParentMatch = IsTypeMatch(parentType) && !IsTypeExcluded(parentType);
            if (isParentMatch)
            {
                // Match member and bind
                MatchAndBind(value, name);
            }

            // Recursive call
            SetFieldsAndProperties(value, level + 1, done);
        }

        private void MatchAndBind(
            object value, 
            string name)
        {
            // Is IHear?
            if (!(value is IHear<object>)) return;
            IHear<object> hear = value as IHear<object>;

            // Check IsCreated (to prevent auto-instantiation)
            // Does not work, because even if it is not created, 
            // you still have to bind to OnLive.
            // if (!hear.IsCreated) return; 

            // Match
            if (!IsMemberMatch(name)) return;
            if (IsMemberExcluded(name)) return;

            // Bind
            Bind(hear);
        }

        public void Bind(IHear<object> hear)
        {
            // Bind to IHear

            IHearJoinPoints.Add(hear);
            hear.Changed += OnChanged;
            // Also bind to changed event in order to bind new join points.
            hear.Changed += OnLive;

            // Bind to Events

            if (hear is Events.Events)
            {
                Events.Events events = hear as Events.Events;

                EventsJoinPoints.Add(events);
                events.Getting += OnGetting;
                events.Gotten += OnGotten;
                events.Assigning += OnAssigning;
                events.Changing += OnChanging;
                events.Creating += OnCreating;
                events.Created += OnCreated;
                events.Annulling += OnAnnulling;
                events.Annulled += OnAnnulled;
            }
        }

        private bool IsTypeMatch(Type type)
        {
            return Types.Contains(type);
        }

        private bool IsTypeExcluded(Type type)
        {
            return ExcludedTypes.Contains(type);
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

        // Annull

        public void Annull()
        {
            foreach (IHear<object> joinPoint in IHearJoinPoints)
            {
                joinPoint.Changed -= OnChanged;
                joinPoint.Changed -= OnLive;
            }

            IHearJoinPoints.Clear();

            foreach (Events.Events joinPoint in EventsJoinPoints)
            {
                joinPoint.Getting -= OnGetting;
                joinPoint.Gotten -= OnGotten;
                joinPoint.Assigning -= OnAssigning;
                joinPoint.Changing -= OnChanging;
                joinPoint.Creating -= OnCreating;
                joinPoint.Created -= OnCreated;
                joinPoint.Annulling -= OnAnnulling;
                joinPoint.Annulled -= OnAnnulled;
            }

            EventsJoinPoints.Clear();
        }

        private void AnnullFieldsAndProperties(object parent, int level, HashSet<object> done)
        {
            // Conditions            
            if (level >= MaxLevels) return; // Check max level
            if (parent == null) return; // Skip nulls
            if (parent is ValueType) return; // Values cannot be parents

            // Handle circularity
            if (done.Contains(parent)) { return; }
            done.Add(parent);

            // Traverse fields
            foreach (var field in parent.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                AnnullField(parent, field, level, done);
            }

            // Traverse properties
            foreach (var property in parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                AnnullProperty(parent, property, level, done);
            }
        }

        private void AnnullField(object parent, FieldInfo field, int level, HashSet<object> done)
        {
            // Get value
            object value = field.GetValue(parent);

            // Mutual base method
            AnnullFieldOrProperty(value, level, done);
        }

        private void AnnullProperty(object parent, PropertyInfo property, int level, HashSet<object> done)
        {
            // Skip indexers
            if (property.GetIndexParameters().Length > 0) return;

            // Get value
            object value = property.GetValue(parent, null);

            // Mutual base method
            AnnullFieldOrProperty(value, level, done);
        }

        private void AnnullFieldOrProperty(object value, int level, HashSet<object> done)
        {
            // Skip nulls
            if (value == null) return;

            // Unbind and continue
            Unbind(value);

            // Recursive call
            AnnullFieldsAndProperties(value, level + 1, done);
        }

        public void Unbind(object value)
        {
            // Is IHear?

            if (!(value is IHear<object>)) return;

            IHear<object> hear = value as IHear<object>;

            // Never check IsCreated to prevent auto-instantiation,
            // because even when it is not created, you still have to unbind OnLive.

            // Unbind IHear

            IHearJoinPoints.Remove(hear);

            hear.Changed -= OnChanged;
            hear.Changed -= OnLive;

            // Unbind Events

            if (value is Events.Events)
            {
                Events.Events events = value as Events.Events;

                if (EventsJoinPoints.Contains(events))
                {
                    EventsJoinPoints.Remove(events);

                    events.Getting -= OnGetting;
                    events.Gotten -= OnGotten;
                    events.Assigning -= OnAssigning;
                    events.Changing -= OnChanging;
                    events.Creating -= OnCreating;
                    events.Created -= OnCreated;
                    events.Annulling -= OnAnnulling;
                    events.Annulled -= OnAnnulled;
                }
            }
        }

        // Create new join points

        private void OnLive(object old, object value)
        {
            // TODO: The max levels thing will fail here

            if (old != null)
            {
                AnnullFieldsAndProperties(old, level: 0, done: new HashSet<object>());
            }

            if (value != null)
            {
                SetFieldsAndProperties(value, level: 0, done: new HashSet<object>());
            }
        }

        // Event procedures

        private bool OnGetting(object value)
        {
            if (Getting != null) return Getting(value);
            return true;
        }

        private void OnGotten(object value)
        {
            if (Gotten != null) Gotten(value);
        }

        private void OnAssigning(object value)
        {
            if (Assigning != null) Assigning(value);
        }

        private bool OnChanging(object old, object value)
        {
            if (Changing != null) return Changing(old, value);
            return true;
        }

        private void OnChanged(object old, object value)
        {
            if (Changed != null) Changed(old, value);
        }

        private bool OnCreating(object value)
        {
            if (Creating != null) return Creating(value);
            return true;
        }

        private void OnCreated(object value)
        {
            if (Created != null) Created(value);
        }

        private bool OnAnnulling(object value)
        {
            if (Annulling != null) return Annulling(value);
            return true;
        }

        private void OnAnnulled(object old)
        {
            if (Annulled != null) Annulled(old);
        }
    }
}
