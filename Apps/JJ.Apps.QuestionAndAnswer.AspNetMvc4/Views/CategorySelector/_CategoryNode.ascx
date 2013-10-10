<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.CategoryNodeViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views" %>

<%: Model.Category.NameParts.Last() %>

<ul>
    <% foreach (var category in Model.SubCategories)
        { %>
            <li draggable="true" 
                ondragstart="liAvailableCategory_onDragStart(event)"
                data-category-id="<%:category.Category.ID%>"
                class="liAvailableCategory"
                id="liAvailableCategory<%:category.Category.ID%>"> <%-- The element needs an ID for HTML5 drag and drop to work --%>

                <% Html.RenderPartial(ViewNames._CategoryNode, category); %>
            </li>
    <% } %>
</ul>