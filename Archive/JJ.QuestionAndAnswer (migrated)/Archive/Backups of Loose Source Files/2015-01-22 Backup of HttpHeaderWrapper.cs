using JJ.Apps.QuestionAndAnswer.Mvc.Names;
using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Helpers
{
    public class HttpHeaderWrapper
    {
        private NameValueCollection _httpHeader;

        public HttpHeaderWrapper(NameValueCollection httpHeader)
        {
            if (httpHeader == null) throw new NullException(() => httpHeader);
            _httpHeader = httpHeader;
        }

        public string AcceptLanguage
        {
            get { return _httpHeader[HttpHeaderNames.AcceptLanguage]; }
        }
    }
}