using JJ.Framework.Persistence;
using JJ.Framework.Presentation.WinForms;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Enums;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex
{
    public partial class MainForm : SimpleFileProcessForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_OnRunProcess(object sender, EventArgs e)
        {
            using (IContext context = ContextHelper.CreateContext())
            {
                var process = new ImportProcess(context);

                process.Execute(
                    FilePath,
                    ImportTypeEnum.Html,
                    progressCallback: (message) => ShowProgress(message),
                    isCancelledCallback: () => !IsRunning);
            }
        }
    }
}
