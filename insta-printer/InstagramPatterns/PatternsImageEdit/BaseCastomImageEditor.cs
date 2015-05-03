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
    class BaseCastomImageEditor : AbstractImageEditor
    {
        public BaseCastomImageEditor()
        {
            defaultFont = new Font(new FontFamily("Arial"), (int)(horisontalPixels * 0.026));
            textBrash = Brushes.Black;//new SolidBrush(Color.FromArgb(255, 23, 104, 169));
            aspectRatio = 1.5;
            
            ImageOpen.Filter = "image|*jpg;*png;*bmp|All|*.*";
        }

        private Bitmap logo = Properties.Resources.InstaPrinterSamara;
        private Bitmap footer = null;
        protected bool isElepssProfilePicture = true;
        protected bool isLogo = true;

        public override Bitmap EditImage(DownloadedMedia media)
        {
            this.media = media;
            if (media == null)
                return null;

            Bitmap photo = new Bitmap(media.Photo.GetStream());// DowlandBitmap(new Uri(media.Images.StandardResolution.Url));

            Bitmap profilePictur = new Bitmap(media.User.ProfilePicture.GetStream());// DowlandBitmap(new Uri(media.User.ProfilePicture));

            int height = (int)Math.Round(horisontalPixels * aspectRatio);
            int width = horisontalPixels;

            Bitmap image = new Bitmap(width, height);

            using (Graphics gImage = Graphics.FromImage(image))
            {
                gImage.Clear(bacground);

                gImage.DrawImage(photo, new Rectangle(0, percentageOfWidth(18), horisontalPixels, horisontalPixels));


                Rectangle profilePicturSize =  new Rectangle(0, 0, percentageOfWidth(16), percentageOfWidth(16));
                if (isElepssProfilePicture)
                {
                    using (Bitmap TextureBrushProfilePictur = new Bitmap(profilePictur, new Size(profilePicturSize.Width, profilePicturSize.Height)))
                    {
                        gImage.FillEllipse(new TextureBrush(TextureBrushProfilePictur), profilePicturSize);
                    }
                }
                else gImage.DrawImage(profilePictur, profilePicturSize);

                string userName = media.User.FullName;
                if (media.User.FullName.Length > 18)
                    userName = media.User.FullName.Substring(0, 17) + "...";

                gImage.DrawString(userName, defaultFont, textBrash,
                    new PointF(percentageOfWidth(18), percentageOfWidth(6.7)));

                if (isLogo)
                {
                    int inPrWidth = logo.Width * percentageOfWidth(16) / logo.Height;
                    gImage.DrawImage(logo, new Rectangle(width - inPrWidth, 0, inPrWidth, percentageOfWidth(16)));
                }
                else
                {
                    gImage.DrawString(media.CreatedTime.ToString("dd MMM HH:mm"), defaultFont, Brushes.Gray,
                        new PointF(percentageOfWidth(74), percentageOfWidth(6.7)));

                    gImage.DrawImage(Properties.Resources.clock, new Rectangle(percentageOfWidth(70), percentageOfWidth(6.7), percentageOfWidth(3.7), percentageOfWidth(3.7)));
                }

                if (footer != null)
                {
                    try
                    {
                        int ww = width;
                        int hh = height - percentageOfWidth(118);
                        int xx = 0;
                        int yy = 0;

                        if (horisontalPixels * footer.Height > hh * footer.Width)
                        {
                            xx = ww;
                            ww = hh * footer.Width / footer.Height;
                            xx -= ww;
                            xx /= 2;
                        }
                        else
                        {
                            yy = hh;
                            hh = ww * footer.Height / footer.Width;
                            yy -= hh;
                            yy /= 2;
                        }
                        gImage.DrawImage((Bitmap)footer.Clone(), new Rectangle(xx, percentageOfWidth(118) + yy, ww, hh));
                    }
                    catch { }
                }
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

            CheckBox isElepss = new CheckBox();
            isElepss.Content = "Круглый аватар";
            isElepss.IsChecked = true;
            isElepss.Checked += isElepss_Checked;
            isElepss.Unchecked += isElepss_Checked;
            panel.Children.Add(isElepss);


            panel.Children.Add(new Separator());

            CheckBox isLogo = new CheckBox();
            isLogo.Content = "Использовать логотип";
            isLogo.IsChecked = true;
            isLogo.Checked += isLogo_Checked;
            isLogo.Unchecked += isLogo_Checked;
            panel.Children.Add(isLogo);

            Button OpenLogoButton = new Button();
            OpenLogoButton.Content = "Загрузить лого";
            OpenLogoButton.Click += OpenLogoButton_Click;
            OpenLogoButton.Margin = new System.Windows.Thickness(2);
            panel.Children.Add(OpenLogoButton);

            panel.Children.Add(new Separator());

            Button OpenFooterButton = new Button();
            OpenFooterButton.Content = "Загрузить подвал";
            OpenFooterButton.Click += OpenFooterButton_Click;
            OpenFooterButton.Margin = new System.Windows.Thickness(2);
            panel.Children.Add(OpenFooterButton);

            panel.Children.Add(new Separator());

            TextBlock tb = new TextBlock();
            tb.Text = "Цвет фона";
            panel.Children.Add(tb);

            TextBox colorBox = new TextBox();
            colorBox.Text = "FFFFFF";
            colorBox.TextChanged += colorBox_TextChanged;
            panel.Children.Add(colorBox);

            return panel;
        }

        void isElepss_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            isElepssProfilePicture = (sender as CheckBox).IsChecked == true;
            OnUpdateImage(EditImage(media));
        }

        void isLogo_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            isLogo = (sender as CheckBox).IsChecked == true;
            OnUpdateImage(EditImage(media));
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
                OnUpdateImage(EditImage(media));
            }
            catch
            {
                tb.Background = System.Windows.Media.Brushes.Pink;
            }
        }

        private OpenFileDialog ImageOpen = new OpenFileDialog();

        void OpenFooterButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ImageOpen.ShowDialog() == true)
            {
                footer = new Bitmap(ImageOpen.FileName);
                    OnUpdateImage(EditImage(media));
                
            }
        }

        private void OpenLogoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ImageOpen.ShowDialog() == true)
            {
                logo = new Bitmap(ImageOpen.FileName);
                OnUpdateImage(EditImage(media));

            }
        }
    }
}
