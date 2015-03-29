﻿using JJ.Framework.Presentation.Svg.Models;
using JJ.Framework.Presentation.Svg.Models.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.Visitors
{
    // public for use in tests.
    public abstract class ElementVisitorBase
    {
        protected virtual void VisitPolymorphic(Element element)
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

            throw new Exception(String.Format("Unexpected Element type '{0}'", element.GetType().FullName));
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

        protected virtual void VisitChildren(Element parentElement)
        {
            foreach (Element child in parentElement.Children)
            {
                VisitPolymorphic(child);
            }
        }
    }
}
