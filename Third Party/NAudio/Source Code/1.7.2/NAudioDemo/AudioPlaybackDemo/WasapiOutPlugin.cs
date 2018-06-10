using System;
using NAudio.Wave;
using System.Windows.Forms;
using System.ComponentModel.Composition;

namespace NAudioDemo.AudioPlaybackDemo
{
    [Export(typeof(IOutputDevicePlugin))]
    class WasapiOutPlugin : IOutputDevicePlugin
    {
        WasapiOutSettingsPanel settingsPanel;

        public IWavePlayer CreateDevice(int latency)
        {
            var wasapi = new WasapiOut(
                settingsPanel.SelectedDevice,
                settingsPanel.ShareMode,
                settingsPanel.UseEventCallback,
                latency);
            return wasapi;
        }

        public UserControl CreateSettingsPanel()
        {
            this.settingsPanel = new WasapiOutSettingsPanel();
            return settingsPanel;
        }

        public string Name => "WasapiOut";

        public bool IsAvailable => Environment.OSVersion.Version.Major >= 6;

        public int Priority => 3;
    }
}
