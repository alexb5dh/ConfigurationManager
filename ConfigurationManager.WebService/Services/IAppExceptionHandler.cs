using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConfigurationManager.WebService.Exceptions;

namespace ConfigurationManager.WebService.Services
{
    public interface IAppExceptionHandler
    {
        void HandleException(HttpContextBase context, DomainException exception);
    }
}