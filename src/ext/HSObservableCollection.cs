namespace SearchAThing.Ext;

/// <summary>
/// ObservableCollection backed with hashset for the Contains test.
/// </summary>
public class HSObservableCollection<T> : ObservableCollection<T>
{

    HashSet<T> hs;

    public HSObservableCollection()
    {
        hs = new HashSet<T>();
    }

    public HSObservableCollection(IEnumerable<T> items) : base(items)
    {
        hs = new HashSet<T>(items);
    }

    public new bool Contains(T item) => hs.Contains(item);

    protected override void ClearItems()
    {
        base.ClearItems();
        hs.Clear();
    }

    protected override void RemoveItem(int index)
    {
        var oldItem = this[index];
        hs.Remove(oldItem);
        base.RemoveItem(index);
    }

    protected override void InsertItem(int index, T item)
    {
        hs.Add(item);
        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, T item)
    {
        var oldItem = this[index];
        hs.Remove(oldItem);
        hs.Add(item);
        base.SetItem(index, item);
    }

}
