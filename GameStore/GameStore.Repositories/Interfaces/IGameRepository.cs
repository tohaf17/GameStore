using GameStore.Domain.Entities;
namespace GameStore.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task AddAsync(Game game, CancellationToken token);
        Task<Game> GetGameByKeyAsync(string key, CancellationToken token);

    }
}
