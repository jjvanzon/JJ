<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.QuestionDetailViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers" %>

<% if (Model.Question.Flag.CanFlag) { %>

    <br />

    <div>

        <%: Html.TextAreaFor(x => x.Question.Flag.Comment) %>            

    </div>

    <div>

        <% if (!Model.Question.Flag.IsFlagged) { %>

            <input type="submit" value="<%:Titles.Flag%>" formaction="<%:Url.Action(ActionNames.Flag, ControllerNames.Questions)%>" />

        <% } %>

        <% else { %>

            <input type="submit" value="<%:Titles.Unflag%>" formaction="<%:Url.Action(ActionNames.Unflag, ControllerNames.Questions)%>" />

        <% } %>

    </div>

<% } %>
