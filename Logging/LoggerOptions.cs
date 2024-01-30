using Pastel;

namespace StellarAdvisorCore.Logging
{
    public class LoggerOptions
    {
        /// <summary>
        /// Set color for default log function.
        /// </summary>
        public string DefaultColor { get; set; } = "#8096ff";

        /// <summary>
        /// Set color for error log function.
        /// </summary>
        public string ErrorColor { get; set; } = "#ff8d8d";

        /// <summary>
        /// Set color for warning log function.
        /// </summary>
        public string WarningColor { get; set; } = "#fac376";

        /// <summary>
        /// Set color for success log function.
        /// </summary>
        public string SuccessColor { get; set; } = "#82d185";

        /// <summary>
        /// Format the log message to send in future.
        /// </summary>
        /// <param name="message">Message to send using logging.</param>
        /// <param name="color">Color param for logging.</param>
        /// <param name="tag">Tag param for logging.</param>
        /// <returns></returns>
        public string FormatLogMessage(string message, string color, string tag = "Log")
        {
            var messageTag = $"[{tag}]".Pastel(color);
            return $"[{DateTime.UtcNow}] {messageTag} {message}";
        }
    }
}
