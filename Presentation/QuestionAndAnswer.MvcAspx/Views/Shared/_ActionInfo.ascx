<%@ Control Language="C#" Inherits="ViewUserControl<ActionInfo>" %>

<%: Html.HiddenFor(x => x.PresenterName) %>   
<%: Html.HiddenFor(x => x.ActionName) %>   

<% using (Html.BeginCollection(() => Model.Parameters)) {

    foreach (var parameter in Model.Parameters)
    {
        using (Html.BeginCollectionItem())
        {
            Html.RenderPartial(ViewNames._ActionParameterInfo, parameter);
        }
    }
}%>
