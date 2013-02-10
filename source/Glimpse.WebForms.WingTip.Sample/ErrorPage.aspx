<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"  CodeBehind="ErrorPage.aspx.cs" Inherits="WingtipToys.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <h2>Error:</h2>
    <p></p>
    <asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label" Font-Size="Large" style="color: red"></asp:Label>

    <asp:Panel ID="DetailedErrorPanel" runat="server" Visible="false">
        <p>
            Detailed Error:
            <br />
            <asp:Label ID="ErrorDetailedMsg" runat="server" Font-Bold="true" Font-Size="Large" /><br />
        </p>
        <p>
            Error Handler:
            <br />
            <asp:Label ID="ErrorHandler" runat="server" Font-Bold="true" Font-Size="Large" /><br />
        </p>
        <p>
            Detailed Error Message:
            <br />
            <asp:Label ID="InnerMessage" runat="server" Font-Bold="true" Font-Size="Large" /><br />
        </p>
        <pre>
            <asp:Label ID="InnerTrace" runat="server"  />
        </pre>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>


