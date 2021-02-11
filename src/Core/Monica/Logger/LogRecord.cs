using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Monica.Configuration;
using Monica.Options;

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monica.Logger
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public sealed class LogRecord
    {
        private static readonly string _logName = nameof(LogRecord);
        private static readonly LogWriter _logWriter = new LogWriter();
        private static string _logPath;
        private static bool _isConsoleEnabled;
        private static LogLevel _logLevel;
        private static LoggerOptions _options = null;
        internal static LoggerOptions Options
        {
            get
            {
                //if (_options == null)
                //{
                //    _options = ConfigurationManager.Appsettings.GetSection("Framework:Logger").Get<LoggerOptions>();
                //    if (_options == null)
                //    {
                //        _options = new LoggerOptions
                //        {
                //            LogPath = @"/home/admin/logs/temp",
                //            IsConsoleEnabled = false,
                //            LogLevel = LogLevel.Debug,
                //            UseMonicaLog = true
                //        };
                //    }
                //    Reset();
                //}

                return _options;
            }
            set
            {
                _options = value;
                Reset();
            }
        }

        private static void Reset()
        {
            var options = Options;
            _logPath = options.LogPath;
            _isConsoleEnabled = options.IsConsoleEnabled;
            _logLevel = options.LogLevel;
            ResetLog(_logLevel);
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
        }

        public static bool IsNoneEnabled { get; set; }
        public static bool IsCriticalEnabled { get; private set; }
        public static bool IsErrorEnabled { get; private set; }
        public static bool IsWarnEnabled { get; private set; }
        public static bool IsInfoEnabled { get; private set; }
        public static bool IsDebugEnabled { get; private set; }
        public static bool IsTraceEnabled { get; private set; }

        public static void ResetLog(LogLevel logLevel)
        {
            _logLevel = logLevel;
            #region LogLevel

            switch (_logLevel)
            {
                case LogLevel.Trace:
                    IsTraceEnabled = true;
                    IsDebugEnabled = true;
                    IsInfoEnabled = true;
                    IsWarnEnabled = true;
                    IsErrorEnabled = true;
                    IsCriticalEnabled = true;
                    IsNoneEnabled = true;
                    break;
                case LogLevel.Debug:
                    IsTraceEnabled = false;
                    IsDebugEnabled = true;
                    IsInfoEnabled = true;
                    IsWarnEnabled = true;
                    IsErrorEnabled = true;
                    IsCriticalEnabled = true;
                    IsNoneEnabled = true;
                    break;
                case LogLevel.Information:
                    IsTraceEnabled = false;
                    IsDebugEnabled = false;
                    IsInfoEnabled = true;
                    IsWarnEnabled = true;
                    IsErrorEnabled = true;
                    IsCriticalEnabled = true;
                    IsNoneEnabled = true;
                    break;
                case LogLevel.Warning:
                    IsTraceEnabled = false;
                    IsDebugEnabled = false;
                    IsInfoEnabled = false;
                    IsWarnEnabled = true;
                    IsErrorEnabled = true;
                    IsCriticalEnabled = true;
                    IsNoneEnabled = true;
                    break;
                case LogLevel.Error:
                    IsTraceEnabled = false;
                    IsDebugEnabled = false;
                    IsInfoEnabled = false;
                    IsWarnEnabled = false;
                    IsErrorEnabled = true;
                    IsCriticalEnabled = true;
                    IsNoneEnabled = true;
                    break;
                case LogLevel.Critical:
                    IsTraceEnabled = false;
                    IsDebugEnabled = false;
                    IsInfoEnabled = false;
                    IsWarnEnabled = false;
                    IsErrorEnabled = false;
                    IsCriticalEnabled = true;
                    IsNoneEnabled = true;
                    break;
                default:
                    IsTraceEnabled = false;
                    IsDebugEnabled = true;
                    IsInfoEnabled = true;
                    IsWarnEnabled = true;
                    IsErrorEnabled = true;
                    IsCriticalEnabled = true;
                    IsNoneEnabled = true;
                    break;
            }

            #endregion
        }

        private static void WriteLog(string fileName, string logMessage)
        {
            try
            {
                var dt = DateTime.Now;
                logMessage = $"{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")}: {logMessage}{Environment.NewLine}";
                string logName = string.Format("{0}{1}{2}_{3}", _logPath, Path.DirectorySeparatorChar, dt.ToString("yyyyMMdd_HH"), fileName);

                _logWriter.Push(logName, logMessage);
            }
            catch (Exception ex)
            {
                _logWriter.Push(_logName, "将日志信息推送到队列失败：" + ex.GetBaseException().ToString());
            }
        }

        private static async Task WirteLogCoreAsync(string file, string message)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        public static bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    if (!IsTraceEnabled)
                        return false;
                    break;
                case LogLevel.Debug:
                    if (!IsDebugEnabled)
                        return false;
                    break;
                case LogLevel.Information:
                    if (!IsInfoEnabled)
                        return false;
                    break;
                case LogLevel.Warning:
                    if (!IsInfoEnabled)
                        return false;
                    break;
                case LogLevel.Error:
                    if (!IsErrorEnabled)
                        return false;
                    break;
                case LogLevel.Critical:
                    if (!IsCriticalEnabled)
                        return false;
                    break;
                case LogLevel.None:
                    return false;
                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static void Log(LogLevel logLevel, string fileName, string logMessage)
        {
            if (!IsEnabled(_logLevel))
                return;

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            WriteLog($"{fileName}_{logLevel.ToString()}", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logParams">日志参数</param>
        public static void Log(LogLevel logLevel, string fileName, string logMessage, params object[] logParams)
        {
            if (!IsEnabled(_logLevel))
                return;

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {string.Format(logMessage, logParams)}";
            WriteLog($"{fileName}_{logLevel.ToString()}", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Trace日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static void Trace(string fileName, string logMessage)
        {
            if (!IsTraceEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            WriteLog(fileName + "_Trace", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Trace日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logParams">日志参数</param>
        public static void Trace(string fileName, string logMessage, params object[] logParams)
        {
            if (!IsTraceEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {string.Format(logMessage, logParams)}";
            WriteLog(fileName + "_Trace", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static void Debug(string fileName, string logMessage)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            WriteLog(fileName + "_Debug", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
            }
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logParams">日志参数</param>
        public static void Debug(string fileName, string logMessage, params object[] logParams)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {string.Format(logMessage, logParams)}";
            WriteLog(fileName + "_Debug", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
            }
        }


        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static void Info(string fileName, string logMessage)
        {
            if (!IsInfoEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            WriteLog(fileName + "_Info", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Info: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logParams">日志参数</param>
        public static void Info(string fileName, string logMessage, params object[] logParams)
        {
            if (!IsInfoEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {string.Format(logMessage, logParams)}";
            WriteLog(fileName + "_Info", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Info: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Warn日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static void Warn(string fileName, string logMessage)
        {
            if (!IsWarnEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            WriteLog(fileName + "_Warn", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Warn: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Warn日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logParams">日志参数</param>
        public static void Warn(string fileName, string logMessage, params object[] logParams)
        {
            if (!IsWarnEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {string.Format(logMessage, logParams)}";
            WriteLog(fileName + "_Warn", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Warn: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static void Error(string fileName, string logMessage)
        {
            if (!IsErrorEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            WriteLog(fileName + "_Error", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Error: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logParams">日志参数</param>
        public static void Error(string fileName, string logMessage, params object[] logParams)
        {
            if (!IsErrorEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {string.Format(logMessage, logParams)}";
            WriteLog(fileName + "_Error", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Error: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Critical日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static void Critical(string fileName, string logMessage)
        {
            if (!IsCriticalEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            WriteLog(fileName + "_Critical", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "-Fatal: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Critical日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logParams">日志参数</param>
        public static void Critical(string fileName, string logMessage, params object[] logParams)
        {
            if (!IsCriticalEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {string.Format(logMessage, logParams)}";
            WriteLog(fileName + "_Critical", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "-Fatal: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// None 日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static void None(string fileName, string logMessage)
        {
            return;
        }

        /// <summary>
        /// None 日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logParams">日志参数</param>
        public static void None(string fileName, string logMessage, params object[] logParams)
        {
            return;
        }
    }
}
