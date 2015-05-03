using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Drawing;
using System.Drawing.Printing;
using System.Printing;
using System.Windows.Xps;
using InstagramPatterns.InstagramApi;
using System.Threading;
using System.Windows.Threading;

namespace InstagramPrint 
{
    /// <summary>
    /// Логика взаимодействия для CastomPrinterControl.xaml
    /// </summary>
    public partial class CastomPrinterControl : UserControl
    {
        public CastomPrinterControl()
        {
            InitializeComponent();
        }

        private bool isReady = true;

        public bool IsReady
        { 
            get { return isReady; }
            private set 
            { 
                isReady = value;
                if (isReady)
                    OnStateChange(PrinterState.Ready);
                else OnStateChange(PrinterState.None);
            }
        }

        PrintDialog printDialog = new PrintDialog();

        private int pageWidth = 1280;

        public int PageWidth
        { get { return pageWidth; } }
        private int pageHeint = 1920;

        public int PageHeint
        { get { return pageHeint; } }

        BitmapValueComverter bitmapConverter = new BitmapValueComverter();

        public void PrintImage(Bitmap image, DownloadedMedia media)
        {
            this.Dispatcher.InvokeAsync((Action)(() =>
            {
                switch (FormapBox.SelectedIndex)
                {
                    case 0:
                        printDialog.PrintVisual(ToDrawingVisual10x15(image), string.Format("{0}_{1}", media.UserName, media.CreatedTime));
                        break;
                    case 1:
                        printDialog.PrintVisual(ToDrawingVisual5x7(image), string.Format("{0}_{1}", media.UserName, media.CreatedTime));
                        break;
                }
            }));
            OnPrintComplite(media);
        }

        private Visual ToDrawingVisual10x15(Bitmap image)
        {
            double zoom = 0.96;

            int width = (int)(pageWidth * zoom);
            int height = (int)(pageHeint * zoom);

            int x = 0;//(int)((1 - zoom) * width / 2);
            int y = (int)((1 - zoom) * width / 2);
            if (width * image.Height > height * image.Width)
            {
                x = width;
                width = height * image.Width / image.Height;
                x -= width;
                x /= 2;
            }
            else
            {
                y = height;
                height = width * image.Height / image.Width;
                y -= height;
                y /= 2;
            }

            System.Windows.Media.DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dContext = dv.RenderOpen())
            {
                dContext.DrawImage(bitmapConverter.Convert(image), new System.Windows.Rect(x + (int)(width * 0.023), y, width, height));
            }
            return dv;
        }

        private Visual ToDrawingVisual5x7(Bitmap image)
        {
            double zoom = 0.96;

            int width = (int)((pageHeint / 2) * zoom);
            int height = (int)(pageWidth * zoom);

            int x = 0;//(int)((1 - zoom) * width / 2);
            int y = (int)((1 - zoom) * width / 2);
            if (width * image.Height > height * image.Width)
            {
                x = width;
                width = height * image.Width / image.Height;
                x -= width;
                x /= 2;
            }
            else
            {
                y = height;
                height = width * image.Height / image.Width;
                y -= height;
                y /= 2;
            }

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dContext = dv.RenderOpen())
            {
                dContext.DrawImage(bitmapConverter.Convert(image), new System.Windows.Rect(x + (int)(width * 0.075), y + (int)(width * 0.025), width, height));
            }
            return dv;
        }

        public delegate void PrintComplited(object sender, DownloadedMedia media);
        public event PrintComplited PrintComplite;
        private void OnPrintComplite(DownloadedMedia media)
        {
            if (PrintComplite != null)
                PrintComplite(this, media);
        }
        public delegate void StateChanged (object sender, PrinterState state);
        public event StateChanged StateChange;
        private void OnStateChange(PrinterState state)
        {
            if (StateChange != null)
                StateChange(this, state);
        }

        public enum PrinterState
        {
            Ready,
            None
        }

        public double AspectRatio
        { 
            get
            {
                switch (FormapBox.SelectedIndex)
                { 
                    case 0:
                        return 1.5;
                    case 1:
                        return 4.0/3.0;
                    default: 
                        return 1.4;
                }
            }
        }

        private void DialogButton_Click(object sender, RoutedEventArgs e)
        {
            if (printDialog.ShowDialog() == true)
            {
                pageWidth = (int)printDialog.PrintableAreaWidth;
                pageHeint = (int)printDialog.PrintableAreaHeight;
            }
        }
    }
}
