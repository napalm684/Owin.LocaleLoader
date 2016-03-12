namespace Napalm684.Owin.LocaleLoader.Services
{
    /// <summary>
    /// Locale Service Contract
    /// </summary>
    public interface ILocaleService
    {
        /// <summary>
        /// Retrieves desired locale code for use as a replacement parameter 
        /// in the actual locale specific script name 
        /// </summary>
        /// <returns></returns>
        string GetLocale();
    }
}