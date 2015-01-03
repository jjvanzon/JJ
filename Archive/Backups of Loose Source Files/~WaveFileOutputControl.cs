// 2012-03-24 - 2012-03-24

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Circle.AppsAndMedia.Sound.Client.Runner
{
    public partial class WaveFileOutputControl : UserControl
    {
        public WaveFileOutputControl()
        {
            InitializeComponent();
        }

        private WaveFileOutput _waveFileOutput;
        public WaveFileOutput WaveFileOutput
        {
            get { return _waveFileOutput; }
            set
            {
                if (_waveFileOutput == value) return;
                _waveFileOutput = value;
                ApplyToNameLabel();
                ApplyToFilePathTextBox();
            }
        }

        public string FilePath
        {
            get
            {
                if (WaveFileOutput == null) return null;
                return WaveFileOutput.FilePath;
            }
            set
            {
                if (WaveFileOutput != null)
                {
                    WaveFileOutput.FilePath = value;
                    ApplyToFilePathTextBox();
                }
            }
        }

        private void ApplyToNameLabel()
        {
            if (WaveFileOutput != null && !String.IsNullOrEmpty(WaveFileOutput.Name))
            {
                NameLabel.Text = WaveFileOutput.Name;
            }
            else
            {
                NameLabel.Text = null;
            }
        }

        private void ApplyToFilePathTextBox()
        {
            if (WaveFileOutput != null)
            {
                FilePathTextBox.Text = WaveFileOutput.FilePath;
            }
            else
            {
                FilePathTextBox.Text = null;
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog.FileName = FilePath;
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = SaveFileDialog.FileName;
            }
        }
    }
}
