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
        /// <param name="localePlaceholder">Locale Placeholder file</param>
        /// <param name="actualLocale">Actual Locale Specific file initially with string parameter</param>
        /// <returns>App Builder</returns>
        public static IAppBuilder UseLocaleLoader(this IAppBuilder appBuilder, string localePlaceholder, string actualLocale)
        {
            return appBuilder.Use(typeof(LocaleLoaderMiddleware), 
                new LocaleLoaderOptions {
                    LocaleMappings =
                    {
                        { localePlaceholder, actualLocale }
                    }
                });
        }
    }
}
