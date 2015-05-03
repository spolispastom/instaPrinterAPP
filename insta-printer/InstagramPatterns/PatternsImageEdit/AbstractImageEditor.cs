using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InstagramPatterns.InstagramApi;

namespace InstagramPatterns.PatternsImageEdit
{
    abstract class AbstractImageEditor : IImageEditor
    {
        protected Bitmap DowlandBitmap(Uri addres)
        {
            while (true)
            {
                try
                {
                    WebClient wClient = new WebClient();
                    byte[] imageByte = wClient.DownloadData(addres);
                    using (MemoryStream ms = new MemoryStream(imageByte, 0, imageByte.Length))
                    {
                        return new Bitmap(ms);
                    }
                }
                catch
                { }
            }
        }

        protected System.Drawing.Brush textBrash;
        protected Font defaultFont;
        protected DownloadedMedia media;
        protected int horisontalPixels = 1280;
        protected Color bacground = Color.White;

        protected int percentageOfWidth(double percentage)
        { return (int)(horisontalPixels * percentage / 100); }

        abstract public Bitmap EditImage(DownloadedMedia media);

        abstract public System.Windows.UIElement GetController();

        public event UpdateImaged UpdateImage;
        protected  void OnUpdateImage(Bitmap newImage)
        {
            if (UpdateImage != null)
                UpdateImage(this, newImage);
        }

        protected double aspectRatio = 1.5;
        public void SetAspectRatio(double aspectRatio)
        {
            this.aspectRatio = aspectRatio;
        }
    }
}
