using System;

namespace WebApp1
{
    public interface ITestLifeCycle { 
        string Id { get; set; }
    }

    public interface ISingleton : ITestLifeCycle { }
    public interface IScoped : ITestLifeCycle { }
    public interface ITransient : ITestLifeCycle { }

    public class TestLifeCycle : ISingleton, IScoped, ITransient
    {
        public string Id { get; set; }

        public TestLifeCycle()
        { 
            Id = Guid.NewGuid().ToString();         
        }
    }
}