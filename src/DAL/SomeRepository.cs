using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace WebApp.DAL
{
    public class SomeRepository : ISomeRepository
    {
        private readonly DbConnection _dbConnection;
        private readonly ILogger<SomeRepository> _logger;

        public SomeRepository(DbConnection dbConnection, ILogger<SomeRepository> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public async Task<SomeItem> GetById(int id, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(new {id}))
                _logger.LogDebug("Requested an item");

            return await _dbConnection.QueryFirstAsync<SomeItem>(new CommandDefinition(
                "SELECT ID, NAME FROM SOME_ITEM WHERE ID = @ID",
                new {id}, cancellationToken: cancellationToken));
        }
    }
}