using InstagramPatterns.InstagramApi;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPatterns
{
    public class ConcurrentQueueDownloadedImage : IEnumerable<DownloadedMedia>, ICollection, INotifyCollectionChanged
    {
        protected ConcurrentQueueDownloadedImage(){ }

        private static ConcurrentQueueDownloadedImage instance = new ConcurrentQueueDownloadedImage();
        public static ConcurrentQueueDownloadedImage Instance
        { 
            get { return instance; } 
        }

        private readonly ConcurrentQueue<DownloadedMedia> queue = new ConcurrentQueue<DownloadedMedia>();

        public void Enqueue(DownloadedMedia item)
        {
            queue.Enqueue(item);
            OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public DownloadedMedia Dequeue()
        {
            DownloadedMedia item;
            if (queue.TryDequeue(out item))
            {
                List<DownloadedMedia> oldItems = new List<DownloadedMedia>();
                oldItems.Add(item);
                OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, 0));
            }
            return item;
        }

        public DownloadedMedia Peek()
        {
            DownloadedMedia item;
            queue.TryPeek(out item);
            return item;
        }

        public IEnumerator<DownloadedMedia> GetEnumerator()
        {
            return queue.GetEnumerator();
        }
    
        IEnumerator IEnumerable.GetEnumerator()
        {
 	        return queue.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            queue.CopyTo((DownloadedMedia[])array, index);
        }

        public int Count
        {
	        get { return queue.Count; }
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)queue).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)queue).SyncRoot; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs a)
        {
            if (CollectionChanged != null)
                CollectionChanged(sender, a);
        }
    }
}
