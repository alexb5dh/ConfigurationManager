using System;
using System.Net;

namespace ConfigurationManager.WebService.Exceptions
{
    public class KeyNotSpecifiedException : DomainException
    {
        public KeyNotSpecifiedException() : base("Key not specified.", HttpStatusCode.BadRequest)
        {
        }
    }
}