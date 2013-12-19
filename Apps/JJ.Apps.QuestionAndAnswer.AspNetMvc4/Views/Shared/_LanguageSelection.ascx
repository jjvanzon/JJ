<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LanguageSelectionViewModel>" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.Resources" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.ViewModels" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Views" %>
<%@ Import Namespace="JJ.Apps.QuestionAndAnswer.AspNetMvc4.Controllers.Helpers" %>

<script>
    $(document).ready(function () {
        $("#selectedLanguageDropDownListBox").change(function () {
            var cultureName = $(this).val();
            window.location.href = "<%:Url.Action(ActionNames.SetLanguage)%>?<%:ActionParameterNames.cultureName%>=" + cultureName;
        });
    });
</script>

<%: Labels.Language %>

<%: 
    Html.DropDownListFor(
        x => x.SelectedLanguageCultureName,
        Model.Languages.Select(x => new SelectListItem 
        { 
            Value = x.CultureName, 
            Text = x.Name
        }),
        htmlAttributes: new { id = "selectedLanguageDropDownListBox" })
%>
