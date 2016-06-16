using System;
using System.Linq;
using System.Web;
using ConfigurationManager.WebService.Exceptions;
using ConfigurationManager.WebService.Services;
using ConfigurationManager.WebService.Setup;

namespace ConfigurationManager.WebService
{
    public class Application : HttpApplication
    {
        public void Application_Start()
        {
            var setuperType = typeof(ISetuper);
            foreach (var setuper in AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => setuperType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<ISetuper>())
            {
                setuper.Setup(this);
            }

            _allowOrigin = System.Configuration.ConfigurationManager.AppSettings["Access-Control-Allow-Origin"];  //Todo: move to ISetuper
        }

        private static string _allowOrigin;

        public void Application_OnPreSendRequestHeaders()
        {
            if (_allowOrigin != null)
            {
                Context.Response.Headers["Access-Control-Allow-Origin"] = _allowOrigin;
            }
        }

        public void Application_OnError()
        {
            var exception = Server.GetLastError();

            var appException = exception as DomainException ?? new DomainException();
            var handler = DependencyResolver.Current.GetService<IAppExceptionHandler>();
            handler.HandleException(new HttpContextWrapper(Context), appException);

            Server.ClearError();
        }
    }
}