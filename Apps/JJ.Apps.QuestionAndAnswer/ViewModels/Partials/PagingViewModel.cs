using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Partials
{
    public class PagingViewModel
    {
        public int PageCount { get; set; }

        /// <summary>
        /// 1-based
        /// </summary>
        public int PageIndex { get; set; }

        public bool CanGoToFirstPage { get; set; }
        public bool CanGoToPreviousPage { get; set; }
        public bool CanGoToNextPage { get; set; }
        public bool CanGoToLastPage { get; set; }
    }
}
