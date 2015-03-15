using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Persistence.Svg
{
    public class Label
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public Point Point { get; set; }
        public Alignment Alignment { get; set; }
    }
}
