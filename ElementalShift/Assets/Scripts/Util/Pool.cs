using System;
using UnityEngine;
using System.Collections.Generic;
 
namespace Pooling
{
    public enum LoadingMode { Eager, Lazy, LazyExpanding };
 
    public enum AccessMode { FIFO, LIFO, Circular };
 
    public class Pool<T> where T : new() {
        private LoadingMode loadingMode;
        private IItemStore itemStore;
        private int size;
        private int count;
 
        public Pool(int size) : this(size, LoadingMode.Lazy, AccessMode.FIFO) {
        }
 
        public Pool(int size, LoadingMode loadingMode, AccessMode accessMode) {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", size,
                    "Argument 'size' must be greater than zero.");
 
            this.size = size;
            this.loadingMode = loadingMode;
            this.itemStore = CreateItemStore(accessMode, size);
            if (loadingMode == LoadingMode.Eager) {
                PreloadItems();
            }
        }
 
        public T Acquire() {
            switch (loadingMode)
            {
                case LoadingMode.Eager:
                    return AcquireEager();
                case LoadingMode.Lazy:
                    return AcquireLazy();
                default:
                    //Debug.Assert(loadingMode == LoadingMode.LazyExpanding, "Unknown LoadingMode encountered in Acquire method.");
                    return AcquireLazyExpanding();
            }
        }
 
        public void Release(T item) {
            itemStore.Store(item);
        }
 
        #region Acquisition
 
        private T AcquireEager() {
            return itemStore.Fetch();
        }
 
        private T AcquireLazy() {
            if (itemStore.Count > 0) {
                return itemStore.Fetch();
            }
            count++;
            return new T();
        }
 
        private T AcquireLazyExpanding() {
            bool shouldExpand = false;
            if (count < size) {
                int newCount = count++;
                if (newCount <= size) {
                    shouldExpand = true;
                } else {
                    // Another thread took the last spot - use the store instead
                    count--;
                }
            }
            if (shouldExpand) {
                return new T();
            } else {
				return itemStore.Fetch();
            }
        }
 
        private void PreloadItems() {
            for (int i = 0; i < size; i++) {
                T item = new T();
                itemStore.Store(item);
            }
            count = size;
        }
 
        #endregion
 
        #region Collection Wrappers
 
        interface IItemStore {
            T Fetch();
            void Store(T item);
            int Count { get; }
        }
 
        private IItemStore CreateItemStore(AccessMode mode, int capacity) {
            switch (mode) {
                case AccessMode.FIFO:
                    return new QueueStore(capacity);
                case AccessMode.LIFO:
                    return new StackStore(capacity);
                default:
                    return new QueueStore(capacity);
            }
        }
 
        class QueueStore : Queue<T>, IItemStore {
            public QueueStore(int capacity) : base(capacity)
            {
            }
 
            public T Fetch() {
                return Dequeue();
            }
 
            public void Store(T item) {
                Enqueue(item);
            }
        }
 
        class StackStore : Stack<T>, IItemStore {
            public StackStore(int capacity) : base(capacity) {
            }
 
            public T Fetch() {
                return Pop();
            }
 
            public void Store(T item) {
                Push(item);
            }
        }
   
        #endregion
    }
}