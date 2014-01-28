using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels
{
    public class ListViewModel<T> : List<T>
    {
        /// <summary>
        /// Indicates whether items were added or removed and possibly if they changed order,
        /// but says nothing about the item's contents being changed.
        /// </summary>
        internal bool IsDirty { get; set; }
    }
}
