using System.Collections;
using System.Collections.Generic;

namespace Html.Rt
{
    public class CacheEnumerable<T> :IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        public CacheEnumerable(IEnumerable<T> source)
        {
            this._source = source;
        }
        public IEnumerator<T> GetEnumerator()
        {
            var result = this._source.GetEnumerator();
            return new CacheEnumerator<T>(result);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CacheEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator<T> _cache;
        private IList<T> _items;

        private bool _isDone = false;

        public bool IsDone
        {
            get { return this._isDone; }
        }

        public CacheEnumerator(IEnumerator<T> enumerator)
        {
            this._cache = enumerator;
            this._items = new List<T>();
        }
        public bool MoveNext()
        {
            var result = this._cache.MoveNext();
            if (!result)
            {
                this._isDone = true;
                this._cache = this._items.GetEnumerator();
                this._cache.Dispose();
            }

            this._items.Add(this.Current);
            return true;

        }

        public void Reset()
        {
            this._cache.Reset();
        }

        public T Current => this._cache.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            this._cache.Reset();
        }
    }
}