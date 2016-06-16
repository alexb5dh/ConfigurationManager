using System;
using System.Web;

namespace ConfigurationManager.WebService.Formatters
{
    /// <summary>
    /// Always returns instance of <see cref="JsonFormatter"/>.
    /// </summary>
    public class JsonOnlyFormatterResolver : IFormatterResolver
    {
        public IFormatter GetFormatter(HttpContextBase httpContext)
        {
            return DependencyResolver.Current.GetService<JsonFormatter>();
        }
    }
}