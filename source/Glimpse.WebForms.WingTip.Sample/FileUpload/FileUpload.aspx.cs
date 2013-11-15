using System;

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
                byte[] input = new byte[File1.PostedFile.ContentLength];
                File1.PostedFile.InputStream.Read(input, 0, File1.PostedFile.ContentLength);

                UploadResult.Text = "The file has been uploaded.";
            }
            else
            {
                UploadResult.Text = "File was empty.";
            }
        }
    }
}