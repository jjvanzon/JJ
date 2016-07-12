﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Framework.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private readonly string _message;

        public EntityNotFoundException(Type entityType, object key)
        {
            if (entityType == null) throw new ArgumentNullException("entityType");

            _message = String.Format("{0} with key '{1}' not found.", entityType.Name, key);
        }

        public override string Message
        {
            get { return _message; }
        }
    }
}