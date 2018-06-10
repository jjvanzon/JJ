using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace NAudioDemo.SimplePlaybackDemo
{
    [Export(typeof(INAudioDemoPlugin))]
    class SimplePlaybackPlugin : INAudioDemoPlugin
    {
        public string Name => "Simple Playback";

        public Control CreatePanel() => new SimplePlaybackPanel();
    }
}
