//
//  Circle.Diagram.Engine.StepWithSteps
//
//      Author: Jan-Joost van Zon
//      Date: 2011-02-10 - 2011-02-10
//
//  -----
//
//      Under Construction...
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Diagram.Engine
{
    /*
    public class StepWithSteps : Step
    {
        public List<Step> Steps;

        protected override void OnExecute()
        {
            foreach (Step step in Steps)
            {
                step.Execute();
            }
        }
    }

    public class HasSteps
    {
        public List<Step> Steps;

        /*
        protected override void OnExecute()
        {
            foreach (Step step in Steps)
            {
                step.Execute();
            }
        }
    }
    */

    /*
    public class HasStepsWithDiagram
    {
        public List<StepWithDiagram> Steps;

        private Entities.Diagram _diagram;
        public Entities.Diagram Diagram
        {
            get
            {
                return _diagram;
            }
            set
            {
                if (_diagram != value)
                {
                    _diagram = value;
                    foreach (StepWithDiagram step in Steps)
                    {
                        step.Diagram = _diagram;
                    }
                }
            }
        }
    }

    public class HasMethod<T>
    {
        public HasMethod(T _default)
        {
            _method = _default;
        }

        public List<StepOfMethod<T>> Steps;

        private T _method;

        public T Method
        {
            get
            {
                return _method;
            }
            set
            {
                if (Equals(_method, value))
                {
                    _method = value;
                    ApplyMethod();
                }
            }
        }
        private void ApplyMethod()
        {
            foreach (StepOfMethod<T> step in Steps)
            {
                step.Active = Equals(Method, step.Method);
            }
        }
    }

    public class StepOfMethod<T> : Step
    {
        public T Method;
    }
    */
}
