using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JJ.Framework.Presentation.WinForms.Forms;

namespace JJ.Presentation.QuestionAndAnswer.Import.WinForms
{
    internal partial class MainForm : SimpleFileProcessForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_OnRunProcess(object sender, EventArgs e)
        {
            ImportExecutor.RunAllImportsFromConfiguration(
                progressCallback: (message) => ShowProgress(message),
                isCancelledCallback: () => !IsRunning);
        }
    }
}
