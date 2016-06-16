using System.IO;
using System.Web;

namespace ConfigurationManager.WebService.Formatters
{
    public interface IFormatterResolver
    { 
        IFormatter GetFormatter(HttpContextBase httpContext);
    }
}