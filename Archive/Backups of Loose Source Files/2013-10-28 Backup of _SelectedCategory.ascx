<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.CategoryViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views" %>

<% if (Model.Visible) { %>

    <li draggable="true" 
        ondragstart="liSelectedCategory_onDragStart(event)"
        data-category-id="<%:Model.ID%>"
        class="liSelectedCategory"
        id="liSelectedCategory<%:Model.ID%>"> <%-- The element needs an ID for HTML5 drag and drop to work --%>
            
        <%: Model.NameParts.Last() %>

<% } %>

    <% if (Model.SubCategories.Count > 0) { %>

        <ul>

            <% for (int i = 0; i < Model.SubCategories.Count; i++) { 
                           
                var subCategory = Model.SubCategories[i]; %>

                <% Html.RenderPartial(ViewNames._SelectedCategory, subCategory); %>

                <% if (subCategory.Visible) { %>

                    <%: Html.HiddenFor(x => x.SubCategories[i].ID) %>

                <% } %>

            <% } %>

        </ul>

    <% } %>

<% if (Model.Visible) { %>

    </li>

<% } %>
