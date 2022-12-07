<%@ Page 
    Title="" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionDetailViewModel>" %>
<%@ Import Namespace="JJ.Framework.Presentation.AspNetMvc4" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views" %>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script>

    $(document).ready(function () {
        $("#aNextQuestion").click(function () {
            location.href = "Questions/RandomQuestionAboutCategories" + location.search;
        });
    });

</script>
</asp:Content>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.Question %>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) {%>

    <% Html.RenderPartial(ViewNames._Question); %>

    <%: Html.HiddenForAllProperties(Model) %>

<% } %>

<br />

<div id="nextQuestion">
    <a href="" id="aNextQuestion"><%: Titles.NextQuestion %> </a>
</div>

</asp:Content>
