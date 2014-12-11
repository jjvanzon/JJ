<%@ Control Language="C#" Inherits="ViewUserControl<LanguageSelectorViewModel>" %>

<script>
    $(document).ready(function () {
        $("#selectedLanguageDropDownListBox").change(function () {
            var cultureName = $(this).val();
            window.location.href = "<%:Url.Action(ActionNames.SetLanguage)%>?<%:ActionParameterNames.cultureName%>=" + cultureName;
        });
    });
</script>

<%: CommonTitles.Language %>

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
