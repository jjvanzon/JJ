<%@ Control Language="C#" Inherits="ViewUserControl<SmallLoginViewModel>" %>

<% if (Model.LogInActionIsVisible) { %>

    <%: Html.ActionLink(CommonTitles.LogIn, ActionNames.Index, ControllerNames.Login) %>

<% } %>

<%: Model.DisplayName %>

<% if (Model.LogOutActionIsVisible) { %>

    <div id="smallLoginViewDropDown">
        <%: Html.ActionLink(CommonTitles.LogOut, ActionNames.LogOut, ControllerNames.Login) %>
    </div>

<% } %>

