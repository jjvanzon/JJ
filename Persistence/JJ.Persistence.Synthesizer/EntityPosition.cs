﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Persistence.Synthesizer
{
    public class EntityPosition
    {
        public virtual string EntityTypeName { get; set; }
        public virtual int EntityID { get; set; }
        public virtual float X { get; set; }
        public virtual float Y { get; set; }
    }
}
