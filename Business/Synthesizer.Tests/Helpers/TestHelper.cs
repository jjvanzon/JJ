﻿using System.IO;
using JJ.Framework.Common;
using JJ.Framework.Configuration;

namespace JJ.Business.Synthesizer.Tests.Helpers
{
    internal static class TestHelper
    {
        private const string VIOLIN_16BIT_MONO_RAW_FILE_NAME = "violin_16bit_mono.raw";
        private const string VIOLIN_16BIT_MONO_44100_WAV_FILE_NAME = "violin_16bit_mono_44100.wav";

        private static bool _configurationSectionsAreSet;
        public static void SetConfigurationSections()
        {
            if (!_configurationSectionsAreSet)
            {
                _configurationSectionsAreSet = true;

                var config = CustomConfigurationManager.GetSection<JJ.Business.Synthesizer.Configuration.ConfigurationSection>();
                ConfigurationHelper.SetSection(config);
            }
        }

        public static Stream GetViolin16BitMonoRawStream()
        {
            Stream stream = EmbeddedResourceHelper.GetEmbeddedResourceStream(typeof(TestHelper).Assembly, "TestResources", VIOLIN_16BIT_MONO_RAW_FILE_NAME);
            return stream;
        }

        public static Stream GetViolin16BitMono44100WavStream()
        {
            Stream stream = EmbeddedResourceHelper.GetEmbeddedResourceStream(typeof(TestHelper).Assembly, "TestResources", VIOLIN_16BIT_MONO_44100_WAV_FILE_NAME);
            return stream;
        }
    }
}
