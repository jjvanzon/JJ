<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SmallLoginViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.ViewModels" %>

<% if (!Model.IsLoggedIn)
   { %>
        <a>Login</a>
<% }
   else
   { %>
        <%: Model.UserName %>

        <div id="smallLoginViewDropDown">
            Logout
        </div>
<% } %>

<%-- LOGIN / LOGOUT BUTTONS HERE --%>

