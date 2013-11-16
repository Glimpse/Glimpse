<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopulatedControls.aspx.cs" Inherits="WingtipToys.PopulatedControls" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Glimpse ViewState Tests</title>
    <webopt:BundleReference runat="server" Path="~/Content/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>This page has miscellaneous populated ASP.NET Web Forms Controls</h1>
        
        <h2>Standard Toolbox (WebControls)</h2>

        <h3>asp:AdRotator</h3>
        <asp:AdRotator ID="AdRotator1" runat="server" />
        
        <h3>asp:BulletedList</h3>
        <asp:BulletedList ID="BulletedList1" runat="server"></asp:BulletedList>
        
        <h3>asp:Button</h3>
        <asp:Button ID="Button1" runat="server" Text="Button" />
        
        <h3>asp:Calendar</h3>
        <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
        
        <h3>asp:Checkbox</h3>
        <asp:CheckBox ID="CheckBox1Checked" runat="server" />
        <asp:CheckBox ID="CheckBox2UnChecked" runat="server" />
        
        <h3>asp:CheckBoxList</h3>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server"></asp:CheckBoxList>
        
        <h3>asp:DropDownList</h3>
        <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
        
        <h3>asp:FileUpload</h3>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        
        <h3>asp:HiddenField</h3>
        <asp:HiddenField ID="HiddenField1" runat="server" />

        <h3>asp:Hyperlink</h3>
        <asp:HyperLink ID="HyperLink1" runat="server">HyperLink</asp:HyperLink>

        <h3>asp:Image</h3>
        <asp:Image ID="Image1" runat="server" />

        <h3>asp:ImageButton</h3>
        <asp:ImageButton ID="ImageButton1" runat="server" />

        <h3>asp:ImageMap</h3>
        <asp:ImageMap ID="ImageMap1" runat="server"></asp:ImageMap>

        <h3>asp:Label</h3>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

        <h3>asp:LinkButton</h3>
        <asp:LinkButton ID="LinkButton1" runat="server">LinkButton</asp:LinkButton>

        <h3>asp:ListBox</h3>
        <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>

        <h3>asp:Literal</h3>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        <h3>asp:Localize</h3>
        <asp:Localize ID="Localize1" runat="server" meta:resourcekey="Localize1Resource1"></asp:Localize>

        <h3>asp:MultiView</h3>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View runat="server" ID="MultiView1View0"><asp:Label runat="server" Text="This is view 0."></asp:Label></asp:View>
            <asp:View runat="server" ID="MultiView1View1"><asp:Label runat="server" Text="This is view 1."></asp:Label></asp:View>
            <asp:View runat="server" ID="MultiView1View2"><asp:Label runat="server" Text="This is view 2."></asp:Label></asp:View>
        </asp:MultiView>

        <h3>asp:Panel</h3>
        <asp:Panel ID="Panel1" runat="server"></asp:Panel>

        <h3>asp:PlaceHolder</h3>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

        <h3>asp:RadioButton</h3>
        <asp:RadioButton ID="RadioButton1" runat="server" /><br />
        <asp:RadioButton ID="RadioButton2" runat="server" /><br />
        <asp:RadioButton ID="RadioButton3" runat="server" />

        <h3>asp:RadioButtonList</h3>
        <asp:RadioButtonList ID="RadioButtonList1" runat="server"></asp:RadioButtonList>

        <h3>asp:Substitution</h3>
        <asp:Substitution ID="Substitution1" runat="server" methodname="GetCurrentDateTime" />

        <h3>asp:Table</h3>
        <asp:Table ID="Table1" runat="server"></asp:Table>

        <h3>asp:TextBox</h3>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

        <h3>asp:View</h3>
        <asp:View ID="View1" runat="server"></asp:View>

        <h3>asp:Wizard</h3>
        <asp:Wizard ID="Wizard1" runat="server">
            <WizardSteps>
                <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1"></asp:WizardStep>
                <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2"></asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>

        <h3>asp:Xml</h3>
        <asp:Xml ID="Xml1" runat="server"></asp:Xml>

        <h2>HTML Toolbox (HtmlControls)</h2>

        <h3>input (Button)</h3>
        <input id="HtmlButton1" type="button" value="button" runat="server" />

        <h3>input (Reset)</h3>
        <input id="HtmlReset1" type="reset" value="reset" runat="server" />

        <h3>input (Submit)</h3>
        <input id="HtmlSubmit1" type="submit" value="submit" runat="server" />

        <h3>input (Text)</h3>
        <input id="HtmlText1" type="text" runat="server"  />

        <h3>input (File)</h3>
        <input id="HtmlFile1" type="file" runat="server" />

        <h3>input (Password)</h3>
        <input id="HtmlPassword1" type="password" runat="server" />

        <h3>input (Checkbox)</h3>
        <input id="HtmlCheckbox1" type="checkbox" runat="server" />

        <h3>input (Radio)</h3>
        <input id="HtmlRadio1" type="radio" runat="server" />

        <h3>input (Hidden)</h3>
        <input id="HtmlHidden1" type="hidden" runat="server" />

        <h3>Textarea</h3>
        <textarea id="HtmlTextArea1" cols="20" rows="2" runat="server" ></textarea>

        <h3>Table</h3>
        <table id="HtmlTable" style="width: 100%;" runat="server" >
            <tr runat="server"><th>Header 1</th><th>Header 2</th></tr>
        </table>

        <h3>Image</h3>
        <img id="HtmlImage1" alt="" src="../Images/bullet.png" runat="server" />

        <h3>Select</h3>
        <select id="HtmlSelect1" runat="server" >
        </select>

        <h3>Horizontal Rule</h3>
        <hr id="horizontalRule1" runat="server" />

        <h3>Div</h3>
        <div id="HtmlDiv1" runat="server"></div>

        <h2>Data Toolbox (WebControls)</h2>

        <h3>asp:Chart</h3>
        <asp:Chart ID="Chart1" runat="server">
            <Series>
                <asp:Series Name="Series1"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
    </form>
</body>
</html>
