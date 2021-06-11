using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Shouldly;

namespace Di.Samples
{
    public class FactoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldResolveFactory()
        {
            using var serviceProvider = new ServiceCollection()
                .AddTransient<ISomeServiceFactory, SomeServiceFactory>()
                .BuildServiceProvider();

            serviceProvider
                .GetRequiredService<ISomeServiceFactory>()
                .Create()
                .ShouldNotBeNull();
        }

        [Test]
        public void ShouldResolveWithDelegateFactory1()
        {
            using var serviceProvider = new ServiceCollection()
                .AddTransient<ISomeServiceFactory, SomeServiceFactory>()
                .AddTransient(container =>
                    container.GetRequiredService<ISomeServiceFactory>().Create())
                .BuildServiceProvider();

            serviceProvider
                .GetRequiredService<ISomeService>()
                .ShouldNotBeNull();
        }

        [Test]
        public void ShouldResolveWithDelegateFactory2()
        {
            using var serviceProvider = new ServiceCollection()
                .AddTransient<IAnotherService>(_ =>
                    new AnotherService(new OptionsWrapper<SomeSettings>(new SomeSettings())))
                .BuildServiceProvider();

            serviceProvider
                .GetRequiredService<IAnotherService>()
                .ShouldNotBeNull();
        }

        [Test]
        public void ShouldResolveWithDelegateFactory3()
        {
            using var serviceProvider = new ServiceCollection()
                .AddSingleton<Func<ISomeService>>(() =>
                    new SomeService())
                .BuildServiceProvider();

            serviceProvider
                .GetRequiredService<Func<ISomeService>>()
                ()
                .ShouldNotBeNull();
        }

        [Test]
        public async Task ShouldResolveWithAsyncDelegateFactory()
        {
            using var serviceProvider = new ServiceCollection()
                .AddSingleton<Func<Task<ISomeService>>>(async () =>
                {
                    await Task.Delay(100);
                    return new SomeService();
                })
                .BuildServiceProvider();

            var someService = await serviceProvider
                .GetRequiredService<Func<Task<ISomeService>>>()
                ();

            someService.ShouldNotBeNull();
        }
    }

    public interface ISomeServiceFactory
    {
        ISomeService Create();
    }

    public class SomeServiceFactory : ISomeServiceFactory
    {
        public ISomeService Create() => new SomeService();
    }
}