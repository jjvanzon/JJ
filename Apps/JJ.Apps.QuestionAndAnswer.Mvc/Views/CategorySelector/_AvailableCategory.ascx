<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.CategoryViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Views.Helpers" %>
<%@ Import Namespace="JJ.Framework.Presentation.Mvc" %>

<% if (Model.Visible) { %>

    <li draggable="true" 
        ondragstart="liAvailableCategory_onDragStart(event)"
        data-category-id="<%:Model.ID%>"
        class="category available"
        id="liAvailableCategory<%:Model.ID%>"> <%-- The element needs an ID for HTML5 drag and drop to work --%>

        <%: Model.NameParts.Last() %>
            
<% } %>

    <% if (Model.SubCategories.Count > 0) { %>

        <ul class="category available">
            
            <% foreach (var subCategory in Model.SubCategories) { %>

                <% Html.RenderPartial(ViewNames._AvailableCategory, subCategory); %>

            <% } %>

        </ul>

    <% } %>

<% if (Model.Visible) { %>

    </li>

<% } %>
