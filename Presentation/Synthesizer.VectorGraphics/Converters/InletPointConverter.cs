﻿using JJ.Framework.Presentation.VectorGraphics.Models.Elements;
using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.VectorGraphics.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Presentation.Synthesizer.VectorGraphics.Converters
{
	internal class InletPointConverter
	{
		private readonly Dictionary<int, Point> _destInletPointDictionary = new Dictionary<int, Point>();
		private readonly HashSet<Point> _destInletPointHashSet = new HashSet<Point>();

		public IList<Point> ConvertToInletPoints(OperatorViewModel sourceOperatorViewModel, Rectangle destOperatorRectangle)
		{
			if (sourceOperatorViewModel == null) throw new NullException(() => sourceOperatorViewModel);
			if (destOperatorRectangle == null) throw new NullException(() => destOperatorRectangle);

			IList<InletViewModel> sourceInletViewModelsToConvert = sourceOperatorViewModel.Inlets
																						  .Where(inlet => inlet.Visible)
																						  .ToArray();
			if (sourceInletViewModelsToConvert.Count == 0)
			{
				return new Point[0];
			}

			IList<Point> destInletPoints = new List<Point>(sourceInletViewModelsToConvert.Count);

			float inletWidth = destOperatorRectangle.Position.Width / sourceInletViewModelsToConvert.Count;
			float x = 0;

			foreach (InletViewModel sourceInletViewModel in sourceInletViewModelsToConvert)
			{
				Point destInletPoint = ConvertToInletPoint(sourceInletViewModel, destOperatorRectangle);

				destInletPoint.Position.X = x + inletWidth / 2f;
				destInletPoint.Position.Y = 0;

				destInletPoints.Add(destInletPoint);

				x += inletWidth;
			}

			return destInletPoints;
		}

		/// <summary> Converts everything but its coordinates. </summary>
		private Point ConvertToInletPoint(InletViewModel sourceInletViewModel, Rectangle destOperatorRectangle)
		{
			int inletID = sourceInletViewModel.ID;

			Point destInletPoint;
			if (!_destInletPointDictionary.TryGetValue(inletID, out destInletPoint))
			{
				destInletPoint = new Point
				{
					Diagram = destOperatorRectangle.Diagram,
					Parent = destOperatorRectangle,
					Tag = VectorGraphicsTagHelper.GetInletTag(inletID)
				};

				_destInletPointDictionary.Add(inletID, destInletPoint);
				_destInletPointHashSet.Add(destInletPoint);
			}

			if (sourceInletViewModel.HasWarningAppearance)
			{
				destInletPoint.PointStyle = StyleHelper.PointStyleWarning;
			}
			else
			{
				destInletPoint.PointStyle = StyleHelper.PointStyle;
			}

			return destInletPoint;
		}

		public void TryRemove(Point destElement)
		{
			if (_destInletPointHashSet.Contains(destElement))
			{
				int inletID = VectorGraphicsTagHelper.GetInletID(destElement.Tag);

				_destInletPointDictionary.Remove(inletID);
				_destInletPointHashSet.Remove(destElement);

				destElement.Children.Clear();
				destElement.Parent = null;
				destElement.Diagram = null;
			}
		}
	}
}