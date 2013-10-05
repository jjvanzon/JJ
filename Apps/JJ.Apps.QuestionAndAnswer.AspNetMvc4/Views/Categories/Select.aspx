<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Absolute.Master" Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.CategorySelectorViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.SelectCategories %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
    <script>

        var CategoriesSelectView =
            {
                selectedCategoryIDs: new Array(),

                liAvailableCategory_onDragStart: function (ev) {
                    var categoryID = ev.target.getAttribute("data-category-id");
                    ev.dataTransfer.setData("categoryId", categoryID);
                },

                divSelectedCategories_onDragOver: function (ev) {
                    ev.preventDefault();
                },

                divSelectedCategories_onDrop: function (ev) {
                    ev.preventDefault();
                    var categoryID = ev.dataTransfer.getData("categoryId");

                    CategoriesSelectView.selectedCategoryIDs[CategoriesSelectView.selectedCategoryIDs.length] = categoryID;

                    //ev.target.appendChild(document.getElementById(data));

                    alert(CategoriesSelectView.selectedCategoryIDs);
                }
        }

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
                             ondrop="CategoriesSelectView.divSelectedCategories_onDrop(event)" 
                             ondragover="CategoriesSelectView.divSelectedCategories_onDragOver(event)">

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
    <% } %>

</asp:Content>
