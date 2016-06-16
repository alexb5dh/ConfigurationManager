# Dependency Injection

Dependency injection is used for handlers and services they consume.
[SimpleInjector](https://simpleinjector.org/) is used.


# Handlers

ConfigurationHandler is at the moment single end-point for serving requests.  

DebugHandler and HistoryHandler are not implented.

# Storage
_Redis_ is in-memory key-value storage.
It provides very fast exetion for simple queries like __"GET value"__ but leads to problem cosidering data persistence and section queries.

The first one have many [different resolutions](http://redis.io/topics/persistence) and is not as big as it seems. 

The second one is resolved via additinal entry for each key that stores all its direct subsections.

# Routing

Routing is implemented via [UrlRoutingModule](https://msdn.microsoft.com/en-us/library/system.web.routing.urlroutingmodule.aspx) in _RoutingSetup_ class.