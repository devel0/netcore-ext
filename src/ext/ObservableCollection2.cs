namespace SearchAThing.Ext;

// https://stackoverflow.com/a/225778/5521766

/// <summary>
/// ObservableCollection specialized with ItemsAdded, ItemsRemoved events where
/// removed event occurs even for the obc.Clear()
/// </summary>
/// <example>
/// \snippet ObservableCollection/ObservableCollectionTest_0001.cs example
/// </example>
public class ObservableCollection2<T> : ObservableCollection<T>
{

    /// <summary>
    /// one or more items added
    /// </summary>
    public event EventHandler<IList<T>>? ItemsAdded;

    /// <summary>
    /// one or more items removed
    /// </summary>
    public event EventHandler<IList<T>>? ItemsRemoved;

    /// <summary>
    /// override clear items
    /// </summary>
    protected override void ClearItems()
    {
        if (ItemsRemoved is not null)
        {
            var itemsRemoved = new List<T>(Items);
            base.ClearItems();
            ItemsRemoved.Invoke(this, itemsRemoved);
        }
        else
            base.ClearItems();
    }

    protected override void RemoveItem(int index)
    {
        if (ItemsRemoved is not null)
        {
            var itemsRemoved = new List<T> { this[index] };
            base.RemoveItem(index);
            ItemsRemoved.Invoke(this, itemsRemoved);
        }
        else
            base.RemoveItem(index);

    }

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        ItemsAdded?.Invoke(this, new List<T> { item });
    }

    protected override void SetItem(int index, T item)
    {
        if (ItemsAdded is not null || ItemsRemoved is not null)
        {
            var oldItem = this[index];
            base.SetItem(index, item);
            ItemsRemoved?.Invoke(this, new List<T> { oldItem });
            ItemsAdded?.Invoke(this, new List<T> { item });
        }
        else
            base.SetItem(index, item);
    }

}
