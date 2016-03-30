using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Napalm684.Owin.LocaleLoader.Services;

namespace Napalm684.Owin.LocaleLoader
{
    /// <summary>
    /// Locale Loader Middleware intercepts requests to a placeholder file
    /// and changes the request's file to a language/culture specific version of
    /// the placeholder file
    /// </summary>
    public class LocaleLoaderMiddleware : OwinMiddleware
    {
        private const string AcceptLanguage = "Accept-Language";

        /// <summary>
        /// Middleware Options
        /// </summary>
        protected readonly LocaleLoaderOptions Options;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="next">Next Middleware in Pipeline to Execute</param>
        /// <param name="options">Locale Loader Options</param>
        public LocaleLoaderMiddleware(OwinMiddleware next, LocaleLoaderOptions options) : base(next)
        {
            Options = options;
        }

        /// <summary>
        /// Invoke locale loader middleware component
        /// </summary>
        /// <param name="context">Owin Context</param>
        /// <returns>Task</returns>
        public override async Task Invoke(IOwinContext context)
        {
            Options.LocaleMappings.Keys.ToList().ForEach(x =>
            {
                if (!context.Request.Path.Value.Contains(x)) return;

                if (Options.DependencyResolver == null)
                    UtilizeBrowsersLocale(context.Request, x);
                else
                    UtilizeServicesLocale(context.Request, x, context.Authentication.User.Identity.Name);
            });

            await Next.Invoke(context);
        }

        /// <summary>
        /// Updates OwinRequest Path to the location of the locale specific file for 
        /// the given browser accept-language header
        /// </summary>
        /// <param name="owinRequest">Request</param>
        /// <param name="placeholder">Placeholder file</param>
        protected void UtilizeBrowsersLocale(IOwinRequest owinRequest, string placeholder)
        {
            var locale = GetLocaleCodeFromHeaders(owinRequest.Headers);
            string actualLocaleFile;

            Options.LocaleMappings.TryGetValue(placeholder, out actualLocaleFile);
            owinRequest.Path = new PathString(owinRequest.Path.Value.Replace(placeholder, UpdateFileName(actualLocaleFile, locale)));
        }

        /// <summary>
        /// Updates OwinRequest Path to the location of the locale specific file for
        /// the value retrieved by the injected culture service/passed user name
        /// </summary>
        /// <param name="owinRequest">Request</param>
        /// <param name="placeholder">Placeholder file</param>
        /// <param name="userName">User Name</param>
        protected void UtilizeServicesLocale(IOwinRequest owinRequest, string placeholder, string userName)
        {
            var service = Options.DependencyResolver.GetService(typeof(ILocaleService)) as ILocaleService;
            var locale = service.GetLocale(userName);

            string actualLocaleFile;
            Options.LocaleMappings.TryGetValue(placeholder, out actualLocaleFile);
            owinRequest.Path = new PathString(owinRequest.Path.Value.Replace(placeholder, UpdateFileName(actualLocaleFile, locale)));
        }

        /// <summary>
        /// Updates actual file name with passed locale
        /// </summary>
        /// <param name="actual">Actual file with Parameter Placeholder</param>
        /// <param name="locale">Locale</param>
        /// <returns>Actual file Name</returns>
        private string UpdateFileName(string actual, string locale)
        {
            return String.Format(actual, locale);
        }

        /// <summary>
        /// Get Locale Code from header dictionary
        /// </summary>
        /// <param name="headers">Request Headers</param>
        /// <returns></returns>
        private string GetLocaleCodeFromHeaders(IHeaderDictionary headers)
        {
            const string semicolon = ";";
            var acceptLangs = headers.GetCommaSeparatedValues(AcceptLanguage);
            var localeCode = acceptLangs.FirstOrDefault();
            return localeCode != null && localeCode.Contains(semicolon) ? localeCode.Split(semicolon.ToCharArray())[0] : localeCode;
        }
    }
}
