using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationManager.Client
{
    public class ConfigurationClient
    {
        public HttpClient HttpClient { get; private set; }

        private void Init()
        {
        }

        private const char SegmentSeparator = '/';

        private void ValidateKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            var lastCharIsSeparator = false;
            foreach (var @char in key)
            {
                if (@char == SegmentSeparator)
                {
                    if (lastCharIsSeparator)
                    {
                        throw new ArgumentException("Duplicated separator in key.", "key");
                    }
                    lastCharIsSeparator = true;
                }
                else
                {
                    lastCharIsSeparator = false;
                    if (!char.IsLetterOrDigit(@char))
                    {
                        throw new ArgumentException("Each key segment can contain letters of digits only.", "key");
                    }
                }
            }
        }

        private void ValidateSection(string section)
        {
            if (section == "" || section == SegmentSeparator.ToString())
            {
                return;
            }
            ValidateKey(section);
        }

        private async Task CheckResponseAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorInfo = await response.Content.ReadAsAsync<ErrorInfo>();
                    throw new ConfigurationServiceException(errorInfo);
                }
                catch (InvalidOperationException)
                {
                    throw new ConfigurationServiceException();
                }
            }
        }

        public ConfigurationClient(string url)
        {
            HttpClient = new HttpClient() {BaseAddress = new Uri(url, UriKind.Absolute)};
            Init();
        }

        public async Task<string> GetAsync(string key, CancellationToken? cancellationToken = null)
        {
            ValidateKey(key);

            var ct = cancellationToken ?? CancellationToken.None;
            var keyParameter = Uri.EscapeDataString(key);

            var response = await
                HttpClient.GetAsync(
                    string.Format("value?key={0}", key),
                    ct);

            ct.ThrowIfCancellationRequested();

            await CheckResponseAsync(response);

            return await response.Content.ReadAsAsync<string>();
        }

        public async Task SetAsync(string key, string value, CancellationToken? cancellationToken = null)
        {
            ValidateKey(key);

            var ct = cancellationToken ?? CancellationToken.None;
            var keyParameter = Uri.EscapeDataString(key);

            var response = await
                HttpClient.PostAsync(
                    string.Format("value?key={0}", key),
                    new StringContent(value),
                    ct);

            ct.ThrowIfCancellationRequested();

            await CheckResponseAsync(response);
        }

        public async Task DeleteAsync(string key, CancellationToken? cancellationToken = null)
        {
            ValidateKey(key);

            var ct = cancellationToken ?? CancellationToken.None;
            var keyParameter = Uri.EscapeDataString(key);

            var response = await
                HttpClient.DeleteAsync(
                    string.Format("value?key={0}", key),
                    cancellationToken ?? CancellationToken.None);

            ct.ThrowIfCancellationRequested();

            await CheckResponseAsync(response);
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetSectionAsync(string section, CancellationToken? cancellationToken = null)
        {
            ValidateSection(section);

            var ct = cancellationToken ?? CancellationToken.None;
            var sectionParameter = Uri.EscapeDataString(section);

            var response = await
                HttpClient.GetAsync(
                    string.Format("children?key={0}", section),
                    ct);

            ct.ThrowIfCancellationRequested();

            await CheckResponseAsync(response);

            return await response.Content.ReadAsAsync<IEnumerable<KeyValuePair<string, string>>>();
        }

        public async Task DeleteSectionAsync(string section, CancellationToken? cancellationToken = null)
        {
            ValidateSection(section);

            var ct = cancellationToken ?? CancellationToken.None;
            var sectionParameter = Uri.EscapeDataString(section);

            var response = await
                HttpClient.DeleteAsync(
                    string.Format("value?key={0}", section),
                    cancellationToken ?? CancellationToken.None);

            ct.ThrowIfCancellationRequested();

            await CheckResponseAsync(response);
        }
    }
}
