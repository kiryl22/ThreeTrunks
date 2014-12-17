using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace ThreeTrunks.UI.Handlers
{
    public class HttpImageHandler : IHttpHandler
    {
        private int _width;
        private int _height;

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string imageUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(context.Request.Params["img"]))
            {
                imageUrl = context.Request.Params["img"];
            }
            else
            {
                //ToDo: default image url
                //imageUrl = HttpContext.Current.Server.MapPath(Constants.Profile.DefaultAvatarVirtualUrl);
            }
            if (!imageUrl.ToLower().Contains("http"))
            {
                imageUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host
                    + ":" + HttpContext.Current.Request.Url.Port + imageUrl;
            }
            if (!string.IsNullOrWhiteSpace(context.Request.Params["width"]))
            {
                if (!int.TryParse(context.Request.Params["width"], out _width))
                {
                    _width = 0;
                }
            }

            if (!string.IsNullOrWhiteSpace(context.Request.Params["height"]))
            {
                if (!int.TryParse(context.Request.Params["height"], out _height))
                {
                    _height = 0;
                }
            }
            context.Response.Clear();
            context.Response.ContentType = GetContentType(imageUrl);
            byte[] imgBuffer = GetResizedImage(imageUrl, _width, _height);
            if (imgBuffer != null)
                context.Response.OutputStream.Write(imgBuffer, 0, imgBuffer.Length);
            context.Response.End();
        }

        private byte[] GetResizedImage(string path, int width, int height)
        {
            var wRequest = (HttpWebRequest)WebRequest.Create(path);
            var wResponse = (HttpWebResponse)wRequest.GetResponse();
            Bitmap imgIn = null;

            try
            {
                var stream = wResponse.GetResponseStream();
                if (stream != null)
                {
                    imgIn = new Bitmap(stream);
                    return GetResizedImageFromBitmap(path, imgIn, width, height);
                }

            }
            catch
            {
                //ToDo: default image url
                //if (imgIn != null) imgIn.Dispose();
                //FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(Constants.Profile.DefaultAvatarVirtualUrl),
                //    FileMode.Open, FileAccess.Read);
                //using (Bitmap defaultImg = new Bitmap(fs))
                //{
                //    return GetResizedImageFromBitmap(path, defaultImg, width, height);
                //}
            }
            return null;
        }

        private string GetContentType(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp":
                    return "Image/bmp";
                case ".jpeg":
                case ".jpg":
                    return "Image/jpeg";
                case ".png":
                    return "Image/png";
                case ".gif":
                    return "Image/gif";
            }
            return "";
        }

        private ImageFormat GetImageFormat(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".jpeg":
                case ".jpg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".gif":
                    return ImageFormat.Gif;
            }
            return ImageFormat.Png;
        }

        private byte[] GetResizedImageFromBitmap(string path, Bitmap imgIn, int width, int height)
        {
            double x = imgIn.Width;
            double y = imgIn.Height;

            double factor = 1;

            if (width < 0 || height < 0)
            {
                width = Convert.ToInt32(x);
                height = Convert.ToInt32(y);
            }
            else if (width > 0 && height > 0)
            {
                factor = (x >= y) ? (width / x) : (height / y);
            }
            else
            {
                factor = (width == 0) ? (height / y) : (width / x);
            }

            using (Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor)))
            {
                using (Graphics g = Graphics.FromImage(imgOut))
                {
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        g.Clear(Color.White);
                        g.DrawImage(imgIn, new Rectangle(0, 0, (int)(x * factor), (int)(y * factor)),
                                    new Rectangle(0, 0, (int)(x), (int)(y)), GraphicsUnit.Pixel);
                        imgOut.Save(outStream, GetImageFormat(path));
                        return outStream.ToArray();
                    }
                }
            }
        }
    }
}