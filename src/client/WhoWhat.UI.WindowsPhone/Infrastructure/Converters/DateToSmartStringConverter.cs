using System;
using System.Globalization;
using System.Windows.Data;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.Converters
{
    public class DateToSmartStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            TimeSpan span = DateTime.UtcNow - date;

            long seconds = Math.Abs((long)span.TotalSeconds);

            if (seconds == 0 || date == DateTime.MinValue)
            {
                return "just now";
            }

            if (seconds < 60)
            {
                return string.Format("{0} secs", seconds);
            }

            if (seconds < 60 * 60)
            {
                return string.Format("{0} mins", seconds / 60);
            }

            if (seconds < 60 * 60 * 24)
            {
                return string.Format("{0} hours", seconds / 60 / 60);
            }

            if (seconds < 60 * 60 * 24 * 7)
            {
                return string.Format("{0} days", seconds / 60 / 60 / 24);
            }


            return date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
