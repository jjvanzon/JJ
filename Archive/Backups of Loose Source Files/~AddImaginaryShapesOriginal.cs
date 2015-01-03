//
//  Circle.Diagram.Engine.AddImaginaryShapes
//
//      Author: Jan-Joost van Zon
//      Date: 2010-07-25 - 2011-02-09
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using Circle.CodeBase;
using Circle.Diagram.Entities;

namespace Circle.Diagram.Engine
{
    public class AddImaginaryShapesOriginal : StepWithDiagram
    {
        protected override void OnExecute()
        {
            Clear();
            AddAll(Diagram.MainRealShape);
            RemoveWithOneOrZeroReferences(Diagram.MainRealShape);
            RemoveWithSameCountAsDeeper(Diagram.MainRealShape);
            Debug();
        }

        /// <summary>
        /// This Clear method only resets reference counts to 0.
        /// Reference counts will then be re-filled-in.
        /// Later, ones with reference count 0 or 1 will be removed, unless they have children
        /// And later also ones with the same reference count as one on a deeper level will be removed,
        /// unless they have children in it.
        /// </summary>
        private void Clear()
        {
            foreach(Shape imaginaryShape in Diagram.ImaginaryShapes)
            {
                imaginaryShape.RealShapeCount = 0;
            }
            Debug();
        }

        private void AddAll(Shape parent)
        {
            RecursivelyScan(higherParent: parent, parent: parent);
            foreach (Shape child in parent.Children)
            {
                // Recursive call
                AddAll(child);
            }
            Diagram.SecondaryCurrentShape = null;
        }

        private void RecursivelyScan(Shape higherParent, Shape parent)
        {
            Shape imaginaryShape;
            List<Shape> children = parent.Children.ToList();
            foreach (Shape child in children)
            {
                // Add imaginary item For object of descendent
                if (child.IsReal) // Only count real shapes
                {
                    if (child.Item != null)
                    {
                        // Add to RealShape
                        imaginaryShape = UpdateImaginaryShapeHistorygramForItem(higherParent, child.Item);
                        // Add to Diagram Data if not already present
                        if (Diagram.ImaginaryShapes.Find(ish => ReferenceEquals(ish, imaginaryShape)) == null)
                        {
                            Diagram.ImaginaryShapes.Add(imaginaryShape);
                        }
                        Debug(imaginaryShape, child);
                    }
                }
                RecursivelyScan(higherParent, child);
            }
        }

        private void RemoveWithOneOrZeroReferences(Shape parent)
        {
            // Create a copy of the list, because you can not remove items from the list you are looping through
            List<Shape> copyOfChildImaginaryShapesList = parent.Children.Where(x => x.IsImaginary).ToList();
            // Go through imaginary shapes
            foreach (Shape imaginaryShape in copyOfChildImaginaryShapesList)
            {
                if (imaginaryShape.Children.Count() == 0)
                {
                    // Remove this object's unneeded imaginary shapes
                    if (imaginaryShape.RealShapeCount <= 1)
                    {
                        Debug(imaginaryShape);
                        Diagram.ImaginaryShapes.Remove(imaginaryShape);
                        parent.Children.Remove(imaginaryShape);
                    }
                }
            }
            // Recursive call on all children
            foreach (Shape child in parent.Children)
            {
                RemoveWithOneOrZeroReferences(child);
            }
        }
        
        private void RemoveWithSameCountAsDeeper(Shape parent)
        {
            // Create a copy of the list, because you can not remove items from the list you are looping through
            List<Shape> copyOfChildImaginaryShapeList = parent.Children.Where(x => x.IsImaginary).ToList();
            // Go through imaginary shapes
            foreach (Shape higherImaginaryShape in copyOfChildImaginaryShapeList)
            {
                if (higherImaginaryShape.Children.Count() == 0)
                {
                    // Go through children
                    List<Shape> realChildren = parent.Children.Where(x => x.IsReal).ToList();
                    foreach (Shape childRealShape in realChildren)
                    {
                        // Find imaginary shape inside child that
                        // represents the same item and has the same amount of real shapes associated with it.
                        Shape foundImaginaryShape =
                            childRealShape.Children.Find(
                                ish => (
                                    ish.IsImaginary &&
                                    ReferenceEquals(ish.Item, higherImaginaryShape.Item) && // Item the same
                                    ish.RealShapeCount == higherImaginaryShape.RealShapeCount) // Same real shape count
                            );
                        if (foundImaginaryShape != null)
                        {
                            // If it is present remove the higher imaginary shape which has the same
                            // amount of associated real shapes.
                            Debug(higherImaginaryShape, foundImaginaryShape);
                            Diagram.ImaginaryShapes.Remove(higherImaginaryShape);
                            parent.Children.Remove(higherImaginaryShape);
                        }
                    }
                }
            }
            // Recursively call for child real shapes
            foreach (Shape child in parent.Children)
            {
                RemoveWithSameCountAsDeeper(child);
            }
        }

        // Helpers

        public Shape UpdateImaginaryShapeHistorygramForItem(Shape higherParent, Item item, int count = 1)
        {
            // Find
            Shape imaginaryShape = FindImaginaryShapeForItem(higherParent, item);
            // Add
            if (imaginaryShape == null)
            {
                imaginaryShape = new Shape() {
                    Item = item, 
                    Layer = higherParent.Layer + 1,
                    Parent = higherParent,
                    IsImaginary = true,
                    Aspect = Aspect.Object
                };
                higherParent.Children.Add(imaginaryShape);
            }
            // Add Real Shape to Imaginary Shape
            imaginaryShape.RealShapeCount += count;
            // Return
            return imaginaryShape;
        }

        private Shape FindImaginaryShapeForItem(Shape higherParent, Item item)
        {
            return higherParent.Children.Find(x => x.IsImaginary && ReferenceEquals(x.Item, item));
        }

        // Debugging

        public bool DoDebug = false;

        public event EventHandler OnDebug;

        private void Debug(Shape currentShape = null, Shape secondaryCurrentShape = null)
        {
            if (!DoDebug) return;

            Diagram.CurrentShape = currentShape;
            Diagram.SecondaryCurrentShape = secondaryCurrentShape;
            if (OnDebug != null) { this.OnDebug(this, new EventArgs()); }
        }
    }
}
