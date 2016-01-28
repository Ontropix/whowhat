using System.Windows;
using System.Windows.Controls;

namespace WhoWhat.UI.WindowsPhone.Controls
{
    public class Mode : ContentControl
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Content = View;

            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        public static readonly DependencyProperty IsEditProperty = DependencyProperty.Register(
            "IsEdit", typeof(bool), typeof(Mode), new PropertyMetadata(false, OnIsEditChanged));

        private static void OnIsEditChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            bool isEdit = (bool)e.NewValue;
            Mode @this = (Mode)dependencyObject;
            @this.Content = isEdit ? @this.Edit : @this.View;
        }

        public bool IsEdit
        {
            get { return (bool)GetValue(IsEditProperty); }
            set { SetValue(IsEditProperty, value); }
        }

        public object Edit { get; set; }
        public object View { get; set; }
    }
}
