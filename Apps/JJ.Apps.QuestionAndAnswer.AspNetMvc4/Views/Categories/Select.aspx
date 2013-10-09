<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Absolute.Master" Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.CategorySelectorViewModel>" %>
<%@ Import Namespace="JJ.Framework.Presentation.AspNetMvc4" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.SelectCategories %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
    <script>

        var selectedCategoryIDs = new Array();

        function liAvailableCategory_onDragStart(ev)
        {
            ev.dataTransfer.setData("elementId", ev.target.id);
        }

        function divSelectedCategories_onDragOver(ev)
        {
            ev.preventDefault();
        }

        function divSelectedCategories_onDrop(ev)
        {
            ev.preventDefault();

            var elementId = ev.dataTransfer.getData("elementId");
            var element = document.getElementById(elementId);
            var categoryID = element.getAttribute("data-category-id");

            // TODO: instead of the code lines below, immediately delegate to the controller.
            selectedCategoryIDs[selectedCategoryIDs.length] = categoryID;
            ev.target.appendChild(element.cloneNode(true));
            //alert(CategoriesSelectView.selectedCategoryIDs);
        }

        $(document).ready(function () {

            // TODO: This should be delegated to the controller.
            $("#buttonStart").click(function () {
                // TODO: This is not type-safe.
                var categoryUrlParameters = "categoryID=" + selectedCategoryIDs.join("&categoryID=");
                var url = "/Questions/Question?" + categoryUrlParameters;
                location.href = url;
            });

        });

    </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Titles.SelectCategories %></h2>

    <% if (Model.NoCategoriesAvailable)
       { %>
            <div id="divNoCategoriesAvailable">
                <%: Messages.NoCategoriesAvailable %>
            </div>
    <% } 
       else
       { %>
            <%-- <div class="trysomething">
                qqwerqwrewr
            </div>
            --%>
    
            <table class="pane">
                <tr>
                    <td class="col1of2">
                        <div id="divAvailableCategories">
                            <h3><%: Labels.AvailableCategories %></h3>

                            <ul>
                                <% foreach (var availableCategory in Model.AvailableCategories)
                                   { %>
                                        <li>
                                            <% Html.RenderPartial("_CategoryNode", availableCategory); %>
                                        </li>
                                <% } %>
                            </ul>
                        </div>
                    </td>
                    <td  class="col2of2">
                        <div id="divSelectedCategories" 
                             ondrop="divSelectedCategories_onDrop(event)" 
                             ondragover="divSelectedCategories_onDragOver(event)">

                            <h3><%: Labels.Selection %></h3>

                            <ul>
                                <% foreach (var selectedCategory in Model.SelectedCategories)
                                   { %>
                                        <li>
                                            <%: selectedCategory.NameParts.Last() %>
                                        </li>
                                <% } %>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>

            <button id="buttonStart">Start Training</button>

        <%-- This link will not work until the selected categories will be in the viewmodel.
        <%: Html.ActionLinkWithCollection(Titles.StartTraining + "- NEW", ActionNames.Question, ControllerNames.Questions, ActionParameterNames.categoryID, Model.SelectedCategories.Select(x => x.ID).ToArray()) %>
        --%>

    <% } %>

</asp:Content>
