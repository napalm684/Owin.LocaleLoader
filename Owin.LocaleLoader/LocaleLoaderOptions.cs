using System.Collections.Generic;
using Napalm684.Owin.LocaleLoader.Dependencies;

namespace Napalm684.Owin.LocaleLoader
{
    /// <summary>
    /// Locale Loader Options to configure the Locale Loader Middleware with
    /// </summary>
    public class LocaleLoaderOptions
    {
        /// <summary>
        /// Locale Mappings Dictionary
        /// </summary>
        public Dictionary<string, string> LocaleMappings { get; set; }

        /// <summary>
        /// Optional parameters for locale service
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// Dependency Resolver
        /// </summary>
        public IDependencyResolver DependencyResolver { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LocaleLoaderOptions()
        {
            LocaleMappings = new Dictionary<string, string>();
        }
    }
}