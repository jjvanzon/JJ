using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace NAudioDemo.SimplePlaybackDemo
{
    [Export(typeof(INAudioDemoPlugin))]
    class SimplePlaybackPlugin : INAudioDemoPlugin
    {
        public string Name
        {
            get { return "Simple Playback"; }
        }

        public Control CreatePanel()
        {
            return new SimplePlaybackPanel();
        }
    }
}
