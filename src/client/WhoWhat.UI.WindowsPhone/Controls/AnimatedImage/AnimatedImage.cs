using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WhoWhat.UI.WindowsPhone.Infrastructure;

namespace WhoWhat.UI.WindowsPhone.Controls
{
    public class AnimatedImage : Control
    {

        private ImageSource loaded;

        private Image Image_Loaded;
        private Image Image_Failed;

        public AnimatedImage()
        {
            DefaultStyleKey = typeof(AnimatedImage);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(string), typeof(AnimatedImage), new PropertyMetadata(default(string), (o, args) =>
            {
                string source = (string)args.NewValue;

                if (string.IsNullOrEmpty(source)) return;

                AnimatedImage @this = (AnimatedImage)o;
                @this.DownloadImage(source);
            }));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty FailedImageSourceProperty = DependencyProperty.Register(
            "FailedImageSource", typeof(ImageSource), typeof(AnimatedImage), new PropertyMetadata(default(ImageSource),
                (o, args) =>
                {
                    ImageSource source = (ImageSource)args.NewValue;
                    AnimatedImage @this = (AnimatedImage)o;

                    if (@this.Image_Failed != null)
                    {
                        @this.Image_Failed.Source = source;
                    }

                }));

        public ImageSource FailedImageSource
        {
            get { return (ImageSource)GetValue(FailedImageSourceProperty); }
            set { SetValue(FailedImageSourceProperty, value); }
        }

        private async void DownloadImage(string source)
        {
            await ImageDownloader.Download(
                new Uri(source, UriKind.RelativeOrAbsolute),
                bitmap =>
                {
                    loaded = bitmap;
                    SetLoadedImage();
                    VisualStateManager.GoToState(this, "Loaded", true);
                },
                () => VisualStateManager.GoToState(this, "Failed", true)
           );
        }


        private void SetLoadedImage()
        {
            if (Image_Loaded != null)
            {
                Image_Loaded.Source = loaded;
            }
        }

        public override void OnApplyTemplate()
        {
            Image_Loaded = (Image)GetTemplateChild("Image_Loaded");
            Image_Failed = (Image)GetTemplateChild("Image_Failed");

            Image_Failed.Source = FailedImageSource;
            SetLoadedImage();

            base.OnApplyTemplate();
        }
    }
}
