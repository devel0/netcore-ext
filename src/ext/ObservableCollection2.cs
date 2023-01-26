namespace SearchAThing.Ext;

// https://stackoverflow.com/a/225778/5521766

/// <summary>
/// Override of the ClearItems behavior for ObservableCollection in order to prepend a Clearing event
/// useful to operate on items being cleared.
/// Address the problem that using ObservableCollection the Clear() fire a CollectionChanged Reset event
/// but within a null OldItems.
/// </summary>
public class ObservableCollection2<T> : ObservableCollection<T>
{

    /// <summary>
    /// Allow to track clearing event    
    /// </summary>    
    public event EventHandler<IList<T>>? Clearing;

    /// <summary>
    /// override clear items
    /// </summary>
    protected override void ClearItems()
    {
        Clearing?.Invoke(this, Items);
        base.ClearItems();
    }
}
