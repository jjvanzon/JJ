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
using System.Diagnostics;

namespace Circle.Code.Concepts
{
    // TODO: consider using new Circle.Data collections.
    // TODO: consider renaming the horrible term JoinPoint.
    // TODO: make a better solution for circularities.
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
            //return new Regex(format, RegexOptions.Compiled); // In my tests, Compiled was slower.
            return new Regex(format);
        }

        // Joint Points

        public readonly ListWithEvents<IHear<object>> IHearJoinPoints = new ListWithEvents<IHear<object>>(); 
        public readonly ListWithEvents<Events.Events> EventsJoinPoints = new ListWithEvents<Events.Events>(); 

        public int MaxLevels = 10;

        // Set

        private Stopwatch Stopwatch = new Stopwatch();

        [DebuggerDisplay("{Diff} {Name} {Obj} {Time}")]
        private class LogRecord
        {
            public string Name;
            public string Obj;
            public long Time;
            public long Diff;
        }

        long StartTime;
        private List<LogRecord> Log = new List<LogRecord>();

        public void Set()
        {
            // Debugging
            Log.Clear();
            Stopwatch.Start();

            StartTime = Stopwatch.ElapsedTicks;

            /*Log.Add(new LogRecord()
            {
                Name = "Set",
                Time = Stopwatch.ElapsedTicks,
                Obj = null
            }); */

            // Collect join points
            foreach (var root in Roots)
            {
                SetFieldsAndProperties(root, level: 0, done: new HashSet<object>());
            }

            // Debugging
            var prev = Log.First();

            foreach (var x in Log.Skip(1))
            {
                x.Time -= StartTime;
                x.Diff = x.Time - prev.Time;
                prev = x;
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

            // Debugging
            /*Log.Add(new LogRecord()
            {
                Name = "SetFieldsAndProperties",
                Time = Stopwatch.ElapsedTicks,
                Obj = parent != null ? parent.ToString() : ""
            });*/

            // Traverse fields
            foreach (var field in Cache.GetFields(parent.GetType()))
            {
                SetField(parent, field, level, done);
            }

            // Traverse properties
            foreach (var property in Cache.GetProperties(parent.GetType()))
            {
                SetProperty(parent, property, level, done);
            }
        }

        private void SetField(object parent, FieldInfo field, int level, HashSet<object> done)
        {
            // Debugging
            /*Log.Add(new LogRecord()
            {
                Name = "SetField",
                Time = Stopwatch.ElapsedTicks,
                Obj = field != null ? field.ToString() : ""
            });*/
            
            // Get value
            object value = field.GetValue(parent);

            // Mutual base method
            SetFieldOrProperty(parent, value, field.Name, level, done);
        }

        private void SetProperty(object parent, PropertyInfo property, int level, HashSet<object> done)
        {
            // Debugging
            /*Log.Add(new LogRecord()
            {
                Name = "SetProperty",
                Time = Stopwatch.ElapsedTicks,
                Obj = property != null ? property.ToString() : ""
            });*/

            // Skip indexers
            if (property.GetIndexParameters().Length > 0) return;

            // Get value
            object value = property.GetValue(parent, null);

            // Mutual base method
            SetFieldOrProperty(parent, value, property.Name, level, done);
        }

        private void SetFieldOrProperty(object parent, object value, string name, int level, HashSet<object> done)
        {
            // Debugging
            /*Log.Add(new LogRecord()
            {
                Name = "SetFieldOrProperty",
                Time = Stopwatch.ElapsedTicks,
                Obj = value != null ? value.ToString() : ""
            });*/

            // Skip nulls
            if (value == null) return;

            // Match and bind
            MatchAndBind(parent, parent.GetType(), value, value.GetType(), name);

            // Recursive call
            SetFieldsAndProperties(value, level + 1, done);
        }

        private void MatchAndBind(
            object parent, 
            Type parentType, 
            object value, 
            Type type, 
            string name)
        {

            // Is IHear?
            if (!(value is IHear<object>)) return;

            IHear<object> iHear = value as IHear<object>;

            // Check IsCreated (to prevent auto-instantiation)
            if (!iHear.IsCreated) return;

            // Match
            if (!IsTypeMatch(parentType)) return;
            if (IsTypeExcluded(parentType)) return;
            if (!IsMemberMatch(name)) return;
            if (IsMemberExcluded(name)) return;

            // Debugging
            Log.Add(new LogRecord()
            {
                Name = "MatchAndBind",
                Time = Stopwatch.ElapsedTicks,
                Obj = value != null ? value.ToString() : ""
            });

            // Bind to IHear

            IHearJoinPoints.Add(iHear);
            iHear.Changed += JoinPoint_Changed;
            // Also bind to changed event in order to bind new join points.
            iHear.Changed += JoinPoint_Live;

            // Bind to Events

            if (value is Events.Events)
            {
                Events.Events events = value as Events.Events;

                EventsJoinPoints.Add(events);
                events.Getting += JoinPoint_Getting;
                events.Gotten += JoinPoint_Gotten;
                events.Assigning += JoinPoint_Assigning;
                events.Changing += JoinPoint_Changing;
                events.Creating += JoinPoint_Creating;
                events.Created += JoinPoint_Created;
                events.Annulling += JoinPoint_Annulling;
                events.Annulled += JoinPoint_Annulled;
            }
        }

        private bool IsTypeMatch(Type type)
        {
            // Debugging
            /*Log.Add(new LogRecord()
            {
                Name = "IsTypeMatch",
                Time = Stopwatch.ElapsedTicks
            });*/

            return Types.Contains(type);
        }

        private bool IsTypeExcluded(Type type)
        {
            // Debugging
            /*Log.Add(new LogRecord()
            {
                Name = "IsTypeExcluded",
                Time = Stopwatch.ElapsedTicks
            });*/

            return ExcludedTypes.Contains(type);
        }

        private bool IsMemberMatch(string name)
        {
            // Debugging
            /*Log.Add(new LogRecord()
            {
                Name = "IsMemberMatch",
                Time = Stopwatch.ElapsedTicks
            });*/

            foreach (var member in MemberRegexs)
            {
                if (member.IsMatch(name))
                {
                    return true;
                }
            }

            /*foreach (var member in Members)
            {
                if (name.Match(member))
                {
                    return true;
                }
            }*/

            return false;
        }

        private bool IsMemberExcluded(string name)
        {
            // Debugging
            /*Log.Add(new LogRecord()
            {
                Name = "IsMemberExcluded",
                Time = Stopwatch.ElapsedTicks
            });*/

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
                joinPoint.Changed -= JoinPoint_Changed;
                joinPoint.Changed -= JoinPoint_Live;
            }

            IHearJoinPoints.Clear();

            foreach (Events.Events joinPoint in EventsJoinPoints)
            {
                joinPoint.Getting -= JoinPoint_Getting;
                joinPoint.Gotten -= JoinPoint_Gotten;
                joinPoint.Assigning -= JoinPoint_Assigning;
                joinPoint.Changing -= JoinPoint_Changing;
                joinPoint.Creating -= JoinPoint_Creating;
                joinPoint.Created -= JoinPoint_Created;
                joinPoint.Annulling -= JoinPoint_Annulling;
                joinPoint.Annulled -= JoinPoint_Annulled;
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
            Unbind(value, level);

            // Recursive call
            AnnullFieldsAndProperties(value, level + 1, done);
        }

        private void Unbind(object value, int level)
        {
            // Is Events?

            if (!(value is IHear<object>)) return;

            // Unbind IHear

            IHear<object> iHear = value as IHear<object>;

            IHearJoinPoints.Remove(iHear);
            iHear.Changed -= JoinPoint_Changed;
            iHear.Changed -= JoinPoint_Live;

            // Unbind Events

            if (value is Events.Events)
            {
                Events.Events events = value as Events.Events;

                if (EventsJoinPoints.Contains(events))
                {
                    EventsJoinPoints.Remove(events);
                    events.Getting -= JoinPoint_Getting;
                    events.Gotten -= JoinPoint_Gotten;
                    events.Assigning -= JoinPoint_Assigning;
                    events.Changing -= JoinPoint_Changing;
                    events.Creating -= JoinPoint_Creating;
                    events.Created -= JoinPoint_Created;
                    events.Annulling -= JoinPoint_Annulling;
                    events.Annulled -= JoinPoint_Annulled;
                }
            }
        }

        // Create new join points

        private void JoinPoint_Live(object old, object value)
        {
            // The max levels thing will fail here

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
