<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LoginViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.ViewModels" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers" %>

<% if (!Model.IsLoggedIn)
   { %>
        <%: Html.ActionLink(Titles.LogIn, ActionNames.Index, ControllerNames.Login) %>
<% }
   else
   { %>
        <%: Model.Name %>

        <div id="smallLoginViewDropDown">
            <%: Html.ActionLink(Titles.LogOut, ActionNames.LogOut, ControllerNames.Login) %>
        </div>
<% } %>

