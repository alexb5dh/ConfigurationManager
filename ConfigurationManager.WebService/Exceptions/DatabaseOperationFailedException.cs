using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigurationManager.WebService.Exceptions
{
    public class DatabaseOperationFailedException: DomainException
    {
        public DatabaseOperationFailedException() : this(null)
        {
            
        }

        public DatabaseOperationFailedException(Exception innerException) : base("", innerException)
        {
        }
    }
}