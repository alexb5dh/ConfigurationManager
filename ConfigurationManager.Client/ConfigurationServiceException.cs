using System;

namespace ConfigurationManager.Client
{
    [Serializable]
    public sealed class ConfigurationServiceException: ApplicationException
    {
        public string ExceptionName { get; set; }

        public string ExceptionMessage { get; set; }

        public ConfigurationServiceException(ErrorInfo errorInfo): base("{0}:{1}")
        {
            ExceptionName = errorInfo.Exception;
            ExceptionMessage = errorInfo.ExceptionMessage;
        }

        public ConfigurationServiceException()
        {
        }

        public override string Message
        {
            get { return string.Format(base.Message, ExceptionName, ExceptionMessage); }
        }
    }
}