<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionDetailViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Framework.Presentation.AspNetMvc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.EditQuestion %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) { %>

        <h2><%: Titles.EditQuestion %></h2>

        <%: Html.ValidationSummary(true) %>

        <% using (Html.BeginItem(() => Model.Question)) { %>

            <%: Labels.ID %> <%: Model.Question.ID %>
            <%: Html.HiddenFor(x => x.Question.ID) %>

            <div class="editor-label"><%: Labels.Question %></div>
            <div class="editor-field">
                <%: Html.TextAreaFor(x => x.Question.Text) %>
                <%: Html.ValidationMessageFor(x => x.Question.Text) %>
            </div>

            <div class="editor-label"><%: Labels.Answer %></div>
            <div class="editor-field">
                <%: Html.TextAreaFor(x => x.Question.Answer) %>
                <%: Html.ValidationMessageFor(x => x.Question.Answer) %>
            </div>
            
            <fieldset>
                <legend><%: Titles.Links %></legend>

                <% using (Html.BeginCollection(() => Model.Question.Links)) { %>

                    <table>

                        <% foreach (var link in Model.Question.Links) { 
                            
                            using (Html.BeginCollectionItem()) { %>

                                <tr>
                                    <td>
                                        <div class="editor-label"><%: Labels.Description %></div>
                                        <div class="editor-field">
                                            <%: Html.TextBoxFor(x => link.Description) %>
                                            <%: Html.ValidationMessageFor(x => link.Description) %>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="editor-label"><%: Labels.Url %></div>
                                        <div class="editor-field">
                                            <%: Html.TextBoxFor(x => link.Url) %>
                                            <%: Html.ValidationMessageFor(x => link.Url) %>
                                        </div>
                                    </td>
                                </tr>

                            <% } %>

                        <% } %>

                    </table>

                <% } %>

            </fieldset>
            
            <fieldset>
                <legend><%: Titles.Categories %></legend>

                <% using (Html.BeginCollection(() => Model.Question.Categories)) { 
                           
                    foreach (var category in Model.Question.Categories) { 
                            
                        using (Html.BeginCollectionItem()) { %>
                            
                            <div>
                                <%: category.ID %> - 
                                <%: String.Join(@" \ ", category.NameParts) %>
                                
                                <%: Html.HiddenFor(x => category.ID) %>
                            </div>

                        <% } %>

                    <% } %>

                <% } %>

            </fieldset>
            
            <fieldset>
                <legend><%: Titles.ContentFlags %></legend>

                <% using (Html.BeginCollection(() => Model.Question.Flags)) { 
                           
                    foreach (var flag in Model.Question.Flags) { 
                            
                        using (Html.BeginCollectionItem()) { %>

                            <table>
                                <tr>
                                    <td class="editor-label"><%: Labels.DateAndTime %></td>
                                    <td class="editor-field"><%: flag.DateAndTime %></td>
                                </tr>

                                <tr>
                                    <td class="editor-label"><%: Labels.ContentFlaggedBy %></td>
                                    <td class="editor-field">
                                        <%: flag.FlaggedBy %>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="editor-label"><%: Labels.LastModifiedBy %></td>
                                    <td class="editor-field">
                                        <%: flag.LastModifiedBy %>
                                    </td>
                                </tr>
                    
                                <tr>
                                    <td class="editor-label"><%: Labels.Status %></td>
                                    <td class="editor-field">
                                        <%: Html.DropDownListFor(
                                                x => flag.Status.ID, 
                                                Model.FlagStatuses.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Description })) %>

                                        <%: Html.ValidationMessageFor(x => flag.Status) %>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="editor-label"><%: Labels.Comment %></td>
                                    <td class="editor-field">
                                        <%: Html.TextAreaFor(x => flag.Comment) %>
                                        <%: Html.ValidationMessageFor(x => flag.Comment) %>
                                    </td>
                                </tr>
                            </table>
                                
                        <% } %>

                    <% } %>

                <% } %>

            </fieldset>

        <% } %>

        <p><input type="submit" value="Save" /></p>

        <div>
            <%: Html.ActionLink("Back to List", "Index") %>
        </div>

    <% } %>

</asp:Content>
