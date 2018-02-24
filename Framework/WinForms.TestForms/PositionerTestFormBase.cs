﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JJ.Framework.Collections;
using JJ.Framework.Mathematics;
using JJ.Framework.VectorGraphics.Helpers;
using JJ.Framework.VectorGraphics.Models.Elements;
using JJ.Framework.VectorGraphics.Models.Styling;
using JJ.Framework.VectorGraphics.Positioners;

namespace JJ.Framework.WinForms.TestForms
{
	public partial class PositionerTestFormBase : Form
	{
		protected const float ROW_HEIGHT = 24;
		protected readonly IList<float> _itemWidths;

		private const int MIN_ITEM_COUNT = 7;
		private const int MAX_ITEM_COUNT = 20;
		private const float MIN_ITEM_WIDTH = 8;
		private const float MAX_ITEM_WIDTH = 120;

		private readonly int _itemCount;
		private readonly IList<Rectangle> _rectangles;

		public PositionerTestFormBase()
		{
			_itemCount = Randomizer.GetInt32(MIN_ITEM_COUNT, MAX_ITEM_COUNT);
			_itemWidths = GenerateItemWidths(_itemCount);
			_rectangles = new List<Rectangle>(_itemCount);

			var diagram = new Diagram();

			for (int i = 0; i < _itemCount; i++)
			{
				var rectangle = new Rectangle(diagram.Background);
				_rectangles.Add(rectangle);
			}

			InitializeComponent();

			var backStyle = new BackStyle
			{
				Color = ColorHelper.SetOpacity(ColorHelper.Black, 128)
			};
			_rectangles.ForEach(x => x.Style.BackStyle = backStyle);

			diagramControl.Diagram = diagram;
			diagramControl.Left = 0;
			diagramControl.Top = 0;

			PositionControls();
		}

		private void PositionControls()
		{
			diagramControl.Width = ClientSize.Width;
			diagramControl.Height = ClientSize.Height;

			IPositioner positioner = CreatePositioner();
			IList<(float x, float y, float width, float height)> positions = positioner.Calculate();

			for (int i = 0; i < _itemCount; i++)
			{
				(float x, float y, float width, float height) = positions[i];

				Rectangle rectangle = _rectangles[i];
				rectangle.Position.X = x;
				rectangle.Position.Y = y;
				rectangle.Position.Width = width;
				rectangle.Position.Height = height;
			}
		}

		protected virtual IPositioner CreatePositioner() => throw new NotImplementedException();

		private IList<float> GenerateItemWidths(int itemCount)
		{
			var itemWidths = new float[itemCount];

			for (int i = 0; i < itemCount; i++)
			{
				float itemWidth = Randomizer.GetSingle(MIN_ITEM_WIDTH, MAX_ITEM_WIDTH);
				itemWidths[i] = itemWidth;
			}

			return itemWidths;
		}

		private void Base_Resize(object sender, EventArgs e) => PositionControls();
		private void Base_SizeChanged(object sender, EventArgs e) => PositionControls();
	}
}
