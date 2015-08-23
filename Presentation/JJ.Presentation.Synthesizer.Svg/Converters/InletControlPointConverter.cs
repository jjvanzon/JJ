﻿using JJ.Framework.Presentation.Svg.Models.Elements;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Svg.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.Svg.Converters
{
    internal class InletControlPointConverter
    {
        private const float CONTROL_POINT_DISTANCE = 50;

        public IList<Point> ConvertToInletControlPoints(IList<Point> sourceInletPoints)
        {
            if (sourceInletPoints == null) throw new NullException(() => sourceInletPoints);

            IList<Point> destOutletControlPoints = sourceInletPoints.Select(x => ConvertPoint(x)).ToList();

            return destOutletControlPoints;
        }

        private Point ConvertPoint(Point sourceInletPoint)
        {
            var destInletControlPoint = new Point
            {
                Diagram = sourceInletPoint.Diagram,
                Parent = sourceInletPoint.Parent,
                X = sourceInletPoint.X,
                Y = sourceInletPoint.Y - CONTROL_POINT_DISTANCE,
                PointStyle = StyleHelper.PointStyleInvisible
            };

            return destInletControlPoint;
        }
    }
}