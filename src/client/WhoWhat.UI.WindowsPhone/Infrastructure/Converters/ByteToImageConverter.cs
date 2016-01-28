using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.Converters
{
    public class ByteToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                MemoryStream ms = new MemoryStream((byte[]) value);
                bitmapImage.SetSource(ms);
                return bitmapImage;
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
