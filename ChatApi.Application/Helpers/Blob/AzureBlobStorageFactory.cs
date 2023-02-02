using ChatApi.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace ChatApi.Application.Helpers.Blob
{
    public class AzureBlobStorageFactory
    {
        public IConfiguration _configuration { get; }

        public AzureBlobStorageFactory(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public AzureBlobStorageService GetAzureBlobStorageService(eBlobStorageType type)
        {
            return type switch
            {
                eBlobStorageType.Public => new AzureBlobStorageService(_configuration, _configuration.GetValue<string>("AzureBlobPublicContainer")),
                _ => new AzureBlobStorageService(_configuration, _configuration.GetValue<string>("AzureBlobPrivateContainer")),
            };
        }
    }
}
