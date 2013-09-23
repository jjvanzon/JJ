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

    <p>
        <input type="submit" value="<%:Titles.ShowAnswer %>" />
    </p>

    <% if (Model.AnswerIsVisible)
       { %>
            <div class="display-label">
                <%: Labels.TheCorrectAnswer %>
            </div>

            <br />

            <div class="display-field">
                <%: Html.DisplayFor(x => x.Answer) %>
            </div>
    <% } %>
        
    <br />

    <%: Html.ActionLink(Titles.NextQuestion, ActionNames.Question) %>

    <%: Html.HiddenForAllProperties(Model) %>

<% } %>

</asp:Content>
