<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<QuestionDeleteConfirmedViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <h2><%: Titles.DeleteQuestion %></h2>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <div id="loginDiv"> <% Html.RenderPartial(ViewNames._Login, Model.Login); %> </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Titles.DeleteQuestion %></h2>
    
    <p><%: Messages.QuestionIsDeleted %></p>

    <p>
        <%: Html.ActionLink(Titles.BackToList, ActionNames.Index) %>
    </p>
</asp:Content>
