using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ThreeTrunks.UI.Helpers
{
    public class ImageProccesor
    {
        public static void CreateThumbnail(string fileLoction, string thumbnailLocation, int width, int height)
        {
            using (var bitmap = GetBitmap(fileLoction, width, height))
            {
                if (bitmap != null)
                {
                    bitmap.Save(thumbnailLocation, GetImageFormat(fileLoction));
                }
                else
                {
                    //ToDo: create default thumbnail
                }
            }
        }

        private static Bitmap GetBitmap(string fileLoction, int width, int height)
        {
            Bitmap bmpOut = null;

            try
            {
                var loBmp = new Bitmap(fileLoction);

                decimal lnRatio;
                var lnNewWidth = 0;
                var lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBmp.Width < width && loBmp.Height < height)
                    return loBmp;

                if (loBmp.Width > loBmp.Height)
                {
                    lnRatio = (decimal)width / loBmp.Width;
                    lnNewWidth = width;
                    decimal lnTemp = loBmp.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)height / loBmp.Height;
                    lnNewHeight = height;
                    decimal lnTemp = loBmp.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBmp, 0, 0, lnNewWidth, lnNewHeight);

                loBmp.Dispose();
                g.Dispose();
            }
            catch
            {
                return null;
            }

            return bmpOut;
        }

        private static ImageFormat GetImageFormat(string path)
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
    }
}