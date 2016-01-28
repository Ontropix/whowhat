using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Need4Auto.Core.Content;

namespace WhoWhat.Core.Content
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly CloudBlobClient client;

        private CloudBlobContainer imageContainer;

        public ImageStorageService(string connectionString)
        {
            // Read storage account configuration settings
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

            client = account.CreateCloudBlobClient();

            //NOTE: if you see "Additional information: Unable to connect to the remote server" it means you need to start a storage emulator
            InitContainers();
        }

        private void InitContainers()
        {
            imageContainer = InitContainer(BlobReferences.Images);
        }

        private CloudBlobContainer InitContainer(string name)
        {
            //Image container
            CloudBlobContainer container = client.GetContainerReference(name);
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Container
            });

            return container;
        }

        public Uri SaveImage(Stream stream, string prefix)
        {
            //TODO: validate file is jpeg

            string uniqueBlobName = prefix + "_" + Guid.NewGuid();
            CloudBlockBlob blockBlob = imageContainer.GetBlockBlobReference(uniqueBlobName);
            blockBlob.Properties.ContentType = "image/jpeg";
            blockBlob.UploadFromStream(stream); // Create or overwrite
            return blockBlob.Uri;
        }

        public async Task DeleteImageAsync(Uri blobUri)
        {
            await Delete(imageContainer, blobUri);
        }

        private async Task Delete(CloudBlobContainer container, Uri blobUri)
        {
            CloudBlockBlob blob = new CloudBlockBlob(blobUri);
            if (blob.Container == container)
            {
                await blob.DeleteAsync();
            }
        }
    }
}
