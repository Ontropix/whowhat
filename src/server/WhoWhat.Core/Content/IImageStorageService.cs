using System;
using System.IO;
using System.Threading.Tasks;

namespace WhoWhat.Core.Content
{
    public interface IImageStorageService
    {
        Uri SaveImage(Stream stream, string prefix);
        Task DeleteImageAsync(Uri blobUri);
    }
}