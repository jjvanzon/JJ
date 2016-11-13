﻿using JJ.Presentation.Synthesizer.Helpers;
using System.Diagnostics;
using JJ.Data.Canonical;

namespace JJ.Presentation.Synthesizer.ViewModels.Items
{
    /// <summary>
    /// NOTE: This is one of the few view models with an inverse property.
    /// OutletViewModel.Operator <=> Operator.Outlets
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public sealed class OutletViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }

        /// <summary> Some inlets that have to be stored, might not have to be shown. </summary>
        public bool Visible { get; set; }

        /// <summary> Does not necessarily need to be displayed, but does need to be passed around, so it is present when a new object is saved. </summary>
        public int ListIndex { get; set; }

        public bool HasWarningAppearance { get; set; }
        public float? AverageConnectionDistance { get; set; }

        /// <summary>
        /// NOTE: This property has an inverse property
        /// OutletViewModel.Operator <=> Operator.Outlets
        /// </summary>
        public OperatorViewModel Operator { get; set; }

        // NOTE:
        // See InletViewModel for the comment that also applies here.

        /// <summary> Read-only, nullable, not necessarily displayed. </summary>
        public IDAndName Dimension { get; set; }

        private string DebuggerDisplay
        {
            get { return DebugHelper.GetDebuggerDisplay(this); }
        }
    }
}