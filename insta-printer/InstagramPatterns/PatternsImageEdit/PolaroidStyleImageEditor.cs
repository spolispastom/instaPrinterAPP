using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using InstagramPatterns.InstagramApi;
using System.Drawing.Design;

namespace InstagramPatterns.PatternsImageEdit
{
    class PolaroidStyleImageEditor : AbstractImageEditor
    {
        public PolaroidStyleImageEditor()
        {
            defaultFont = new Font(new FontFamily("Arial"), (int)(horisontalPixels * 0.026));
            textBrash = new SolidBrush(Color.FromArgb(255, 23, 104, 169));
            aspectRatio = 4.0 / 3.0;
            
            ImageOpen.Filter = "image|*jpg;*png;*bmp|All|*.*";
        }

        private string logoFileName = "";

        public override Bitmap EditImage(DownloadedMedia media)
        {
            this.media = media;
            if (media == null)
                return null;

            Bitmap photo = new Bitmap(media.Photo.GetStream());// DowlandBitmap(new Uri(media.Images.StandardResolution.Url));

            int height = (int)Math.Round(horisontalPixels * aspectRatio);
            int width = horisontalPixels;

            Bitmap image = new Bitmap(width, height);

            using (Graphics gImage = Graphics.FromImage(image))
            {
                gImage.Clear(bacground);

                gImage.DrawImage(photo, new Rectangle(0, 0, horisontalPixels, horisontalPixels));

                try
                {
                    using (Bitmap logo = new Bitmap(logoFileName))
                    {
                        int ww = width;
                        int hh = height - width;
                        int xx = 0;
                        int yy = 0;

                        if (horisontalPixels * logo.Height > hh * logo.Width)
                        {
                            xx = ww;
                            ww = hh * logo.Width / logo.Height;
                            xx -= ww;
                            xx /= 2;
                        }
                        else
                        {
                            yy = hh;
                            hh = ww * logo.Height / logo.Width;
                            yy -= hh;
                            yy /= 2;
                        }
                        gImage.DrawImage((Bitmap)logo.Clone(), new Rectangle(xx, width + yy, ww, hh));
                    }
                }
                catch { }

            }
            return image;
        }


        private static System.Windows.UIElement controller = null;

        public override System.Windows.UIElement GetController()
        {
            if (controller == null)
                controller = CreateController();

            return controller;
        }

        private System.Windows.UIElement CreateController()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            Button OpenLogoButton = new Button();
            OpenLogoButton.Content = "Загрузить логотип";
            OpenLogoButton.Click += OpenLogoButton_Click;
            OpenLogoButton.Margin = new System.Windows.Thickness(2);

            panel.Children.Add(OpenLogoButton);

            TextBlock tb = new TextBlock();
            tb.Text = "Цвет фона";

            panel.Children.Add(tb);

            TextBox colorBox = new TextBox();
            colorBox.Text = "FFFFFF";
            colorBox.TextChanged += colorBox_TextChanged;

            return panel;
        }

        void colorBox_TextChanged(object sender, TextChangedEventArgs e)
        {
                TextBox tb = sender as TextBox;
            try
            {
                if (tb.Text.Length != 6) throw new ArgumentException();

                int r = int.Parse(tb.Text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g = int.Parse(tb.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b = int.Parse(tb.Text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                bacground = Color.FromArgb(r, g, b);

                tb.Background = System.Windows.Media.Brushes.White;
            }
            catch
            {
                tb.Background = System.Windows.Media.Brushes.Pink;
            }
        }

        private OpenFileDialog ImageOpen = new OpenFileDialog();

        void OpenLogoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ImageOpen.ShowDialog() == true)
            {
                logoFileName = ImageOpen.FileName;
                    OnUpdateImage(EditImage(media));
                
            }
        }
    }
}
