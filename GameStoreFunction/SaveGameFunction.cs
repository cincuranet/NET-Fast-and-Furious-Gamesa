using System.Net;
using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Gamesa.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GameStoreFunction
{
    public class SaveGameFunction
    {
        private readonly ILogger _logger;

        public SaveGameFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveGameFunction>();
        }

        [Function("SaveGame")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "save-game/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation("Saving game.");

            var data = await req.ReadFromJsonAsync<PersistentFunctionGameStore.Data>();

            var client = new BlobClient(Environment.GetEnvironmentVariable("GameStoreConnectionString"), "games", $"{id}.json", BlobClientOptionsHelper.Default);
            var binaryData = new BinaryData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true })));
            await client.UploadAsync(binaryData, new BlobUploadOptions() { });

            return req.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
