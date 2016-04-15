<div>
    <img src="https://raw.githubusercontent.com/napalm684/Owin.LocaleLoader/master/Owin.LocaleLoader.png" alt="logo" />
    <h1>Owin.LocaleLoader</h1>
</div>
OWIN Middleware component to replace generic Http Requested files with locale specific files.
### Motivation

Are you developing a single-page application (SPA) featuring an ASP.Net backend and you want to eliminate the heavy/unnecessary MVC dependency?
Is your SPA written with a framework like AngularJS that has an i18n internationalization library which requires you to load a locale specific script dynamically?
Do you want to provide a different style sheet specific to a given user group?
Then you came to the right place!  This nuget library provides an OWIN middleware component to load named files dynamically via two methods:
    
* Browser culture/locale settings (Accept-Language header) 
* A custom locale service implementation (for your specific scenario)

#### Prerequisites

* OWIN based file host - <a href="https://www.nuget.org/packages/Microsoft.Owin.StaticFiles" title="Microsoft.Owin.StaticFiles">Microsoft.Owin.StaticFiles</a>
<br/>

Now let's write some code!

***

### Browser Accept-Language Based

#### Setup

```
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        //Sets root file directory for server
        var fileSystem = new PhysicalFileSystem(@".\wwwroot");

        var options = new FileServerOptions()
        {
            FileSystem = fileSystem,
            EnableDefaultFiles = true,
            EnableDirectoryBrowsing = true  //Must be enabled
        };

        options.StaticFileOptions.FileSystem = fileSystem;
        options.StaticFileOptions.ServeUnknownFileTypes = true;
        options.DefaultFilesOptions.DefaultFileNames 
                    = new List<string>() { "index.html" };

        //This overload can be used for simple 
        //browser-based mappings (one item) - See below for multiple item mappings
        app.UseLocaleLoader("script.js", "script-{0}.js");

        app.UseFileServer(options);
    }
}
```

Additional useage scenario for multiple file mappings, replace the UseLocaleLoader() call in the Startup class above
with the following:

```
app.UseLocaleLoader(new LocaleLoaderOptions
{
    LocaleMappings = new Dictionary<string, string>()
    {
        { "script1.js", "script1-{0}.js" },
        { "script1.js", "script1-{0}.js" }
        //Etc
    }
});
```

With the middleware in place (see first overload demonstrated), a request containing script.js
in the path value will be intercepted by the LocaleLoader, thus changing the request path to have the file name script-en-US.js (for a browser with the accept-language header value en-US (United States of America - English)).
Obviously, the same logic will occur when there are multiple mappings pairing placeholders with locale specific files.  Pretty neat huh?

***

### Locale Service Based

####Setup

In your OWIN Startup class use the fully specified LocaleLoaderOptions extension method with an implementation of the IDependencyResolver:

```
//Dependency Resolver
public class UnityDependencyResolver: Napalm684.Owin.LocaleLoader.Dependencies.IDependencyResolver
{
    protected IUnityContainer _container; //Or your favorite DI framework if Unity is not your preference
    
    public UnityDependencyResolver()
    {
        _container = new UnityContainer();
        
        /* Register dependencies for the container somewhere, in our case one for type
           Napalm684.Owin.LocaleLoader.Services.ILocaleService */         
    }
    
    object GetService(Type serviceType)
    {
        try
        {
            return _container.Resolve(serviceType);
        }
        catch (ResolutionFailedException)
        {
            return null;
        }        
    }
}
```

OWIN Startup Code:
```
app.UseLocaleLoader(new LocaleLoaderOptions
{
    LocaleMappings =
    {
        { "script.js", "script-{0}.js" }
    },
    DependencyResolver = new UnityDependencyResolver(),
    Parameters = new object[] { /* Parameters to pass to ILocaleService */ }
});
```

File(s) specified in LocaleMappings will now be resolved per the parameter(s) passed to the ILocaleService.GetLocale(params object[] parameters) method you implemented
in your custom ILocaleService.

Example ILocaleService:

```
public class MyLocaleService: Napalm684.Owin.LocaleLoader.Services.ILocaleService
{
    private readonly IRepository<User> _repository;
    
    string GetLocale(params object[] parameters)
    {
        return _repository.GetUserLocale(parameters[0] as string);
    }
}
```

Items to note about the above example, for simplicity some details have been removed.  Also, be aware that parameters[0] is always the user name authenticated
by the application.  You can use it or not, but it will be populated in the middleware with the value (or lack of value if not provided) for context.Authentication.User.Identity.Name.
Thus, the sample implementation uses a repository to read the locale of the user name logged into the system.  The parameters give developers the flexibility to
implement this service however they see fit, whether it's a locale or not is up to you!