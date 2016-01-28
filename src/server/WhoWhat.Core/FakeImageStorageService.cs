using System;
using System.IO;
using System.Threading.Tasks;
using WhoWhat.Core.Content;

namespace WhoWhat.Core
{
    public class FakeImageStorageService : IImageStorageService
    {
        public Uri SaveImage(Stream stream, string prefix)
        {
            return new Uri("http://www.projectpawsitive.com/wp-content/uploads/2013/08/CAt_No-Background1.png");
        }

        public Task DeleteImageAsync(Uri blobUri)
        {
            return Task.FromResult(0);
        }
    }
}