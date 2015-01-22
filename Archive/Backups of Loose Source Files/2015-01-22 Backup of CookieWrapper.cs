//using JJ.Apps.QuestionAndAnswer.Mvc.Names;
//using JJ.Framework.Reflection;
//using JJ.Framework.Web;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace JJ.Apps.QuestionAndAnswer.Mvc.Helpers
//{
//    public class CookieWrapper
//    {
//        private HttpContextBase _httpContext;

//        public CookieWrapper(HttpContextBase httpContext)
//        {
//            _httpContext = httpContext;
//        }

//        public string CultureName
//        {
//            get { return CookieHelper.GetCookieValue(_httpContext.Request, CookieNames.cultureName); }
//            set { CookieHelper.SetCookieValue(_httpContext.Response, CookieNames.cultureName, value); }
//        }
//    }
//}
