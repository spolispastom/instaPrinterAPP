using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InstagramPatterns.InstagramApi;
using System.Drawing;

namespace InstagramPatterns.PatternsImageEdit
{
    public interface IImageEditor
    {
        Bitmap EditImage(DownloadedMedia media);
        UIElement GetController();
        void SetAspectRatio(double aspectRatio);

        event UpdateImaged UpdateImage;
    }
    public delegate void UpdateImaged(object sender, Bitmap newImage);
}
