using System.Net;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Gamesa.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GameStoreFunction
{
    public class GetGameFunction
    {
        private readonly ILogger _logger;

        public GetGameFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveGameFunction>();
        }

        [Function("GetGame")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-game/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation("Getting game.");

            var client = new BlobClient(Environment.GetEnvironmentVariable("GameStoreConnectionString"), "games", $"{id}.json", BlobClientOptionsHelper.Default);
            try
            {
                var blob = await client.DownloadContentAsync();
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.WriteBytes(blob.Value.Content.ToArray());
                return response;
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
