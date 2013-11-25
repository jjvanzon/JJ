using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JJ.Framework.Persistence;
using JJ.Framework.Presentation.WinForms;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex.Processing;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex.Enums;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex
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
                var process = new ImportProcess(
                    new QuestionRepository(context, context.Location),
                    new AnswerRepository(context),
                    new CategoryRepository(context),
                    new QuestionCategoryRepository(context),
                    new QuestionLinkRepository (context),
                    new QuestionTypeRepository(context),
                    new SourceRepository(context));

                process.Execute(
                    FilePath,
                    ImportTypeEnum.Html,
                    includeAnswersThatAreReferences: true,
                    progressCallback: (message) => ShowProgress(message),
                    isCancelledCallback: () => !IsRunning);

                context.Commit();
            }
        }
    }
}
