using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WingtipToys.FileUpload
{
    public partial class FileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            {
               UploadResult.Text = "The file has been uploaded.";
            }
            else
            {
                UploadResult.Text = "File was empty.";
            }
        }
    }
}