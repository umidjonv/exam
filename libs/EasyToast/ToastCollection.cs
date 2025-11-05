using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyToast
{
    public class ToastCollection : ICollection<Toast>
    {
        private readonly List<Toast> _privateList;

        /// <summary>
        /// Initialize empty Toast collection
        /// </summary>
        public ToastCollection()
        {
            _privateList = new List<Toast>(ToastManager.MAX_TOASTS_ALLOWED);
        }

        /// <summary>
        /// Initialize Toast collection from other collection
        /// </summary>
        /// <param name="other"></param>
        public ToastCollection(ToastCollection other)
        {
            if (other != null)
            {
                _privateList = other._privateList;
            }
        }

        /// <summary>
        /// Gets or sets value of specific Toast in collection by index
        /// </summary>
        /// <param name="index0"></param>
        /// <returns></returns>
        public Toast this[int index0]
        {
            get => _privateList[index0];
            set => _privateList[index0] = value;
        }

        /// <summary>
        /// Locate index of Toast in collection
        /// </summary>
        /// <param name="toast"></param>
        /// <returns></returns>
        public int Locate(Toast toast)
        {
            return _privateList.IndexOf(toast);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Toast> GetEnumerator()
        {
            return _privateList.GetEnumerator();
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        /// <summary>
        /// Add new Toast item to collection
        /// </summary>
        /// <param name="item">A Toast object</param>
        public void Add(Toast item)
        {
            _privateList.Add(item);
            ToastAdded?.Invoke(this, new ToastChangedEventArgs(item));
        }

        /// <inheritdoc />
        /// <summary>
        /// This will truncate collection, means all items will be cleaned
        /// </summary>
        public void Clear()
        {
            _privateList.Clear();
            if (_privateList.Count == 0)
            {
                CollectionTruncated?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool Contains(Toast item)
        {
            if (item == null) 
                throw new NullReferenceException("Toast item cannot be null");

            if (_privateList.Count == 0) return false;
            foreach (var toast in _privateList)
            {
                if (toast.Guid.Contains(toast.Guid))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(Toast[] array, int arrayIndex)
        {
            _privateList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        /// <summary>
        /// Remove Toast item from current collection
        /// </summary>
        /// <param name="toast"></param>
        /// <returns></returns>
        public bool Remove(Toast toast)
        {

            var res = _privateList.Remove(toast);
            if (res)
            {
                ToastRemoved?.Invoke(this, new ToastChangedEventArgs(toast));
                if (_privateList.Count == 0)
                {
                    CollectionTruncated?.Invoke(this, EventArgs.Empty);
                }
            }

            return res;
        }

        /// <summary>
        /// Remove all Toasts matched condition in predicate
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public int RemoveAll(Predicate<Toast> match)
        {
            var num = _privateList.RemoveAll(match);
            if (_privateList.Count == 0)
            {
                CollectionTruncated?.Invoke(this, EventArgs.Empty);
            }
            return num;
        }

        /// <summary>
        /// Get all Top-Right Toasts in collection
        /// </summary>
        /// <returns>Top-Right Toast list</returns>
        public IEnumerable<Toast> GetTopRightToasts()
        {
            if (_privateList.Count == 0) yield break;
            foreach (var toast in _privateList)
            {
                if (toast.Position == Position.TopRight)
                {
                    yield return toast;
                }
            }
        }

        /// <summary>
        /// Get all Bottom-Right Toasts in collection
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Toast> GetBottomRightToasts()
        {
            if (_privateList.Count == 0) yield break;
            foreach (var toast in _privateList)
            {
                if (toast.Position == Position.BottomRight)
                {
                    yield return toast;
                }
            }
        }

        public int Count => _privateList.Count;
        public bool IsReadOnly => false;

        public delegate void ToastAddedEventHandler(object sender, ToastChangedEventArgs e);

        public event ToastAddedEventHandler ToastAdded;

        public delegate void ToastRemovedEventHandler(object sender, ToastChangedEventArgs e);

        public event ToastRemovedEventHandler ToastRemoved;

        public delegate void CollectionTruncatedEventHandler(object sender, EventArgs e);

        public event CollectionTruncatedEventHandler CollectionTruncated;
    }
}
