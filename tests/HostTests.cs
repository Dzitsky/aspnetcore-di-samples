using System.Data.Common;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Apps72.Dev.Data.DbMocker;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shouldly;
using WebApp;
using WebApp.DAL;

namespace Di.TestSamples
{
    public class HostTests
    {
        [Test]
        public async Task ShouldCallController()
        {
            using var host = CreateTestHost(new MockDbConnection());

            await host.StartAsync();

            var testServer = host.Services.GetRequiredService<IServer>().ShouldBeOfType<TestServer>();
            using var client = testServer.CreateClient();
            var result = await client.GetStringAsync("/test_method");

            result.ShouldBe("some result");

            await host.StopAsync();
        }

        [Test]
        [TestCase("test_name")]
        public async Task ShouldSelectTestItem(string name)
        {
            var connection = new MockDbConnection();
            using var host = CreateTestHost(connection);

            connection.Mocks.When(cmd => cmd.CommandText == "SELECT ID, NAME FROM SOME_ITEM WHERE ID = @ID")
                .ReturnsRow(cmd => new {id = 1, name});
            
            await host.StartAsync();

            var testServer = host.Services.GetRequiredService<IServer>().ShouldBeOfType<TestServer>();
            using var client = testServer.CreateClient();
            var result = await client.GetAsync("/get_item/1");
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            var item = await result.Content.ReadAsAsync<SomeItem>();

            item.Name.ShouldBe(name);

            await host.StopAsync();
        }

        private IHost CreateTestHost(DbConnection connection) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                        .UseTestServer())
                .ConfigureServices(services =>
                    services
                        .AddTransient(_ => connection)
                        .AddLogging(logging =>
                            logging.ClearProviders().AddConsole()))
                .Build();
    }
}