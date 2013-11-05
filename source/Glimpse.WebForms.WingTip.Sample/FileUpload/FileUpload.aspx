<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="WingtipToys.FileUpload.FileUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <br/>
    File is not saved. Just use for testing Files field in the Request Tab.
    <br/>
    <asp:FileUpload ID="File1" runat="server" /> 
    <asp:Button ID="Upload" runat="server" Text="Upload file" OnClick="Upload_Click" />
    
    <asp:Label ID="UploadResult" runat="server" ></asp:Label>

</asp:Content>