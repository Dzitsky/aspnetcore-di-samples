using System.Threading;
using System.Threading.Tasks;

namespace WebApp.DAL
{
    /// <summary>
    /// ����������� 
    /// </summary>
    public interface ISomeRepository
    {
        /// <summary>
        /// ��������� �������� �� ��������������
        /// </summary>
        /// <param name="id">�������������</param>
        /// <param name="cancellationToken">����� ������</param>
        /// <returns></returns>
        Task<SomeItem> GetById(int id, CancellationToken cancellationToken);

        string GetTestLifeCycle();
    }
}