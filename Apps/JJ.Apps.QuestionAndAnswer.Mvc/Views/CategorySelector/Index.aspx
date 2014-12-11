<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<CategorySelectorViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.SelectCategories %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
    <script>

        function liAvailableCategory_onDragStart(ev) {
            ev.dataTransfer.setData("liAvailableCategoryId", ev.target.id);
        }

        function divSelectedCategories_onDragOver(ev) {
            ev.preventDefault();
        }

        function divSelectedCategories_onDrop(ev) {
            ev.preventDefault();

            var elementId = ev.dataTransfer.getData("liAvailableCategoryId");

            addCategory(elementId);
        }

        function liSelectedCategory_onDragStart(ev) {
            ev.dataTransfer.setData("liSelectedCategoryId", ev.target.id);
        }

        function divAvailableCategories_onDragOver(ev) {
            ev.preventDefault();
        }

        function divAvailableCategories_onDrop(ev) {
            ev.preventDefault();

            var elementId = ev.dataTransfer.getData("liSelectedCategoryId");

            removeCategory(elementId);
        }

        $(document).ready(function () {
            $("li.category.available").dblclick(function (ev) {
                var elementId = ev.target.id;
                addCategory(elementId);
            });

            $("li.category.selected").dblclick(function (ev) {
                var elementId = ev.target.id;
                removeCategory(elementId);
            });
        });

        function removeCategory(elementId) {
            var element = document.getElementById(elementId);
            var categoryID = element.getAttribute("data-category-id");

            var url = '<%= Url.ActionWithParams(ActionNames.Remove, ControllerNames.CategorySelector, ActionParameterNames.categoryID, "") %>'; // Parameter value is added later.
            url = url + encodeURI(categoryID);

            window.document.forms[0].action = url;
            window.document.forms[0].submit();
        }

        function addCategory(elementId) {
            var element = document.getElementById(elementId);
            var categoryID = element.getAttribute("data-category-id");

            var url = '<%= Url.ActionWithParams(ActionNames.Add, ControllerNames.CategorySelector, ActionParameterNames.categoryID, "" ) %>'; // Parameter value is added later.
            url = url + encodeURI(categoryID);

            window.document.forms[0].action = url;
            window.document.forms[0].submit();
        };

    </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) {%>

    <h2><%: Titles.SelectCategories %></h2>

    <% if (Model.NoCategoriesAvailable) { %>

        <div id="divNoCategoriesAvailable">
            <%: Messages.NoCategoriesAvailable %>
        </div>

    <% } else { %>
   
        <table class="pane">
            <tr>
                <td class="col1of2">
                    <div id="divAvailableCategories"
                            ondragover="divAvailableCategories_onDragOver(event)"
                            ondrop="divAvailableCategories_onDrop(event)">

                        <h3><%: Titles.AvailableCategories %></h3>

                        <ul class="category available">
                            <% foreach (var availableCategory in Model.AvailableCategories) { %>

                                <% Html.RenderPartial(ViewNames._AvailableCategory, availableCategory); %>

                            <% } %>
                        </ul>
                    </div>
                </td>

                <td class="col2of2">
                    <div id="divSelectedCategories" 
                            ondragover="divSelectedCategories_onDragOver(event)"
                            ondrop="divSelectedCategories_onDrop(event)">

                        <h3><%: Titles.Selection %></h3>

                        <ul class="category selected">

                            <% using (Html.BeginCollection(() => Model.SelectedCategories)) {
            
                                foreach (var selectedCategory in Model.SelectedCategories) { %>

                                    <% Html.RenderPartial(ViewNames._SelectedCategory, selectedCategory); %>

                                <% } %>

                            <% } %>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>

        <%: Html.ActionLinkWithCollection(Titles.StartTraining, ActionNames.Random, ControllerNames.Questions, ActionParameterNames.c, Model.SelectedCategories.SelectRecursive(x => x.SubCategories).Where(x => x.Visible).Select(x => x.ID).ToArray()) %>

    <% } %>
    
<% } %>

</asp:Content>
