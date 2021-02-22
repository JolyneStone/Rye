using Microsoft.Extensions.Logging;

namespace Rye.Options
{
    public class LoggerOptions
    {
        public string LogPath { get; set; }
        public bool IsConsoleEnabled { get; set; }
        public LogLevel LogLevel { get; set; }
        public bool UseRyeLog { get; set; } = true;
    }
}
