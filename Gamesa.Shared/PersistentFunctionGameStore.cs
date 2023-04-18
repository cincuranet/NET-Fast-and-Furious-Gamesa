using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace Gamesa.Shared;

public sealed class PersistentFunctionGameStore : GameStoreBase
{
    public record Data(GamePoint[] Points, int Steps);

    private readonly HttpClient _httpClient;

    public PersistentFunctionGameStore(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<(GamePoint[] points, int steps)?> TryGetGameAsync(string id)
    {
        using (var response = await _httpClient.GetAsync($"{Config.FunctionAddress}/api/get-game/{id}"))
        {
            if (!response.IsSuccessStatusCode)
                return null;
            var data = await response.Content.ReadFromJsonAsync<Data>();
            return (data!.Points, data.Steps);
        }
    }

    public override async Task SaveGameAsync(string id, GamePoint[] points, int steps)
    {
        // Azure Functions need Content-Length to properly handle this request body
        var content = new StringContent(JsonSerializer.Serialize(new Data(points, steps)), Encoding.UTF8, "application/json");
        using (var response = await _httpClient.PostAsync($"{Config.FunctionAddress}/api/save-game/{id}", content))
        {
            response.EnsureSuccessStatusCode();
        }
    }

    public override async Task<string[]?> ListGamesAsync()
    {
        return await _httpClient.GetFromJsonAsync<string[]>($"{Config.FunctionAddress}/api/list-games");
    }
}

