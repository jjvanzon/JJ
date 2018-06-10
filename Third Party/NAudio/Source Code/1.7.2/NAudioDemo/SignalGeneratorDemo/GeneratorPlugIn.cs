using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace NAudioDemo.Generator
{
    [Export(typeof (INAudioDemoPlugin))]
    internal class GeneratorPlugin : INAudioDemoPlugin
    {
        public string Name => "Signal Generator";

        public Control CreatePanel() => new GeneratorPanel();
    }
}
