<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Mobile/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    NerdDinner.com
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1><img alt="nerddinner" src="/Content/Img/Mobile/logo_medium.jpg" runat="server" /></h1>
    <h2>Find a Nerd Dinner near you!</h2>
    <% using (Html.BeginForm("SearchByPlaceNameOrZip", "Search", FormMethod.Post)) { %>
    <div id="searchBox">
        <input type="text" name="placeOrZip" id="placeOrZip" maxlength="56" onfocus="this.value = ''" value="" /><br />
        <input id="search" type="submit" value="Search" />
    </div>
    <% } %>
</asp:Content>
