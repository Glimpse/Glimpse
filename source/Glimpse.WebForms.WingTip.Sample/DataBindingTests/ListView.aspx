<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListView.aspx.cs" Inherits="WingtipToys.DataBindingTests.ListView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" runat="server" Text="some text" />
        <asp:Button ID="Button1" runat="server" Text="Search" />
        <h2>DataSource Control</h2>
        <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1">
            <LayoutTemplate>
                <ul>
                    <li runat="server" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li><%# Eval("Id") %></li>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetItems" TypeName="WingtipToys.DataBindingTests.ListView">
            <SelectParameters>
                <asp:ControlParameter ControlID="TextBox1" Name="filter" />
                <asp:QueryStringParameter QueryStringField="sort" Name="order" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ListView ID="ListView2" runat="server" SelectMethod="GetItems">
            <LayoutTemplate>
                <ul>
                    <li runat="server" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li><%# Eval("Id") %></li>
            </ItemTemplate>
        </asp:ListView>
    </div>
    </form>
</body>
</html>
