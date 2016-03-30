namespace Napalm684.Owin.LocaleLoader.Tests.Helpers
{
    /// <summary>
    /// Helper class to provide test constants
    /// </summary>
    public static class LocaleLoaderConstants
    {
        /// <summary>
        /// Path to files
        /// </summary>
        public const string FilePath = "/scripts/";

        /// <summary>
        /// Placeholder file
        /// </summary>
        public const string Placeholder = "locale-agnostic.js";

        /// <summary>
        /// Actual file with parameter placeholder
        /// </summary>
        public const string Actual = "locale-actual-{0}.js";

        /// <summary>
        /// Accept Language
        /// </summary>
        public const string AcceptLanguge = "Accept-Language";

        /// <summary>
        /// Full Browser Locale
        /// </summary>
        public const string FullLocale = "en-US,en;q=0.8";

        /// <summary>
        /// Generic Browser Locale
        /// </summary>
        public const string LocaleGeneric = "en;q=0.8";

        /// <summary>
        /// Final generic browser locale
        /// </summary>
        public const string LocaleGenericFinal = "en";

        /// <summary>
        /// Specific browser locale
        /// </summary>
        public const string LocaleSpecific = "en-US";

        /// <summary>
        /// Unmapped placeholder file
        /// </summary>
        public const string UnmappedPlaceholder = "dummy.js";
    }
}
