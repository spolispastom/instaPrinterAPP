using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using InstagramPatterns.InstagramApi;

namespace InstagramPatterns.PatternsImageEdit
{
    class InstaSryleImageEdit : AbstractImageEditor 
    {
        public InstaSryleImageEdit()
        {
            defaultFont = new Font(new FontFamily("Arial"), (int)(horisontalPixels * 0.026));
            textBrash = new SolidBrush(Color.FromArgb(255, 23, 104, 169));
            aspectRatio = 4.0 / 3.0;
        }

        private string brandName;

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

                gImage.DrawImage(profilePictur, new Rectangle(0, 0, percentageOfWidth(16), percentageOfWidth(16)));

                string userName = media.User.FullName;
                if (media.User.FullName.Length > 18)
                    userName = media.User.FullName.Substring(0, 17) + "...";

                if (media.LocationName == null || media.LocationName == "")
                {
                    gImage.DrawString(userName, defaultFont, textBrash,
                        new PointF(percentageOfWidth(18), percentageOfWidth(6.7)));
                }
                else
                {
                    gImage.DrawString(userName, defaultFont, textBrash,
                        new PointF(percentageOfWidth(18), percentageOfWidth(3.7)));
                    gImage.DrawString(media.LocationName, defaultFont, textBrash,
                        new PointF(percentageOfWidth(22), percentageOfWidth(9.7)));
                    gImage.DrawImage(Properties.Resources.location, 
                        new Rectangle(percentageOfWidth(18), percentageOfWidth(9.7), percentageOfWidth(3.7), percentageOfWidth(3.7)));

                }
                //gImage.DrawString(media.CreatedTime.ToString("dd MMM HH:mm"), defaultFont, Brushes.Gray,
                //    new PointF(percentageOfWidth(74), percentageOfWidth(6.7)));

                //gImage.DrawImage(Properties.Resources.clock, new Rectangle(percentageOfWidth(70), percentageOfWidth(6.7), percentageOfWidth(3.7), percentageOfWidth(3.7)));
                
                double likeTop = 119.7;
                if (media.Likes.Count > 0)
                {
                    gImage.DrawImage(Properties.Resources.like, new Rectangle(percentageOfWidth(2), percentageOfWidth(120), percentageOfWidth(3.7), percentageOfWidth(3.7)));

                    string likes = "";
                    int likesLen = 0;
                    for (int i = 0; i < media.Likes.Count && i <= 10; i++)
                    {
                        var l = media.Likes[i];
                        if (likesLen + l.FullName.Length > 55)
                        {
                            likesLen = 0;

                            gImage.DrawString(likes, defaultFont, textBrash, new PointF(percentageOfWidth(7), percentageOfWidth(likeTop)));

                            likes = "";
                            likeTop += 4.7;
                        }

                        likes += string.Format("{0}, ", l.FullName);
                        likesLen += l.FullName.Length;
                    }
                    if (likes.Length > 0)
                    {
                        likes = likes.Remove(likes.Length - 3);
                        gImage.DrawString(likes, defaultFont, textBrash, new PointF(percentageOfWidth(7), percentageOfWidth(likeTop)));

                        likeTop += 4.7;
                    }
                }

                if (media.Comments.Count > 0)
                {
                    likeTop += 1;
                    //нарисовать знак комента
                    gImage.DrawImage(Properties.Resources.coment, new Rectangle(percentageOfWidth(2), percentageOfWidth(likeTop), percentageOfWidth(3.7), percentageOfWidth(3.7)));
                    int left = 0;
                    int wordWidth = 0;
                    foreach (var c in media.Comments)
                    {
                        gImage.DrawString(c.User.FullName, defaultFont, textBrash, new PointF(percentageOfWidth(7), percentageOfWidth(likeTop)));

                        left = percentageOfWidth(7) + (int)gImage.MeasureString(c.User.FullName, defaultFont).Width;
                        string[] words = c.Text.Split(' ');
                        for (int i = 0; i < words.Length; i++)
                        {
                            wordWidth = (int)gImage.MeasureString(words[i], defaultFont).Width;

                            if (left + wordWidth > percentageOfWidth(90))
                            {
                                likeTop += 4.7;
                                left = percentageOfWidth(7);
                                if (percentageOfWidth(likeTop) + percentageOfWidth(7) > height) break;
                            }

                            gImage.DrawString(words[i], defaultFont, Brushes.Gray,
                                new PointF(left, percentageOfWidth(likeTop)));
                            left += wordWidth;

                        }

                        likeTop += 4.7;
                        if (percentageOfWidth(likeTop) > height) break; //проверить
                    }
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

            TextBlock brandNameTextBlock = new TextBlock();
            brandNameTextBlock.Text = "Название компании.";
            brandNameTextBlock.Margin = new System.Windows.Thickness(2);

            TextBox brandNameTextBox = new TextBox();
            brandNameTextBox.Text = brandName;
            brandNameTextBox.TextChanged += brandNameTextBox_TextChanged;
            brandNameTextBox.Margin = new System.Windows.Thickness(2);

            panel.Children.Add(brandNameTextBlock);
            panel.Children.Add(brandNameTextBox);

            return panel;
        }

        void brandNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            brandName = ((TextBox)sender).Text;
            OnUpdateImage(EditImage(media));
        }
    }
}
