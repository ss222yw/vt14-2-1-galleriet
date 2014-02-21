using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Galleriet.Model;

namespace Galleriet
{
    public partial class Default : System.Web.UI.Page
    {
        // privat fält
        private Gallery _gallery;

        // egenskap till fältet som retunerar ett värde .
        // _gallery är null , ?? skapar ett nytt objekt med värde när det behövs .  
        private Gallery Gallery
        {
            get { return _gallery ?? (_gallery = new Gallery()); }
        }

        // Privat fält för meddlande.
        private string Message
        {
            get
            {
                string message = Session["responseMessage"] as string;
                Session.Remove("responseMessage");
                return message;
            }
            set
            {
                Session["responseMessage"] = value;
            }
        }

        // Publik bool som retunerar true eller false.
        public bool MessageExists
        {
            get
            {
                return Session["responseMessage"] != null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var v = Request.QueryString["name"];

            if (v != null && Gallery.ImageExists(v))
            {

                FullImage.Visible = true;
                FullImage.ImageUrl = "Pics/" + v;
            }

            if (MessageExists)
            {
                ResponsePanel.Visible = true;
                Label1.Text = Message;
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (TheFileUpload.HasFile)
                {
                    try
                    {
                        string upLoad = Gallery.SaveImage(TheFileUpload.FileContent, TheFileUpload.FileName);
                        Message = String.Format("<img src=Content/Right.png /> Bilden {0} är sparad", upLoad);
                        Response.Redirect(String.Format("?name=" + upLoad));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex);
                    }
                }

            }
        }

        //bildernas filnamn sorterade i bokstavsordning.
        public IEnumerable<Galleriet.Model.ThumImgUrl> Repeater1_GetData()
        {
            //var gallery = new Gallery();
            //return gallery.GetImageNames();
            return Gallery.GetImageNames();
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void closeImg_Click(object sender, ImageClickEventArgs e)
        {
            ResponsePanel.Visible = true;
            var close = Request.QueryString["name"];
            Response.Redirect(String.Format("?name={0}", close));
        }
    }
}