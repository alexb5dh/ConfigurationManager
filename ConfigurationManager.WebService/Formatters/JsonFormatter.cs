using System.IO;

namespace ConfigurationManager.WebService.Formatters
{
    public abstract class JsonFormatter: IFormatter
    {
        public string ContentType
        {
            get { return "application/json"; }
        }

        public abstract string Serialize<T>(T value);
        public abstract void Serialize<T>(T value, Stream stream);
        public abstract T Deserialize<T>(string value);
        public abstract T Deserialize<T>(Stream stream);
        public abstract object Deserialize(string value);
        public abstract object Deserialize(Stream stream);
    }
}