<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<QuestionListViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: PropertyDisplayNames.Questions %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <div id="loginDiv"> <% Html.RenderPartial(ViewNames._Login, Model.Login); %> </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) { %>

        <h2><%: PropertyDisplayNames.Questions %></h2>

        <p>
            <%: Html.ActionLink(Titles.CreateQuestion, ActionNames.Create) %>
        </p>

        <% Html.RenderPartial(ViewNames._Pager, Model.Pager); %>

        <table>

            <tr>
                <th><%: PropertyDisplayNames.ID %></th>
                <th><%: PropertyDisplayNames.Question %></th>
                <th><%: Titles.ContentIsFlagged %> </th>
                <th></th>
            </tr>

            <% foreach (var question in Model.List) { %>

                <tr>
                    <td><%: question.ID %></td>
                    <td><%: Html.ActionLink(question.Text, ActionNames.Details, new { id = question.ID }) %></td>
                    <td><%: question.IsFlagged ? CommonTitlesFormatter.Yes : CommonTitlesFormatter.No %></td>
                    <td><%: Html.ActionLink(CommonTitlesFormatter.Edit, ActionNames.Edit, new { id = question.ID }) %> </td>
                    <td><%: Html.ActionLink(CommonTitlesFormatter.Delete, ActionNames.Delete, new { id = question.ID })%> </td>
                </tr>

            <% } %>

        </table>

        <% Html.RenderPartial(ViewNames._Pager, Model.Pager); %>

    <% } %>
</asp:Content>

