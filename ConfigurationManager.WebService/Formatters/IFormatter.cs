using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ConfigurationManager.WebService.Formatters
{
    public interface IFormatter
    {
        string ContentType { get; }

        string Serialize<T>(T value);

        void Serialize<T>(T value, Stream stream);

        T Deserialize<T>(string value);

        T Deserialize<T>(Stream stream);

        object Deserialize(string value);

        object Deserialize(Stream stream);
    }
}