using Napalm684.Owin.LocaleLoader.Dependencies;

namespace Napalm684.Owin.LocaleLoader
{
    /// <summary>
    /// Locale Loader Options to configure the Locale Loader Middleware with
    /// </summary>
    public class LocaleLoaderOptions
    {
        /// <summary>
        /// Locale neutral placeholder script file name
        /// </summary>
        public string LocalePlaceholderScript { get; set; }

        /// <summary>
        /// Locale specific file name (initially with placeholder {0} for updating by middleware)
        /// </summary>
        public string ActualLocaleScript { get; set; }

        /// <summary>
        /// Dependency Resolver
        /// </summary>
        public IDependencyResolver DependencyResolver { get; set; }
    }
}