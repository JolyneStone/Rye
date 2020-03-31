using KiraNet.AlasFx.Configuration;
using KiraNet.AlasFx.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KiraNet.AlasFx.Log
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public class LogRecord
    {
        private static readonly string _logName = nameof(LogRecord);
        private static readonly Dictionary<string, LockObject> _fileLock = new Dictionary<string, LockObject>();
        private static readonly LockObject _lock = new LockObject();
        private static readonly LogLevel _logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), ConfigurationManager.Appsettings.GetSection("Logging:LogLevel").Value ?? "Debug");
        private static readonly string _logPath = ConfigurationManager.Appsettings.GetSection("Logging:LogPath").Value ?? @"/home/admin/logs/temp";
        private static readonly bool _isConsoleEnabled = (ConfigurationManager.Appsettings.GetSection("Logging:IsConsole").Value ?? "false").TryParseByBool();
        private static readonly Queue<LogItem> _queue = new Queue<LogItem>();
        private static volatile bool _useQueue = false;
        private static bool _initialize = false;

        public static bool IsNoneEnabled { get; set; }
        public static bool IsCriticalEnabled { get; private set; }
        public static bool IsErrorEnabled { get; private set; }
        public static bool IsWarnEnabled { get; private set; }
        public static bool IsInfoEnabled { get; private set; }
        public static bool IsDebugEnabled { get; private set; }
        public static bool IsTraceEnabled { get; private set; }

        static LogRecord()
        {
            try
            {
                if (!Directory.Exists(_logPath))
                {
                    Directory.CreateDirectory(_logPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ResetLog();
        }

        internal static void Initialize(bool useQueue)
        {
            if (!_initialize && useQueue)
            {
                _useQueue = true;
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (_queue.Count > 0)
                        {
                            var count = _queue.Count > 20 ? 20 : _queue.Count;
                            var list = new List<LogItem>(count);
                            _lock.Enter();
                            try
                            {
                                for (var i = 0; i < count; i++)
                                    list.Add(_queue.Dequeue());
                                foreach (var item in list.GroupBy(d => d.FileName))
                                {
                                    WirteLogCore(item.Key, item.Select(d => d.Message));
                                }
                            }
                            finally
                            {
                                _lock.Exit();
                            }
                        }
                        Thread.Sleep(50);
                    }
                }, TaskCreationOptions.LongRunning);
            }
            _initialize = true;
        }

        private static void ResetLog()
        {
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
            if (_useQueue)
            {
                var dt = DateTime.Now;
                logMessage = $"{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {logMessage}{Environment.NewLine}";
                string logName = string.Format("{0}{1}_{2}", _logPath, dt.ToString("yyyyMMdd_HH"), fileName);
                if (!logName.EndsWith(".log"))
                {
                    logName += ".log";
                }
                _queue.Enqueue(new LogItem
                {
                    FileName = logName,
                    Message = logMessage
                });
            }
            else
            {
                WriteLogFileByWait(fileName, logMessage);
            }
        }

        private static Task WriteLogAsync(string fileName, string logMessage)
        {
            if (_useQueue)
            {
                WriteLogFileByQueue(fileName, logMessage);
                return Task.CompletedTask;
            }
            else
            {
                return WriteLogFileByWaitAsync(fileName, logMessage);
            }
        }

        private static void WriteLogFileByQueue(string fileName, string logMessage)
        {
            var dt = DateTime.Now;
            logMessage = $"{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {logMessage}{Environment.NewLine}";
            string logName = string.Format("{0}{1}_{2}", _logPath, dt.ToString("yyyyMMdd_HH"), fileName);
            if (!logName.EndsWith(".log"))
            {
                logName += ".log";
            }
            _lock.Enter();
            try
            {
                _queue.Enqueue(new LogItem
                {
                    FileName = logName,
                    Message = logMessage
                });
            }
            finally
            {
                _lock.Exit();
            }
        }

        private static async Task WriteLogFileByWaitAsync(string fileName, string logMessage)
        {
            var dt = DateTime.Now;
            logMessage = $"{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {logMessage}{Environment.NewLine}";
            string logName = string.Format("{0}{1}_{2}", _logPath, dt.ToString("yyyyMMdd_HH"), fileName);
            if (!logName.EndsWith(".log"))
            {
                logName += ".log";
            }

            LockObject lockObject;
            if (_fileLock.ContainsKey(fileName))
            {
                lockObject = _fileLock[fileName];
            }
            else
            {
                _lock.Enter();
                try
                {
                    if (_fileLock.ContainsKey(fileName))
                    {
                        lockObject = _fileLock[fileName];
                    }
                    else
                    {
                        lockObject = new LockObject();
                        _fileLock.Add(fileName, lockObject);
                    }
                }
                finally
                {
                    _lock.Exit();
                }
            }

            lockObject.Enter();
            try
            {
                await WirteLogCoreAsync(logName, logMessage);
            }
            catch (Exception ex)
            {
                try
                {
                    logMessage = $"{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {ex.GetBaseException().ToString()} - {logMessage}{Environment.NewLine}";
                    logName = string.Format("{0}{1}_{2}", _logPath, dt.ToString("yyyyMMdd_HH"), _logName);
                    await WirteLogCoreAsync(logName, logMessage);
                }
                catch
                {
                }
            }
            finally
            {
                lockObject.Exit();
            }
        }

        private static void WriteLogFileByWait(string fileName, string logMessage)
        {
            var dt = DateTime.Now;
            logMessage = $"{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {logMessage}{Environment.NewLine}";
            string logName = string.Format("{0}{1}_{2}", _logPath, dt.ToString("yyyyMMdd_HH"), fileName);
            if (!logName.EndsWith(".log"))
            {
                logName += ".log";
            }

            LockObject lockObject;
            if (_fileLock.ContainsKey(fileName))
            {
                lockObject = _fileLock[fileName];
            }
            else
            {
                _lock.Enter();
                try
                {
                    if (_fileLock.ContainsKey(fileName))
                    {
                        lockObject = _fileLock[fileName];
                    }
                    else
                    {
                        lockObject = new LockObject();
                        _fileLock.Add(fileName, lockObject);
                    }
                }
                finally
                {
                    _lock.Exit();
                }
            }

            lockObject.Enter();
            try
            {
                WirteLogCore(logName, logMessage);
            }
            catch (Exception ex)
            {
                try
                {
                    logMessage = $"{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {ex.GetBaseException().ToString()} - {logMessage}{Environment.NewLine}";
                    logName = string.Format("{0}{1}_{2}", _logPath, dt.ToString("yyyyMMdd_HH"), _logName);
                    WirteLogCore(logName, logMessage);
                }
                catch
                {
                }
            }
            finally
            {
                lockObject.Exit();
            }
        }


        private static void WirteLogCore(string file, string message)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        private static void WirteLogCore(string file, IEnumerable<string> messages)
        {
            if (messages == null || !messages.Any())
                return;
            using (FileStream fileStream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                foreach (var message in messages)
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
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
        public static async Task LogAsync(LogLevel logLevel, string fileName, string logMessage)
        {
            if (!IsEnabled(_logLevel))
                return;

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            await WriteLogAsync($"{fileName}_{logLevel.ToString()}", logMessage);
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
        public static async Task TraceAsync(string fileName, string logMessage)
        {
            if (!IsTraceEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            await WriteLogAsync(fileName + "_Trace", logMessage);
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
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        public static async Task DebugAsync(string fileName, string logMessage)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            await WriteLogAsync(fileName + "_Debug", logMessage);
            if (_isConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
                Console.WriteLine();
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
        public static async Task InfoAsync(string fileName, string logMessage)
        {
            if (!IsInfoEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            await WriteLogAsync(fileName + "_Info", logMessage);
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
        public static async Task WarnAsync(string fileName, string logMessage)
        {
            if (!IsWarnEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            await WriteLogAsync(fileName + "_Warn", logMessage);
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
        public static async Task ErrorAsync(string fileName, string logMessage)
        {
            if (!IsErrorEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            await WriteLogAsync(fileName + "_Error", logMessage);
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
        public static async Task CriticalAsync(string fileName, string logMessage)
        {
            if (!IsCriticalEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            await WriteLogAsync(fileName + "_Critical", logMessage);
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
        public static Task NoneAsync(string fileName, string logMessage)
        {
            return Task.CompletedTask;
        }

        private class LogItem
        {
            public string FileName { get; set; }
            public string Message { get; set; }
        }
    }
}
