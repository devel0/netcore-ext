namespace SearchAThing.Ext;

/// <summary>
/// ObservableCollection backed with hashset for the Contains test.<br/>
/// </summary>
/// <remarks>
/// Normal behavior of obc still the same, ie. add the same object two times will result in obc increase in size
/// with two references to the same object, and in the same way removing doesn't invalidate the presence until
/// last occurrence of contains tested object will be removed.
/// </remarks>
public class HSObservableCollection<T> : ObservableCollection<T>
{

    HashSet<T> hs;

    /// <summary>
    /// dictionary to count items with more than 1 occurrences.
    /// </summary>
    Dictionary<T, int> itemCnt;

    public HSObservableCollection()
    {
        hs = new HashSet<T>();
        itemCnt = new Dictionary<T, int>();
    }

    public HSObservableCollection(IEnumerable<T> items) : base(items)
    {
        hs = new HashSet<T>(items);
        itemCnt = new Dictionary<T, int>();
    }

    public new bool Contains(T item) => hs.Contains(item);

    protected override void ClearItems()
    {
        base.ClearItems();
        hs.Clear();
        itemCnt.Clear();
    }

    void hsRemove(T oldItem)
    {
        if (itemCnt.TryGetValue(oldItem, out var cnt))
        {
            if (cnt <= 1) throw new InternalError("expecting cnt>1");

            if (cnt == 2)
                itemCnt.Remove(oldItem);
            else
                --itemCnt[oldItem];
        }
        else
            hs.Remove(oldItem);
    }

    protected override void RemoveItem(int index)
    {
        var oldItem = this[index];
        hsRemove(oldItem);
        base.RemoveItem(index);
    }

    void hsAdd(T item)
    {
        if (hs.Contains(item))
        {
            if (itemCnt.TryGetValue(item, out var cnt))
            {
                if (cnt <= 1) throw new InternalError("expecting cnt>1");

                ++itemCnt[item];
            }
            else
                itemCnt.Add(item, 2);
        }
        else
            hs.Add(item);
    }

    protected override void InsertItem(int index, T item)
    {
        hsAdd(item);
        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, T item)
    {
        var oldItem = this[index];
        hsRemove(oldItem);
        hsAdd(item);
        base.SetItem(index, item);
    }    

}
