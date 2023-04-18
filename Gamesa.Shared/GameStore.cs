using System.Collections.Concurrent;

namespace Gamesa.Shared;

public sealed class GameStore : GameStoreBase
{
    private readonly ConcurrentDictionary<string, (GamePoint[] points, int steps)> _games = new(StringComparer.Ordinal);

    public override Task<(GamePoint[] points, int steps)?> TryGetGameAsync(string id)
    {
        if (_games.TryGetValue(id, out var data))
        {
            return Task.FromResult<(GamePoint[] points, int steps)?>(data);
        }
        else
        {
            return Task.FromResult<(GamePoint[] points, int steps)?>(null);
        }
    }

    public void SaveGame(string id, GamePoint[] points, int steps)
    {
        _games[id] = (points, steps);
    }

    public override Task SaveGameAsync(string id, GamePoint[] points, int steps)
    {
        _games[id] = (points, steps);
        return Task.CompletedTask;
    }

    public override Task<string[]?> ListGamesAsync()
    {
        return Task.FromResult<string[]?>(_games.Keys.ToArray());
    }
}

