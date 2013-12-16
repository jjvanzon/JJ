<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HeaderViewModel>" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.ViewModels" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views" %>

<div id="languageDiv">
    <% Html.RenderPartial(ViewNames._Language, Model.Language); %>
</div>

<div id="smallLoginDiv">
    <% Html.RenderPartial(ViewNames._SmallLogin, Model.Login); %>
</div>
