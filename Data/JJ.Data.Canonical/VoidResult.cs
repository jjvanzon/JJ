using System;
using System.Collections.Generic;

namespace JJ.Data.Canonical
{
    public class VoidResult : IResult
    {
        public bool Successful { get; set; }

        // Note that this is one of the few exceptions where we have logic in the canonical model.
        // The reason for this is that Result classes are so important in business logic,
        // that we really do not want to have null-checks all over the place.
        // The downside is that when you use VoidResult over a service,
        // you do not get the same behavior and Messages can become null anyway.
        // An alternative might have been for the Result classes not to be part of the canonical model,
        // put it in Framework.Business and just have a canonical dumb DTO for it in CanonicalModel.
        // TODO: Low priority: consider implementing the above alterntaive.

        private IList<Message> _messages = new List<Message>();
        /// <summary> not nullable, auto-instantiated </summary>
        public IList<Message> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _messages = value;
            }
        }
    }
}
