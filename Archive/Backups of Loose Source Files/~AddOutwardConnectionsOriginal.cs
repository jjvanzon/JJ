//
//  Circle.Diagram.Engine.AddOutwardConnections
//
//      Author: Jan-Joost van Zon
//      Date: 2010-07-25 - 2011-02-22
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Diagram.Entities;

namespace Circle.Diagram.Engine
{
    /// <summary>
    /// The procedure does the following:
    ///      The right imaginary shapes are already there.
    ///      This procedure only connects all the shapes together.
    ///      - Recursively do it for all real shapes
    ///          - Do it for all imaginary shapes inside a real shape
    ///              - Go by all descendants, searching for real or imaginary shapes to connect to 
    ///                  - Connect to all real and imaginary shapes
    ///                  - But if you connected to an imaginary one
    ///                    cut off the recursive branch there,
    ///                    because that work will be done later in the procedure.
    /// 
    /// </summary>
    public class AddOutwardConnectionsOriginal : StepWithDiagram
    {
        /// <summary>
        /// Clear is used to be able to repeat the process of adding outward connections and lines
        /// after other processes have modified the configuation of real and imaginary shapes.
        /// </summary>
        public void Clear()
        {
            foreach (Shape shape in Diagram.Shapes)
            {
                shape.ShapePointedAt = null;
                shape.ConnectedShapes.Clear();
            }
        }

        protected override void OnExecute()
        {
            Execute(Diagram.MainRealShape);
            Debug();
        }

        private void Execute(Shape element)
        {
            // Go by all imaginary shapes
            foreach (Shape imaginaryShape in element.Children.Where(x => x.IsImaginary || x.UsedAsImaginary))
            {
                // Scan the descendant tree for connections to make
                Scan(imaginaryShape, element);
            }
            // Recursively do it for all descendants
            foreach (Shape child in element.Children)
            {
                Execute(child);
            }
        }

        /// <returns>Returns whether to stop the whole scanning procedure.</returns>
        private bool Scan(Shape higherImaginaryShape, Shape parent)
        {
            // Look for children to connect to
            foreach (Shape child in parent.Children.OrderByDescending(x => x.IsImaginary || x.UsedAsImaginary)) // ChildrenImaginaryFirst
            {
                // Debugging
                Debug(higherImaginaryShape, child);
                // Skip imaginary shape itself
                if (ReferenceEquals(higherImaginaryShape, child)) { goto Continue; }
                // Skip items that do not match
                if (!ReferenceEquals(child.Item, higherImaginaryShape.Item)) { goto Continue; }
                // Connect deeper shape to higher imaginary shape
                higherImaginaryShape.ConnectedShapes.Add(child);
                child.ShapePointedAt = higherImaginaryShape;
                // Stop the branch there if it is an imaginary shape
                if (child.IsImaginary || child.UsedAsImaginary)
                {
                    return false;
                    // continue;
                    // But the other braches of the recursion will continue
                    // It stops, because it ended at an imaginary shape and
                    // the rest will be done when adding connections to this imaginary shape.
                }
            Continue:
                // Recursively call all children
                Scan(higherImaginaryShape, child);
            }
            // Return success
            return true;
        }

        // Debugging

        public bool DoDebug = false;

        public event EventHandler OnDebug;

        private void Debug(
            Shape currentShape = null, 
            Shape secondaryCurrentShape = null)
        {
            if (!DoDebug) return;

            Diagram.CurrentShape = currentShape;
            Diagram.SecondaryCurrentShape = secondaryCurrentShape;
            if (OnDebug != null) { OnDebug(this, new EventArgs()); }
        }
    }
}
