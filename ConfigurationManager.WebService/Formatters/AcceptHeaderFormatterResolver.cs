using System;
using System.Web;

namespace ConfigurationManager.WebService.Formatters
{
    /// <summary>
    /// Tries to find corresponding resolver with respect to "Accept" HTTP header.
    /// </summary>
    public class AcceptHeaderFormatterResolver : IFormatterResolver
    {
        public IFormatter GetFormatter(HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }
    }
}