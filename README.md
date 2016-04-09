<div>
    <img src="https://raw.githubusercontent.com/napalm684/Owin.LocaleLoader/master/Owin.LocaleLoader.png" alt="logo" />
    <h1>Owin.LocaleLoader</h1>
</div>
<br/>
<p>OWIN Middleware component to replace generic Http Requested files with locale specific files.</p>

### Motivation

Are you developing a single-page application (SPA) featuring an ASP.Net backend and you want to eliminate the heavy/unnecessary MVC dependency?
Is your SPA written with a framework like AngularJS that has an i18n internationalization library which requires you to load a locale specific script dynamically?
Do you want to provide a different style sheet specific to a given user group?
Then you came to the right place!  This nuget library provides an OWIN middleware component to load named files dynamically via two methods:
    
* Browser culture/locale settings (Accept-Language header) or 
* A custom locale service implementation (for your specific scenario).

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

With the middleware in place (see first overload demonstrated), a request containing <span style="font-style: italic;">script.js</span>
in the path value will be intercepted by the LocaleLoader, thus changing the request path to have the file name <span style="font-style: italic;">script1-en-US.js</span> (for a browser with the accept-language header value en-US (United States of America - English)).
Obviously, the same logic will occur when there are multiple mappings pairing placeholders with locale specific files.  Pretty neat huh?

***

### Locale Service Based

<span style="font-style: italic">Coming soon...</span>