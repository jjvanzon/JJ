<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<QuestionConfirmDeleteViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.DeleteQuestion %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <div id="loginDiv"> <% Html.RenderPartial(ViewNames._Login, Model.Login); %> </div>
    <div id="languageDiv"> <% Html.RenderPartial(ViewNames._LanguageSelector, Model.LanguageSelector); %> </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) { %>

        <h2><%: Titles.DeleteQuestion %></h2>
    
        <p><%: Messages.AreYouSureYouWishToDeleteTheFollowingQuestion %></p>

        <table>
            <tr>
                <th> <%: PropertyDisplayNames.ID %> </th>
                <td> <%: Model.ID %> </td>
            </tr>
            <tr>
                <th> <%: PropertyDisplayNames.Question %> </th>
                <td> <%: Model.Question %> </td>
            </tr>
        </table>

        <p>
            <input type="submit" value="<%: Titles.Confirm %>" /> |
            <a href="javascript:history.back()"> <%: CommonTitles.Cancel %> </a>
        </p>

    <% } %>

</asp:Content>
