using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using InstagramPatterns;

namespace InstagramPatterns.InstagramApi
{
    public class DataImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataImage sourse = ((DataImage)value);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = sourse.GetStream();
            bi.EndInit();

            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
