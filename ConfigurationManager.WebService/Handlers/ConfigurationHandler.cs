using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using ConfigurationManager.WebService.DataAccess;
using ConfigurationManager.WebService.Exceptions;
using ConfigurationManager.WebService.Formatters;
using ConfigurationManager.WebService.Helpers;
using ConfigurationManager.WebService.Models;
using ConfigurationManager.WebService.Setup;

namespace ConfigurationManager.WebService.Handlers
{
    // Todo: too large - SRP violation
    public class ConfigurationHandler : HttpTaskAsyncHandler
    {
        private readonly IFormatterResolver _formatterResolver;
        private readonly IConfigurationStorage _configurationStorage;

        private HttpContextBase _context;

        public ConfigurationHandler(IFormatterResolver formatterResolver, IConfigurationStorage configurationStorage)
        {
            _formatterResolver = formatterResolver;
            _configurationStorage = configurationStorage;
        }

        private RouteValueDictionary _routeValues;

        private RouteValueDictionary RouteValues
        {
            get
            {
                return (_routeValues ?? (_routeValues = _context.Request.RequestContext.RouteData.Values));
            }
        }

        private ActionType? _actionType;

        protected ActionType ActionType
        {
            get { return (_actionType ?? (_actionType = (ActionType) RouteValues[typeof (ActionType).Name]).Value); }
        }

        private RequestType? _requestType;

        protected RequestType RequestType
        {
            get
            {
                return (_requestType ?? (_requestType = (RequestType) RouteValues[typeof (RequestType).Name]).Value);
            }
        }

        private IFormatter _formatter;

        protected IFormatter Formatter
        {
            get { return _formatter ?? (_formatter = _formatterResolver.GetFormatter(_context)); }
        }

        private Key GetKeyParameter(bool isSectionKey = false)
        {
            var key = _context.Request[typeof(Key).Name];
            if (string.IsNullOrEmpty(key))
            {
                throw new KeyNotSpecifiedException();
            }
            try
            {
                return new Key(key, isSectionKey);
            }
            catch (ArgumentException)
            {
                throw new InvalidKeyException(key);
            }
        }

        private string GetValueParameter()
        {
            var value = _context.Request["value"];

            if (value == null)
            {
                using (var reader = new StreamReader(_context.Request.GetBufferedInputStream()))
                {
                    value = reader.ReadToEnd();
                }
            }

            return value;
        }

        protected async Task GetKeyAsync(Key key)
        {
            var value = await _configurationStorage.GetAsync(key);
            if (value != null)
            {
                Formatter.WriteToResponse(_context, value);
            }
            else
            {
                throw new KeyNotFoundException(key);
            }
        }

        protected async Task GetSectionAsync(Key sectionKey)
        {
            var children = await _configurationStorage.GetSectionAsync(sectionKey);
            Formatter.WriteToResponse(_context, children);
        }

        protected async Task SetKeyAsync(Key key, string validatedValue)
        {
            var result = await _configurationStorage.SetAsync(key, validatedValue);
            if (!result)
            {
                throw new DatabaseOperationFailedException();
            }
        }

        protected async Task DeleteKeyAsync(Key key)
        {
            var result = await _configurationStorage.DeleteAsync(key);
            if (!result)
            {
                throw new DatabaseOperationFailedException();
            }
        }

        protected async Task DeleteSectionAsync(string section)
        {
            throw new NotImplementedException();
        }

        public async Task ProcessRequestAsync(HttpContextBase context)
        {
            _context = context;

            switch (ActionType)
            {
                case ActionType.Get:
                {
                    await GetKeyAsync(GetKeyParameter());
                    return;
                }
                case ActionType.GetSection:
                {
                    await GetSectionAsync(GetKeyParameter(isSectionKey: true));
                    return;
                }
                case ActionType.Set:
                {
                    await SetKeyAsync(GetKeyParameter(), GetValueParameter());
                    return;
                }
                case ActionType.Delete:
                {
                    await DeleteKeyAsync(GetKeyParameter());
                    return;
                }
                default:
                    return;
            }
        }

        public override Task ProcessRequestAsync(HttpContext context)
        {
            return ProcessRequestAsync(new HttpContextWrapper(context));
        }
    }
}