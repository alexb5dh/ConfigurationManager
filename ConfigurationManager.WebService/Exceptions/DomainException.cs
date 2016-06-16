using System;
using System.Net;
using System.Security.Policy;

namespace ConfigurationManager.WebService.Exceptions
{
    public class DomainException: ApplicationException
    {
        public HttpStatusCode ResponseCode { get; protected set; }

        public DomainException(): this(null)
        {
        }

        public DomainException(string message, Exception exception = null) : this(message, HttpStatusCode.InternalServerError, exception)
        {
        }

        public DomainException(string message, HttpStatusCode responseCode, Exception exception = null) : base(message, exception)
        {
            ResponseCode = responseCode;
        }
    }
}