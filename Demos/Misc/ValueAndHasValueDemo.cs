using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Demos.Misc
{
    internal class ValueAndHasValueDemo
    {
        int? number;

        private void Recommended()
        {
            if (number.HasValue)
            {
                string message = $"Number = {number.Value}";
            }
        }

        private void LessPreferred()
        {
            if (number != null)
            {
                string message = $"Number = {number}");
            }
        }

    }
}
