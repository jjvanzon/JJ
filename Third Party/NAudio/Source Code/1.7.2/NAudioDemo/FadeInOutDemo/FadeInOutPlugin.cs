using System.ComponentModel.Composition;

namespace NAudioDemo.FadeInOutDemo
{
    [Export(typeof(INAudioDemoPlugin))]
    class FadeInOutPlugin : INAudioDemoPlugin
    {
        public string Name => "Fade In Out";

        public System.Windows.Forms.Control CreatePanel() => new FadeInOutPanel();
    }
}
