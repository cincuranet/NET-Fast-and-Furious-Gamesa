using Azure.Core;
using Azure.Storage.Blobs;

namespace Gamesa.Shared
{
    public static class BlobClientOptionsHelper
    {
        public static readonly BlobClientOptions Default = new BlobClientOptions()
        {
            Retry = 
            {
                Delay = TimeSpan.FromSeconds(1),
                MaxRetries = 5,
                Mode = RetryMode.Exponential,
                MaxDelay = TimeSpan.FromSeconds(5),
                NetworkTimeout = TimeSpan.FromSeconds(10)
            },
        };
    }
}
