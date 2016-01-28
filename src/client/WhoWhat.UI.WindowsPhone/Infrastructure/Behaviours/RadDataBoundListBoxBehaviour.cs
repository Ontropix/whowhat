using System.Windows;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.Behaviours
{
    public class RadDataBoundListBoxBehaviour : Behavior<RadDataBoundListBox>
    {
        public static readonly DependencyProperty IsPullToRefreshLoadingCompletedProperty =
            DependencyProperty.Register("IsPullToRefreshLoadingCompleted", typeof(bool), typeof(RadDataBoundListBoxBehaviour), new PropertyMetadata(default(bool), (o, args) =>
                {
                    var @this = (RadDataBoundListBoxBehaviour)o;
                    if ((bool)args.NewValue)
                    {
                        @this.AssociatedObject.StopPullToRefreshLoading(true);
                    }
                }));

        public bool IsPullToRefreshLoadingCompleted
        {
            get { return (bool)GetValue(IsPullToRefreshLoadingCompletedProperty); }
            set { SetValue(IsPullToRefreshLoadingCompletedProperty, value); }
        }

        public static readonly DependencyProperty IsVirtualizationEnabledProperty =
            DependencyProperty.Register("IsVirtualizationEnabled", typeof(bool), typeof(RadDataBoundListBoxBehaviour), new PropertyMetadata(default(bool),
                (o, args) =>
                {
                    var @this = (RadDataBoundListBoxBehaviour)o;
                    @this.AssociatedObject.DataVirtualizationMode =
                        (bool)args.NewValue ? DataVirtualizationMode.OnDemandAutomatic : DataVirtualizationMode.None;
                }));

        public bool IsVirtualizationEnabled
        {
            get { return (bool)GetValue(IsVirtualizationEnabledProperty); }
            set { SetValue(IsVirtualizationEnabledProperty, value); }
        }
    }
}
