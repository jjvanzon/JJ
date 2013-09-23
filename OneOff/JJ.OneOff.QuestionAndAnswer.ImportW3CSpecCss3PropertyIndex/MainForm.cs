using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            _isRunning = false;
            ApplyIsRunning();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = _isRunning;
        }

        private void Start()
        {
            if (MessageBox.Show("Are you sure?", null, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RunProcessAsync(textBoxFilePath.Text);
            }
        }

        private void RunProcessAsync(string filePath)
        {
            Async(() => RunProcess(filePath));
        }

        private void RunProcess(string filePath)
        {
            _isRunning = true;

            ApplyIsRunningAsync();

            using (IContext context = ContextHelper.CreateContext())
            {
                var process = new ImportProcess(context);
                process.Execute(filePath, progressCallback: (message) => ShowProgresAsync(message), isCancelledCallback: () => !_isRunning);
            }

            _isRunning = false;

            ApplyIsRunningAsync();
        }

        private void ShowProgresAsync(string message)
        {
            OnUiThread(() => ShowProgress(message));
        }

        private void ShowProgress(string message)
        {
            labelProgress.Text = message;
        }

        private void Cancel()
        {
            _isRunning = false;
            ApplyIsRunning();
        }

        private void OnUiThread(Action action)
        {
            this.BeginInvoke(action);
        }

        private void Async(Action action)
        {
            var thread = new Thread(new ThreadStart(action));
            thread.Start();
        }

        private volatile bool _isRunning;

        private void ApplyIsRunningAsync()
        {
            OnUiThread(() => ApplyIsRunning());
        }

        private void ApplyIsRunning()
        {
            buttonStart.Enabled = !_isRunning;
            buttonCancel.Enabled = _isRunning;
        }
    }
}
