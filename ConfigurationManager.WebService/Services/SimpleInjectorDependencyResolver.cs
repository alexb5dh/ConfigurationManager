using System;
using ConfigurationManager.WebService.Setup;
using SimpleInjector;

namespace ConfigurationManager.WebService.Services
{
    public class SimpleInjectorDependencyResolver : IDependencyResolver
    {
        private readonly Container _container;

        public SimpleInjectorDependencyResolver(Container container)
        {
            _container = container;
        }

        public object GetService(Type type)
        {
            return _container.GetInstance(type);
        }

        public T GetService<T>()
            where T : class
        {
            return _container.GetInstance<T>();
        }

        public object Create(Type type)
        {
            return _container.GetInstance(type);
        }

        public T Create<T>() where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}