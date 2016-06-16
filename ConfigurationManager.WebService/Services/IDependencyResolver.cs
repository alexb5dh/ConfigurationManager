using System;

namespace ConfigurationManager.WebService.Services
{
    public interface IDependencyResolver
    {
        object GetService(Type type);

        T GetService<T>()
            where T : class;

        object Create(Type type);

        T Create<T>()
            where T : class;
    }
}