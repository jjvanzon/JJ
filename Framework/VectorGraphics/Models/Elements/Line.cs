﻿using System;
using System.Diagnostics;
using JJ.Framework.VectorGraphics.Helpers;
using JJ.Framework.VectorGraphics.Models.Styling;

namespace JJ.Framework.VectorGraphics.Models.Elements
{
	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
	public class Line : Element
	{
		public Line() => Position = new LinePosition(this);

		public override ElementPosition Position { get; }

		/// <summary> Nullable. Coordinates of the point are related to the Point's parent. </summary>
		public Point PointA { get; set; }

		/// <summary> Nullable. Coordinates of the point are related to the Point's parent. </summary>
		public Point PointB { get; set; }

		private LineStyle _lineStyle = new LineStyle();
		/// <summary> not nullable, auto-instantiated </summary>
		public LineStyle LineStyle
		{
			[DebuggerHidden]
			get => _lineStyle;
			set => _lineStyle = value ?? throw new ArgumentNullException(nameof(LineStyle));
		}

		private string DebuggerDisplay => DebuggerDisplayFormatter.GetDebuggerDisplay(this);
	}
}
