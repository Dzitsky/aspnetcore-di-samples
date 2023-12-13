using System.Threading;
using System.Threading.Tasks;

namespace WebApp.DAL
{
    /// <summary>
    /// Репозиторий 
    /// </summary>
    public interface ISomeRepository
    {
        /// <summary>
        /// Получение элемента по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<SomeItem> GetById(int id, CancellationToken cancellationToken);

        string GetTestLifeCycle();
    }
}