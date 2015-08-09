using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace CAD.Azure
{
    public class BlobStorage : IStorage
    {
        private CloudBlobClient _blobClient;

        public BlobStorage(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            this._blobClient = storageAccount.CreateCloudBlobClient();

            
        }

        public string DownloadToTemp(string container, string azureFile)
        {
            string tempPath = Path.GetTempFileName();
            tempPath = Path.ChangeExtension(tempPath, Path.GetExtension(azureFile));
            this.Download(container, azureFile, tempPath);
            return tempPath;
            
        }

        public void Download(string container, string azureFile, string targetFile)
        {
            CloudBlobContainer blobContainer = this.InitiateContainer(container);
            CloudBlockBlob blockBlob = this.InitiateFile(blobContainer, azureFile);

            //save to file
            using (var fileStream = File.OpenWrite(targetFile))
            {
                blockBlob.DownloadToStream(fileStream);
            }

        }


        public void Upload(string container, string localFile, string targetFile, string contentType = null)
        {
            CloudBlobContainer blobContainer = this.InitiateContainer(container);
            CloudBlockBlob blockBlob = this.InitiateFile(blobContainer, targetFile);
            
            //set content type
            if(!String.IsNullOrEmpty(contentType))
            {
                blockBlob.Properties.ContentType = contentType;
            }

            //save to file
            using (var fileStream = File.OpenRead(localFile))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        private CloudBlobContainer InitiateContainer(string container)
        {
            CloudBlobContainer blobContainer = this._blobClient.GetContainerReference(container);
            if (!blobContainer.Exists())
            {
                blobContainer.Create();
                blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            }
            return blobContainer;
        }

        private CloudBlockBlob InitiateFile(CloudBlobContainer blobContainer, string file)
        {
            string filePath = this.FormatFilePath(blobContainer.Name, file);
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(filePath);
            return blockBlob;
        }

        private string FormatFilePath(string container, string azureFilePath)
        {
            string blobPath = azureFilePath.Replace(@"/", @"\");
            string[] pathItems = blobPath.Split('\\');
            int indexFolder = Array.IndexOf(pathItems, container) + 1;
            int totalChar = pathItems.Count() - indexFolder;

            string filePath = String.Join(@"\", pathItems, indexFolder, totalChar);

            return filePath;
        }
    }
}
