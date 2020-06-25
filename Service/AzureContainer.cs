using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compare_Files_Aws_Azure.Service
{
    class AzureContainer
    {
        public static Task<List<string>> ListAsync(string containerName, string storageAccountName, string storageAccountKey)
        {
            StorageCredentials credentials = new StorageCredentials(storageAccountName, storageAccountKey);
            Uri containerUri = new Uri($"https://{storageAccountName}.blob.core.windows.net/{containerName}");
            CloudBlobContainer cbContainer = new CloudBlobContainer(containerUri, credentials);
            var blobList = cbContainer.ListBlobs(useFlatBlobListing: true);
            Console.WriteLine("AZ listed. Converting...");
            var stringBlobList = blobList.Select(p => ((CloudBlockBlob)p).Name.ToString().Replace(containerUri.ToString() + "/", "")).ToList();
            Console.WriteLine($"Azure completed: {stringBlobList.Count()}");

            return Task.FromResult(stringBlobList);
        }
    }
}
