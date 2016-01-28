using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public string TrueImage { get; set; }
        public string FalseImage { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = ((bool) value) ? TrueImage : FalseImage;
            return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
