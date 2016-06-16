using System.Web;
using System.Web.Routing;
using ConfigurationManager.WebService.Handlers;

namespace ConfigurationManager.WebService.Setup
{
    public class AppRouteHandler: IRouteHandler
    {
        private readonly ActionType _actionType;

        public AppRouteHandler(ActionType actionType)
        {
            _actionType = actionType;
        }

        private THandler CreateHandler<THandler>()
            where THandler: class, IHttpHandler
        {
            return DependencyResolver.Current.Create<THandler>();
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            switch (_actionType)
            {
                case ActionType.Debug:
                    return CreateHandler<DebugHandler>();
                default:
                    return CreateHandler<ConfigurationHandler>();
            }
        }
    }
}