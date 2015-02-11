using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JJ.Framework.Persistence;
using JJ.Framework.Presentation.WinForms;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories;
using JJ.Framework.Configuration;

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
