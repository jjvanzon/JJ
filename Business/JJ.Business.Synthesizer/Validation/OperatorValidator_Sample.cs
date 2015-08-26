﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation
{
    public class OperatorValidator_Sample : OperatorValidator_Base
    {
        public OperatorValidator_Sample(Operator op)
            : base(op, OperatorTypeEnum.Sample, 0, PropertyNames.Result)
        { }

        protected override void Execute()
        {
            base.Execute();

            Operator op = Object;

            For(() => op.Data, PropertyDisplayNames.Data)
                .IsInteger();

            // TODO: Remove return statement when samples always come out of the document, and are not hacked into it in the PatchPlay action in the front-end.
            return;

            int sampleID;
            if (Int32.TryParse(op.Data, out sampleID))
            {
                // Check reference constraint of the Curve.
                // (We are quite tollerant here: we omit the check if it is not in a patch or document.)
                bool mustCheckReference = op.Patch != null && op.Patch.Document != null;
                if (mustCheckReference)
                {
                    bool isRootDocument = op.Patch.Document.ParentDocument == null;

                    // If we're in a child document, we can reference the samples in both child document and root document,
                    // if we are in the root document, the possible samples are only the ones in the root document.
                    IEnumerable<Sample> samples;
                    if (isRootDocument)
                    {
                        samples = op.Patch.Document.Samples;
                    }
                    else
                    {
                        samples = op.Patch.Document.Samples.Union(op.Patch.Document.ParentDocument.Samples);
                    }

                    bool isInList = samples.Any(x => x.ID == sampleID);

                    if (!isInList)
                    {
                        ValidationMessages.Add(PropertyNames.Sample, MessageFormatter.NotFoundInList_WithItemName_AndID(PropertyDisplayNames.Sample, sampleID));
                    }
                }
            }
        }
    }
}