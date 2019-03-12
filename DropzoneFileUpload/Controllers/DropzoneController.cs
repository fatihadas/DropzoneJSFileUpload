using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DropzoneFileUpload.Controllers
{
    public class DropzoneController : Controller
    {
        // GET: Dropzone
        public ActionResult Index()
        {
            if (Request.Files.Count > 0)
            {
                var durum = true;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpfb = Request.Files[file];
                    string path = "/App_Data/" + "resim-" + hpfb.FileName;
                    try
                    {
                        Request.Files[file].SaveAs(Server.MapPath(path));
                        //dbcontext save()
                        //KucukBoyut(System.Drawing.Image.FromStream(hpfb.InputStream), 160, hpfb.FileName);
                    }
                    catch
                    {
                        durum = false;
                        throw;
                    }
                }
                return (durum == true) ? Json(new { Message = "kaydedildi" }, JsonRequestBehavior.AllowGet) :
                    Json(new { Message = "hata" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
            //return View();
        }


        public void KucukBoyut(System.Drawing.Image yuklenenFoto, int resimBoyutu, string resimIsmi)
        {
            System.Drawing.Bitmap islenmisFoto = null;
            System.Drawing.Graphics grafikNesnesi = null;
            int hedeflenenGenislik = resimBoyutu;
            int hedeflenenYukseklik = resimBoyutu;
            int yeni_gen, yeni_yuk;
            yeni_yuk = (int)Math.Round(((float)yuklenenFoto.Height * (float)resimBoyutu) / (float)yuklenenFoto.Width);
            yeni_gen = hedeflenenGenislik;
            hedeflenenYukseklik = yeni_yuk;
            yeni_gen = yeni_gen > hedeflenenGenislik ? hedeflenenGenislik : yeni_gen;
            yeni_yuk = yeni_yuk > hedeflenenYukseklik ? hedeflenenYukseklik : yeni_yuk;
            islenmisFoto = new System.Drawing.Bitmap(hedeflenenGenislik, hedeflenenYukseklik);
            grafikNesnesi = System.Drawing.Graphics.FromImage(islenmisFoto);
            grafikNesnesi.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), new System.Drawing.Rectangle(0, 0, hedeflenenGenislik, hedeflenenYukseklik));
            int x = (hedeflenenGenislik - yeni_gen) / 2;
            int y = (hedeflenenYukseklik - yeni_yuk) / 2;
            grafikNesnesi.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            grafikNesnesi.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            grafikNesnesi.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            System.Drawing.Imaging.ImageCodecInfo codec = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()[1];
            System.Drawing.Imaging.EncoderParameters eParams = new System.Drawing.Imaging.EncoderParameters(1);
            eParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);
            grafikNesnesi.DrawImage(yuklenenFoto, x, y, yeni_gen, yeni_yuk);

            islenmisFoto.Save(Server.MapPath("/Content/" + resimIsmi), codec, eParams);
        }


    }
}