using JJ.Business.Svg.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Svg.Gestures
{
    public interface IGesture
    {
        void MouseDown(MouseDownInfo info);
        void MouseMove(MouseDownInfo info);
        void MouseUp(MouseDownInfo info);
    }
}
