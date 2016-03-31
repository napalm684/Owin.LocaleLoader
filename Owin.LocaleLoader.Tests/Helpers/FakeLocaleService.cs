using Napalm684.Owin.LocaleLoader.Services;

namespace Napalm684.Owin.LocaleLoader.Tests.Helpers
{
    /// <summary>
    /// Helper class to provide a mocked-up locale service
    /// </summary>
    public class FakeLocaleService : ILocaleService
    {
        /// <summary>
        /// Retrieves desired locale code for use as a replacement parameter 
        /// in the actual locale specific file name 
        /// </summary>
        /// <param name="parameters">Parameters</param>
        /// <returns>Locale Designator</returns>
        public virtual string GetLocale(params object[] parameters)
        {
            return LocaleLoaderConstants.LocaleSpecific;
        }
    }
}
