﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Framework.Presentation.WinForms
{
    public class OnRunProcessEventArgs : EventArgs
    {
        public string FilePath { get; set; }

        public OnRunProcessEventArgs()
        { }

        public OnRunProcessEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
