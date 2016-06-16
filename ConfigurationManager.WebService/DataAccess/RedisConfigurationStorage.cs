using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigurationManager.WebService.Helpers;
using ConfigurationManager.WebService.Models;
using StackExchange.Redis;

namespace ConfigurationManager.WebService.DataAccess
{
    public class RedisConfigurationStorage : IConfigurationStorage
    {
        protected static readonly IConnectionMultiplexer Connection = ConnectionMultiplexer.Connect(
            ConfigurationOptions.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["Redis"].ConnectionString));

        protected static IDatabase Database
        {
            get
            {
                return Connection.GetDatabase();
            }
        }

        private string GetKeyForChildren(string key)
        {
            return key + ":sub";
        }

        private async Task<IEnumerable<string>> GetChildrenKeysAsync(string key)
        {
            return (await Database.SetMembersAsync(GetKeyForChildren(key))).Select(subkey => new Key(key, subkey).ToString());
        }

        public async Task<string> GetAsync(Key key)
        {
            return await Database.StringGetAsync(key.ToString());
        }

        public async Task<Dictionary<string, string>> GetSectionAsync(Key sectionKey)
        {
            var childrenKeys = (await GetChildrenKeysAsync(sectionKey)).Select(childKey => (RedisKey)childKey).ToArray();
            return (await Database.StringGetAsync(childrenKeys))
                .Select((childValue, index) =>
                        childValue == RedisValue.Null // If key was removed in parallel
                            ? (KeyValuePair<string, string>?) null
                            : new KeyValuePair<string, string>(childrenKeys[index], childValue))
                .Where(pair => pair != null)
                .Select(pair => pair.Value)
                .ToDictionary();
        }

        public async Task<bool> SetAsync(Key key, string value)
        {
            var transaction = Database.CreateTransaction();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            // see: http://stackoverflow.com/a/25978001/3542151
            transaction.SetAddAsync(GetKeyForChildren(key.Section), key.Subkey);
            transaction.StringSetAsync(key.ToString(), value);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return await transaction.ExecuteAsync();
        }

        public async Task<bool> DeleteAsync(Key key)
        {
            var transaction = Database.CreateTransaction();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            // see: http://stackoverflow.com/a/25978001/3542151
            transaction.SetRemoveAsync(GetKeyForChildren(key.Section), key.Subkey);
            transaction.KeyDeleteAsync(key.ToString());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return await transaction.ExecuteAsync();
        }
    }
}