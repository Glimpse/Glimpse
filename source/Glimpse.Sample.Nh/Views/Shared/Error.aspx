<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
   <h2>Oh Snap!</h2>
    <p>Sorry, an error occurred while processing your request. We've been notified and we will check it out. If you know what you were doing, or if you're evil, let us know on our <a href="http://feedback.nerddiner.com">feedback site.</a></p>
</asp:Content>
