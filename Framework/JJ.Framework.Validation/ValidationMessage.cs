﻿using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Validation
{
    public class ValidationMessage
    {
        public string PropertyKey { get; private set; }
        public string Text { get; private set; }

        public ValidationMessage(string propertyKey, string text)
        {
            if (String.IsNullOrEmpty(propertyKey)) throw new NullException(() => propertyKey);
            if (String.IsNullOrEmpty(text)) throw new NullException(() => text);

            PropertyKey = propertyKey;
            Text = text;
        }
    }
}
