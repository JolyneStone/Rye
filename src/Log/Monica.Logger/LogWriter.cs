using Disruptor;
using Disruptor.Dsl;
using System;
using System.Threading.Tasks;

namespace Monica.Logger
{
    internal sealed class LogWriter : IDisposable
    {
        private RingBuffer<LogEntry> _ringBuffer;
        private Disruptor<LogEntry> _disruptor;
        private const int RingBufferSize = 1024;

        internal LogWriter()
        {
            _disruptor = new Disruptor<LogEntry>(() => new LogEntry(), RingBufferSize, TaskScheduler.Default);
            _disruptor.HandleEventsWith(new LogHandler());
            _ringBuffer = _disruptor.Start();
        }

        public void Push(string fileName, string logMessage)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            long sequence = _ringBuffer.Next();
            try
            {
                var entry = _ringBuffer[sequence];
                entry.FileName = fileName;
                entry.Message = logMessage;
                _ringBuffer.Publish(sequence);
            }
            finally
            {
                _ringBuffer.Publish(sequence);
            }
        }

        public void Dispose()
        {
            _disruptor?.Shutdown();
            _ringBuffer = null;
            GC.SuppressFinalize(this);
        }
    }
}
