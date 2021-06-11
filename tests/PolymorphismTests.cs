using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using Shouldly;

namespace Di.Samples
{
    public class PolymorphismTests
    {
        [Test]
        public void ShouldResolveSingleton()
        {
            using var serviceProvider = new ServiceCollection()
                .AddSingleton<CombinedService>()
                .AddSingleton<ISomeService>(container => container.GetRequiredService<CombinedService>())
                .AddSingleton<IAnotherService>(container => container.GetRequiredService<CombinedService>())
                .BuildServiceProvider();

            var someService = serviceProvider.GetRequiredService<ISomeService>();
            var anotherService = serviceProvider.GetRequiredService<IAnotherService>();

            someService.ShouldBeSameAs(anotherService);
        }

        [Test]
        public void ShouldResolveEnumerable()
        {
            using var serviceProvider = new ServiceCollection()
                .AddTransient<ISomeService, SomeServiceA>()
                .AddTransient<ISomeService, SomeServiceB>()
                .AddTransient<ISomeService, SomeServiceC>()
                .BuildServiceProvider();

            var services = serviceProvider.GetRequiredService<IEnumerable<ISomeService>>().ToList();

            services.Count.ShouldBe(3);
            services[0].ShouldBeOfType<SomeServiceA>();
            services[1].ShouldBeOfType<SomeServiceB>();
            services[2].ShouldBeOfType<SomeServiceC>();
        }

        [Test]
        public void ShouldResolveIdempotent()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.TryAddTransient<ISomeService, SomeServiceA>();
            serviceCollection.TryAddTransient<ISomeService, SomeServiceB>();

            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var service = serviceProvider.GetRequiredService<ISomeService>();

            service.ShouldBeOfType<SomeServiceA>();
        }
    }

    public class CombinedService : ISomeService, IAnotherService, IDisposable
    {
        public string JoinedList { get; }

        public void Dispose()
        {
        }
    }

    public class SomeServiceA : ISomeService
    {
    }

    public class SomeServiceB : ISomeService
    {
    }

    public class SomeServiceC : ISomeService
    {
    }
}