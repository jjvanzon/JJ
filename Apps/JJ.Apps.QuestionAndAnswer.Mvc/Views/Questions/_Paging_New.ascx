<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PagingViewModel>" %>
<%--
<table>
    <tr>

        <% if (Model.CanGoToFirstPage) { %>
            <td>&lt;&lt;</td>
        <% } %>

        <% if (Model.CanGoToPreviousPage) { %>
            <td>&lt;</td>
        <% } %>
        
        <% foreach (var pageNumber in Model.PageNumbers) { %>
            <% if (pageNumber.IsSelected) { %>

                <td><strong><%=pageNumber.Number%></strong></td>

            <% } else { %>

                <td><%=pageNumber.Number%></td>    

            <% } %>

        <% } %>

        <% if (Model.CanGoToNextPage) { %>
            <td>&gt;</td>
        <% } %>

        <% if (Model.CanGoToLastPage) { %>
            <td>&gt;&gt;</td>
        <% } %>

    </tr>
</table>--%>
