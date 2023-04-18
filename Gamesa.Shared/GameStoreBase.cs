namespace Gamesa.Shared;

public abstract class GameStoreBase : IGameStore
{
    public async Task InitializeNewGameAsync(string id, int count)
    {
        var points = new GamePoint[count];
        for (var i = 0; i < points.Length; i++)
        {
            points[i] = new GamePoint(Random.Shared.Next(0, Config.BoardSize), Random.Shared.Next(0, Config.BoardSize));
        }
        await SaveGameAsync(id, points, 0);
    }

    public abstract Task<(GamePoint[] points, int steps)?> TryGetGameAsync(string id);
    public abstract Task SaveGameAsync(string id, GamePoint[] points, int steps);
    public abstract Task<string[]?> ListGamesAsync();
}
