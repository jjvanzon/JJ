<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionDetailViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views" %>

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
            <input type="submit" value="<%:Titles.ShowAnswer %>" formaction="<%: Url.Action(ActionNames.ShowAnswer) %>" />
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

<% if (Model.AnswerIsVisible)
    { %>
    <div id="categories">
        <% if (Model.Categories.Count > 0) 
            { %>
      
            <br />

            <%: Labels.Categories %>

            <%: String.Join(" | ", Model.Categories.SelectMany(x => x.NameParts).Distinct()) %>
        
        <% } %>
    </div>
<% } %>
    
