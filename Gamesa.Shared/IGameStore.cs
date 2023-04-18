namespace Gamesa.Shared;

public interface IGameStore
{
    Task InitializeNewGameAsync(string id, int count);
    Task<(GamePoint[] points, int steps)?> TryGetGameAsync(string id);
    Task SaveGameAsync(string id, GamePoint[] points, int steps);
    Task<string[]?> ListGamesAsync();
}
