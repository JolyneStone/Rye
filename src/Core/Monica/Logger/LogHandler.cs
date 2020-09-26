using System.IO;
using System.Text;

namespace Monica.Logger
{
    internal sealed class LogHandler : Disruptor.IEventHandler<LogEntry>
    {
        public void OnEvent(LogEntry data, long sequence, bool endOfBatch)
        {
            if (data == null)
                return;
            if (!data.FileName.EndsWith(".log"))
            {
                data.FileName += ".log";
            }
            using (FileStream fileStream = new FileStream(data.FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                var bytes = Encoding.UTF8.GetBytes(data.Message);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
