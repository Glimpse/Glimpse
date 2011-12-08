<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
   <h2>Confused?</h2>
    <p>Are you lost? Try taking a look at the complete list of <a href="/Dinners">Upcoming Dinners</a>.</p>
</asp:Content>
