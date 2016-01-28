using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{
    public class ImageDownloader
    {

        public static async Task Download(Uri imageFileUri, Action<BitmapImage> success, Action failed)
        {
            if (imageFileUri.Scheme != "http" && imageFileUri.Scheme != "https")
            {
                BitmapImage bm = new BitmapImage(imageFileUri);
                success(bm);
                return;
            }

            //No internet
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                failed();
                return;
            }

           //Do wnload from the web
            try
            {
                BitmapImage bitmap = await DownloadFromWeb(imageFileUri);
                success(bitmap);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                failed();
            }
        }

        private static async Task<BitmapImage> DownloadFromWeb(Uri imageFileUri)
        {
            var httpwebrequest = (HttpWebRequest)WebRequest.Create(imageFileUri);
            httpwebrequest.Method = "GET";
            using (WebResponse response = await Task<WebResponse>.Factory.FromAsync(httpwebrequest.BeginGetResponse, httpwebrequest.EndGetResponse, null))
            {
                BitmapImage bitmap = new BitmapImage();

                Stream stream = response.GetResponseStream();
                bitmap.SetSource(stream);

                return bitmap;
            }
        }

    }
}
