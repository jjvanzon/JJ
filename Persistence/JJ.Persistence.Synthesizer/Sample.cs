﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Persistence.Synthesizer
{
    public class Sample
    {
        public Sample ()
	    {
            SampleOperators = new List<SampleOperator>();
	    }

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual double Amplifier { get; set; }
        public virtual double TimeMultiplier { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int SamplingRate { get; set; }

        /// <summary>
        /// E.g. when you need to skip a header of a file.
        /// </summary>
        public virtual int BytesToSkip { get; set; }

        /// <summary>
        /// Optional. E.g. the file path.
        /// </summary>
        public virtual string Location { get; set; }

        /// <summary> not nullable </summary>
        public virtual SampleDataType SampleDataType { get; set; }

        /// <summary> not nullable </summary>
        public virtual SpeakerSetup SpeakerSetup { get; set; }

        /// <summary> not nullable </summary>
        public virtual AudioFileFormat AudioFileFormat { get; set; }

        /// <summary> not nullable </summary>
        public virtual InterpolationType InterpolationType { get; set; }

        public virtual IList<SampleOperator> SampleOperators { get; set; }

        public virtual byte[] Bytes { get; set; }
    }
}
