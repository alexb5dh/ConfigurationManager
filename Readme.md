# Requirements

- __Redis__ is used as storage.
- Solution was created in __Visual Studio 2015__
- __IIS__ or __IIS Express__ can be used as server.


# Installation

- __Redis__ for Windows can be installed from [Github](https://github.com/MSOpenTech/redis/releases).
- By default service and site will try to listen on ports 8080 and 8081 respectively with "localhost" as domain name. This can customized in project properties.


# Solution projects

Each project (except tests) contains separate _Readme.md_ file with additional information.  
  
* __ConfigurationManager.WebService__ - This is the main web service. 
  
* __ConfigurationManager.WebClient__ - Client library for web service.    
  
* __ConfigurationManager.WebApplication__ - Single page app that is used to demonstate basic usage of API. 

* Corresponding __.Tests__ projects - Those are currently in beggining stage.