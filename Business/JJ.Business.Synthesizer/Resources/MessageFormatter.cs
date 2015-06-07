﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Resources
{
    public static class MessageFormatter
    {
        public static string InletNotSet(string operatorTypeName, string operatorName, string operandName)
        {
            return String.Format(Messages.InletNotSet, operatorTypeName, operatorName, operandName);
        }

        public static string ValueOperatorValueIs0(string valueOperatorName)
        {
            return String.Format(Messages.ValueOperatorValueIs0, valueOperatorName);
        }

        public static string UnsupportedOperatorTypeName(string operatorTypeName)
        {
            return String.Format(Messages.UnsupportedOperatorTypeName, operatorTypeName);
        }

        public static string SampleNotLoaded(string sampleName)
        {
            return String.Format(Messages.SampleNotLoaded, sampleName);
        }

        public static string ObjectAmplifier0(string obectTypeName, string objectName)
        {
            return String.Format(Messages.ObjectAmplifier0, obectTypeName, objectName);
        }

        public static string SampleNotActive(string sampleName)
        {
            return String.Format(Messages.SampleNotActive, sampleName);
        }

        public static string SampleCount0(string sampleName)
        {
            return String.Format(Messages.SampleCount0, sampleName);
        }

        public static string ChannelCountDoesNotMatchSpeakerSetup()
        {
            return Messages.ChannelCountDoesNotMatchSpeakerSetup;
        }

        public static string ChannelIndexNumberDoesNotMatchSpeakerSetup()
        {
            return Messages.ChannelIndexNumberDoesNotMatchSpeakerSetup;
        }

        public static string OperatorPatchIsNotTheExpectedPatch(string operatorName, string expectedPatchName)
        {
            return String.Format(Messages.OperatorPatchIsNotTheExpectedPatch, operatorName, expectedPatchName);
        }

        public static string DocumentIsDependentOnDocument(string dependentDocumentName, string dependentOnDocumentName)
        {
            return String.Format(Messages.DocumentIsDependentOnDocument, dependentDocumentName, dependentOnDocumentName);
        }

        public static string CannotDeleteCurveBecauseHasOperators(string name)
        {
            return String.Format(Messages.CannotDeleteCurveBecauseHasOperators, name);
        }

        public static string CannotDeleteSampleBecauseHasOperators(string name)
        {
            return String.Format(Messages.CannotDeleteSampleBecauseHasOperators, name);
        }

        public static string CannotDeletePatchBecauseIsMainPatch(string name)
        {
            return String.Format(Messages.CannotDeletePatchBecauseIsMainPatch, name);
        }
    }
}
