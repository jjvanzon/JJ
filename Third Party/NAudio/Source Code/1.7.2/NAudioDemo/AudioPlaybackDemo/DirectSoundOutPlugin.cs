using System.Linq;
using NAudio.Wave;
using System.Windows.Forms;
using System.ComponentModel.Composition;

namespace NAudioDemo.AudioPlaybackDemo
{
    [Export(typeof(IOutputDevicePlugin))]
    class DirectSoundOutPlugin : IOutputDevicePlugin
    {
        private DirectSoundOutSettingsPanel settingsPanel;
        private readonly bool isAvailable;

        public DirectSoundOutPlugin() => this.isAvailable = DirectSoundOut.Devices.Count() > 0;

        public IWavePlayer CreateDevice(int latency) => new DirectSoundOut(settingsPanel.SelectedDevice, latency);

        public UserControl CreateSettingsPanel()
        {
            this.settingsPanel = new DirectSoundOutSettingsPanel();
            return this.settingsPanel;
        }

        public string Name => "DirectSound";

        public bool IsAvailable => isAvailable;

        public int Priority => 2;
    }
}
