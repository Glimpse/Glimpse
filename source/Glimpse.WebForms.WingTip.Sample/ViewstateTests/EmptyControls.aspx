<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmptyControls.aspx.cs" Inherits="WingtipToys.ViewstateTest" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Glimpse ViewState Tests</title>
    <webopt:BundleReference runat="server" Path="~/Content/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>This page has miscellaneous ASP.NET Web Forms controls</h1>
        
        <h2>asp:AdRotator</h2>
        <asp:AdRotator ID="AdRotator1" runat="server" />
        
        <h2>asp:BulletedList</h2>
        <asp:BulletedList ID="BulletedList1" runat="server"></asp:BulletedList>
        
        <h2>asp:Button</h2>
        <asp:Button ID="Button1" runat="server" Text="Button" />
        
        <h2>asp:Calendar</h2>
        <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
        
        <h2>asp:Checkbox</h2>
        <asp:CheckBox ID="CheckBox1" runat="server" />
        
        <h2>asp:CheckBoxList</h2>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server"></asp:CheckBoxList>
        
        <h2>asp:DropDownList</h2>
        <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
        
        <h2>asp:FileUpload</h2>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        
        <h2>asp:HiddenField</h2>
        <asp:HiddenField ID="HiddenField1" runat="server" />

    </div>
    </form>
</body>
</html>
