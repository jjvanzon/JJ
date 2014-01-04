<%@ Page Title="" Language="C#" 
         MasterPageFile="~/Views/Shared/Site.Master" 
         Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.RandomQuestionViewModel>" %>
<%@ Import Namespace="JJ.Framework.Presentation.AspNetMvc4" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.ViewModels" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views" %>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">

</asp:Content>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.Question %>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) {%>
    
    <h2><%: Titles.Question %></h2>

    <div class="display-field">
        <%: Html.DisplayFor(x => x.Question.Text) %>
    </div>
    
    <br />

    <div class="display-label">
        <%: Labels.Answer %>
    </div>
    <div class="text-field">
        <%: Html.TextBoxFor(x => x.UserAnswer, new { autocomplete = "off", style="width:80%;" })%>
    </div>

    <br />

    <div id="buttons">
        
        <% if (!Model.AnswerIsVisible) { %>

            <input type="submit" value="<%:Titles.ShowAnswer%>" formaction="<%:Url.Action(ActionNames.ShowAnswer)%>" />

        <% } else { %>
            
            <input type="submit" value="<%:Titles.HideAnswer%>" formaction="<%:Url.Action(ActionNames.HideAnswer)%>" />

        <% } %>

    </div>
    
    <div id="answer">

        <% if (Model.AnswerIsVisible) { %>

            <br />

            <div class="display-label">
                <%: Labels.TheCorrectAnswer %>
            </div>

            <br />

            <div class="display-field">
                <%: Html.DisplayFor(x => x.Question.Answer) %>
            </div>

        <% } %>

    </div>

    <div id="links">

        <% if (Model.Question.Links.Count > 0) { %>
        
            <br />

            <br />

            <%: Labels.AdditionalInformation %>

            <ul>

                <% foreach (var link in Model.Question.Links) { %>

                    <li><a href="<%: link.Url %>" target="_blank"><%: link.Description %></a> </li>

                <% } %>

            </ul>

        <% } %>

    </div>

    <% if (Model.AnswerIsVisible) { %>

        <div id="categories">

            <% if (Model.Question.Categories.Count > 0) { %>
      
                <br />

                <%: Labels.Categories %>

                <%: String.Join(" | ", Model.Question.Categories.SelectMany(x => x.NameParts).Distinct()) %>
        
            <% } %>

        </div>

    <% } %>

    <% Html.RenderPartial(ViewNames._SmallQuestionFlag, Model); %>

    <%-- Extra HiddenFor's for information that has to be sent over the line too. --%>

    <%: Html.HiddenFor(x => x.Question.ID) %>
    <%: Html.HiddenFor(() => Model.AnswerIsVisible) %>

    <% for (int i = 0; i < Model.SelectedCategories.Count; i++) { %>

        <%: Html.HiddenFor(x => x.SelectedCategories[i].ID) %>

    <% } %>

<% } %>

<br />

<div id="nextQuestion">

    <%: Html.ActionLinkWithCollection(Titles.NextQuestion, ActionNames.Random, ControllerNames.Questions, ActionParameterNames.c, Model.SelectedCategories.Select(x => x.ID)) %>

</div>

</asp:Content>
