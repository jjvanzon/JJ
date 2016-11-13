﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace JJ.Presentation.Synthesizer.VectorGraphics.Configuration
{
    public class ConfigurationSection
    {
        [XmlAttribute]
        public bool MustShowInvisibleElements { get; set; }

        [XmlAttribute]
        public int NodeClickableRegionSizeInPixels { get; set; }

        [XmlAttribute]
        public int PatchLineSegmentCount { get; set; }

        [XmlAttribute]
        public int CurveLineSegmentCount { get; set; }
    }
}