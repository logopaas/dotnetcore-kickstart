using NAFCore.Common.Localization;
using NAFCore.Common.Localization.Attributes;

namespace LogoPaasSampleApp.Resource
{
    /// <summary>
    /// Class Translator.
    /// </summary>
    public static class Translator
    {
        /// <summary>
        /// Helper method for translation of Enum ResourceEnum
        /// </summary>
        /// <param name="lang">enum value to translate</param>
        /// <param name="showErrCodeIfExists">Optional param to display error code</param>
        /// <param name="args">Optional param to evaluate content with String.Format</param>
        /// <returns>System.String.</returns>
        public static string Translate(this ResourceEnum lang, bool showErrCodeIfExists = true, params object[] args)
        {
            return NLocalizationManager.Current.Translate(lang, showErrCodeIfExists, args);
        }
    }

    /// <summary>
    /// Enum ResourceEnum
    /// </summary>
    [NLanguageResource(typeof(Content))]
    public enum ResourceEnum
    {
        /// <summary>
        /// The user information
        /// </summary>
        UserInfo
    }
}
