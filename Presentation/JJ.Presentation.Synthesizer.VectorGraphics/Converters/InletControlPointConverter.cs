﻿using JJ.Framework.Presentation.VectorGraphics.Models.Elements;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.VectorGraphics.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Presentation.Synthesizer.VectorGraphics.Converters
{
    internal class InletControlPointConverter
    {
        private const float CONTROL_POINT_DISTANCE = 50;

        private readonly Dictionary<int, Point> _destInletControlPointDictionary = new Dictionary<int, Point>();

        public IList<Point> ConvertToInletControlPoints(IList<Point> sourceInletPoints)
        {
            if (sourceInletPoints == null) throw new NullException(() => sourceInletPoints);

            IList<Point> destOutletControlPoints = sourceInletPoints.Select(x => ConvertPoint(x)).ToList();

            return destOutletControlPoints;
        }

        private Point ConvertPoint(Point sourceInletPoint)
        {
            int inletID = VectorGraphicsTagHelper.GetInletID(sourceInletPoint.Tag);

            Point destInletControlPoint;
            if (!_destInletControlPointDictionary.TryGetValue(inletID, out destInletControlPoint))
            {
                destInletControlPoint = new Point
                {
                    Diagram = sourceInletPoint.Diagram,
                    Parent = sourceInletPoint.Parent,
                    PointStyle = StyleHelper.PointStyleInvisible,
                    Tag = sourceInletPoint.Tag
                };

                _destInletControlPointDictionary.Add(inletID, destInletControlPoint);
            }

            destInletControlPoint.Position.X = sourceInletPoint.Position.X;
            destInletControlPoint.Position.Y = sourceInletPoint.Position.Y - CONTROL_POINT_DISTANCE;

            return destInletControlPoint;
        }

        public void TryRemove(int inletID)
        {
            Point destElement;
            if (_destInletControlPointDictionary.TryGetValue(inletID, out destElement))
            {
                _destInletControlPointDictionary.Remove(inletID);

                destElement.Children.Clear();
                destElement.Parent = null;
                destElement.Diagram = null;
            }
        }
    }
}