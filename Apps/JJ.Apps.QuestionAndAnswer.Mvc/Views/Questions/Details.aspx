<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionDetailsViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Titles.QuestionDetails %></h2>

    <%-- Replace DisplayFor calls by literal properties? --%>

    <table>
        <colgroup>
            <col style="padding-right: 50px" />
            <col />
        </colgroup>
        <tr>
            <th><%: Labels.ID %></th>
            <td><%: Model.Question.ID %></td>
        </tr>

        <tr>
            <th><%: Labels.Question %></th>
            <td><%: Model.Question.Text %></td>
        </tr>
        <tr>
            <th><%: Labels.Answer %></th>
            <td><%: Model.Question.Answer %></td>
        </tr>

        <tr>
            <th><%: Labels.IsActive %></th>
            <td><%: Model.Question.IsActive ? Titles.Yes : Titles.No %></td>
        </tr>
        <tr>
            <th><%: Labels.Source %></th>
            <td>
                <% if (!String.IsNullOrEmpty(Model.Question.Source.Url)) { %>

                    <a href="<%: Model.Question.Source.Url %>" target="_blank">

                <% } %>

                <%: Model.Question.Source.Description %>

                <% if (!String.IsNullOrEmpty(Model.Question.Source.Url)) { %>

                    </a>

                <% } %>
            </td>
        </tr>
        <tr>
            <th><%: Labels.Type %></th>
            <td><%: Model.Question.Type.Name %></td>
        </tr>

        <tr>
            <th><%: Labels.Categories %></th>
            <td>

                <% foreach (var category in Model.Question.Categories.Select(x => x.Category)) { %>

                    <div>
                        <%: category.ID %> - 
                        <%: String.Join(@" \ ", category.NameParts) %>
                    </div>

                <% } %>

                
                <% if (Model.Question.Categories.Count == 0) { %>

                    <%: Titles.None %>

                <% } %>

            </td>
        </tr>
        <tr>
            <th><%: Labels.Links %></th>
            <td>
                
                <% foreach (var link in Model.Question.Links) { %>

                    <p>
                        <%: link.Description %>: <a href="<%: link.Url %>" target="_blank"> <%: link.Url %> </a>
                    </p>

                <% } %>

                <% if (Model.Question.Links.Count == 0) { %>

                    <%: Titles.None %>

                <% } %>

            </td>
        </tr>
    </table>

    <fieldset>
        <legend><%: Titles.ContentFlags %></legend>

        <table>
            <tr>
                <th><%: Labels.HasActiveFlags %></th>
                <td><%: Model.Question.IsFlagged ? Titles.Yes : Titles.No %></td>
            </tr>

            <% foreach (var flag in Model.Question.Flags) { %>

                <tr>
                    <th><%: flag.FlaggedBy%></th>
                    <td>
                        <div>
                            <%: Labels.DateAndTime %>
                            <%: flag.DateAndTime.ToString() %>
                        </div>
                        <div>
                            <%: Labels.Status %>
                            <%: flag.Status.Description %>
                        </div>
                        <div>
                            <%: Labels.Comment %>
                            <%: flag.Comment %>
                        </div>
                        <div>
                            <%: Labels.LastModifiedBy %>
                            <%: flag.LastModifiedBy %>
                        </div>
                    </td>
                </tr>

            <% } %>
        </table>

    </fieldset>

    <%-- TODO: These actions must be reflected by the Presenter code. --%>

    <p>
        <%: Html.ActionLink(Titles.Edit, ActionNames.Edit, new { id = Model.Question.ID }) %> |
        <%: Html.ActionLink(Titles.New, ActionNames.Create) %> |
        <%: Html.ActionLink(Titles.Delete, ActionNames.Delete, new { id = Model.Question.ID }) %> |
        <%: Html.ActionLink(Titles.BackToList, ActionNames.Index) %>
    </p>

</asp:Content>

