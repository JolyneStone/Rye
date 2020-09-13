using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Monica.AspectFlare
{
    public class GlobalInterceptorCollection : IEnumerable<IInterceptor>
    {
        private List<ICallingInterceptor> _callings;
        private List<ICalledInterceptor> _calleds;
        private IExceptionInterceptor _exception;

        private GlobalInterceptorCollection()
        {
            _callings = new List<ICallingInterceptor>();
            _calleds = new List<ICalledInterceptor>();
        }

        private static readonly Lazy<GlobalInterceptorCollection> _value = new Lazy<GlobalInterceptorCollection>(() => new GlobalInterceptorCollection(), true);
        public static GlobalInterceptorCollection GlobalInterceptors { get => _value.Value; } 

        public int Count => _callings.Count + _calleds.Count + (_exception == null ? 0 : 1);

        public void Clear()
        {
            _callings.Clear();
            _calleds.Clear();
            _exception = null;
        }

        public void AddCalling(ICallingInterceptor callingInterceptor) => _callings.Add(callingInterceptor);

        public void AddCalled(ICalledInterceptor calledInterceptor) => _calleds.Add(calledInterceptor);

        public void AddException(IExceptionInterceptor exceptionInterceptor) => _exception = exceptionInterceptor;

        public void AddRange(IEnumerable<ICallingInterceptor> callings) => _callings.AddRange(callings);

        public void AddRange(IEnumerable<ICalledInterceptor> calleds) => _calleds.AddRange(calleds);

        public IEnumerable<ICallingInterceptor> GetCallingInterceptors()
        {
            return _callings;
        }

        public IEnumerable<ICalledInterceptor> GetCalledInterceptors()
        {
            return _calleds;
        }

        public IExceptionInterceptor GetExceptionInterceptor()
        {
            return _exception;
        }

        public bool Contains(ICallingInterceptor calling)
        {
            if (calling == null)
            {
                return false;
            }

            return _callings.Any(x => x.Equals(calling));
        }

        public bool Contains(ICalledInterceptor called)
        {
            if (called == null)
            {
                return false;
            }

            return _calleds.Any(x => x.Equals(called));
        }

        public bool Contains(IExceptionInterceptor exception)
        {
            if (_exception == null || exception == null)
            {
                return false;
            }

            return _exception.Equals(exception);
        }

        public IEnumerator<IInterceptor> GetEnumerator()
        {
            foreach (var calling in _callings)
            {
                yield return calling;
            }

            foreach (var called in _calleds)
            {
                yield return called;
            }

            yield return _exception;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
