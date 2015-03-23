﻿using JJ.Framework.Presentation.Svg.Models;
using JJ.Framework.Presentation.Svg.Models.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.Visitors
{
    public abstract class ElementVisitorBase
    {
        protected virtual void VisitPolymorphic(ElementBase element)
        {
            var point = element as Point;
            if (point != null)
            {
                VisitPoint(point);
                return;
            }

            var line = element as Line;
            if (line != null)
            {
                VisitLine(line);
                return;
            }

            var rectangle = element as Rectangle;
            if (rectangle != null)
            {
                VisitRectangle(rectangle);
                return;
            }

            var label = element as Label;
            if (label != null)
            {
                VisitLabel(label);
                return;
            }

            throw new Exception(String.Format("Unexpected ElementBase type '{0}'", element.GetType().FullName));
        }

        protected virtual void VisitPoint(Point point)
        {
            VisitChildren(point);
        }

        protected virtual void VisitLine(Line line)
        {
            VisitChildren(line);
        }

        protected virtual void VisitRectangle(Rectangle rectangle)
        {
            VisitChildren(rectangle);
        }

        protected virtual void VisitLabel(Label label)
        {
            VisitChildren(label);
        }

        protected virtual void VisitChildren(ElementBase parentElement)
        {
            foreach (Point point in parentElement.ChildPoints)
            {
                VisitPolymorphic(point);
            }

            foreach (Rectangle rectangle in parentElement.ChildRectangles)
            {
                VisitPolymorphic(rectangle);
            }

            // Force Z-Index problem in our test by drawing rectangles before lines.
            foreach (Line line in parentElement.ChildLines)
            {
                VisitPolymorphic(line);
            }

            foreach (Label label in parentElement.ChildLabels)
            {
                VisitPolymorphic(label);
            }
        }
    }
}
