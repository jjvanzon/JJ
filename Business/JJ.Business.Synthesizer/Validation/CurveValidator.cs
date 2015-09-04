﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Business.Synthesizer.Validation
{
    public class CurveValidator : FluentValidator<Curve>
    {
        public CurveValidator(Curve obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            Curve curve = Object;

            Execute(new NameValidator(curve.Name, required: false));

            For(() => curve.Nodes.Count, CommonTitleFormatter.EntityCount(PropertyDisplayNames.Nodes)).MinValue(2);

            int i = 1;
            foreach (Node node in curve.Nodes)
            {
                Execute(new NodeValidator(node), ValidationHelper.GetMessagePrefix(node, i));

                i++;
            }
        }
    }
}
