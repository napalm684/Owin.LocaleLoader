using Owin;

namespace Napalm684.Owin.LocaleLoader
{
    /// <summary>
    /// Locale Loader Middleware Extension Methods
    /// </summary>
    public static class LocaleLoaderExtensions
    {
        /// <summary>
        /// Use the locale loader with provided options, note if Dependency Resolver property is not provided
        /// the browser accept-language is used for the locale
        /// </summary>
        /// <param name="appBuilder">App Builder</param>
        /// <param name="options">Locale Loader Options</param>
        /// <returns>App Builder</returns>
        public static IAppBuilder UseLocaleLoader(this IAppBuilder appBuilder, LocaleLoaderOptions options)
        {
            return appBuilder.Use(typeof(LocaleLoaderMiddleware), options);
        }

        /// <summary>
        /// Use the locale loader with the browser accept-language providing the locale
        /// </summary>
        /// <param name="appBuilder">App Builder</param>
        /// <param name="localePlaceholderScript">Locale Placeholder Script</param>
        /// <param name="actualLocaleScript">Actual Locale Specific script initially with string parameter</param>
        /// <returns>App Builder</returns>
        public static IAppBuilder UseLocaleLoader(this IAppBuilder appBuilder, string localePlaceholderScript, string actualLocaleScript)
        {
            return appBuilder.Use(typeof(LocaleLoaderMiddleware), 
                new LocaleLoaderOptions {
                    LocalePlaceholderScript = localePlaceholderScript,
                    ActualLocaleScript = actualLocaleScript
                });
        }
    }
}
