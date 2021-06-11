using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp1.DAL;

namespace WebApp1.Controllers
{
    public class SomeController : Controller
    {
        private readonly ISomeRepository _someRepository;
        private readonly ILogger<SomeController> _logger;

        public SomeController(ISomeRepository someRepository, ILogger<SomeController> logger)
        {
            _someRepository = someRepository;
            _logger = logger;
        }

        [HttpGet("/test_method")]
        public string GetSomething()
        {
            _logger.LogInformation("Get method called");

            return "some result";
        }

        [HttpGet("/get_item/{id:int}")]
        public async Task<SomeItem> GetItemById(int id, CancellationToken cancellationToken) =>
            await _someRepository.GetById(id, cancellationToken);
    }
}