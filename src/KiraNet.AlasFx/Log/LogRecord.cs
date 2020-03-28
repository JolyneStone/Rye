using KiraNet.AlasFx.Configuration;
using KiraNet.AlasFx.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace KiraNet.AlasFx.Log
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public class LogRecord
    {
        private static readonly Dictionary<string, LockObject> _fileLock = new Dictionary<string, LockObject>();
        private static readonly object _lock = new object();
        private static readonly LogLevel LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), ConfigurationManager.Appsettings.GetSection("Logging:LogLevel").Value ?? "Debug");
        private static readonly string LogPath = ConfigurationManager.Appsettings.GetSection("Logging:LogPath").Value ?? @"/home/admin/logs/temp";
        private static readonly bool IsConsoleEnabled = (ConfigurationManager.Appsettings.GetSection("Logging:IsConsole").Value ?? "false").TryParseByBool();

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
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ResetLog();
        }

        private static void ResetLog()
        {
            #region LogLevel

            switch (LogLevel)
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

        private static void WriteLogSingle(string fileName, string logMessage)
        {
            logMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {logMessage}{Environment.NewLine}";
            string logName = string.Format("{0}{1}_{2}", LogPath, DateTime.Now.ToString("yyyyMMdd_HH"), fileName);
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
                lock (_lock)
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
            }

            lockObject.Enter();
            try
            {
                using (FileStream fileStream = new FileStream(logName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, System.Text.Encoding.UTF8))
                    {
                        binaryWriter.Write(logMessage.ToCharArray());
                    }
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                lockObject.Exit();
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
            if (!IsEnabled(LogLevel))
                return;

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            WriteLogSingle($"{fileName}_{logLevel.ToString()}", logMessage);
            if (IsConsoleEnabled)
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
            WriteLogSingle(fileName + "_Trace", logMessage);
            if (IsConsoleEnabled)
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
            WriteLogSingle(fileName + "_Debug", logMessage);
            if (IsConsoleEnabled)
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
            WriteLogSingle(fileName + "_Info", logMessage);
            if (IsConsoleEnabled)
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
            WriteLogSingle(fileName + "_Warn", logMessage);
            if (IsConsoleEnabled)
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
            WriteLogSingle(fileName + "_Error", logMessage);
            if (IsConsoleEnabled)
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
            WriteLogSingle(fileName + "_Critical", logMessage);
            if (IsConsoleEnabled)
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
    }
}
