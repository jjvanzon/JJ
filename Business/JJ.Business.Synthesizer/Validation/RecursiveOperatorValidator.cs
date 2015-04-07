﻿using JJ.Business.Synthesizer.Names;
using JJ.Business.Synthesizer.Validation.Entities;
using JJ.Framework.Validation;
using JJ.Persistence.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Validation
{
    /// <summary>
    /// Validates a root operator and its descendant operators.
    /// Also validates related curves and samples.
    /// Makes sure that objects are only validated once to 
    /// prevent problems with circularities.
    /// </summary>
    public class RecursiveOperatorValidator : ValidatorBase<Operator>
    {
        private ISet<object> _alreadyDone;

        /// <summary>
        /// Validates a root operator and its descendant operators.
        /// Also validates related curves and samples.
        /// Makes sure that objects are only validated once to 
        /// prevent problems with circularity.
        /// </summary>
        public RecursiveOperatorValidator(Operator obj, ISet<object> alreadyDone = null)
            : base(obj, postponeExecute: true)
        { 
            _alreadyDone = alreadyDone ?? new HashSet<object>();

            Execute();
        }

        protected override void Execute()
        {
            if (_alreadyDone.Contains(Object))
            {
                return;
            }
            _alreadyDone.Add(Object);

            string messagePrefix = String.Format("{0} '{1}': ", Object.OperatorTypeName, Object.Name);

            Execute(new VersatileOperatorValidator(Object), messagePrefix);

            CurveIn curveIn = Object.AsCurveIn;
            if (curveIn != null)
            {
                Curve curve = curveIn.Curve;
                if (curve != null)
                {
                    if (!_alreadyDone.Contains(curve))
                    {
                        _alreadyDone.Add(curve);
                        Execute(new CurveValidator(curve));
                    }
                }
            }

            SampleOperator sampleOperator = Object.AsSampleOperator;
            if (sampleOperator != null)
            {
                Sample sample = sampleOperator.Sample;
                if (sample != null)
                {
                    if (!_alreadyDone.Contains(sample))
                    {
                        _alreadyDone.Add(sample);
                        Execute(new SampleValidator(sample));
                    }
                }
            }
            
            foreach (Inlet inlet in Object.Inlets)
            {
                if (inlet.InputOutlet != null)
                {
                    Execute(new RecursiveOperatorValidator(inlet.InputOutlet.Operator, _alreadyDone));
                }
            }
        }
    }
}
