using System.Threading;
using System.Threading.Tasks;

namespace WebApp.DAL
{
    public interface ISomeRepository
    {
        Task<SomeItem> GetById(int id, CancellationToken cancellationToken);
    }
}