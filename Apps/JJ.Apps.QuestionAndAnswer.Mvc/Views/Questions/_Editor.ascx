<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionEditViewModel>" %>
<%@ Import Namespace="JJ.Framework.Common" %>
<%@ Import Namespace="JJ.Framework.Presentation.Mvc" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Views" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Controllers.Helpers" %>
    
<table>
    <tr>
        <th><%: Labels.ID %></th>
        <td style="text-align:right">
            <%: Model.Question.ID %>
            <%: Html.HiddenFor(x => x.Question.ID) %>
        </td>
    </tr>
    <tr>
        <th><%: Labels.IsActive %></th>
        <td style="text-align:right">
            <%: Html.CheckBoxFor(x => x.Question.IsActive) %>
            <%: Html.ValidationMessageFor(x => x.Question.IsActive) %>
        </td>
    </tr>
    <tr>
        <th><%: Labels.Source %></th>
        <td style="text-align:right">
            <% if (!String.IsNullOrEmpty(Model.Question.Source.Url)) { %>

                <a href="<%: Model.Question.Source.Url %>" target="_blank">

            <% } %>

            <%: Model.Question.Source.Description %>

            <% if (!String.IsNullOrEmpty(Model.Question.Source.Url)) { %>

                </a>

            <% } %>

            <%: Html.HiddenFor(x => x.Question.Source.ID) %>
        </td>
    </tr>
    <tr>
        <th><%: Labels.Type %></th>
        <td style="text-align:right">
            <%: Model.Question.Type.Name %>
            <%: Html.HiddenFor(x => x.Question.Type.ID) %>
        </td>
    </tr>
</table>

<div class="editor-label"><%: Labels.Question %> <%: Html.ValidationMessageFor(x => x.Question.Text) %></div>
<div>
    <%: Html.TextAreaFor(x => x.Question.Text) %>
</div>
        
<div class="editor-label"><%: Labels.Answer %> <%: Html.ValidationMessageFor(x => x.Question.Answer) %></div>
<div>
    <%: Html.TextAreaFor(x => x.Question.Answer) %>
</div>
        
<br />
        
<fieldset>
    <legend><%: Titles.Categories %></legend>

    <% if (Model.Question.Categories.Count != 0) { %>

        <table>
            <% for (int i = 0; i < Model.Question.Categories.Count; i++) { %>

                <tr style="height:27px">
                    <td>

                        <%: Html.DropDownListFor(
                            x => x.Question.Categories[i].Category.ID,
                            Model.Categories.SelectRecursive(x => x.SubCategories).Select(x => new SelectListItem 
                            {
                                Value = x.ID.ToString(), 
                                Text = String.Join(@" \ ", x.NameParts), 
                                Selected = x.ID == Model.Question.Categories[i].Category.ID
                            }),
                            "")%>
                    </td>
                    <td style="vertical-align:bottom;text-align:center;">
                        <input type="submit" value="<%: Titles.Remove %>" formaction="<%: Url.ActionWithParams(ActionNames.RemoveCategory, 
                                                                                                                ControllerNames.Questions,
                                                                                                                ActionParameterNames.temporaryID,
                                                                                                                Model.Question.Categories[i].TemporaryID) %>" />
                        <%: Html.HiddenFor(x => x.Question.Categories[i].QuestionCategoryID) %>
                        <%: Html.HiddenFor(x => x.Question.Categories[i].TemporaryID) %>
                    </td>
                    <td>
                        <%: Html.ValidationMessageFor(x => x.Question.Categories[i].Category.ID) %>
                    </td>

                </tr>

            <% } %>
            
            <tr style="height:27px">
                <td />
                <td style="vertical-align:bottom;text-align:center;">
                    <input type="submit" value="<%: Titles.Add %>" formaction="<%: Url.Action(ActionNames.AddCategory) %>" />
                </td>
                <td />
            </tr>
        </table>
        
    <% } else { %>

        <br />
        <input type="submit" value="<%: Titles.Add %>" formaction="<%: Url.Action(ActionNames.AddCategory) %>" />

    <% } %>

</fieldset>
        
<fieldset>
    <legend><%: Titles.Links %></legend>

    <% if (Model.Question.Links.Count != 0) { %>

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
                        <% if (!String.IsNullOrEmpty(Model.Question.Links[i].Url)) { %>

                            <a href="<%: Model.Question.Links[i].Url %>" target="_blank">

                        <% } %>

                        <%: Labels.Url %>

                        <% if (!String.IsNullOrEmpty(Model.Question.Links[i].Url)) { %>

                            </a>

                        <% } %>

                        <span class="editor-field">
                            <%: Html.TextBoxFor(x => x.Question.Links[i].Url, new { style = "width:400px;" })%>
                            <%: Html.ValidationMessageFor(x => x.Question.Links[i].Url) %>
                        </span>
                    </td>
                    <td style="vertical-align:bottom;text-align:center;">
                        <input type="submit" value="<%: Titles.Remove %>" formaction="<%: Url.ActionWithParams(ActionNames.RemoveLink, 
                                                                                                                ControllerNames.Questions, 
                                                                                                                ActionParameterNames.temporaryID, 
                                                                                                                Model.Question.Links[i].TemporaryID) %>" />

                        <%: Html.HiddenFor(x => x.Question.Links[i].ID) %>
                        <%: Html.HiddenFor(x => x.Question.Links[i].TemporaryID) %>
                    </td>
                </tr>
                
            <% } %>

            <tr style="height:47px">
                <td />
                <td />
                <td style="vertical-align:bottom;text-align:center;">
                    <input type="submit" value="<%: Titles.Add %>" formaction="<%: Url.Action(ActionNames.AddLink) %>" />
                </td>
            </tr>
        </table>

    <% } else { %>

        <br />
        <input type="submit" value="<%: Titles.Add %>" formaction="<%: Url.Action(ActionNames.AddLink) %>" />

    <% } %>

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
                    <th class="editor-label"><%: Labels.Status%></th>
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
                    <th class="editor-label"><%: Labels.Comment%></th>
                    <td class="editor-field">
                        <%: Html.TextAreaFor(x => Model.Question.Flags[i].Comment)%>
                        <%: Html.ValidationMessageFor(x => Model.Question.Flags[i].Comment)%>
                    </td>
                </tr>

                <tr>
                    <th class="editor-label"><%: Labels.LastModifiedBy%></th>
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
