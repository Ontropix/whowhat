using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.ViewModels.Question;

namespace WhoWhat.UI.WindowsPhone.Views.Question
{
    public partial class ViewQuestionView
    {
        ViewQuestionViewModel ViewModel { get; set; }

        public ViewQuestionView()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel = (ViewQuestionViewModel)DataContext;

            ViewModel.PropertyChanged += (o, args) =>
            {
                if (args.PropertyName == GetPropertyName(prop => prop.IsCurrentUserAuthor) ||
                    args.PropertyName == GetPropertyName(prop => prop.Question))
                {
                    ApplicationBar = Default();
                }
                else if (args.PropertyName == GetPropertyName(prop => prop.IsQuestionEdit) &&
                         ViewModel.IsQuestionEdit == false)
                {
                    ApplicationBar = Default();
                }
                else if (args.PropertyName == GetPropertyName(prop => prop.IsBusy) && ApplicationBar != null)
                {
                    //Hide app bar on busy
                    ApplicationBar.IsVisible = !ViewModel.IsBusy;
                }
            }; 
        }

        public static string GetPropertyName<TReturn>(Expression<Func<ViewQuestionViewModel, TReturn>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }

        public ApplicationBar Default()
        {
            var bar = CreateBar();

            if (ViewModel.IsCurrentUserAuthor)
            {
                var delete = new ApplicationBarIconButton()
                {
                    IconUri = new Uri("Assets/AppBar/delete.png", UriKind.Relative),
                    Text = "delete",
                };

                delete.Click += (sender, args) => ViewModel.RemoveQuestion();

                bar.Buttons.Add(delete);

                var edit = new ApplicationBarIconButton()
                {
                    IconUri = new Uri("Assets/AppBar/edit.png", UriKind.Relative),
                    Text = "edit",
                    IsEnabled = ViewModel.CanQuestionBeEdited
                };

                edit.Click += (sender, args) =>
                {
                    ViewModel.EditQuestion();
                    ApplicationBar = QuestionEdit();
                };

                bar.Buttons.Add(edit);
            }

            var answer = new ApplicationBarIconButton()
            {
                IconUri = new Uri("Assets/AppBar/appbar.chat.png", UriKind.Relative),
                Text = "answer"
            };

            answer.Click += (sender, args) => ViewModel.Answer();

            bar.Buttons.Add(answer);

            return bar;
        }

        private static ApplicationBar CreateBar()
        {
            var bar = new ApplicationBar();
            bar.ForegroundColor = Colors.White;
            bar.BackgroundColor = (Color)Application.Current.Resources["WW.Color.Accent"];
            return bar;
        }

        public ApplicationBar QuestionEdit()
        {
            var bar = CreateBar();

            var cancel = new ApplicationBarIconButton()
            {
                IconUri = new Uri("Assets/AppBar/cancel.png", UriKind.Relative),
                Text = AppResources.AppBar_Cancel,
            };

            cancel.Click += (sender, args) => ViewModel.CancelEditQuestion();

            bar.Buttons.Add(cancel);

            var save = new ApplicationBarIconButton()
            {
                IconUri = new Uri("Assets/AppBar/save.png", UriKind.Relative),
                Text = AppResources.AppBar_Save,
            };

            save.Click += async (sender, args) =>
            {
                //Removal of focus from the TagsLine to the Page will force the bind.
                this.Focus();

                // Wait till the next UI thread tick so that the binding gets updated
                await Task.Yield();

                ViewModel.SaveEditedQuestion();
            };

            bar.Buttons.Add(save);

            return bar;
        }

        public ApplicationBar AnswerEdit()
        {
            var bar = CreateBar();

            var cancel = new ApplicationBarIconButton()
            {
                IconUri = new Uri("Assets/AppBar/cancel.png", UriKind.Relative),
                Text = AppResources.AppBar_Cancel,
            };

            cancel.Click += (sender, args) =>
            {
                ViewModel.CancelEditAnswer();
                ApplicationBar = Default();
            };

            bar.Buttons.Add(cancel);

            var save = new ApplicationBarIconButton()
            {
                IconUri = new Uri("Assets/AppBar/save.png", UriKind.Relative),
                Text = AppResources.AppBar_Save,
            };

            save.Click += (sender, args) =>
            {
                ViewModel.SaveEditedAnswer();
                ApplicationBar = Default();
            };

            bar.Buttons.Add(save);

            return bar;
        }

        private void OnQuestionBodyLoaded(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Focus();

            //Set cursor at the end
            textBox.Select(textBox.Text.Length, 0);
        }

        private void OnAnswerEdit(object sender, GestureEventArgs e)
        {
            ApplicationBar = AnswerEdit();
        }

        private void AnswerEditedBodyLostFocus(object sender, RoutedEventArgs e)
        {
            ApplicationBar = Default();
        }

    }
}