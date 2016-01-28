using System.Net;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.Behaviours
{
    public class CookieBehaviour : Behavior<WebBrowser>
    {
        public static readonly DependencyProperty CookiedCollectionProperty =
            DependencyProperty.Register("CookiedCollection", typeof (CookieCollection), typeof (CookieBehaviour), new PropertyMetadata(default(CookieCollection)));

        public CookieCollection CookiedCollection
        {
            get { return (CookieCollection) GetValue(CookiedCollectionProperty); }
            set { SetValue(CookiedCollectionProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Navigated += AssociatedObjectOnNavigated;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Navigated -= AssociatedObjectOnNavigated;
        }

        private void AssociatedObjectOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            CookiedCollection = this.AssociatedObject.GetCookies();
        }
    }
}