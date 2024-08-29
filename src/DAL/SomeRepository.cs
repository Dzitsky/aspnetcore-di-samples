using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using WebApp1;

namespace WebApp.DAL
{
    /// <inheritdoc />
    public class SomeRepository : ISomeRepository
    {
        private readonly DbConnection _dbConnection;
        private readonly ILogger<SomeRepository> _logger;

        private readonly ISingleton _singleton1, _singleton2;
        private readonly IScoped _scoped1, _scoped2;
        private readonly ITransient _transient1, _transient2;

        public SomeRepository(
            DbConnection dbConnection, ILogger<SomeRepository> logger
            , ISingleton singleton1, ISingleton singleton2
            , IScoped scoped1, IScoped scoped2
            , ITransient transient1, ITransient transient2
            )
        {
            //_dbConnection = dbConnection;
            //_logger = logger;

            _singleton1 = singleton1;
            _singleton2 = singleton2;
            _scoped1 = scoped1;
            _scoped2 = scoped2;
            _transient1 = transient1;
            _transient2 = transient2;
        }

        public string GetTestLifeCycle()
        {
            var result = new StringBuilder();
            result.AppendLine($"singleton: {_singleton1.Id}, scoped: {_scoped1.Id}, transient: {_transient1.Id}"); ;
            result.AppendLine($"singleton: {_singleton2.Id}, scoped: {_scoped2.Id}, transient: {_transient2.Id}");
            return result.ToString();
        }

        public async Task<SomeItem> GetById(int id, CancellationToken cancellationToken)
        {
            //using (_logger.BeginScope(new {id}))
            //    _logger.LogDebug("Requested an item");

            //return await _dbConnection.QueryFirstAsync<SomeItem>(new CommandDefinition(
            //    "SELECT ID, NAME FROM SOME_ITEM WHERE ID = @ID",
            //    new {id}, cancellationToken: cancellationToken));

            return new SomeItem();
        }
    }
}