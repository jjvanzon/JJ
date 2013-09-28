<%@ Page 
    Title="" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionDetailViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers" %>
<%@ Import Namespace="JJ.Framework.Presentation.AspNetMvc4" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Titles.Question %>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) {%>

    <h2><%: Titles.Question %></h2>

    <div class="display-field">
        <%: Html.DisplayFor(x => x.Question) %>
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
        <% if (!Model.AnswerIsVisible)
           { %>
                <input type="submit" value="<%:Titles.ShowAnswer %>" formaction="<%: Url.Action(ActionNames.Question) %>" />
        <% }
           else
           { %>
                <input type="submit" value="<%:Titles.HideAnswer %>" formaction="<%: Url.Action(ActionNames.HideAnswer) %>" />
        <% } %>
    </div>
    
    <div id="answer">
        <% if (Model.AnswerIsVisible)
           { %>
                <br />

                <div class="display-label">
                    <%: Labels.TheCorrectAnswer %>
                </div>

                <br />

                <div class="display-field">
                    <%: Html.DisplayFor(x => x.Answer) %>
                </div>
        <% } %>
    </div>
    
    <div id="links">
        <% if (Model.Links.Count > 0) 
           { %>
        
            <br />

            <br />

            <%: Labels.AdditionalInformation %>

            <ul>

                <% foreach (var link in Model.Links)
                   { %>
                            <li><a href="<%: link.Url %>" target="_blank"><%: link.Description %></a> </li>
                <% } %>

            </ul>

        <% } %>
    </div>

    <br />

    <div id="nextQuestion">
        <%: Html.ActionLink(Titles.NextQuestion, ActionNames.Question) %>
    </div>

    <%: Html.HiddenForAllProperties(Model) %>

<% } %>

</asp:Content>
