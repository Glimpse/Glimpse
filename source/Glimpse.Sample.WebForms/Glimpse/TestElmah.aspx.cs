using System;
using System.Linq;
using System.Reflection;

namespace Glimpse.Sample.WebForms.Glimpse
{
    public partial class TestElmah : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateExceptionsDropDownList();
            }
        }

        private void PopulateExceptionsDropDownList()
        {
            var exceptionType = (typeof(Exception));
            var exceptionTypes = Assembly
                .GetAssembly(exceptionType)
                .GetTypes()
                .Where(t => t.IsSubclassOf(exceptionType))
                .ToList();

            ExceptionsDropDownList.DataValueField = "FullName";
            ExceptionsDropDownList.DataTextField = "Name";
            ExceptionsDropDownList.DataSource = exceptionTypes;
            ExceptionsDropDownList.DataBind();
        }

        protected void ThrowExceptionButton_OnClick(object sender, EventArgs e)
        {
            var exceptionType = Type.GetType(ExceptionsDropDownList.SelectedValue);
            var exception = (Exception)Activator.CreateInstance(exceptionType);
            throw exception;
        }
    }
}