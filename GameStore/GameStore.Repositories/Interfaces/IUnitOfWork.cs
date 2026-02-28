using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Repositories.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IGameRepository Games { get; }
        IGenreRepository Genres { get; }
        IPlatformRepository Platforms { get; }
        Task SaveChangesAsync(CancellationToken token);
    }
}
