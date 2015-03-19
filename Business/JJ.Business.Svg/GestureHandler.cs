using JJ.Business.Svg.EventArgs;
using JJ.Business.Svg.Gestures;
using JJ.Business.Svg.Infos;
using JJ.Persistence.Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Svg
{
    public class GestureHandler
    {
        private IList<IGesture> _gestures;

        public event EventHandler<GestureEventArgs> OnGesture;

        // TODO: Some sort of wrapper model instead of a bunch of collections?
        public GestureHandler(
            IList<Rectangle> rectangles, IList<Line> lines, IList<Point> points, 
            IList<IGesture> gestures)
        {
            _gestures = gestures;

            throw new NotImplementedException();
        }

        public void MouseDown(MouseDownInfo info)
        {
            throw new NotImplementedException();
        }

        public void MouseMove(MouseMoveInfo info)
        {
            throw new NotImplementedException();
        }

        public void MouseUp(MouseUpInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
