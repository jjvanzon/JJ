using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Circle.Integration.DotNet;

namespace Circle.Client.WinForms
{
    /// <summary>
    /// A window to offer tools and tests on the fly.
    /// </summary>
    public partial class ToolsWindow : Form
    {
        public ToolsWindow()
        {
            InitializeComponent();
            Utilities.ApplyStyling(this);
            Utilities.AutomaticTabIndex(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GacList gacList = new GacList();
            gacList.Load();
        }
    }
}
