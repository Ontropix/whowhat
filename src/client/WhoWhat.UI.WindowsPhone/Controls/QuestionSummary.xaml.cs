using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WhoWhat.UI.WindowsPhone.Controls
{

    public class TagTappedEvenArgs : EventArgs
    {
        public string Tag { get; set; }
    }

    public partial class QuestionSummary : UserControl
    {
        public event EventHandler AuthorTapped = (s, e) => { };
        public event EventHandler QuestionTapped = (s, e) => { };
        public event EventHandler<TagTappedEvenArgs> TagTapped = (s, e) => { };


        public static readonly DependencyProperty IsOwnQuestionProperty = DependencyProperty.Register(
            "IsOwnQuestion", typeof (bool), typeof (QuestionSummary), new PropertyMetadata(default(bool)));

        public bool IsOwnQuestion
        {
            get { return (bool) GetValue(IsOwnQuestionProperty); }
            set { SetValue(IsOwnQuestionProperty, value); }
        }

        public QuestionSummary()
        {
            InitializeComponent();
        }

        private void OnAuthorTapped(object sender, GestureEventArgs e)
        {
            AuthorTapped(this, EventArgs.Empty);
        }

        private void OnQuestonTapped(object sender, GestureEventArgs e)
        {
            QuestionTapped(this, EventArgs.Empty);
        }

        private void OnTagTapped(object sender, GestureEventArgs e)
        {
            string text = ((Button)sender).DataContext as string;
            TagTapped(this, new TagTappedEvenArgs()
            {
                Tag = text
            });
        }
    }
}
