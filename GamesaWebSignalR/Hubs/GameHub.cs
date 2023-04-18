using Gamesa.Shared;
using Microsoft.AspNetCore.SignalR;

namespace GamesaWebSignalR.Hubs;

public class GameHub : Hub
{
    private readonly IGameStore _gameStore;

    public GameHub(IGameStore gameStore)
    {
        _gameStore = gameStore;
    }

    public async Task StartGame(string id)
    {
        var game = await _gameStore.TryGetGameAsync(id);
        if (game == null)
        {
            return;
        }
        var (points, steps) = game.Value;
        await Groups.AddToGroupAsync(Context.ConnectionId, id);
        await Clients.Group(id).SendAsync("RefreshGame", points, steps);
    }

    public async Task StartMove(string id, int pointIndex, int x, int y)
    {
        var game = await _gameStore.TryGetGameAsync(id);
        var (points, steps) = game.Value;
        steps += 1;
        await _gameStore.SaveGameAsync(id, points, steps);
        await Clients.Group(id).SendAsync("RefreshGame", points, steps);
    }

    public async Task DoMove(string id, int pointIndex, int x, int y)
    {
        var game = await _gameStore.TryGetGameAsync(id);
        var (points, steps) = game.Value;
        points[pointIndex] = points[pointIndex] with { X = x, Y = y };
        await _gameStore.SaveGameAsync(id, points, steps);
        await Clients.Group(id).SendAsync("RefreshGame", points, steps);
    }
}
