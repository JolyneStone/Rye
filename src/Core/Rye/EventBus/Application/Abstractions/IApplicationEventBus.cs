using Rye.EventBus.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.EventBus.Application
{
    public interface IApplicationEventBus: IEventBus, IApplicationEventPublisher, IApplicationEventSubscriber
    {
    }
}
