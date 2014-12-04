<%@ Control Language="C#" Inherits ="ViewUserControl<RandomQuestionViewModel>" %>

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
