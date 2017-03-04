<%@ Control Language="C#" Inherits="ViewUserControl<LoginPartialViewModel>" %>

<% if (Model.CanLogIn) { %>

    <%: Html.ActionLink(CommonTitlesFormatter.LogIn, ActionNames.Index, ControllerNames.Login) %>

<% } %>

<%: Model.UserDisplayName %>

<% if (Model.CanLogOut) { %>

    <div id="loginPartialViewDropDown">
        <%: Html.ActionLink(CommonTitlesFormatter.LogOut, ActionNames.LogOut, ControllerNames.Login) %>
    </div>

<% } %>

