namespace SearchAThing.Ext;

// https://stackoverflow.com/a/225778/5521766

public interface INotifyCollectionChanged2
{

    /// <summary>
    /// one or more items added
    /// </summary>
    event EventHandler<IList>? ItemsAdded;

    /// <summary>
    /// one or more items removed
    /// </summary>
    event EventHandler<IList>? ItemsRemoved;

    /// <summary>
    /// an item was replaced
    /// </summary>
    event EventHandler<(object? oldItem, object? newItem)>? ItemReplaced;

}

/// <summary>
/// ObservableCollection specialized with ItemsAdded, ItemsRemoved that allow to
/// track for Clear, Remove, Insert, Set actions
/// </summary>
/// <example>
/// \snippet ObservableCollection/ObservableCollectionTest_0001.cs example
/// </example>
public class ObservableCollection2<T> : ObservableCollection<T>, INotifyCollectionChanged2
{

    #region INotifyCollectionChanged2

    /// <summary>
    /// one or more items added
    /// </summary>
    public event EventHandler<IList>? ItemsAdded;

    /// <summary>
    /// one or more items removed
    /// </summary>
    public event EventHandler<IList>? ItemsRemoved;

    /// <summary>
    /// an item was replaced
    /// </summary>
    public event EventHandler<(object? oldItem, object? newItem)>? ItemReplaced;

    #endregion

    public ObservableCollection2()
    {
    }

    public ObservableCollection2(IEnumerable<T> items) : base(items)
    {

    }

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
            ItemReplaced?.Invoke(this, (oldItem, item));
        }
        else
            base.SetItem(index, item);
    }

}
