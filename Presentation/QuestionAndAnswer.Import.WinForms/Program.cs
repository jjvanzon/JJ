using System;
using System.Windows.Forms;
using JJ.Framework.WinForms.Helpers;

namespace JJ.Presentation.QuestionAndAnswer.Import.WinForms
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // Message box for unhandled exceptions.
            UnhandledExceptionMessageBoxShower.Initialize(Resources.ApplicationName);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
