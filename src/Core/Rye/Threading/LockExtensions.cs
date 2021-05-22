
using System;

namespace Rye.Threading
{
    /// <summary>
    /// lock 扩展
    /// </summary>
    public static class LockExtensions
    {
		public static void Locking(this object source, Action action)
		{
			lock (source)
			{
				action();
			}
		}

		public static void Locking<T>(this T source, Action<T> action) where T : class
		{
			object obj = source;
			lock (obj)
			{
				action(source);
			}
		}

		public static TResult Locking<TResult>(this object source, Func<TResult> func)
		{
			TResult result;
			lock (source)
			{
				result = func();
			}
			return result;
		}

		public static TResult Locking<T, TResult>(this T source, Func<T, TResult> func) where T : class
		{
			object obj = source;
			TResult result;
			lock (obj)
			{
				result = func(source);
			}
			return result;
		}
	}
}
