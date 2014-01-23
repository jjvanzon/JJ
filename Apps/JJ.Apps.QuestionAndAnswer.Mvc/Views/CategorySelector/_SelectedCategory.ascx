<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.CategoryViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Views.Helpers" %>
<%@ Import Namespace="JJ.Framework.Presentation.Mvc" %>

<% using (Html.BeginCollectionItem()) { %>

    <% if (Model.Visible) { %>

        <%: Html.HiddenFor(x => x.ID) %>

        <%: Html.HiddenFor(x => x.Visible) %>

        <li draggable="true" 
            ondragstart="liSelectedCategory_onDragStart(event)"
            data-category-id="<%:Model.ID%>"
            class="category selected"
            id="liSelectedCategory<%:Model.ID%>"> <%-- The element needs an ID for HTML5 drag and drop to work --%>
            
            <%: Model.NameParts.Last() %>

    <% } %>
    
        <% if (Model.SubCategories.Count > 0) { %>

            <ul class="category selected">

                <% using (Html.BeginCollection(() => Model.SubCategories)) {

                    foreach (var subCategory in Model.SubCategories) { %>

                        <% Html.RenderPartial(ViewNames._SelectedCategory, subCategory); %>

                    <% } %>

                <% } %>

            </ul>

        <% } %>

    <% if (Model.Visible) { %>

        </li>

    <% } %>

<% } %>
