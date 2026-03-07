using GameStore.Domain.Entities;
namespace GameStore.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task AddGameAsync(Game game);
        Task DeleteGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task<Game?> GetGameByKeyAsync(string key, CancellationToken token=default);
        Task<Game?> GetGameByIdAsync(Guid id, CancellationToken token=default);
        Task<IEnumerable<Genre>> GetGameGenresByKeyAsync(string key, CancellationToken token = default);
        Task<IEnumerable<Platform>> GetGamePlatformsByKeyAsync(string key, CancellationToken token = default);
        Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken token=default);
    }
}
