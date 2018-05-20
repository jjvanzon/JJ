<%@ Control Language="C#" Inherits="ViewUserControl<LoginPartialViewModel>" %>

<% if (Model.CanLogIn) { %>

    <%: Html.ActionLink(CommonResourceFormatter.LogIn, ActionNames.Index, ControllerNames.Login) %>

<% } %>

<%: Model.UserDisplayName %>

<% if (Model.CanLogOut) { %>

    <div id="loginPartialViewDropDown">
        <%: Html.ActionLink(CommonResourceFormatter.LogOut, ActionNames.LogOut, ControllerNames.Login) %>
    </div>

<% } %>

