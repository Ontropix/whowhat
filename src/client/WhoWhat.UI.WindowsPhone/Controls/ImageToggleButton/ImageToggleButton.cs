using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WhoWhat.UI.WindowsPhone.Controls
{
    public class ImageToggleButton : ToggleButton
    {
        public ImageToggleButton()
        {
            DefaultStyleKey = typeof(ImageToggleButton);
        }

        public static readonly DependencyProperty ImageUncheckedProperty = DependencyProperty.Register("ImageUnchecked", typeof(ImageSource), typeof(ImageToggleButton), null);

        public ImageSource ImageUnchecked
        {
            get { return (ImageSource)this.GetValue(ImageUncheckedProperty); }
            set { this.SetValue(ImageUncheckedProperty, value); }
        }

        public static readonly DependencyProperty PressedImageProperty = DependencyProperty.Register("ImageChecked", typeof(ImageSource), typeof(ImageToggleButton), null);

        public ImageSource ImageChecked
        {
            get { return (ImageSource)this.GetValue(PressedImageProperty); }
            set { this.SetValue(PressedImageProperty, value); }
        }

    }
}
