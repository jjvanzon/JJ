<%@ Control Language="C#" Inherits="ViewUserControl<SmallLoginViewModel>" %>

<% if (Model.LogInActionIsVisible) { %>

    <%: Html.ActionLink(Titles.LogIn, ActionNames.Index, ControllerNames.Login) %>

<% } %>

<%: Model.DisplayName %>

<% if (Model.LogOutActionIsVisible) { %>

    <div id="smallLoginViewDropDown">
        <%: Html.ActionLink(Titles.LogOut, ActionNames.LogOut, ControllerNames.Login) %>
    </div>

<% } %>

