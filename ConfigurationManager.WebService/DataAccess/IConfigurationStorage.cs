using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigurationManager.WebService.Models;

namespace ConfigurationManager.WebService.DataAccess
{
    public interface IConfigurationStorage
    {
        Task<string> GetAsync(Key key);

        Task<Dictionary<string, string>> GetSectionAsync(Key sectionKey);

        Task<bool> SetAsync(Key key, string value);

        Task<bool> DeleteAsync(Key key);
    }
}