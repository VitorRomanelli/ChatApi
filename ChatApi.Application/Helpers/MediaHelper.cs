using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Application.Helpers
{
    public class MediaHelper
    {

        string accessKey = string.Empty;
        public string _subdomain { get; set; }
        public IConfiguration _configuration { get; }

        [Obsolete]
        public readonly IHostingEnvironment _env;

        [Obsolete]
        public MediaHelper(IHostingEnvironment env, IConfiguration configuration, string subdomain)
        {
            this.accessKey = configuration.GetConnectionString("AzureBlob");
            _env = env;
            _subdomain = subdomain;
            _configuration = configuration;
        }

        public static string SaveImage(string base64img, string path, string extension, string outputImgFilename = "image.jpg")
        {
            var folderPath = Path.Combine($"wwwroot/Uploads/{path}/", "imgs");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string cleaned = base64img.Replace($"data:image/{extension};base64,", String.Empty);

            File.WriteAllBytes(Path.Combine(folderPath, outputImgFilename), Convert.FromBase64String(cleaned));

            var result = $"{folderPath.Replace("wwwroot/", string.Empty)}/{outputImgFilename}";

            return result;
        }

        #region Save on Server

        public static string SaveMedia(string path, string media)
        {
            try
            {

                int index = media.IndexOf(",");

                media = media.Replace(media.Substring(0, index + 1), "");
                byte[] bytes = Convert.FromBase64String(media);

                string filePath;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    filePath = path + "/image_1.png";
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    FileInfo[] files = di.GetFiles();
                    int imageNum;
                    if (files.Length > 0)
                    {
                        FileInfo file = files[0];

                        var file_splitted = file.Name.Split('_');

                        if (file_splitted.Length > 1)
                        {
                            imageNum = Convert.ToInt32(file_splitted[1].Split('.')[0]);
                        }
                        else
                        {
                            imageNum = 0;
                        }

                        filePath = path + "/image_" + (imageNum + 1).ToString() + ".png";

                        file.Delete();
                    }
                    else
                    {
                        filePath = path + "/image_1.png";
                    }
                }

                using (FileStream stream = new System.IO.FileStream(filePath, FileMode.Create))
                {
                    Stream mstream = new MemoryStream(bytes);
                    mstream.CopyTo(stream);
                    stream.Close();
                }
                index = filePath.IndexOf("Upload");
                filePath = filePath.Substring(index);
                return filePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string SaveFiles(string path, string name, string media, string fileExtension)
        {
            try
            {
                int index = media.IndexOf(",");

                media = media.Replace(media.Substring(0, index + 1), "");
                byte[] bytes = Convert.FromBase64String(media);

                string filePath;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    filePath = path + $"/{name}.{fileExtension}";
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    FileInfo[] files = di.GetFiles();
                    int fileNum;
                    if (files.Length > 0)
                    {
                        FileInfo file = files[0];

                        var file_splitted = file.Name.Split('_');

                        if (file_splitted.Length > 1)
                        {
                            fileNum = Convert.ToInt32(file_splitted[1].Split('.')[0]);
                        }
                        else
                        {
                            fileNum = 0;
                        }

                        filePath = path + $"/{name}_" + $".{fileExtension}";
                    }
                    else
                    {
                        filePath = path + $"/{name}_1.{fileExtension}";
                    }
                }

                using (FileStream stream = new System.IO.FileStream(filePath, FileMode.Create))
                {
                    Stream mstream = new MemoryStream(bytes);
                    mstream.CopyTo(stream);
                    stream.Close();
                }
                index = filePath.IndexOf("Upload");
                filePath = filePath.Substring(index);
                return filePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region Using Azure Blob

        [Obsolete]
        public async Task<string> UploadBase64ImageToBlob(string strFileName, string base64)
        {
            try
            {
                int index = base64.IndexOf(",");
                string mimeType = base64.Substring(base64.IndexOf(":") + 1, base64.IndexOf(";") - (base64.IndexOf(":") + 1));
                base64 = base64.Replace(base64.Substring(0, index + 1), "");
                byte[] bytes = Convert.FromBase64String(base64);
                return await UploadFileToBlob(strFileName, bytes, mimeType);
            }
            catch
            {
                throw;
            }
        }

        [Obsolete]
        public async Task<string> UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                return await UploadFileToBlobAsync(strFileName, fileData, fileMimeType);
            }
            catch
            {
                throw;
            }
        }

        [Obsolete]
        private string GetEnvironmentBlobContainer()
        {
            if (!_env.IsProduction())
            {
                return $"{this._subdomain}-hmg";
            }
            else
            {
                return $"{this._subdomain}";
            }
        }

        [Obsolete]
        public async void DeleteBlobData(string fileUrl)
        {
            try
            {
                Uri uriObj = new Uri(fileUrl);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                string strContainerName = GetEnvironmentBlobContainer();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                string pathPrefix = fileUrl.Split(strContainerName)[1].Split(BlobName)[0].Substring(1);

                // string pathPrefix = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/";
                CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
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

        [Obsolete]
        public async Task<byte[]> GetBlobFile(string fileUrl)
        {
            try
            {
                Uri uriObj = new Uri(fileUrl);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                string strContainerName = GetEnvironmentBlobContainer();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                string pathPrefix = fileUrl.Split(strContainerName)[1].Split(BlobName)[0].Substring(1);
                CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
                // get block blob refarence    
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
            catch { throw; }
        }

        private static string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

        [Obsolete]
        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = GetEnvironmentBlobContainer();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (strFileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(strFileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch { throw; }
        }

        #endregion
    }
}
