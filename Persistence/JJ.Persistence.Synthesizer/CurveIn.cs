﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Persistence.Synthesizer
{
    public class CurveIn
    {
        public virtual int ID { get; set; }

        /// <summary> base </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Needed for NHibernate to understand 1-to-1 relationships. 
        /// </summary>
        public virtual int OperatorID { get; private set; }

        /// <summary> nullable </summary>
        public virtual Curve Curve { get; set; }
    }
}
