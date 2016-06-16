using System;
using System.Net;

namespace ConfigurationManager.WebService.Exceptions
{
    [Serializable]
    public class KeyNotFoundException : DomainException
    {
        public KeyNotFoundException(string key)
            : base(string.Format("Key \"{0}\" not found.", key), HttpStatusCode.NotFound)
        {
        }
    }
}