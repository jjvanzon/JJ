﻿using System.Diagnostics;
using JJ.Framework.Exceptions;
using JJ.Framework.Presentation.VectorGraphics.Models.Styling;

namespace JJ.Framework.Presentation.VectorGraphics.Models.Elements
{
	public class EllipseStyle
	{
		internal EllipseStyle()
		{ }

		private LineStyle _lineStyle = new LineStyle();
		/// <summary> not nullable, auto-instantiated </summary>
		public LineStyle LineStyle
		{
			[DebuggerHidden]
			get => _lineStyle;
			set => _lineStyle = value ?? throw new NullException(() => value);
		}

		private BackStyle _backStyle = new BackStyle();
		/// <summary> not nullable, auto-instantiated </summary>
		public BackStyle BackStyle
		{
			[DebuggerHidden]
			get => _backStyle;
			set => _backStyle = value ?? throw new NullException(() => value);
		}
	}
}