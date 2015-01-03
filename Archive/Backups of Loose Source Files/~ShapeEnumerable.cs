//
//  Circle.Diagram.Entities.ShapeEnumerable
//
//      Author: Jan-Joost van Zon
//      Date: 2010-10-19 - 2010-10-19
//
//  -----

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Diagram.Entities
{
    public class ShapeEnumerable : IEnumerable<Shape>
    {
        public bool ImaginaryShapesFirst = false;

        public List<Shape> RealShapes;
        public List<Shape> ImaginaryShapes;

        // IEnumerable

        public IEnumerator<Shape> GetEnumerator()
        {
            if (ImaginaryShapesFirst)
            {
                foreach (Shape imaginaryShape in ImaginaryShapes)
                {
                    yield return imaginaryShape;
                }
                foreach (Shape realShape in RealShapes)
                {
                    yield return realShape;
                }
            }
            else
            {
                foreach (Shape realShape in RealShapes)
                {
                    yield return realShape;
                }
                foreach (Shape imaginaryShape in ImaginaryShapes)
                {
                    yield return imaginaryShape;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
