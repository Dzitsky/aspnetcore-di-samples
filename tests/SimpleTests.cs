using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace Di.Samples
{
    public class SimpleTests
    {
        [Test]
        public void ShouldResolveTransient()
        {
            var services = new ServiceCollection();

            services.AddTransient<ISomeService, SomeService>();

            using var serviceProvider = services.BuildServiceProvider();

            var someServiceA = serviceProvider.GetRequiredService<ISomeService>();
            var someServiceB = serviceProvider.GetRequiredService<ISomeService>();

            someServiceA.ShouldNotBeSameAs(someServiceB);
        }

        [Test]
        public void ShouldResolveSingleton()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ISomeService, SomeService>();

            using var serviceProvider = services.BuildServiceProvider();

            var someServiceA = serviceProvider.GetRequiredService<ISomeService>();
            var someServiceB = serviceProvider.GetRequiredService<ISomeService>();

            someServiceA.ShouldBeSameAs(someServiceB);
        }

        [Test]
        public void ShouldResolveScoped()
        {
            var services = new ServiceCollection();

            services.AddScoped<ISomeService, SomeService>();

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var someServiceA = scope.ServiceProvider.GetRequiredService<ISomeService>();
            var someServiceB = scope.ServiceProvider.GetRequiredService<ISomeService>();

            someServiceA.ShouldBeSameAs(someServiceB);
        }

        [Test]
        public void ShouldResolveScopedTwoScopes()
        {
            var services = new ServiceCollection();

            services.AddScoped<ISomeService, SomeService>();

            using var serviceProvider = services.BuildServiceProvider();
            using var scopeA = serviceProvider.CreateScope();
            using var scopeB = serviceProvider.CreateScope();

            var someServiceA = scopeA.ServiceProvider.GetRequiredService<ISomeService>();
            var someServiceB = scopeB.ServiceProvider.GetRequiredService<ISomeService>();

            someServiceA.ShouldNotBeSameAs(someServiceB);
        }
    }

    public interface ISomeService
    {
    }

    public class SomeService : ISomeService
    {
    }
}