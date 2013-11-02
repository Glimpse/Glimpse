using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WingtipToys
{
    public partial class PopulatedControls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                return;
            }

            #region " AdRotator "

            AdRotator1.Height = 100;
            AdRotator1.Width = 100;
            AdRotator1.AdvertisementFile = "AdRotatorControlTestData.xml";
            AdRotator1.BorderColor = System.Drawing.Color.Black;
            AdRotator1.BorderStyle = BorderStyle.Solid;
            AdRotator1.BorderWidth = 2;

            #endregion 

            #region " BulletedList "

            //todo: for BulletedList, the third part of the triplet appears to be if the item is enabled.  Could improve Glimpse visualizer for this.
            BulletedList1.Items.Add(new ListItem("Item Zero", "A"));
            BulletedList1.Items.Add(new ListItem("Item One", "B"));
            BulletedList1.Items.Add(new ListItem("Item Two", "C"));
            BulletedList1.Items.Add(new ListItem("Item Three (disabled)", "D"));
            BulletedList1.Items[3].Enabled = false;
            
            #endregion

            #region " Button "

            Button1.Text = "I am Button1";
            Button1.ToolTip = "Button1 Tooltip is here.";
            Button1.Width = 300;
            Button1.Height = 80;
            Button1.BackColor = System.Drawing.Color.Red;
            Button1.ForeColor = System.Drawing.Color.Blue;

            #endregion 

            #region " Calendar "

            Calendar1.DayNameFormat = DayNameFormat.FirstTwoLetters;
            Calendar1.FirstDayOfWeek = FirstDayOfWeek.Monday;
            Calendar1.Caption = "Super calendar!";
            
            #endregion 

            #region " Checkbox "
            CheckBox1Checked.Checked = true;
            CheckBox1Checked.Text = "This is a checked Checkbox.";
            CheckBox2UnChecked.Checked = false;
            CheckBox2UnChecked.Text = "This is an unchecked Checkbox.";

            #endregion

            #region " CheckboxList "

            CheckBoxList1.Items.Add(new ListItem("Item Zero (default)", "A"));
            CheckBoxList1.Items.Add(new ListItem("Item One (selected)", "B"));
            CheckBoxList1.Items.Add(new ListItem("Item Two (not selected)", "C"));
            CheckBoxList1.Items.Add(new ListItem("Item Three (disabled)", "D"));
            CheckBoxList1.Items[1].Selected = true;
            CheckBoxList1.Items[2].Selected = false;
            CheckBoxList1.Items[3].Enabled = false;
            
            #endregion

            #region " DropDownList "

            DropDownList1.Items.Add(new ListItem("Item Zero (default)", "A"));
            DropDownList1.Items.Add(new ListItem("Item One (selected)", "B"));
            DropDownList1.Items.Add(new ListItem("Item Two (not selected)", "C"));
            DropDownList1.Items.Add(new ListItem("Item Three (disabled)", "D"));
            DropDownList1.Items[1].Selected = true;
            DropDownList1.Items[2].Selected = false;
            DropDownList1.Items[3].Enabled = false;
            
            #endregion

            #region " FileUpload "

            FileUpload1.AllowMultiple = true;
            FileUpload1.Height = 80;
            FileUpload1.Width = 300;
            FileUpload1.ToolTip = "this is a file uploader";

            #endregion

            #region " HiddenField "

            HiddenField1.Value = "this is a hidden field.";

            #endregion

            #region " Hyperlink "

            HyperLink1.ForeColor = System.Drawing.Color.Gray;
            HyperLink1.NavigateUrl = "https://github.com/Glimpse/Glimpse";
            HyperLink1.Text = "Go to Glimpse project site!";
            
            #endregion

            #region " Image "

            Image1.ImageUrl = "/Images/logo.jpg";
            Image1.AlternateText = "This is a logo";
            Image1.DescriptionUrl = "this is the description URL... whatever that is.";
            
            #endregion

            #region " ImageButton "

            ImageButton1.ImageUrl = "/Images/logo.jpg";
            ImageButton1.AlternateText = "This is a logo (ImageButton)";
            ImageButton1.DescriptionUrl = "this is the description URL... whatever that is.  (ImageButton)";
            ImageButton1.PostBackUrl = "PopulatedControls.aspx?ImageButtonPostBack=1";

            #endregion

            #region " ImageMap "

            ImageMap1.ImageUrl = "/Images/logo.jpg";
            ImageMap1.HotSpotMode = HotSpotMode.PostBack;
            RectangleHotSpot hotSpot1 = new RectangleHotSpot();
            hotSpot1.PostBackValue = "hotSpot 1";
            hotSpot1.Top = 0;
            hotSpot1.Bottom = 101;
            hotSpot1.Left = 0;
            hotSpot1.Right = 181;
            hotSpot1.NavigateUrl = "PopulatedControls.aspx";
            ImageMap1.HotSpots.Add(hotSpot1);
            RectangleHotSpot hotSpot2 = new RectangleHotSpot();
            hotSpot2.PostBackValue = "hotSpot 2";
            hotSpot2.Top = 0;
            hotSpot2.Bottom = 101;
            hotSpot2.Left = 182;
            hotSpot2.Right = 362;
            hotSpot2.NavigateUrl = "PopulatedControls.aspx";
            ImageMap1.HotSpots.Add(hotSpot2);

            #endregion

            #region " Label "

            Label1.Text = "This is the best label.  Ever.";
            Label1.ForeColor = System.Drawing.Color.Orange;
            Label1.Font.Name = "Comic Sans MS";
            Label1.Font.Size = 24;

            #endregion

            #region " LinkButton "

            LinkButton1.PostBackUrl = "PopulatedControls.aspx?LinkButton1Postback=1";
            LinkButton1.Text = "This is a link button";
            LinkButton1.Font.Italic = true;

            #endregion

            #region " ListBox "

            ListBox1.Items.Add(new ListItem("Item Zero (default)", "A"));
            ListBox1.Items.Add(new ListItem("Item One (selected)", "B"));
            ListBox1.Items.Add(new ListItem("Item Two (not selected)", "C"));
            ListBox1.Items.Add(new ListItem("Item Three (disabled)", "D"));
            ListBox1.Items[1].Selected = true;
            ListBox1.Items[2].Selected = false;
            ListBox1.Items[3].Enabled = false;
            ListBox1.BorderColor = System.Drawing.Color.Blue;
            ListBox1.BorderStyle = BorderStyle.Dashed;
            ListBox1.BorderWidth = 3;

            #endregion

            #region " Literal "

            Literal1.Text = "<b>This is some literal content</b><pre>OH YEAH!!!" + Environment.NewLine + "This is on the next line.</pre>";

            #endregion

            #region " Localize "

            Localize1.Mode = LiteralMode.Encode;

            #endregion

            #region " MultiView "

            //todo: The MultiView and View controls don't seem to have any sort of visibility into their data.
            MultiView1.ActiveViewIndex = 1; 
            
            #endregion

            #region " Panel "

            Panel1.Width = 400;
            Panel1.Height = 200;
            Panel1.BorderColor = System.Drawing.Color.Green;
            Panel1.BorderWidth = 5;
            Panel1.BorderStyle = BorderStyle.Inset;

            var panel1TextBox1 = new TextBox();
            panel1TextBox1.ID = "panel1TextBox1";
            panel1TextBox1.Text = "This is the panel1TextBox1 text value.";
            panel1TextBox1.Width = 300;
            Panel1.Controls.Add(panel1TextBox1);

            #endregion

            #region " PlaceHolder "
            
            var placeHolder1TextBox1 = new TextBox();
            placeHolder1TextBox1.ID = "placeHolder1TextBox1";
            placeHolder1TextBox1.Text = "This is the placeHolder1TextBox1 text value.";
            placeHolder1TextBox1.Width = 500;
            PlaceHolder1.Controls.Add(placeHolder1TextBox1);

            #endregion

            #region " RadioButton "

            RadioButton1.Checked = true;
            RadioButton2.Checked = false;
            RadioButton1.Text = "This is RadioButton1!  The initial value is checked.";
            RadioButton2.Text = "This is RadioButton2!  The initial value is not checked.";
            RadioButton3.Text = "This is RadioButton3!  The initial value is not specified.";
            
            #endregion

            #region " RadioButtonList "

            RadioButtonList1.Items.Add(new ListItem("Item Zero (default)", "A"));
            RadioButtonList1.Items.Add(new ListItem("Item One (selected)", "B"));
            RadioButtonList1.Items.Add(new ListItem("Item Two (not selected)", "C"));
            RadioButtonList1.Items.Add(new ListItem("Item Three (disabled)", "D"));

            RadioButtonList1.Items[1].Selected = true;
            RadioButtonList1.Items[2].Selected = false;
            RadioButtonList1.Items[3].Enabled = false;
            RadioButtonList1.BorderColor = System.Drawing.Color.Gray;
            RadioButtonList1.BorderStyle = BorderStyle.Outset;
            RadioButtonList1.BorderWidth = 2;

            #endregion

            #region " Substitution "

            Substitution1.Visible = true;

            #endregion

            #region " Table "

            Table1.Rows.Add(TableHeaderRowFromStringArray(new string[] { "Column 1 Header", "Column 2 Header", "Column 3 Header" }));
            Table1.Rows.Add(TableRowFromStringArray(new string[] { "Field1 A", "Field2", "Field3" }));
            Table1.Rows.Add(TableRowFromStringArray(new string[] { "Field1 B", "Field2", "Field3" }));
            Table1.Rows.Add(TableRowFromStringArray(new string[] { "Field1 C", "Field2", "Field3" }));
            
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion

            #region "  "
            #endregion



        }


        #region " helper functions "

        public static string GetCurrentDateTime(HttpContext context)
        {
            return DateTime.Now.ToString();
        }

        public static TableRow TableRowFromStringArray(string[] cells)
        {
            var row = new TableRow();
            foreach (var cellValue in cells)
            {
                var cell = new TableCell();
                cell.Text = cellValue;
                row.Cells.Add(cell);
            }
            return row;
        }

        public static TableHeaderRow TableHeaderRowFromStringArray(string[] cells)
        {
            var row = new TableHeaderRow();
            foreach (var cellValue in cells)
            {
                var cell = new TableCell();
                cell.Text = cellValue;
                cell.Font.Bold = true;
                row.Cells.Add(cell);
            }
            return row;
        }


        #endregion

    }
}