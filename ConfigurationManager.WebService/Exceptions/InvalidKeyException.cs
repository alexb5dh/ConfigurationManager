using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ConfigurationManager.WebService.Exceptions
{
    [Serializable]
    public class InvalidKeyException: DomainException
    {
        public string Key { get; set; }

        public InvalidKeyException(string key) : base(string.Format("Key \"{0}\" is not valid.", key), HttpStatusCode.BadRequest)
        {
            Key = key;
        }
    }
}