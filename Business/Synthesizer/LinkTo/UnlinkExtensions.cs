﻿using JJ.Data.Synthesizer;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.LinkTo
{
    public static class UnlinkExtensions
    {
        public static void UnlinkDocument(this AudioFileOutput audioFileOutput)
        {
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);

            audioFileOutput.LinkTo((Document)null);
        }

        public static void UnlinkOutlet(this AudioFileOutput audioFileOutput)
        {
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);

            audioFileOutput.LinkTo((Outlet)null);
        }

        public static void UnlinkDocument(this Curve curve)
        {
            if (curve == null) throw new NullException(() => curve);

            curve.LinkTo((Document)null);
        }

        public static void UnlinkAudioOutput(this Document document)
        {
            if (document == null) throw new NullException(() => document);

            document.LinkTo((AudioOutput)null);
        }

        public static void UnlinkDependentDocument(this DocumentReference documentReference)
        {
            if (documentReference == null) throw new NullException(() => documentReference);

            documentReference.LinkToDependentDocument((Document)null);
        }

        public static void UnlinkDependentOnDocument(this DocumentReference documentOnReference)
        {
            if (documentOnReference == null) throw new NullException(() => documentOnReference);

            documentOnReference.LinkToDependentOnDocument((Document)null);
        }

        public static void UnlinkCurve(this Node node)
        {
            if (node == null) throw new NullException(() => node);

            node.LinkTo((Curve)null);
        }

        public static void UnlinkDocument(this Patch patch)
        {
            if (patch == null) throw new NullException(() => patch);

            patch.LinkTo((Document)null);
        }

        public static void UnlinkPatch(this Operator op)
        {
            if (op == null) throw new NullException(() => op);

            op.LinkTo((Patch)null);
        }

        public static void UnlinkOperator(this Inlet inlet)
        {
            if (inlet == null) throw new NullException(() => inlet);

            inlet.LinkTo((Operator)null);
        }

        public static void UnlinkOperator(this Outlet outlet)
        {
            if (outlet == null) throw new NullException(() => outlet);

            outlet.LinkTo((Operator)null);
        }

        public static void UnlinkInputOutlet(this Inlet inlet)
        {
            if (inlet == null) throw new NullException(() => inlet);

            inlet.LinkTo((Outlet)null);
        }

        public static void UnlinkDocument(this Scale scale)
        {
            if (scale == null) throw new NullException(() => scale);

            scale.LinkTo((Document)null);
        }

        public static void UnlinkScale(this Tone tone)
        {
            if (tone == null) throw new NullException(() => tone);

            tone.LinkTo((Scale)null);
        }

        public static void UnlinkDocument(this Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);

            sample.LinkTo((Document)null);
        }

        // Enum-Like Entities

        public static void UnlinkSpeakerSetup(this AudioFileOutput audioFileOutput)
        {
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);

            audioFileOutput.LinkTo((SpeakerSetup)null);
        }

        public static void UnlinkAudioFileFormat(this AudioFileOutput audioFileOutput)
        {
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);

            audioFileOutput.LinkTo((AudioFileFormat)null);
        }

        public static void UnlinkSampleDataType(this AudioFileOutput audioFileOutput)
        {
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);

            audioFileOutput.LinkTo((SampleDataType)null);
        }

        public static void UnlinkSpeakerSetup(this AudioOutput audioOutput)
        {
            if (audioOutput == null) throw new NullException(() => audioOutput);

            audioOutput.LinkTo((SpeakerSetup)null);
        }

        public static void UnlinkDimension(this Inlet inlet)
        {
            if (inlet == null) throw new NullException(() => inlet);

            inlet.LinkTo((Dimension)null);
        }

        public static void UnlinkNodeType(this Node node)
        {
            if (node == null) throw new NullException(() => node);

            node.LinkTo((NodeType)null);
        }

        public static void UnlinkOperatorType(this Operator op)
        {
            if (op == null) throw new NullException(() => op);

            op.LinkTo((OperatorType)null);
        }

        public static void UnlinkStandardDimension(this Operator op)
        {
            if (op == null) throw new NullException(() => op);

            op.LinkTo((Dimension)null);
        }

        public static void UnlinkDimension(this Outlet outlet)
        {
            if (outlet == null) throw new NullException(() => outlet);

            outlet.LinkTo((Dimension)null);
        }

        public static void UnlinkAudioFileFormat(this Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);

            sample.LinkTo((AudioFileFormat)null);
        }

        public static void UnlinkInterpolationType(this Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);

            sample.LinkTo((InterpolationType)null);
        }

        public static void UnlinkSampleDataType(this Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);

            sample.LinkTo((SampleDataType)null);
        }

        public static void UnlinkSpeakerSetup(this Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);

            sample.LinkTo((SpeakerSetup)null);
        }

        public static void UnlinkScaleType(this Scale scale)
        {
            if (scale == null) throw new NullException(() => scale);

            scale.LinkTo((ScaleType)null);
        }
    }
}