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
            CheckBoxList1.Items.Add(new ListItem("Item One (checked)", "B"));
            CheckBoxList1.Items.Add(new ListItem("Item Two (selected)", "C"));
            CheckBoxList1.Items.Add(new ListItem("Item Three (disabled)", "D"));
            CheckBoxList1.Items[1].Selected = true;
            CheckBoxList1.Items[2].Selected = false;
            CheckBoxList1.Items[3].Enabled = false;
            
            #endregion

            #region " DropDownList "

            DropDownList1.Items.Add(new ListItem("Item Zero (default)", "A"));
            DropDownList1.Items.Add(new ListItem("Item One (checked)", "B"));
            DropDownList1.Items.Add(new ListItem("Item Two (selected)", "C"));
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
    }
}