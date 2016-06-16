using System;
using System.Web;
using ConfigurationManager.WebService.Exceptions;
using ConfigurationManager.WebService.Formatters;

namespace ConfigurationManager.WebService.Handlers
{
    public class DebugHandler: IHttpHandler
    {
        private readonly IFormatter _formatter;

        public DebugHandler(IFormatter formatter)
        {
            _formatter = formatter;
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException("");
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}