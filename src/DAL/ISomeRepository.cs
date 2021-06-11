using System.Threading;
using System.Threading.Tasks;

namespace WebApp1.DAL
{
    public interface ISomeRepository
    {
        Task<SomeItem> GetById(int id, CancellationToken cancellationToken);
    }
}