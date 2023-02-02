using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ChatApi.Application.Helpers.Blob
{
    public class AzureBlobStorageService
    {
        string accessKey = string.Empty;
        string container = string.Empty;
        public IConfiguration configuration { get; }

        public AzureBlobStorageService(IConfiguration Configuration, string container)
        {
            configuration = Configuration;
            this.accessKey = Configuration.GetValue<string>("AzureBlobConnectionString");
            this.container = container;
        }

        public async Task<byte[]> GetBlobFileAsync(string fileUrl)
        {
            try
            {
                Uri uriObj = new Uri(fileUrl);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                string strContainerName = container;
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                string pathPrefix = fileUrl.Split(strContainerName)[1].Split(BlobName)[0].Substring(1);
                CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);

                CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(BlobName);

                if (blockBlob.ExistsAsync().Result)
                {
                    using (var ms = new MemoryStream())
                    {
                        await blockBlob.DownloadToStreamAsync(ms);
                        return ms.ToArray();

                    }
                }
                return new byte[0];

            }
            catch
            {
                throw;
            }
        }

        public async Task<string> UploadBase64FileToBlobAsync(string strFileName, string base64)
        {
            try
            {
                int index = base64.IndexOf(",");
                string mimeType = base64.Substring(base64.IndexOf(":") + 1, base64.IndexOf(";") - (base64.IndexOf(":") + 1));
                base64 = base64.Replace(base64.Substring(0, index + 1), "");
                byte[] bytes = Convert.FromBase64String(base64);
                return await this.UploadFileToBlobAsync(strFileName, bytes, mimeType);
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string? fileMimeType = null)
        {
            try
            {
                if (fileMimeType == null)
                {
                    new FileExtensionContentTypeProvider().TryGetContentType(strFileName, out fileMimeType);
                }

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = container;
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (strFileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(GenerateFileName(strFileName));
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteBlobDataAsync(string fileUrl)
        {
            try
            {
                Uri uriObj = new Uri(fileUrl);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = container;
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(fileUrl);
                // get block blob refarence    
                CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(BlobName);

                // delete blob from container        
                await blockBlob.DeleteAsync();
            }
            catch
            {
                throw;
            }
        }

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            strFileName = fileName + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToUniversalTime().ToString("hh-mm-ss-fff");
            return strFileName;
        }
    }
}
