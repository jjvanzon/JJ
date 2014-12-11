<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<LoginViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: CommonTitles.LogIn %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) { %>

        <h2><%: CommonTitles.LogIn %></h2>

        <p>
            <%: PropertyDisplayNames.UserName %>
            <%: Html.TextBoxFor(x => x.UserName) %>
        </p>

        <p>
            <%: PropertyDisplayNames.Password %>
            <%: Html.PasswordFor(x => x.Password) %>
        </p>

        <input type="submit" value="<%: CommonTitles.LogIn %>" />
    
    <% } %>
</asp:Content>
