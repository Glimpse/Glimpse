<%@ Page Title="Test Elmah Page" Language="C#" AutoEventWireup="true" CodeBehind="TestElmah.aspx.cs" Inherits="Glimpse.Sample.WebForms.Glimpse.TestElmah" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    
    <head>
        
        <title>Elmah plugin for Glimpse</title>

    </head>

    <body>
        
        <form id="Form" runat="server">
            <asp:DropDownList ID="ExceptionsDropDownList" runat="server"></asp:DropDownList>
            <asp:Button ID="ThrowExceptionButton" runat="server" Text="Throw" OnClick="ThrowExceptionButton_OnClick" />
        </form>

    </body>

</html>