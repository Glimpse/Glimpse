<%@ Page Title="WebMethods and Post/Redirect/Get Tests" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebMethodsAndPostRedirectGetTests.aspx.cs" Inherits="WingtipToys.WebMethodsAndPostRedirectGetTests" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>
    
    <asp:Button runat="server" Text="Redirect to default page" ID="buttonRedirectToDefaultPage" />
    <asp:Button runat="server" Text="Server.Transfer to default page" ID="buttonServerTransferToDefaultPage" />
    <asp:Button runat="server" Text="Throw exception during processing" ID="buttonThrowAnException" />

    <div id="the-time">
    </div>
    <input type="button" id="get-time" value="Get The Time" />

    <script type="text/javascript">

        $(function () {
            $("#get-time").click(getServerTime);
            document.forms[0].target = "_blank";
        });

        function getServerTime() {
            $.ajax({
                type: "POST",
                url: "WebMethodsAndPostRedirectGetTests.aspx/GetTheTime",
                contentType: "application/json",
                dataType: "json",
                success: function (msg) {
                    $('#the-time').text(msg.d);
                }
            });
        }
    </script>
</asp:Content>