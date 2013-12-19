<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers" %>

<h3><%:Titles.FlagContent %></h3>

<p>
    <% if (!Model.Flag.IsFlagged)
       { %>
            <%: Html.ActionLink(Titles.Flag, ActionNames.Flag, ControllerNames.Questions) %>
    <% } 
       else
       { %>
            <%: Html.ActionLink(Titles.Unflag, ActionNames.Unflag, ControllerNames.Questions) %>
    <% } %>

    <% if (Model.Flag.IsFlagged)
       { %>
            <%: Html.TextAreaFor(x => x.Flag.Comment) %>            
    <% } %>
</p>