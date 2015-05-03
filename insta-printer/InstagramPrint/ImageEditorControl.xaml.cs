using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InstagramPatterns.PatternsImageEdit;
using InstagramPatterns.InstagramApi;

namespace InstagramPrint
{
    /// <summary>
    /// Логика взаимодействия для ImageEditorControl.xaml
    /// </summary>
    partial class ImageEditorControl : UserControl, IImageEditor
    {
        public ImageEditorControl()
        {
            testmedia = InstagramImageDownloader.GetMedia("instacube");

            editor = ImageEditorsFactory.Create(TypeImageEditors.InstaStyle);
            aspectRatio = 1.5;
            editor.SetAspectRatio(aspectRatio);

            InitializeComponent();

            LoedImageEditors();

            UpdateTest();
        }

        public void SetTestMedia(DownloadedMedia media)
        {
            testmedia = media;
            UpdateTest();
        }

        private void LoedImageEditors()
        {
            var typesImageEditors = Enum.GetValues(typeof(TypeImageEditors));
            foreach (var v in typesImageEditors)
            {
                ListBoxItem ti = new ListBoxItem();
                ti.VerticalAlignment = VerticalAlignment.Stretch;
                ti.HorizontalAlignment = HorizontalAlignment.Stretch;
                ti.Width = 190;
                ti.Name = v.ToString();

                Grid gr = new Grid();
                gr.HorizontalAlignment = HorizontalAlignment.Stretch;
                gr.VerticalAlignment = VerticalAlignment.Stretch;
                gr.RowDefinitions.Add(new RowDefinition());
                gr.RowDefinitions.Add(new RowDefinition());
                gr.RowDefinitions[1].Height = new GridLength(0);

                TextBlock header = new TextBlock();
                header.Margin = new Thickness(1);
                header.Text = v.ToString();
                Grid.SetRow(header, 0);
                gr.Children.Add(header);

                ti.VerticalAlignment = VerticalAlignment.Top;
                IImageEditor ie = ImageEditorsFactory.Create((TypeImageEditors)Enum.Parse(typeof(TypeImageEditors), v.ToString()));
                ie.UpdateImage += ie_UpdateImage;
                UIElement controller = ie.GetController();
                Grid.SetRow(controller, 1);
                gr.Children.Add(controller);

                ti.Content = gr;
                SettingListBox.Items.Add(ti);
            }
            SettingListBox.SelectedIndex = 0;
        }

        void ie_UpdateImage(object sender, Bitmap newImage)
        {
            ImageBox.Source = converter.Convert(newImage);
            if (UpdateImage != null)
                UpdateImage(sender, newImage);
        }

        IImageEditor editor = null;
        private DownloadedMedia testmedia;
        private BitmapValueComverter converter = new BitmapValueComverter();

        private void UpdateTest()
        {
            if (testmedia != null)
                ImageBox.Source = converter.Convert(EditImage(testmedia));
        }

        public Bitmap EditImage(DownloadedMedia media)
        {
            return editor.EditImage(media);
        }

        private void SettingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectItem = (ListBoxItem)SettingListBox.SelectedItem;
            foreach (ListBoxItem item in SettingListBox.Items)
            {
                ((Grid)item.Content).RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                item.VerticalAlignment = VerticalAlignment.Stretch;
            }

            ((Grid)selectItem.Content).RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);

            editor = ImageEditorsFactory.Create((TypeImageEditors)Enum.Parse(typeof(TypeImageEditors), selectItem.Name));
            editor.SetAspectRatio(aspectRatio);
            UpdateTest();
        }

        public UIElement GetController()
        {
            return this;
        }

        public event UpdateImaged UpdateImage;

        protected double aspectRatio = 3 / 4;
        public void SetAspectRatio(double aspectRatio)
        {
            this.aspectRatio = aspectRatio;
            editor.SetAspectRatio(aspectRatio);
            UpdateTest();
        }
    }
}
