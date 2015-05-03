using InstagramPatterns.InstagramApi;
using InstagramPatterns.PatternsImageEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Xps;
using InstagramPatterns;

namespace InstagramPrint
{
    /// <summary>
    /// Логика взаимодействия для PrintQueueControl.xaml
    /// </summary>
    public partial class PrintQueueControl : UserControl, IDisposable
    {
        public PrintQueueControl()
        {
            this.InitializeComponent();
   
            queueDownloadedImage = ConcurrentQueueDownloadedImage.Instance;

            timer.Interval = new TimeSpan(500);
            timer.Tick += timer_Tick;

            importQueueSourseView.Filter = TextFilter;
            printedQueueSourseView.Filter = TextFilter;
            canseledQueueSourseView.Filter = TextFilter;

            importQueueSourseView.SortDescriptions.Add(new SortDescription("CreatedTime", ListSortDirection.Descending));
            printedQueueSourseView.SortDescriptions.Add(new SortDescription("CreatedTime", ListSortDirection.Descending));
            canseledQueueSourseView.SortDescriptions.Add(new SortDescription("CreatedTime", ListSortDirection.Descending));

            this.Loaded += (s, e) => { timer.Start(); };
        }

        BitmapValueComverter bitmapConverter = new BitmapValueComverter();

        private bool Contains(DownloadedMedia media)
        {
            if (importQueueSourse.Where(x => x.Id == media.Id).Count() > 0) return true;
            if (printedQueueSourse.Where(x => x.Id == media.Id).Count() > 0) return true;
            if (canseledQueueSourse.Where(x => x.Id == media.Id).Count() > 0) return true;
            return false; 
        }

        DispatcherTimer timer = new DispatcherTimer();
        DateTime printTime = DateTime.Now;
        TimeSpan PrintInterval = new TimeSpan(0, 0, 10);
        void timer_Tick(object sender, object e)
        {
            ImpCountBox.Text = importQueueSourse.Count.ToString();
            PrintedCountBox.Text = printedQueueSourse.Count.ToString();
            CanseledCountBox.Text = canseledQueueSourse.Count.ToString();
            DownloadedMedia item = queueDownloadedImage.Dequeue();
            while (item != null)
            {
                if (!Contains(item))
                {
                    try
                    {
                        var lw = Logger.log.Where(x => x.ID == item.Id);
                        if (lw != null && lw.First() != null && lw.First().IsPrinted == true)
                        {
                            printedQueueSourse.Add(item);
                        }
                        else
                        {
                            importQueueSourse.Add(item);
                        }
                    }
                    catch
                    { importQueueSourse.Add(item); }

                    OnMediaLoed(item);
                }
                item = queueDownloadedImage.Dequeue();
                //ImputImages.ScrollIntoView(ImputImages.Items[ImputImages.Items.Count - 1]);
            }

            if (isPrinted  && importQueueSourse.Count > 0)
            {
                if (DateTime.Now - printTime > PrintInterval)
                {
                    printTime = DateTime.Now;
                    DownloadedMedia printedMedia = importQueueSourse[importQueueSourse.Count - 1];

                    App.Current.Dispatcher.BeginInvoke((ThreadStart)delegate
                    {
                        PrintMedia(printedMedia);
                    });
                }
            }       
        }

        public void PrintMediaComplite(DownloadedMedia media)
        {
            printedQueueSourse.Add(media);
            importQueueSourse.Remove(media);
        }

        private void PrintMedia(DownloadedMedia media)
        {
            OnMediaPrinted(media);
        } 

        IImageEditor editor = null;
        private bool isPrinted = false;
        
        public void SetEditor(IImageEditor Editor)
        { editor = Editor; }

        public bool StartPrinted()
        {
            isPrinted = true;
            return true;
        }
        public void StopPrinted()
        { isPrinted = false; }

        public delegate void PageSizeChanged(object sender, PageSizeChangedEventArg a);

        public struct PageSizeChangedEventArg
        {
            public PageSizeChangedEventArg(double width, double height)
            {
                this.Width = width;
                this.Height = height;
            }

            public double Width;
            public double Height;
        }

        public event PageSizeChanged PageSizeChange;

        private void OnPageSizeChange(double width, double height)
        {
            if (PageSizeChange != null)
                PageSizeChange(this, new PageSizeChangedEventArg(width, height));
        }

        ConcurrentQueueDownloadedImage queueDownloadedImage;

        static ObservableCollection<DownloadedMedia> importQueueSourse = new ObservableCollection<DownloadedMedia>();
        private ICollectionView importQueueSourseView = CollectionViewSource.GetDefaultView(importQueueSourse);
        public ICollectionView ImportQueueSourse
        {
            get { return importQueueSourseView; }
        }

        static ObservableCollection<DownloadedMedia> printedQueueSourse = new ObservableCollection<DownloadedMedia>();
        private ICollectionView printedQueueSourseView = CollectionViewSource.GetDefaultView(printedQueueSourse);
        public ICollectionView PrintedQueueSourse
        {
            get { return printedQueueSourseView; }
        }

        static ObservableCollection<DownloadedMedia> canseledQueueSourse = new ObservableCollection<DownloadedMedia>();
        private ICollectionView canseledQueueSourseView = CollectionViewSource.GetDefaultView(canseledQueueSourse);
        public ICollectionView CanseledQueueSourse
        {
            get { return canseledQueueSourseView; }
        }

        private void ImputImagesItemButtonClick(object sender, RoutedEventArgs e)
        {
            DownloadedMedia canseledMedia = ((Button)sender).DataContext as DownloadedMedia;
            canseledQueueSourse.Add(canseledMedia);
            importQueueSourse.Remove(canseledMedia);
        }
        private void PrintedQueueItemButtonClick(object sender, RoutedEventArgs e)
        {
            DownloadedMedia canseledMedia = ((Button)sender).DataContext as DownloadedMedia;
            importQueueSourse.Add(canseledMedia);
            printedQueueSourse.Remove(canseledMedia);
        }
        private void CanseledQueueItemButtonClick(object sender, RoutedEventArgs e)
        {
            DownloadedMedia canseledMedia = ((Button)sender).DataContext as DownloadedMedia;
            importQueueSourse.Add(canseledMedia);
            canseledQueueSourse.Remove(canseledMedia);
        }

        public void Dispose()
        {
        }

        public event SelectionChangedEventHandler ImageSelected;
        private void Images_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (ImageSelected != null)
                ImageSelected(sender,  e);
        }

        public delegate void MediaPrintedd(object sender, DownloadedMedia media);

        public event MediaPrintedd MediaPrinted;

        private void OnMediaPrinted(DownloadedMedia media)
        {
            if (MediaPrinted != null)
                MediaPrinted(this, media);
        }

        public delegate void MediaLoedd(object sender, DownloadedMedia media);

        public event MediaLoedd MediaLoed;

        private void OnMediaLoed(DownloadedMedia media)
        {
            if (MediaLoed != null)
                MediaLoed(this, media);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            importQueueSourseView.Refresh();
            printedQueueSourseView.Refresh();
            canseledQueueSourseView.Refresh();
        }

        public bool TextFilter(object o)
        {
            DownloadedMedia m = (o as DownloadedMedia);
            if (m == null)
                return false;

            if (SearchTextBox.Text == "")
                return true;

            if (m.User.FullName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                m.StringCreatedTime.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                m.User.Name.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                m.User.Id.ToLower().Contains(SearchTextBox.Text.ToLower()))
                return true;
            else
                return false;
        }
    }
}
