using System.Web.Routing;

namespace ConfigurationManager.WebService.Setup
{
    public class RoutingSetup: ISetuper
    {
        public void Setup(Application application)
        {
            var routes = RouteTable.Routes;

            //Todo: simplify (RouteBuilder?)
            SetupApiRoutes(routes);
        }

        private void SetupApiRoutes(RouteCollection routes)
        {
            var actionTypeKey = typeof(ActionType).Name;
            var requestTypeKey = typeof (RequestType).Name;

            var actionType = ActionType.Debug;
            routes.Add(actionType.ToString(), new Route(
                url: "debug",
                defaults: new RouteValueDictionary()
                {
                    {requestTypeKey, RequestType.Api},
                    {actionTypeKey, actionType}
                },
                routeHandler: new AppRouteHandler(actionType),
                constraints: new RouteValueDictionary
                {
                    {"method", new HttpMethodConstraint("GET")}
                }));

            actionType = ActionType.Get;
            routes.Add(actionType.ToString(), new Route(
                url: "value",
                defaults: new RouteValueDictionary()
                {
                    {requestTypeKey, RequestType.Api},
                    {actionTypeKey, actionType}
                },
                routeHandler: new AppRouteHandler(actionType),
                constraints: new RouteValueDictionary
                {
                    {"method", new HttpMethodConstraint("GET")}
                }));

            actionType = ActionType.GetSection;
            routes.Add(actionType.ToString(), new Route(
                url: "section",
                defaults: new RouteValueDictionary()
                {
                    {requestTypeKey, RequestType.Api},
                    {actionTypeKey, actionType}
                },
                routeHandler: new AppRouteHandler(actionType),
                constraints: new RouteValueDictionary
                {
                    {"method", new HttpMethodConstraint("GET")}
                }));

            actionType = ActionType.Set;
            routes.Add(actionType.ToString(), new Route(
                url: "value",
                defaults: new RouteValueDictionary()
                {
                    {requestTypeKey, RequestType.Api},
                    {actionTypeKey, actionType}
                },
                routeHandler: new AppRouteHandler(actionType),
                constraints: new RouteValueDictionary
                {
                    {"method", new HttpMethodConstraint("POST", "PUT")}
                }));

            actionType = ActionType.Delete;
            routes.Add(actionType.ToString(), new Route(
                url: "value",
                defaults: new RouteValueDictionary
                {
                    {requestTypeKey, RequestType.Api},
                    {actionTypeKey, actionType}
                },
                routeHandler: new AppRouteHandler(actionType),
                constraints: new RouteValueDictionary
                {
                    {"method", new HttpMethodConstraint("DELETE")}
                }));
        }
    }
}