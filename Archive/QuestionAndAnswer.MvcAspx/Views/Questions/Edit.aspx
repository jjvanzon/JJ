<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<QuestionEditViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Title %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <div id="loginDiv"> <% Html.RenderPartial(ViewNames._Login, Model.Login); %> </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()) { %>
    
        <%: Html.HiddenFor(x => x.IsNew) %>

        <h2><%: Model.Title %></h2>

        <table>
            <tr>
                <th><%: PropertyDisplayNames.ID %></th>
                <td style="text-align:right">
                    <%: Model.Question.ID %>
                    <%: Html.HiddenFor(x => x.Question.ID) %>
                </td>
            </tr>
            <tr>
                <th><%: PropertyDisplayNames.IsActive %></th>
                <td style="text-align:right">
                    <%: Html.CheckBoxFor(x => x.Question.IsActive) %>
                    <%: Html.ValidationMessageFor(x => x.Question.IsActive) %>
                </td>
            </tr>
            <tr>
                <th><%: PropertyDisplayNames.Source %></th>
                <td style="text-align:right">
                    <% if (!string.IsNullOrEmpty(Model.Question.Source.Url)) { %>

                        <a href="<%: Model.Question.Source.Url %>" target="_blank">

                    <% } %>

                    <%: Model.Question.Source.Description %>

                    <% if (!string.IsNullOrEmpty(Model.Question.Source.Url)) { %>

                        </a>

                    <% } %>

                    <%: Html.HiddenFor(x => x.Question.Source.ID) %>
                </td>
            </tr>
            <tr>
                <th><%: PropertyDisplayNames.QuestionType %></th>
                <td style="text-align:right">
                    <%: Model.Question.Type.Name %>
                    <%: Html.HiddenFor(x => x.Question.Type.ID) %>
                </td>
            </tr>
            <tr>
                <th><%: PropertyDisplayNames.LastModifiedByUser %></th>
                <td style="text-align:right"><%: Model.Question.LastModifiedBy %> </td>
            </tr>
            <tr>
                <th><%: PropertyDisplayNames.IsManual %></th>
                <td style="text-align:right">
                    <%: Model.Question.IsManual ? CommonResourceFormatter.Yes : CommonResourceFormatter.No %>

                    <% if (Model.Question.IsManual) { %>

                        <br />
                        <%: Messages.ExplanationOfIsManualWithParenthesis %>

                    <% } %>
                </td>
            </tr>
        </table>

        <div class="editor-label"><%: PropertyDisplayNames.Question %> <%: Html.ValidationMessageFor(x => x.Question.Text) %></div>
        <div>
            <%: Html.TextAreaFor(x => x.Question.Text) %>
        </div>
        
        <div class="editor-label"><%: PropertyDisplayNames.Answer %> <%: Html.ValidationMessageFor(x => x.Question.Answer) %></div>
        <div>
            <%: Html.TextAreaFor(x => x.Question.Answer) %>
        </div>
        
        <br />
        
        <fieldset>
            <legend><%: PropertyDisplayNames.Categories %></legend>

            <% if (Model.Question.Categories.Count != 0) { %>

                <table>
                    <% for (int i = 0; i < Model.Question.Categories.Count; i++) { %>

                        <tr style="height:27px">
                            <td>

                                <%: Html.DropDownListFor(
                                        x => x.Question.Categories[i].Category.ID,
                                        Model.AllCategories.UnionRecursive(x => x.SubCategories).Select(x => new SelectListItem 
                                        {
                                            Value = x.ID.ToString(), 
                                            Text = string.Join(@" \ ", x.NameParts), 
                                            Selected = x.ID == Model.Question.Categories[i].Category.ID
                                        }),
                                        "")%>
                            </td>
                            <td style="vertical-align:bottom;text-align:center;">
                                <input type="submit" value="<%: CommonResourceFormatter.Remove %>" formaction="<%: Url.ActionWithParams(ActionNames.RemoveCategory, 
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
                            <input type="submit" value="<%: CommonResourceFormatter.Add %>" formaction="<%: Url.Action(ActionNames.AddCategory) %>" />
                        </td>
                        <td />
                    </tr>
                </table>
        
            <% } else { %>

                <br />
                <input type="submit" value="<%: CommonResourceFormatter.Add %>" formaction="<%: Url.Action(ActionNames.AddCategory) %>" />

            <% } %>

        </fieldset>
        
        <fieldset>
            <legend><%: Titles.Links %></legend>

            <% if (Model.Question.Links.Count != 0) { %>

                <table style="width:500px">

                    <% for (int i = 0; i < Model.Question.Links.Count; i++) { %>

                        <tr style="height:47px">
                            <td>
                                <%: PropertyDisplayNames.Description %>
                                <span class="editor-field">
                                    <%: Html.TextBoxFor(x => x.Question.Links[i].Description, new { style = "width:200px;" }) %>
                                    <%: Html.ValidationMessageFor(x => x.Question.Links[i].Description) %>
                                </span>
                            </td>
                            <td style="width:250px">
                                <% if (!string.IsNullOrEmpty(Model.Question.Links[i].Url)) { %>

                                    <a href="<%: Model.Question.Links[i].Url %>" target="_blank">

                                <% } %>

                                <%: PropertyDisplayNames.Url %>

                                <% if (!string.IsNullOrEmpty(Model.Question.Links[i].Url)) { %>

                                    </a>

                                <% } %>

                                <span class="editor-field">
                                    <%: Html.TextBoxFor(x => x.Question.Links[i].Url, new { style = "width:400px;" })%>
                                    <%: Html.ValidationMessageFor(x => x.Question.Links[i].Url) %>
                                </span>
                            </td>
                            <td style="vertical-align:bottom;text-align:center;">
                                <input type="submit" value="<%: CommonResourceFormatter.Remove %>" formaction="<%: Url.ActionWithParams(ActionNames.RemoveLink, 
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
                            <input type="submit" value="<%: CommonResourceFormatter.Add %>" formaction="<%: Url.Action(ActionNames.AddLink) %>" />
                        </td>
                    </tr>
                </table>

            <% } else { %>

                <br />
                <input type="submit" value="<%: CommonResourceFormatter.Add %>" formaction="<%: Url.Action(ActionNames.AddLink) %>" />

            <% } %>

        </fieldset>
            
        <fieldset>
            <legend><%: Titles.ContentFlags %></legend>

            <%: Titles.HasActiveFlags %> <%: Model.Question.IsFlagged ? CommonResourceFormatter.Yes : CommonResourceFormatter.No %>

            <br />

            <br />

            <% for (int i = 0; i < Model.Question.Flags.Count; i++ ) { %>

                <%: Html.HiddenFor(x => x.Question.Flags[i].ID) %>

                <fieldset>
                    <legend><%: Model.Question.Flags[i].FlaggedBy%>, <%: Model.Question.Flags[i].DateAndTime.ToShortDateString() %></legend>
                    <table>
                        <tr>
                            <th class="editor-label"><%: PropertyDisplayNames.FlagStatus %></th>
                            <td class="editor-field">
                                <%: Html.DropDownListFor(
                                        x => Model.Question.Flags[i].Status.ID,
                                        Model.Question.Flags[i].AllFlagStatuses.Select(x => new SelectListItem 
                                        {
                                            Value = x.ID.ToString(), 
                                            Text = x.Name, 
                                            Selected = x.ID == Model.Question.Flags[i].Status.ID 
                                        }))%>

                                <%: Html.ValidationMessageFor(x => Model.Question.Flags[i].Status)%>
                            </td>
                        </tr>

                        <tr>
                            <th class="editor-label"><%: PropertyDisplayNames.Comment%></th>
                            <td class="editor-field">
                                <%: Html.TextAreaFor(x => Model.Question.Flags[i].Comment)%>
                                <%: Html.ValidationMessageFor(x => Model.Question.Flags[i].Comment)%>
                            </td>
                        </tr>

                        <tr>
                            <th class="editor-label"><%: PropertyDisplayNames.LastModifiedByUser%></th>
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

        <p>
            <%-- Save --%>
            <input type="submit" value="<%: CommonResourceFormatter.Save %>" /> |

            <%-- TODO: The view is not supposed to make these decisions. --%>

            <%-- Cancel --%>
            <% if (Model.IsNew) { %>

                <a href="javascript:history.back()"> <%: CommonResourceFormatter.Cancel %></a> |
                <noscript>
                    <%: Messages.YouHaveNoJavaScript %>
                    <%: Html.ActionLink(CommonResourceFormatter.Cancel, ActionNames.Index) %> ?
                </noscript>

            <% } else { %>

                <%: Html.ActionLink(CommonResourceFormatter.Cancel, ActionNames.Details, new { id = Model.Question.ID }) %> |

            <% } %>

            <%-- Delete --%>
            <% if (Model.CanDelete) { %>

                <%: Html.ActionLink(CommonResourceFormatter.Delete, ActionNames.Delete, new { id = Model.Question.ID }) %> |

            <% } %>

            <%-- Index --%>
            <%: Html.ActionLink(Titles.BackToList, ActionNames.Index) %>
        </p>

    <% } %>

</asp:Content>
