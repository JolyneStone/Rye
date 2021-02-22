using System;
using System.Collections.Generic;

namespace Rye.AspectFlare.Utilities
{
    internal class HandleCollection
    {
        private static object _sync = new object();
        private readonly static Lazy<IDictionary<int, IList<RuntimeMethodHandle>>> _handlesDictionary =
            new Lazy<IDictionary<int, IList<RuntimeMethodHandle>>>(() => new Dictionary<int, IList<RuntimeMethodHandle>>(), true);

        public static IList<RuntimeMethodHandle> GetHandles(int key)
        {
            return _handlesDictionary.Value[key];
        }

        public static void AddHandles(int key, IList<RuntimeMethodHandle> methodHandles)
        {
            lock (_sync)
            {
                _handlesDictionary.Value[key] = methodHandles;
            }
        }
    }
}
