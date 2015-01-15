<%@ Control Language="C#" Inherits="ViewUserControl<LoginPartialViewModel>" %>

<% if (Model.CanLogIn) { %>

    <%: Html.ActionLink(CommonTitles.LogIn, ActionNames.Index, ControllerNames.Login) %>

<% } %>

<%: Model.UserDisplayName %>

<% if (Model.CanLogOut) { %>

    <div id="smallLoginViewDropDown">
        <%: Html.ActionLink(CommonTitles.LogOut, ActionNames.LogOut, ControllerNames.Login) %>
    </div>

<% } %>

