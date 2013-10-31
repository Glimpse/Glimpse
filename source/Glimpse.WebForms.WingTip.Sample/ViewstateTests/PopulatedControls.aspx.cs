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
            
            //todo: Do AdRotator

            #region " BulletedList "
            
            //todo: for BulletedList, the third part of the triplet appears to be if the item is enabled.
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

            

        }
    }
}