<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionDetailViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers" %>
<%@ Import Namespace="JJ.Framework.Presentation.Mvc" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.EditQuestion %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) { %>

        <h2><%: Titles.EditQuestion %></h2>

        <table>
            <tr>
                <td><%: Labels.ID %></td>
                <td style="text-align:right">
                    <%: Model.Question.ID %>
                    <%: Html.HiddenFor(x => x.Question.ID) %>
                </td>
            </tr>
            <tr>
                <td><%: Labels.IsActive %></td>
                <td style="text-align:right">
                    <%: Html.CheckBoxFor(x => x.Question.IsActive) %>
                    <%: Html.ValidationMessageFor(x => x.Question.IsActive) %>
                </td>
            </tr>
        </table>

        <div class="editor-label"><%: Labels.Question %></div>
        <div>
            <%: Html.TextAreaFor(x => x.Question.Text) %>
            <%: Html.ValidationMessageFor(x => x.Question.Text) %>
        </div>
        
        <div class="editor-label"><%: Labels.Answer %></div>
        <div>
            <%: Html.TextAreaFor(x => x.Question.Answer) %>
            <%: Html.ValidationMessageFor(x => x.Question.Answer) %>
        </div>
        
        <br />
        
        <fieldset>
            <legend><%: Titles.Categories %></legend>

            <% for (int i = 0; i < Model.Question.Categories.Count; i++) { %>
                            
                <div>
                    <%: Model.Question.Categories[i].ID %> - 
                    <%: String.Join(@" \ ", Model.Question.Categories[i].NameParts) %>
                              
                    <%: Html.HiddenFor(x => Model.Question.Categories[i].ID) %>
                </div>

            <% } %>

        </fieldset>
        
        <fieldset>
            <legend><%: Titles.Links %></legend>
            <table style="width:500px">

                <% for (int i = 0; i < Model.Question.Links.Count; i++) { %>

                    <tr style="height:47px">
                        <td>
                            <%: Labels.Description %>
                            <span class="editor-field">
                                <%: Html.TextBoxFor(x => x.Question.Links[i].Description, new { style = "width:200px;" }) %>
                                <%: Html.ValidationMessageFor(x => x.Question.Links[i].Description) %>
                            </span>
                        </td>
                        <td style="width:250px">
                            <%: Labels.Url %>
                            <span class="editor-field">
                                <%: Html.TextBoxFor(x => x.Question.Links[i].Url, new { style = "width:400px;" })%>
                                <%: Html.ValidationMessageFor(x => x.Question.Links[i].Url) %>
                            </span>
                        </td>
                        <td style="vertical-align:bottom;text-align:center;">
                            <input type="submit" value="<%: Titles.Remove %>" formaction="<%: UrlHelpers.GetUrl(ActionNames.RemoveLink, ControllerNames.Questions, ActionParameterNames.questionLinkID, Model.Question.Links[i].ID, ActionParameterNames.questionLinkTemporaryID, Model.Question.Links[i].TemporaryID) %>" />
                            <%: Html.HiddenFor(x => x.Question.Links[i].ID) %>
                            <%: Html.HiddenFor(x => x.Question.Links[i].TemporaryID) %>
                        </td>
                    </tr>
                
                <% } %>

                <tr style="height:47px">
                    <td />
                    <td />
                    <td style="vertical-align:bottom;text-align:center;">
                        <input type="submit" value="<%: Titles.Add %>" formaction="<%: UrlHelpers.GetUrl(ActionNames.AddLink, ControllerNames.Questions) %>" />
                    </td>
                </tr>
            </table>
        </fieldset>
            
        <fieldset>
            <legend><%: Titles.ContentFlags %></legend>

            <%: Labels.HasActiveFlags %> <%: Model.Question.IsFlagged ? Titles.Yes : Titles.No %>

            <br />

            <br />

            <% for (int i = 0; i < Model.Question.Flags.Count; i++ ) { %>

                <%: Html.HiddenFor(x => x.Question.Flags[i].ID) %>

                <fieldset>
                    <legend><%: Model.Question.Flags[i].FlaggedBy%>, <%: Model.Question.Flags[i].DateAndTime.ToShortDateString() %></legend>
                    <table>
                        <tr>
                            <td class="editor-label"><%: Labels.Status%></td>
                            <td class="editor-field">
                                <%: Html.DropDownListFor(
                                        x => Model.Question.Flags[i].Status.ID,
                                        Model.FlagStatuses.Select(x => new SelectListItem 
                                        {
                                            Value = x.ID.ToString(), 
                                            Text = x.Description, 
                                            Selected = x.ID == Model.Question.Flags[i].Status.ID 
                                        }))%>

                                <%: Html.ValidationMessageFor(x => Model.Question.Flags[i].Status)%>
                            </td>
                        </tr>

                        <tr>
                            <td class="editor-label"><%: Labels.Comment%></td>
                            <td class="editor-field">
                                <%: Html.TextAreaFor(x => Model.Question.Flags[i].Comment)%>
                                <%: Html.ValidationMessageFor(x => Model.Question.Flags[i].Comment)%>
                            </td>
                        </tr>

                        <tr>
                            <td class="editor-label"><%: Labels.LastModifiedBy%></td>
                            <td class="editor-field">
                                <%: Model.Question.Flags[i].LastModifiedBy%>
                            </td>
                        </tr>

                    </table>
                </fieldset>

                <% if (i != Model.Question.Flags.Count - 1) { %>

                  <br />  

                <% } %>

            <% } %>

        </fieldset>
    
        <%: Html.ValidationSummary() %>

        <p><input type="submit" value="<%: Titles.Save %>" /></p>

        <div>
            <%: Html.ActionLink("Back to List", "Index") %>
        </div>

    <% } %>

</asp:Content>
