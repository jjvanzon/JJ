<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<JJ.Framework.Presentation.ActionDescriptor>" %>

<%: Html.HiddenFor(x => x.PresenterName) %>   
<%: Html.HiddenFor(x => x.ActionName) %>   

<% using (Html.BeginCollection(() => Model.Parameters)) {

    foreach (var parameter in Model.Parameters)
    {
        using (Html.BeginCollectionItem())
        {
            Html.RenderPartial(ViewNames._ParameterDescriptor, parameter);
        }
    }
}%>
