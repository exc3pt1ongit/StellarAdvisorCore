namespace StellarAdvisorCore.Logging
{
    public static class Logger
    {
        private static LoggerOptions _options = new LoggerOptions();

        public static void Log(string message) => Console.WriteLine(_options.FormatLogMessage(message, _options.DefaultColor));
        public static void LogError(string errorMessage) => Console.WriteLine(_options.FormatLogMessage(errorMessage, _options.ErrorColor, "Error"));
        public static void LogWarning(string warningMessage) => Console.WriteLine(_options.FormatLogMessage(warningMessage, _options.WarningColor, "Warning"));

        public static async Task LogAsync(string message) => await Console.Out.WriteLineAsync(_options.FormatLogMessage(message, _options.DefaultColor));
        public static async Task LogErrorAsync(string errorMessage) => await Console.Out.WriteLineAsync(_options.FormatLogMessage(errorMessage, _options.ErrorColor, "Error"));
        public static async Task LogWarningAsync(string warningMessage) => await Console.Out.WriteLineAsync(_options.FormatLogMessage(warningMessage, _options.WarningColor, "Warning"));

        public static void SetLoggerOptions(LoggerOptions options) => _options = options ?? throw new ArgumentNullException(nameof(options));
    }
}
