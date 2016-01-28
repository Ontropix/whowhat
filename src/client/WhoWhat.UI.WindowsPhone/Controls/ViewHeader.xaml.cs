using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.ViewModels.Feeds;
using WhoWhat.UI.WindowsPhone.ViewModels.Question;
using WhoWhat.UI.WindowsPhone.ViewModels.UserProfile;
using WhoWhat.UI.WindowsPhone.Views.UserProfile;

namespace WhoWhat.UI.WindowsPhone.Controls
{
    /*
     * We know it's now not very MVVM but it much simplier to do it right inside code behind.
     */

    public partial class ViewHeader : UserControl
    {
        private AppSettings settings;
        public ViewHeader()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            settings.NotificationsCountChanged -= SettingsOnNotificationsCountChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            settings = IoC.Get<AppSettings>();
            int notificationCount = settings.NotificationsCount;
            settings.NotificationsCountChanged += SettingsOnNotificationsCountChanged;

            Counter.Text = notificationCount.ToString();

            bool isAuthenticated = settings.IsAuthenticated;

            NotificationsIndicator.Visibility = isAuthenticated && notificationCount != 0 ? Visibility.Visible : Visibility.Collapsed;
            AskQuestionButton.Visibility = isAuthenticated ? Visibility.Visible : Visibility.Collapsed;
            NotificationButton.Visibility = isAuthenticated ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SettingsOnNotificationsCountChanged(object sender, EventArgs eventArgs)
        {
            //Update the notification count indicator
            int notificationCount = settings.NotificationsCount;

            Dispatcher.BeginInvoke(() =>
            {
                Counter.Text = notificationCount.ToString();
                bool isAuthenticated = settings.IsAuthenticated;

                NotificationsIndicator.Visibility = isAuthenticated && notificationCount != 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            });

        }

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof (string), typeof (ViewHeader), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void AskQuestionTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            IoC.Get<INavigationService>().UriFor<AskQuestionViewModel>().Navigate();
        }

        private void NotificationsTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UserProfileView userProfileView =
                ((PhoneApplicationFrame) Application.Current.RootVisual).Content as UserProfileView;

            if (userProfileView != null) //User profile is opened
            {
                UserProfileViewModel viewModel = (UserProfileViewModel) userProfileView.DataContext;
                viewModel.OpenNotificationTab();
            }
            else
            {
                IoC.Get<INavigationService>().UriFor<UserProfileViewModel>()
                    .WithParam(vm => vm.UserId, IoC.Get<AppSettings>().UserId) //current user
                    .WithParam(vm => vm.IsNotificationsActive, true) //set notification tab as active
                    .Navigate();
            }
        }

        private void HomeTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            INavigationService navigationService = IoC.Get<INavigationService>();

            //Clear back stack, left only the first item (home screen)
            while (navigationService.BackStack.Count() > 1)
            {
                navigationService.RemoveBackEntry();
            }

            if (navigationService.CanGoBack)
            {
                navigationService.GoBack();
            }
            else
            {
                IoC.Get<INavigationService>().UriFor<FeedsViewModel>().Navigate();
            }
        }
    }
}
