using InstagramPatterns.InstagramApi;
using InstagramPatterns.PatternsImageEdit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstagramPrint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            saverv = new ImageSaver();
            saverv.SetDirectory(string.Format("{0}/{1}", Directory.GetCurrentDirectory(), TagBox.Text));
            SaveCheckBox.IsChecked = true;

            logger = new Logger();
            loeder = new InstagramImageDownloader(TagBox.Text);
            loeder.MediaDownloaded += loeder_MediaDownloaded;

            PrintControl.SetEditor(MainImageEditor);
            PrintControl.MediaPrinted += PrintControl_MediaPrinted;
            PrintControl.MediaLoed += ImageSaver_SaveMedia;
            Printer.PrintComplite += Printer_PrintComplite;

            EndDataTimePicker.SelectedDate = DateTime.Now.AddDays(-1);

            UpdateTagBox();
            //TwittwrApi.TwitterDownloader td = new TwittwrApi.TwitterDownloader("iphone");
        }

        private void UpdateTagBox()
        {
            string currentTag = TagBox.Text;
            TagBox.Items.Clear();
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fi = new FileInfo(files[i]);
                if (fi.Extension == ".txt")
                {
                    string tag = fi.Name.Substring(0, fi.Name.Length - 5).ToLower();

                    if (!TagBox.Items.Contains(tag))
                        TagBox.Items.Add(tag);
                }
            }
            TagBox.Text = currentTag;
        }

        void Printer_PrintComplite(object sender, DownloadedMedia media)
        {
            PrintControl.PrintMediaComplite(media);
        }

        void loeder_MediaDownloaded(object sender, DownloadedMedia media)
        {
            logger.SetDowlandedMediaID(media.Id);
        }

        void PrintControl_MediaPrinted(object sender, DownloadedMedia media)
        {
            MainImageEditor.SetAspectRatio(Printer.AspectRatio);
            Printer.PrintImage(MainImageEditor.EditImage(media), media);
            logger.SetPrintedMediaID(media.Id);
        }

       InstagramImageDownloader loeder;
       ImageSaver saverv;
       public Logger logger;

        private void PrintQueue_PageSizeChange(object sender, PrintQueueControl.PageSizeChangedEventArg a)
        {
            MainImageEditor.SetAspectRatio(a.Height / a.Width);
        }

        private void StartLoed_Click(object sender, RoutedEventArgs e)
        {
            StartLoedButton.IsEnabled = false;
            StopLoedButton.IsEnabled = true;
            ContentTypeTabBox.IsEnabled = false;
            EndDataTimePicker.IsEnabled = false;
            IsFastDowlandChackBox.IsEnabled = false;

            loeder.IsFastDowland = IsFastDowlandChackBox.IsChecked == true;
            if (EndDataTimePicker.SelectedDate != null)
                loeder.EndLoedTime = new DateTime(EndDataTimePicker.SelectedDate.Value.Year, EndDataTimePicker.SelectedDate.Value.Month, EndDataTimePicker.SelectedDate.Value.Day, 0, 0, 0);
            else loeder.EndLoedTime = DateTime.Now.AddDays(-1);

            if (ContentTypeTabBox.SelectedIndex == 0)
            {
                SaveCheckBox.IsChecked = saverv.SetDirectory(string.Format("{0}/{1}", Directory.GetCurrentDirectory(), TagBox.Text));
                logger.SetCurrentTag(TagBox.Text);
            
                List<string> idCollection = new List<string>();
                //var log = logger.GetLog();
                // foreach (var item in log)
                //  idCollection.Add(item.ID);
                loeder.RunFromTag(TagBox.Text, idCollection);
            }
            else if (ContentTypeTabBox.SelectedIndex == 1)
            {
                SaveCheckBox.IsChecked = saverv.SetDirectory(string.Format("{0}/{1}", Directory.GetCurrentDirectory(), "user"));
                logger.SetCurrentTag(UserBox.Text);

                loeder.RunFromUser(UserBox.Text);
            }
            else if (ContentTypeTabBox.SelectedIndex == 2)
            {
                SaveCheckBox.IsChecked = saverv.SetDirectory(string.Format("{0}/{1}", Directory.GetCurrentDirectory(), "shortcode"));
                logger.SetCurrentTag(ShortcodeBox.Text);

                loeder.RunFromShortcode(ShortcodeBox.Text);
            }
        }
        private void StopLoed_Click(object sender, RoutedEventArgs e)
        {
            StartLoedButton.IsEnabled = true;
            StopLoedButton.IsEnabled = false;
            ContentTypeTabBox.IsEnabled = true;
            EndDataTimePicker.IsEnabled = true;
            IsFastDowlandChackBox.IsEnabled = true;
            loeder.Cancel();

            UpdateTagBox();
        }
        private void StartPrint_Click(object sender, RoutedEventArgs e)
        {
            StartPrintButton.IsEnabled = false;
            StopPrintButton.IsEnabled = true;
            Printer.IsEnabled = false;
            PrintControl.StartPrinted();
        }
        private void StopPrint_Click(object sender, RoutedEventArgs e)
        {
            StartPrintButton.IsEnabled = true;
            StopPrintButton.IsEnabled = false;
            Printer.IsEnabled = true;
            PrintControl.StopPrinted();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            loeder.Dispose();
        }

        private void PrintControl_ImageSelected(object sender, SelectionChangedEventArgs e)
        {
            MainImageEditor.SetAspectRatio(Printer.AspectRatio);
            MainImageEditor.SetTestMedia((sender as System.Windows.Controls.Primitives.Selector).SelectedItem as DownloadedMedia);
        }

        private void SaveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!saverv.IsSetSavedDirectory && !saverv.SetDirectoryShowDialog())
            {
                SaveCheckBox.IsChecked = false;
                return;
            }
            PrintControl.MediaPrinted += ImageSaver_SaveMedia;
        }

        private void SaveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PrintControl.MediaPrinted -= ImageSaver_SaveMedia;
        }

        DataImageValueConverter dataImageValueConverter = new DataImageValueConverter();

        private void ImageSaver_SaveMedia(object sender, DownloadedMedia media)
        {
            saverv.SaveImage(MainImageEditor.EditImage(media), string.Format("P_{0} {1}", media.User.FullName, media.StringCreatedTime));
            saverv.SaveImage(new Bitmap(media.Photo.GetStream()), string.Format("O_{0} {1}", media.User.FullName, media.StringCreatedTime));
        }

        private void SetSaveFolderButton_Click(object sender, RoutedEventArgs e)
        {
            saverv.SetDirectoryShowDialog();
        }
    }
}
