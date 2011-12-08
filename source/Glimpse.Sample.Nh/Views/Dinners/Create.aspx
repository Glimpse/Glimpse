<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<NerdDinner.Models.DinnerFormViewModel>" MasterPageFile="~/Views/Shared/Site.Master"  %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    Host a Nerd Dinner
</asp:Content>

<asp:Content ID="Create" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Host a Dinner</h2>

    <% Html.RenderPartial("DinnerForm"); %>

</asp:Content>

