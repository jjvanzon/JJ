﻿using JJ.Data.Canonical;
using System.Collections.Generic;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class OperatorPropertiesViewModel_WithDimensionAndInterpolation : OperatorPropertiesViewModelBase
    {
        public IDAndName OperatorType { get; set; }
        public IDAndName Interpolation { get; set; }
        public IList<IDAndName> InterpolationLookup { get; set; }
        public IDAndName Dimension { get; set; }
        public IList<IDAndName> DimensionLookup { get; set; }
    }
}