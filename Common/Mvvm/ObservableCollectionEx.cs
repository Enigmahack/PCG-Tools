using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Common.Mvvm
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Unsubscribe(e.OldItems);
            Subscribe(e.NewItems);
            base.OnCollectionChanged(e);
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void ClearItems()
        {
            foreach (var element in this)
                element.PropertyChanged -= ContainedElementChanged;
            base.ClearItems();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iList"></param>
        private void Subscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (T element in iList)
                    element.PropertyChanged += ContainedElementChanged;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iList"></param>
        private void Unsubscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (T element in iList)
                    element.PropertyChanged -= ContainedElementChanged;
            }
        }


        /// <summary>
        /// Replaces all items in one operation and fires a single Reset notification
        /// instead of one notification per item, avoiding O(n²) layout work in WPF.
        /// </summary>
        public void ReplaceAll(IEnumerable<T> newItems)
        {
            CheckReentrancy();
            foreach (var item in this)
                item.PropertyChanged -= ContainedElementChanged;
            Items.Clear();
            if (newItems != null)
            {
                foreach (var item in newItems)
                {
                    Items.Add(item);
                    item.PropertyChanged += ContainedElementChanged;
                }
            }
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }
    }
}
