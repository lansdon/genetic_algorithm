using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ga
{
	class ThreadSafeAddList<T> : List<T>
	{
//		private List<T> _list = new List<T>();
		private object _sync = new object();

		public void AddSafely(T value)
		{
			lock (_sync)
			{
				Add(value);
			}
		}
	}
}
