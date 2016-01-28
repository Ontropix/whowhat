using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WhoWhat.UI.WindowsPhone.Controls
{
    public class ImageButton : Button
    {
        public ImageButton()
        {
            DefaultStyleKey = typeof (ImageButton);
        }

        public static readonly DependencyProperty ImageNormalProperty =
            DependencyProperty.Register("ImageNormal", typeof (ImageSource), typeof (ImageButton), null);

        public ImageSource ImageNormal
        {
            get { return (ImageSource) this.GetValue(ImageNormalProperty); }
            set { this.SetValue(ImageNormalProperty, value); }
        }

        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register("PressedImage", typeof (ImageSource), typeof (ImageButton), null);

        public ImageSource PressedImage
        {
            get { return (ImageSource) this.GetValue(PressedImageProperty); }
            set { this.SetValue(PressedImageProperty, value); }
        }


        public static readonly DependencyProperty DisabledImageProperty =
            DependencyProperty.Register("DisabledImage", typeof (ImageSource), typeof (ImageButton), null);

        public ImageSource DisabledImage
        {
            get { return (ImageSource) this.GetValue(DisabledImageProperty); }
            set { this.SetValue(DisabledImageProperty, value); }
        }
    }
}
