using ConfigurationManager.WebService.DataAccess;
using ConfigurationManager.WebService.Formatters;
using ConfigurationManager.WebService.Services;
using ConfigurationManager.WebService.Setup;
using SimpleInjector;

namespace ConfigurationManager.WebService
{
    public static class DependencyResolver
    {
        public static IDependencyResolver Current { get; set; }

        static DependencyResolver()
        {
            Current = GetResolver();
        }

        private static IDependencyResolver GetResolver()
        {
            var container = new Container();

            container.RegisterPerWebRequest<IConfigurationStorage, RedisConfigurationStorage>();

            container.RegisterPerWebRequest<IAppExceptionHandler, WriteToResponseAppExceptionHandler>();

            container.RegisterPerWebRequest<JsonFormatter, JsonNETJsonFormatter>();
            container.RegisterPerWebRequest<IFormatterResolver, AcceptHeaderFormatterResolver>();

            container.Verify();

            return new SimpleInjectorDependencyResolver(container);
        }
    }
}