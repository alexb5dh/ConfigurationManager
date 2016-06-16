using System.Web;
using ConfigurationManager.WebService.Formatters;

namespace ConfigurationManager.WebService.Helpers
{
    public static class FormatterExtensions
    {
        public static void WriteToResponse<T>(this IFormatter formatter, HttpContextBase context, T entity)
        {
            context.Response.ContentType = formatter.ContentType;
            context.Response.Write(formatter.Serialize(entity));
        }
    }
}