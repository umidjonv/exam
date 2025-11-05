using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace EX.Common.Collections
{
    /// <summary>
    ///     https://jamesmccaffrey.wordpress.com/2010/11/26/a-c-big-array/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HugeArray<T> : IEnumerable<T> where T : struct
    {
        public static int Size = (int.MaxValue >> 4) / Marshal.SizeOf<T>();
        private readonly long _capacity;
        private readonly T[][] _data;

        public HugeArray(long capacity)
        {
            _capacity = capacity;

            var chunks = (int)(capacity / Size);
            var remainder = (int)(capacity % Size);

            _data = remainder == 0 ? new T[chunks][] : new T[chunks + 1][];

            for (var i = 0; i < chunks; i++)
                _data[i] = new T[Size];

            if (remainder > 0)
                _data[_data.Length - 1] = new T[remainder];
        }

        public T this[long index]
        {
            get
            {
                if (index < 0 || index >= _capacity)
                    throw new IndexOutOfRangeException();

                var chunk = (int)(index / Size);
                var offset = (int)(index % Size);

                return _data[chunk][offset];
            }
            set
            {
                if (index < 0 || index >= _capacity)
                    throw new IndexOutOfRangeException();

                var chunk = (int)(index / Size);
                var offset = (int)(index % Size);

                _data[chunk][offset] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.SelectMany(c => c).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}