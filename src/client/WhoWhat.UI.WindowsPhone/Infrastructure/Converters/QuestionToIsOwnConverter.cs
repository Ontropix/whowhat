using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.Converters
{
    public class QuestionToIsOwnConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty AuthorIdProperty =
            DependencyProperty.Register("AuthorId", typeof (string), typeof (QuestionToIsOwnConverter), new PropertyMetadata(default(string)));

        public string AuthorId
        {
            get { return (string) GetValue(AuthorIdProperty); }
            set { SetValue(AuthorIdProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Question question = (Question) value;
            return question.Author.UserId == this.AuthorId;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
