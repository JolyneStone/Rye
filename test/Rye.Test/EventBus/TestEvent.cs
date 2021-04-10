using Rye.EventBus.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Test.EventBus
{
    public class TestEvent : IEvent
    {
        public int Id { get; set; }
    }
}
