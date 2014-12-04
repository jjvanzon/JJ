<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<LoginViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.LogIn %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) { %>

        <h2><%: Titles.LogIn %></h2>

        <p>
            <%: Labels.UserName %>
            <%: Html.TextBoxFor(x => x.UserName) %>
        </p>

        <p>
            <%: Labels.Password %>
            <%: Html.PasswordFor(x => x.Password) %>
        </p>

        <input type="submit" value="<%: Titles.LogIn %>" />
    
    <% } %>
</asp:Content>
