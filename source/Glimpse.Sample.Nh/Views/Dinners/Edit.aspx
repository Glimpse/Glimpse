<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<NerdDinner.Models.DinnerFormViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    Edit: <%=Html.Encode(Model.Dinner.Title) %>
</asp:Content>

<asp:Content ID="Edit" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Dinner</h2>

    <% Html.RenderPartial("DinnerForm"); %>

</asp:Content>

