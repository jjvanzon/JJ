<%@ Control Language="C#" Inherits="ViewUserControl<LanguageSelectorPartialViewModel>" %>

<script>
    $(document).ready(function () {
        $("#langDdl").change(function () {
            var cultureName = $(this).val();

            //var url = window.location.pathname + '?<%:ActionParameterNames.lang%>=' + cultureName;
            var url = JJ.Framework.Url.setParameter(window.location.href, "<%:ActionParameterNames.lang%>", cultureName);

            if (window.document.forms.length == 0) {
                window.location.href = url;
            }
            else {
                window.document.forms[0].action = url;
                window.document.forms[0].submit();
            }
        });
    });
</script>

<%: CommonTitlesFormatter.Language %>

<%: 
    Html.DropDownListFor(
        x => x.SelectedLanguageCultureName,
        Model.Languages.Select(x => new SelectListItem 
        { 
            Value = x.CultureName, 
            Text = x.Name
        }),
        htmlAttributes: new { id = "langDdl" })
%>
