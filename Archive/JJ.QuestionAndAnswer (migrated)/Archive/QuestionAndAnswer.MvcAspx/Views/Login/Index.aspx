<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<LoginViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: CommonResourceFormatter.LogIn %>
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <div id="languageDiv"> <% Html.RenderPartial(ViewNames._LanguageSelector, Model.LanguageSelector); %> </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) { %>

        <h2><%: CommonResourceFormatter.LogIn %></h2>

        <p>
            <%: PropertyDisplayNames.UserName %>
            <%: Html.TextBoxFor(x => x.UserName) %>
        </p>

        <p>
            <%: PropertyDisplayNames.Password %>
            <%: Html.PasswordFor(x => x.Password) %>
        </p>

        <input type="submit" value="<%: CommonResourceFormatter.LogIn %>" />

        <% using (Html.BeginItem(() => Model.ReturnAction)) { %>

            <% Html.RenderPartial(ViewNames._ActionInfo, Model.ReturnAction); %>

        <% } %>
    <% } %>
</asp:Content>
