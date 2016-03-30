﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using System;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Mathematics;
using JJ.Framework.Common;

namespace JJ.Business.Synthesizer.Validation
{
    internal class OperatorValidator_Spectrum : OperatorValidator_Base
    {
        public OperatorValidator_Spectrum(Operator obj)
            : base(
                  obj, 
                  OperatorTypeEnum.Spectrum, 
                  expectedInletCount: 1, 
                  expectedOutletCount: 1,
                  allowedDataKeys: new string[] 
                  {
                      PropertyNames.StartTime,
                      PropertyNames.EndTime,
                      PropertyNames.FrequencyCount
                  })
        { }

        protected override void Execute()
        {
            base.Execute();

            string startTimeString = DataPropertyParser.TryGetString(Object, PropertyNames.StartTime);
            string endTimeString = DataPropertyParser.TryGetString(Object, PropertyNames.EndTime);
            string frequencyCountString = DataPropertyParser.TryGetString(Object, PropertyNames.FrequencyCount);

            For(() => startTimeString, PropertyDisplayNames.StartTime, DataPropertyParser.FormattingCulture)
                .NotNullOrEmpty()
                .IsDouble()
                .NotInfinity()
                .NotNaN();

            For(() => endTimeString, PropertyDisplayNames.EndTime, DataPropertyParser.FormattingCulture)
                .NotNullOrEmpty()
                .IsDouble()
                .NotInfinity()
                .NotNaN();

            For(() => frequencyCountString, PropertyDisplayNames.FrequencyCount)
                .NotNullOrEmpty()
                .IsInteger();

            double startTime;
            if (Doubles.TryParse(startTimeString, DataPropertyParser.FormattingCulture, out startTime))
            {
                double endTime;
                if (Doubles.TryParse(endTimeString, DataPropertyParser.FormattingCulture, out endTime))
                {
                    if (endTime < startTime)
                    {
                        ValidationMessages.AddLessThanMessage(PropertyNames.EndTime, PropertyDisplayNames.EndTime, PropertyDisplayNames.StartTime);
                    }
                }
            }

            int frequencyCount;
            if (Int32.TryParse(frequencyCountString, out frequencyCount))
            {
                For(() => frequencyCount, PropertyDisplayNames.FrequencyCount)
                    .GreaterThanOrEqual(2);

                if (!Maths.IsPowerOf2(frequencyCount))
                {
                    string message = MessageFormatter.MustBePowerOf2(PropertyDisplayNames.FrequencyCount);
                    ValidationMessages.Add(PropertyDisplayNames.FrequencyCount, message);
                }
            }
        }
    }
}
