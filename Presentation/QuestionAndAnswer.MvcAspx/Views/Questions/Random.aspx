<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<RandomQuestionViewModel>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: PropertyDisplayNames.Question %>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <div id="loginDiv"> <% Html.RenderPartial(ViewNames._Login, Model.Login); %> </div>
    <div id="languageDiv"> <% Html.RenderPartial(ViewNames._LanguageSelector, Model.LanguageSelector); %> </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) {%>
    
    <h2><%: PropertyDisplayNames.Question %></h2>

    <div class="display-field">
        <%: Html.DisplayFor(x => x.Question.Text) %>
    </div>
    
    <br />

    <div class="display-label">
        <%: PropertyDisplayNames.Answer %>
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
                <%: Titles.TheCorrectAnswer %>
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

            <%: Titles.AdditionalInformation %>

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

                <%: PropertyDisplayNames.Categories %>

                <%: string.Join(" | ", Model.Question.Categories.SelectMany(x => x.Category.NameParts).Distinct()) %>
        
            <% } %>

        </div>

    <% } %>

    <% Html.RenderPartial(ViewNames._SmallQuestionFlag, Model); %>

    <% for (int i = 0; i < Model.SelectedCategories.Count; i++) { %>

        <%: Html.HiddenFor(x => x.SelectedCategories[i].ID) %>

    <% } %>
    
    <%: Html.HiddenFor(x => x.Question.ID) %>
    <%: Html.HiddenFor(x => x.AnswerIsVisible) %>

<% } %>

<br />

<div id="nextQuestion">

    <%: Html.ActionLinkWithCollection(Titles.NextQuestion, ActionNames.Random, ControllerNames.Questions, ActionParameterNames.c, Model.SelectedCategories.Select(x => x.ID)) %>

</div>

</asp:Content>
