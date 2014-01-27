<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListView.aspx.cs" Inherits="WingtipToys.DataBindingTests.ListView" %>
<%@ Register assembly="WingtipToys" namespace="WingtipToys.DataBindingTests" tagprefix="wtp" %>
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
        <asp:ListView ID="ObjectDataSourceListView" runat="server" DataSourceID="ObjectDataSource1">
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
                <wtp:CustomParameter Name="custom" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ListView ID="ManualDataBindListView" runat="server">
            <LayoutTemplate>
                <ul>
                    <li runat="server" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li><%# Eval("Id") %></li>
            </ItemTemplate>
        </asp:ListView>
        <asp:ListView ID="ModelBindingListView" runat="server" SelectMethod="GetItems">
            <LayoutTemplate>
                <ul>
                    <li runat="server" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li><%# Eval("Id") %></li>
            </ItemTemplate>
        </asp:ListView>
        <asp:ListView ID="SqlDataSourceListView" runat="server" DataSourceID="SqlDataSource">
            <LayoutTemplate>
                <ul>
                    <li runat="server" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li><%# Eval("ProductName") %></li>
            </ItemTemplate>
        </asp:ListView>
        <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:WingtipToys%>" SelectCommand="SELECT * FROM Products WHERE ProductName = @Name">
            <SelectParameters>
                <asp:SessionParameter Name="Name" SessionField="Name" DefaultValue="paper boat"/>
            </SelectParameters>
            <FilterParameters>
                <asp:QueryStringParameter Name="Filter" QueryStringField="Test"/>
            </FilterParameters>
        </asp:SqlDataSource>
        <asp:ListView ID="LinqDataSourceListView" runat="server" DataSourceID="LinqDataSource">
            <LayoutTemplate>
                <ul>
                    <li runat="server" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li><%# Eval("ProductName") %></li>
            </ItemTemplate>
        </asp:ListView>
        <asp:LinqDataSource ID="LinqDataSource" runat="server" ContextTypeName="WingtipToys.Models.ProductContext" TableName="Products" Where="ProductName = @Name">
            <SelectParameters>
                <asp:RouteParameter Name="Select" RouteKey="SomeRouteKey"/>
            </SelectParameters>
            <WhereParameters>
                <asp:SessionParameter Name="Name" SessionField="Name" DefaultValue="paper boat"/>
            </WhereParameters>
            <OrderByParameters>
                <asp:QueryStringParameter Name="Order" QueryStringField="Test" DefaultValue="ProductName"/>
            </OrderByParameters>
            <GroupByParameters>
                <asp:CookieParameter Name="Group" CookieName="SomeCookie"/>
            </GroupByParameters>
            <OrderGroupsByParameters>
                <asp:FormParameter Name="OrderGroups" FormField="SomeFormKey"/>
            </OrderGroupsByParameters>
        </asp:LinqDataSource>
    </div>
    </form>
</body>
</html>
