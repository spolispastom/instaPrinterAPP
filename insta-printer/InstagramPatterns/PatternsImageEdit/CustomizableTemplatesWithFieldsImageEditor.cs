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
    class CustomizableTemplatesWithFieldsImageEditor : AbstractImageEditor
    {
        public CustomizableTemplatesWithFieldsImageEditor()
        {
            defaultFont = new Font(new FontFamily("Arial"), (int)(horisontalPixels * 0.026), FontStyle.Bold);
            textBrash = Brushes.Black;//new SolidBrush(Color.FromArgb(255, 23, 104, 169));
            aspectRatio = 1.5;
            
            ImageOpen.Filter = "image|*jpg;*png;*bmp|All|*.*";
            footer = Properties.Resources.bigvillBackground;
        }

        private Bitmap logo = Properties.Resources.InstaPrinterSamara;
        private Bitmap footer = null;
        protected bool isElepssProfilePicture = true;
        protected bool isLogo = true;
        private double margin = 2;

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

                if (footer != null)
                {
                    try
                    {
                        int ww = width;
                        int hh = height;
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
                        gImage.DrawImage((Bitmap)footer.Clone(), new Rectangle(xx, yy, ww, hh));
                    }
                    catch { }
                }


                gImage.DrawImage(photo, new Rectangle(percentageOfWidth(margin), percentageOfWidth(18 + margin), percentageOfWidth(100 - 2 * margin), percentageOfWidth(100 - 2 * margin)));

                Rectangle profilePicturSize = new Rectangle(percentageOfWidth(margin), percentageOfWidth(margin), percentageOfWidth(16), percentageOfWidth(16));
                
                if (isElepssProfilePicture)
                {
                    Rectangle profilePicturBuffSize = new Rectangle(percentageOfWidth(0), percentageOfWidth(0), percentageOfWidth(16), percentageOfWidth(16));
                
                    Bitmap profilePicturBuff = new Bitmap(profilePicturBuffSize.Width, profilePicturBuffSize.Height);

                    using (Bitmap TextureBrushProfilePictur = new Bitmap(profilePictur, new Size(profilePicturBuffSize.Width, profilePicturBuffSize.Height)))
                    {
                        using (Graphics gProfilePictur = Graphics.FromImage(profilePicturBuff))
                        {
                            gProfilePictur.FillEllipse(new TextureBrush(TextureBrushProfilePictur), profilePicturBuffSize);
                            gProfilePictur.DrawEllipse(Pens.Silver, profilePicturBuffSize);
                        }
                    }

                    gImage.DrawImage(profilePicturBuff, profilePicturSize);
                }
                else gImage.DrawImage(profilePictur, profilePicturSize);

                string userName = media.User.FullName;
                if (media.User.FullName.Length > 18)
                    userName = media.User.FullName.Substring(0, 17) + "...";

                gImage.DrawString(userName, defaultFont, textBrash,
                    new PointF(percentageOfWidth(18 + margin), percentageOfWidth(6.0 + margin)));

                if (isLogo)
                {
                    int inPrWidth = logo.Width * percentageOfWidth(16) / logo.Height;
                    gImage.DrawImage(logo, new Rectangle(width - inPrWidth - percentageOfWidth(margin), percentageOfWidth(margin), inPrWidth, percentageOfWidth(16)));
                }
                else
                {
                    gImage.DrawString(media.CreatedTime.ToString("dd MMM HH:mm"), defaultFont, Brushes.Gray,
                        new PointF(percentageOfWidth(74 - margin), percentageOfWidth(6.0 + margin)));

                    gImage.DrawImage(Properties.Resources.clock, new Rectangle(percentageOfWidth(70 - margin ), percentageOfWidth(6.2 + margin), percentageOfWidth(3.7), percentageOfWidth(3.7)));
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

            StackPanel marginPanel = new StackPanel();
            marginPanel.Orientation = Orientation.Vertical;
            marginPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            marginPanel.Width = 200;

            TextBlock marginText = new TextBlock();
            marginText.Text = "Отступы";
            marginPanel.Children.Add(marginText);

            Slider marginSlider = new Slider();
            marginSlider.Minimum = 0;
            marginSlider.Maximum = 10;
            marginSlider.Value = 2;
            marginSlider.ValueChanged += marginSlider_ValueChanged;

            marginPanel.Children.Add(marginSlider);
            panel.Children.Add(marginPanel);

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

        void marginSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            margin = ((Slider)sender).Value;
            OnUpdateImage(EditImage(media));
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
