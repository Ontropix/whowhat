using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Services;
using WhoWhat.UI.WindowsPhone.Services.Model;
using WhoWhat.UI.WindowsPhone.ViewModels;
using WhoWhat.UI.WindowsPhone.ViewModels.Feeds;
using WhoWhat.UI.WindowsPhone.ViewModels.Login;
using WhoWhat.UI.WindowsPhone.ViewModels.Question;
using WhoWhat.UI.WindowsPhone.ViewModels.Search;
using WhoWhat.UI.WindowsPhone.ViewModels.UserProfile;

namespace WhoWhat.UI.WindowsPhone
{
    public class Bootstrapper : PhoneBootstrapper
    {
        PhoneContainer container;

        protected override void Configure()
        {
            container = new PhoneContainer();

            if (!Execute.InDesignMode)
                container.RegisterPhoneServices(RootFrame);


            // View model
            container.PerRequest<LoginViewModel>();
            container.PerRequest<FacebookLoginViewModel>();
            container.PerRequest<VkontakteLoginViewModel>();

            container.PerRequest<FeedsViewModel>();
            container.PerRequest<QuestionListViewModel>();

            container.PerRequest<UserProfileViewModel>();
            container.PerRequest<UserProfileInfoViewModel>();
            container.PerRequest<UserProfileQuestionsViewModel>();
            container.PerRequest<UserProfileActivityViewModel>();
            container.PerRequest<UserProfileAnswersViewModel>();

            container.PerRequest<AskQuestionViewModel>();
            container.PerRequest<ViewQuestionViewModel>();
            container.PerRequest<ImageViewModel>();
            container.PerRequest<AnswerQuestionViewModel>();

            container.PerRequest<SearchByKeywordViewModel>();
            container.PerRequest<SearchByTagViewModel>();

            container.PerRequest<AboutViewModel>();
            container.PerRequest<SettingsViewModel>();

            // Application services
            container.Singleton<AppSettings>();
            container.Singleton<ToastService>();
            container.Singleton<SingOuter>();

            // Rest services
            container.PerRequest<QuestionRestService>();
            container.PerRequest<AnswerRestService>();
            container.PerRequest<SearchRestService>();
            container.PerRequest<UsersRestService>();
            container.PerRequest<PushSharpClient>();

            AddCustomConventions();
        }

        private Timer notificationPullTimer;

        protected override void OnLaunch(object sender, LaunchingEventArgs e)
        {
            base.OnLaunch(sender, e);

            //Flurry
            FlurryWP8SDK.Api.StartSession("G7N9WKXG4MSTTBQRZQMY");

            ApplicationUsageHelper.Init("0.5");

            ThemeManager.OverrideOptions = ThemeManagerOverrideOptions.None;
            ThemeManager.ToLightTheme();
            ThemeManager.SetAccentColor((System.Windows.Media.Color)Application.Resources["WW.Color.Accent"]);

            notificationPullTimer = new Timer(async (state) =>
                {
                    bool isAuthenticated = IoC.Get<AppSettings>().IsAuthenticated;
                    if (isAuthenticated)
                    {
                        UsersRestService usersRestService = IoC.Get<UsersRestService>();
                        try
                        {
                            NotificationsCountResponse response = await usersRestService.NotificationsCount();
                            IoC.Get<AppSettings>().NotificationsCount = response.Count;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Update notification failed. " + ex.Message);
                        }
                    }


                }, null, 0, 10 * 1000);

        }

        protected override void OnActivate(object sender, ActivatedEventArgs e)
        {
            base.OnActivate(sender, e);
            ThemeManager.ToLightTheme();
            FlurryWP8SDK.Api.StartSession("G7N9WKXG4MSTTBQRZQMY");
            ApplicationUsageHelper.OnApplicationActivated();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        static void AddCustomConventions()
        {

            // App Bar Conventions
            ConventionManager.AddElementConvention<BindableAppBarButton>(
                Control.IsEnabledProperty, "DataContext", "Click");
            ConventionManager.AddElementConvention<BindableAppBarMenuItem>(
                Control.IsEnabledProperty, "DataContext", "Click");

            ConventionManager.AddElementConvention<Pivot>(Pivot.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) =>
                {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention))
                    {
                        ConventionManager
                            .ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, null, viewModelType);
                        return true;
                    }

                    return false;
                };


        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
