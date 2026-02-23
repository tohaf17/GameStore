using GameStore.Domain.Entities;
namespace GameStore.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task AddGameAsync(Game game, CancellationToken token);
        Task DeleteGameAsync(Game game, CancellationToken token);
        Task<Game> GetGameByKeyAsync(string key, CancellationToken token);
        Task<Game> GetGameByIdAsync(Guid id, CancellationToken token);

    }
}
