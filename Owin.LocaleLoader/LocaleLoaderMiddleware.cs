using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Napalm684.Owin.LocaleLoader.Services;
using MiddlewareResources = Napalm684.Owin.LocaleLoader.Resources.Resources;

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

        protected readonly OwinMiddleware _next;
        protected readonly LocaleLoaderOptions _options;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="next">Next Middleware in Pipeline to Execute</param>
        /// <param name="options">Locale Loader Options</param>
        public LocaleLoaderMiddleware(OwinMiddleware next, LocaleLoaderOptions options) : base(next)
        {
            _options = options;
        }

        /// <summary>
        /// Invoke locale loader middleware component
        /// </summary>
        /// <param name="context">Owin Context</param>
        /// <returns>Task</returns>
        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.Value.Contains(_options.LocalePlaceholderScript))
            {
                if (_options.DependencyResolver == null)
                    UtilizeBrowsersLocale(context.Request);
                else
                    UtilizeServicesLocale(context.Request);
            }

            await _next.Invoke(context);
        }

        /// <summary>
        /// Updates OwinRequest Path to the location of the locale specific file for 
        /// the given browser accept-language header
        /// </summary>
        /// <param name="owinRequest">Request</param>
        protected void UtilizeBrowsersLocale(IOwinRequest owinRequest)
        {
            var acceptLangValues = owinRequest.Headers.GetValues(AcceptLanguage);
            var locale = string.Empty;

            if (acceptLangValues == null) throw new NullReferenceException(MiddlewareResources.MISSING_ACCEPT_LANG);

            ///TODO: Loop through or get first/default to use
            throw new NotImplementedException();

            owinRequest.Path.Value.Replace(_options.LocalePlaceholderScript,
                UpdateScriptName(_options.ActualLocaleScript, locale));
        }

        /// <summary>
        /// Updates OwinRequest Path to the location of the locale specific file for
        /// the value retrieved by the injected culture service
        /// </summary>
        /// <param name="owinRequest">Request</param>
        protected void UtilizeServicesLocale(IOwinRequest owinRequest)
        {
            var service = _options.DependencyResolver.GetService(typeof(ILocaleService)) as ILocaleService;
            var locale = service.GetLocale();

            owinRequest.Path.Value.Replace(_options.LocalePlaceholderScript,
                UpdateScriptName(_options.ActualLocaleScript, locale));
        }

        /// <summary>
        /// Updates actual script name with passed locale
        /// </summary>
        /// <param name="actualScript">Actual Script with Parameter Placeholder</param>
        /// <param name="locale">Locale</param>
        /// <returns>Actual Script Name</returns>
        private static string UpdateScriptName(string actualScript, string locale)
        {
            return String.Format(actualScript, locale);
        }
    }
}
