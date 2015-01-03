<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.CategoryViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views" %>
<%@ Import Namespace="JJ.Framework.Presentation.AspNetMvc4" %>

<% using (Html.BeginCollectionItem()) { %>

    <%: Model.NameParts.Last() %>

    <%: Html.HiddenFor(() => Model.ID, Model.ID) %>

    <ul>

        <% using (Html.BeginCollection(() => Model.SubCategories)) {
            
            foreach (var category in Model.SubCategories) { %>

                <li draggable="true" 
                    ondragstart="liAvailableCategory_onDragStart(event)"
                    data-category-id="<%:category.ID%>"
                    class="liAvailableCategory"
                    id="liAvailableCategory<%:category.ID%>"> <%-- The element needs an ID for HTML5 drag and drop to work --%>

                    <% Html.RenderPartial(ViewNames._AvailableCategory, category); %>
                </li>

            <% } %>

        <% } %>

    </ul>

<% } %>