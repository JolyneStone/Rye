using Microsoft.Extensions.Logging;

namespace Monica.Options
{
    public class LoggerOptions
    {
        public string LogPath { get; set; }
        public bool IsConsoleEnabled { get; set; }
        public LogLevel LogLevel { get; set; }
        public bool UseMonicaLog { get; set; } = true;
    }
}
