using System;
using System.IO;
using System.Threading.Tasks;
using WhoWhat.Core.Content;

namespace WhoWhat.Api.Test.Mocks
{
    public class MockImageStorageService: IImageStorageService 
    {
        public Uri SaveImage(Stream stream, string prefix)
        {
            Console.WriteLine("MockImageStorageService. SaveImage. Stream.Length = {0} bytes", stream.Length);
            return new Uri("http://test.com/" + prefix);
        }

        public Task DeleteImageAsync(Uri blobUri)
        {
            return Task.FromResult(0);
        }
    }
}
