﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JJ.Framework.Presentation.AspNetMvc4
{
    public static class UrlHelpers // Plural not to conflict with 'UrlHelper'.
    {
        public static string GetUrlWithCollectionParameter<T>(string actionName, string controllerName, string parameterName, IEnumerable<T> collection)
        {
            // First URL-encode everything.
            actionName = HttpUtility.UrlEncode(actionName);
            controllerName = HttpUtility.UrlEncode(controllerName);
            parameterName = HttpUtility.UrlEncode(parameterName);

            var values = new List<string>();
            foreach (var x in collection)
            {
                string str = Convert.ToString(x);
                string value = HttpUtility.UrlEncode(str);
                values.Add(value);
            }

            // Build the URL parameter string.
            string parameterString = "";
            if (collection.Count() != 0)
            {
                parameterString = "?" + parameterName + "=" + String.Join("&" + parameterName + "=", values);
            }

            // Build the URL.
            string url = "/" + controllerName + "/" + actionName + parameterString;

            return url;
        }

        public static string GetUrl(string actionName, string controllerName, ICollection<KeyValuePair<string, object>> parameters = null)
        {
            // First HTML-encode all elements of the url, for safety.
            actionName = HttpUtility.HtmlEncode(actionName);
            controllerName = HttpUtility.HtmlEncode(controllerName);

            // Build the URL parameter string.
            string parametersString = "";
            if (parameters != null && parameters.Count != 0)
            {
                var list = new List<string>();
                foreach (var entry in parameters)
                {
                    string name = HttpUtility.UrlEncode(entry.Key);
                    string value =  HttpUtility.UrlEncode(Convert.ToString(entry.Value));
                    string str = name + "=" + value;
                    list.Add(str);
                }

                parametersString = "?" + String.Join("&", list);
            }

            // Build the URL.
            string url = "/" + controllerName + "/" + actionName + parametersString;

            return url;
        }
    }
}
