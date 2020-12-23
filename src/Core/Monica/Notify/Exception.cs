using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monica.Notify
{
    public class NotificationException : Exception
    {
        public NotificationException(string message, INotification notification) : base(message)
        {
            Notification = notification;
        }

        public NotificationException(string message, INotification notification, Exception innerException)
            : base(message, innerException)
        {
            Notification = notification;
        }

        public INotification Notification { get; set; }
    }

    public class RetryAfterException : NotificationException
    {
        public RetryAfterException(INotification notification, string message, DateTime retryAfterUtc) : base(message, notification)
        {
            RetryAfterUtc = retryAfterUtc;
        }

        public DateTime RetryAfterUtc { get; set; }
    }
}
