using System;
using System.Net;
using System.Web;
using ConfigurationManager.WebService.Exceptions;
using ConfigurationManager.WebService.Formatters;
using ConfigurationManager.WebService.Helpers;

namespace ConfigurationManager.WebService.Services
{
    public class WriteToResponseAppExceptionHandler: IAppExceptionHandler
    {
        public class ErrorInfo
        {
            public ErrorInfo(DomainException exception)
            {
                Exception = exception.GetType().Name;
                ExceptionMessage = exception.Message;
            }

            public string Exception { get; set; }

            public string ExceptionMessage { get; set; }
        }

        private readonly IFormatterResolver _formatterResolver;

        public WriteToResponseAppExceptionHandler(IFormatterResolver formatterResolver)
        {
            _formatterResolver = formatterResolver;
        }

        public void HandleException(HttpContextBase context, DomainException exception)
        {
            var formatter = _formatterResolver.GetFormatter(context);
            formatter.WriteToResponse(context, new ErrorInfo(exception));
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        }
    }
}