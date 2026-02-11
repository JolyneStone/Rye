using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Rye.EventBus.RabbitMQ.Event
{
    public class ConnectionEventArgs : AsyncEventArgs
    {
        public IConnection Connection { get; set; }

        public ConnectionEventArgs(CancellationToken cancellationToken = default) : base(cancellationToken)
        {

        }

        public ConnectionEventArgs(IConnection connection, CancellationToken cancellationToken = default) : base(cancellationToken)
        {
            this.Connection = connection;
        }
    }
}
