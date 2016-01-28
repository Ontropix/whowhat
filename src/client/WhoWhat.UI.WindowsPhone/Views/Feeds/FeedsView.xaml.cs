using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.ViewModels.Feeds;
using WhoWhat.UI.WindowsPhone.ViewModels.Question;
using View = WhoWhat.UI.WindowsPhone.Infrastructure.View;
using Telerik.Windows.Controls;

namespace WhoWhat.UI.WindowsPhone.Views.Feeds
{
    public partial class FeedsView : View
    {
        public FeedsView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            FeedsViewModel vm = (FeedsViewModel) DataContext;
            if (vm.IsFirstLaunch)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                RadToolTipService.Open(TooltipRect);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e); 

            const string questionId = "QuestionId";

            if (NavigationContext.QueryString.ContainsKey(questionId)) //From push notifications
            {
                IoC.Get<INavigationService>().UriFor<ViewQuestionViewModel>()
                    .WithParam(x => x.QuestionId, NavigationContext.QueryString[questionId])
                    .Navigate();
            }
        }
    }
}