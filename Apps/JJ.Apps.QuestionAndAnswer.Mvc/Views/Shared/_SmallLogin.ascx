<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SmallLoginViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.ViewModels" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Helpers" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Names" %>

<% if (Model.LogInActionIsVisible) { %>

    <%: Html.ActionLink(Titles.LogIn, ActionNames.Index, ControllerNames.Login) %>

<% } %>

<%: Model.DisplayName %>

<% if (Model.LogOutActionIsVisible) { %>

    <div id="smallLoginViewDropDown">
        <%: Html.ActionLink(Titles.LogOut, ActionNames.LogOut, ControllerNames.Login) %>
    </div>

<% } %>

