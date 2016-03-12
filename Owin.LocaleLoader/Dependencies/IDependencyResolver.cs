using System;

namespace Napalm684.Owin.LocaleLoader.Dependencies
{
    /// <summary>
    /// Dependency Resolver Contract
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Get Service from Type passed
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <returns>Service as an Object</returns>
        object GetService(Type serviceType);
    }
}