using NAudio.Wave;
using System.Windows.Forms;
using System.ComponentModel.Composition;

namespace NAudioDemo.AudioPlaybackDemo
{
    [Export(typeof(IOutputDevicePlugin))]
    class AsioOutPlugin : IOutputDevicePlugin
    {
        AsioOutSettingsPanel settingsPanel;

        public IWavePlayer CreateDevice(int latency) => new AsioOut(settingsPanel.SelectedDeviceName);

        public UserControl CreateSettingsPanel()
        {
            this.settingsPanel = new AsioOutSettingsPanel();
            return settingsPanel;
        }

        public string Name => "AsioOut";

        public bool IsAvailable => AsioOut.isSupported();

        public int Priority => 4;
    }
}
