<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionListViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.Questions %>
</asp:Content>


<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Titles.Questions %></h2>

    <p>
        <%: Html.ActionLink(Titles.CreateQuestion, ActionNames.Create) %>
    </p>

    <table>

        <tr>
            <th><%: Titles.ID %></th>
            <th><%: Titles.Question %></th>
            <th><%: Titles.ContentIsFlagged %> </th>
            <th></th>
        </tr>

        <% foreach (var question in Model.List) { %>

            <tr>
                <td><%: question.ID %></td>
                <td><%: Html.ActionLink(question.Text, ActionNames.Details, new { id = question.ID }) %></td>
                <td><%: question.IsFlagged ? Titles.Yes : Titles.No %></td>
                <td><%: Html.ActionLink(Titles.Edit, ActionNames.Edit, new { id = question.ID }) %> </td>
                <td><%: Html.ActionLink(Titles.Delete, ActionNames.Delete, new { id = question.ID })%> </td>
            </tr>

        <% } %>

    </table>

</asp:Content>

