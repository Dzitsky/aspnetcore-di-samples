using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Di.Samples
{
    public class ConfigurationTests
    {
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [Test]
        public void ShouldInjectOptionsFromConfiguration()
        {
            using var serviceProvider = new ServiceCollection()
                .AddTransient<IAnotherService, AnotherService>()
                .Configure<SomeSettings>(_configuration.GetSection("SectionA"))
                .BuildServiceProvider();

            var anotherService = serviceProvider.GetRequiredService<IAnotherService>();

            anotherService.JoinedList.ShouldBe("4, 5, 6");
        }

        [Test]
        public void ShouldInjectOptionsFromMoq()
        {
            using var serviceProvider = new ServiceCollection()
                .AddTransient<IAnotherService, AnotherService>()
                .AddSingleton<IOptions<SomeSettings>>(new OptionsWrapper<SomeSettings>(new SomeSettings
                {
                    SomeList = new List<int> {7, 8, 9}
                }))
                .BuildServiceProvider();

            var anotherService = serviceProvider.GetRequiredService<IAnotherService>();

            anotherService.JoinedList.ShouldBe("7, 8, 9");
        }
    }

    public interface IAnotherService
    {
        string JoinedList { get; }
    }

    public class AnotherService : IAnotherService
    {
        private readonly IOptions<SomeSettings> _options;

        public AnotherService(IOptions<SomeSettings> options)
        {
            _options = options;
        }

        public string JoinedList => string.Join(", ", _options.Value.SomeList);
    }

    public class SomeSettings
    {
        public int SomeInt { get; set; }
        public string SomeString { get; set; }
        public SubObj SubObject { get; set; }
        public List<int> SomeList { get; set; }
    }

    public class SubObj
    {
        public string SomeProp { get; set; }
    }
}