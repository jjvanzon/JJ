using System;
using JJ.Framework.WinForms.Forms;

namespace JJ.Presentation.QuestionAndAnswer.Import.WinForms
{
    internal partial class MainForm : SimpleProcessForm
    {
        public MainForm()
        {
            InitializeComponent();

            Text = Resources.ApplicationName;
        }

        private void MainForm_OnRunProcess(object sender, EventArgs e) => ImportExecutor.RunAllImportsFromConfiguration(
            progressCallback: ShowProgress,
            isCancelledCallback: () => !IsRunning);
    }
}