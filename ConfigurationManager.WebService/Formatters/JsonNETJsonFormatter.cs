using System.IO;
using Newtonsoft.Json;

namespace ConfigurationManager.WebService.Formatters
{
    public class JsonNETJsonFormatter : JsonFormatter
    {
        public override string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public override void Serialize<T>(T value, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, value);
            }
        }

        public override T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public override T Deserialize<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        public override object Deserialize(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public override object Deserialize(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize(jsonReader);
            }
        }
    }
}