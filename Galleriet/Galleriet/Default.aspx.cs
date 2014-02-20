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
            get{ return _gallery ?? (_gallery = new Gallery());}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //var gallery = new Gallery();
            //Repeater1.DataSource = gallery.GetImageNames();
            //Repeater1.DataBind();

            var v = Request.QueryString["name"];

            if (v != null)
            {

                BiggImage.Visible = true;
                Gallery.ImageExists(v);

                BiggImage.ImageUrl = "Pics/" + v;
            }

            if (Request.QueryString["uploaded"] == "made")
            {
                ClosePanel.Visible = true;
                Label1.Text = string.Format("<img src=Content/Right.png /> Bilden   {0}  har sparat.", v);
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (IsValid )
            {
                if (TheFileUpload.HasFile)
                {   
                    try
                    {
                      string upLoad =  Gallery.SaveImage(TheFileUpload.FileContent, TheFileUpload.FileName);
                        Response.Redirect(String.Format("?name=" + upLoad + "&uploaded=made"));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("" , ex);
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
            ClosePanel.Visible = true;
            var close = Request.QueryString["name"];
            Response.Redirect(String.Format("?name={0}", close));
        }
    }
}