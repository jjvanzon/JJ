<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Apps.QuestionAndAnswer.ViewModels.RandomQuestionViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Mvc.Names" %>

<% if (Model.CurrentUserQuestionFlag.CanFlag) { %>

    <br />

    <div>

        <%: Html.TextAreaFor(x => x.CurrentUserQuestionFlag.Comment) %>            

    </div>

    <div>

        <% if (!Model.CurrentUserQuestionFlag.IsFlagged) { %>

            <input type="submit" value="<%:Titles.FlagContent%>" formaction="<%:Url.Action(ActionNames.Flag, ControllerNames.Questions)%>" />

        <% } %>

        <% else { %>

            <input type="submit" value="<%:Titles.UnflagContent%>" formaction="<%:Url.Action(ActionNames.Unflag, ControllerNames.Questions)%>" />

        <% } %>

    </div>

<% } %>
