using Microsoft.Extensions.Logging;
using Rye.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye
{
    public sealed class Log
    {
        private static IStaticLog _current;
        public static IStaticLog Current { get => _current; internal set => _current = value; }

        public static void None(string message)
        {
            _current.None(message);
        }

        public static void Debug(string message)
        {
            _current.Debug(message);
        }

        public static void Trace(string message)
        {
            _current.Trace(message);
        }

        public static void Warning(string message)
        {
            _current.Warning(message);
        }

        public static void Information(string message)
        {
            _current.Information(message);
        }

        public static void Error(string message)
        {
            _current.Error(message);
        }

        public static void Critical(string message)
        {
            _current.Critical(message);
        }
    }

}
