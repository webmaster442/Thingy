using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Thingy.Controls
{
    public sealed class TrulyObservableCollection<T> : ObservableCollection<T>, INotifyPropertyChanged
         where T : INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged
        {
            add { base.PropertyChanged += value; }
            remove { base.PropertyChanged -= value; }
        }

        public event EventHandler<ItemChangedEventArgs<T>> ItemChanged;

        public TrulyObservableCollection()
            : base()
        {
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            item.PropertyChanged += ItemPropertyChanged;
        }

        protected override void RemoveItem(int index)
        {
            Items[index].PropertyChanged -= ItemPropertyChanged;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (T item in Items)
            {
                item.PropertyChanged -= ItemPropertyChanged;
            }
            base.ClearItems();
        }

        protected override void SetItem(int index, T item)
        {
            T oldItem = Items[index];
            T newItem = item;
            oldItem.PropertyChanged -= ItemPropertyChanged;
            newItem.PropertyChanged += ItemPropertyChanged;
            base.SetItem(index, item);
        }

        public void AddRange(IEnumerable<T> dataToAdd)
        {
            this.CheckReentrancy();

            //int startingIndex = this.Count;

            foreach (T data in dataToAdd)
            {
                var listener = data as INotifyPropertyChanged;
                listener.PropertyChanged += ItemPropertyChanged;
                this.Items.Add(data);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, dataToAdd.ToList()));
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new ItemChangedEventArgs<T>((T)sender, e.PropertyName);
            this.ItemChanged?.Invoke(this, args);
        }
    }

    internal interface ICollectionItemPropertyChanged<T>
    {
        event EventHandler<ItemChangedEventArgs<T>> ItemChanged;
    }

    public class ItemChangedEventArgs<T>
    {
        public T ChangedItem { get; }
        public string PropertyName { get; }

        public ItemChangedEventArgs(T item, string propertyName)
        {
            this.ChangedItem = item;
            this.PropertyName = propertyName;
        }
    }
}
