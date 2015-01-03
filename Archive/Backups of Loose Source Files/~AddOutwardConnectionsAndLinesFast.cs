//
//  Circle.Diagram.Engine.AddOutwardConnectionsAndLines
//
//      Author: Jan-Joost van Zon
//      Date: 2011-02-22 - 2011-02-22
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Diagram.Entities;
using Circle.CodeBase;

namespace Circle.Diagram.Engine
{
    public class AddOutwardConnectionsAndLinesFast : StepWithDiagram
    {
        /// <summary>
        /// Clear is used to be able to repeat the process of adding outward connections and lines
        /// after other processes have modified the configuation of real and imaginary shapes.
        /// </summary>
        public void Clear()
        {
            foreach (Shape imaginaryShape in Diagram.Shapes)
            {
                imaginaryShape.ShapePointedAt = null;
                imaginaryShape.ConnectedShapes.Clear();
                imaginaryShape.ConnectedLines.Clear();
                imaginaryShape.LinePointingAway = null;
            }
            Diagram.Lines.Clear();
        }

        /// <summary>
        /// Instead of recursively going by all shapes,
        /// and then recursively scanning all sibblings and their descendants for shapes to connect to,
        /// a single sorting query puts them in the order to process them,
        /// without having to scan around.
        /// </summary>
        protected override void OnExecute()
        {
            var query = 
                from shape in Diagram.Shapes
                orderby 
                    shape.Item.GetHashCode(),
                    shape.Layer,
                    shape.UsedAsImaginary descending,
                    shape.IsImaginary descending
                select shape;

            Item item = null;
            int layer = -1;
            Shape imaginary = null;

            foreach (Shape shape in query)
            {
                // Next Item
                if (!ReferenceEquals(item, shape.Item)) 
                {
                    item = shape.Item;
                    layer = shape.Layer;
                    imaginary = shape;
                }

                // Next Layer
                if (shape.Layer > layer + 1)
                {
                    layer = shape.Layer;
                    imaginary = shape;
                }

                if (ReferenceEquals(imaginary, shape)) continue;

                // Connect this shape to imaginary counterpart
                Connect(shape, imaginary);
            }
        }

        private void Connect(Shape shape, Shape imaginary)
        {
            Line line = new Line();
            Diagram.Lines.Add(line);
            imaginary.ConnectedShapes.Add(shape);
            imaginary.ConnectedLines.Add(line);
            line.ShapePointedAt = imaginary;
            line.ShapePointing = shape;
            shape.ShapePointedAt = imaginary;
            shape.LinePointingAway = line;

            Debug(imaginary, shape, line);
        }

        // Debugging

        public bool DoDebug = false;

        public event EventHandler OnDebug;

        private void Debug(
            Shape currentShape = null, 
            Shape secondaryCurrentShape = null, 
            Line currentLine = null)
        {
            if (!DoDebug) return;

            Diagram.CurrentShape = currentShape;
            Diagram.SecondaryCurrentShape = secondaryCurrentShape;
            Diagram.CurrentLine = currentLine;
            if (OnDebug != null) { OnDebug(this, new EventArgs()); }
        }
    }
}
