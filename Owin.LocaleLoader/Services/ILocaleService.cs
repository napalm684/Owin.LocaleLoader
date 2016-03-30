namespace Napalm684.Owin.LocaleLoader.Services
{
    /// <summary>
    /// Locale Service Contract
    /// </summary>
    public interface ILocaleService
    {
        /// <summary>
        /// Retrieves desired locale code for use as a replacement parameter 
        /// in the actual locale specific file name 
        /// </summary>
        /// <param name="parameters">Parameters</param>
        /// <returns>Locale Designator</returns>
        string GetLocale(params object[] parameters);
    }
}