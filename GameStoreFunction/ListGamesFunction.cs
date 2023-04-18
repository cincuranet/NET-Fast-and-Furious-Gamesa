using System.Net;
using Azure.Storage.Blobs;
using Gamesa.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GameStoreFunction
{
    public class ListGamesFunction
    {
        private readonly ILogger _logger;

        public ListGamesFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveGameFunction>();
        }

        [Function("ListGames")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "list-games")] HttpRequestData req)
        {
            _logger.LogInformation("Listing games.");

            var client = new BlobContainerClient(Environment.GetEnvironmentVariable("GameStoreConnectionString"), "games", BlobClientOptionsHelper.Default);
            var ids = new List<string>();
            await foreach (var item in client.GetBlobsAsync())
            {
                ids.Add(item.Name);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(ids);
            return response;
        }
    }
}
