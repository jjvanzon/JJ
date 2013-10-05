<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.CategoryNodeViewModel>" %>

<%: Model.Category.NameParts.Last() %>

<ul>
    <% foreach (var category in Model.SubCategories)
        { %>
            <li class="liAvailableCategory" 
                draggable="true" 
                ondragstart="CategoriesSelectView.liAvailableCategory_onDragStart(event)" 
                data-category-id="<%:category.Category.ID%>">

                <% Html.RenderPartial("_CategoryNode", category); %>
            </li>
    <% } %>
</ul>