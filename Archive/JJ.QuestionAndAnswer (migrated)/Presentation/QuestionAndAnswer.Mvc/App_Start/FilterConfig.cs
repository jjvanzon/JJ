using System.Web.Mvc;
using JJ.Framework.Mvc;

namespace JJ.Presentation.QuestionAndAnswer.Mvc
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) => filters.Add(new BasicHandleErrorAttribute());
    }
}