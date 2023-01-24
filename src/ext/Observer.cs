using System.Collections.Specialized;

namespace SearchAThing.Ext;

public enum ObserverEventType { Add = 1, Remove = 2, PropertyChanged = 4 };

/// <summary>
/// propChange will null for Add,Remove events ;
/// items will null for PropertyChanged event
/// </summary>
public delegate void ObserverEventDelegate(
    ObserverEventType eventType,
    PropertyChangedEventArgs? propChange,
    IList? items);

/// <summary>
/// track items add, remove, clear, replace and property changes;
/// clear will reports remove of old items ;
/// replace will reports remove of old items and add of new ones ;
/// </summary>
public class Observer<I> : IDisposable
    where I : INotifyPropertyChanged
{
    public ObservableCollection2<I> OBC { get; private set; }

    /// <summary>
    /// notify about subscribed changes
    /// </summary>
    public event ObserverEventDelegate? Change;

    public bool ListenItemAdd { get; private set; }
    public bool ListenItemRemove { get; private set; }
    public bool ListenItemPropertyChanged { get; private set; }

    /// <summary>
    /// if specify no events ( all are listened )
    /// </summary>
    public Observer(ObservableCollection2<I> obc, params ObserverEventType[] events)
    {
        OBC = obc;

        if (events.Length == 0)
        {
            ListenItemAdd = ListenItemRemove = ListenItemPropertyChanged = true;
        }
        else
        {
            foreach (var ev in events)
            {
                if (ev == ObserverEventType.Add) ListenItemAdd = true;
                else if (ev == ObserverEventType.Remove) ListenItemRemove = true;
                else if (ev == ObserverEventType.PropertyChanged) ListenItemPropertyChanged = true;
            }
        }

        if (ListenItemAdd || ListenItemRemove)
            obc.CollectionChanged += OBCCollectionChanged;

        if (ListenItemRemove)
            obc.Clearing += OBCClearing;

        if (ListenItemPropertyChanged)
            foreach (var item in obc) item.PropertyChanged += OBCItemPropertyChanged;
    }

    private void OBCClearing(object sender, IList<I> items)
    {
        foreach (var item in items) item.PropertyChanged -= OBCItemPropertyChanged;
        Change?.Invoke(ObserverEventType.Remove, null, (IList)items);
    }

    private void OBCItemPropertyChanged(object sender, PropertyChangedEventArgs e) =>
        Change?.Invoke(ObserverEventType.PropertyChanged, e, items: null);

    private void OBCCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
        {
            Change?.Invoke(ObserverEventType.Add, null, e.NewItems);

            if (ListenItemPropertyChanged)
            {
                foreach (var item in e.NewItems)
                    ((INotifyPropertyChanged)item).PropertyChanged += OBCItemPropertyChanged;
            }
        }

        if (e.OldItems is not null)
        {
            Change?.Invoke(ObserverEventType.Remove, null, e.OldItems);

            if (ListenItemPropertyChanged)
            {
                foreach (var item in e.OldItems)
                    ((INotifyPropertyChanged)item).PropertyChanged -= OBCItemPropertyChanged;
            }
        }
    }

    public void Dispose()
    {
        if (ListenItemAdd || ListenItemRemove)
        {
            OBC.CollectionChanged -= OBCCollectionChanged;
            OBC.Clearing -= OBCClearing;
        }

        if (ListenItemPropertyChanged)
        {
            foreach (var item in OBC) item.PropertyChanged -= OBCItemPropertyChanged;
        }
    }

}

public static partial class Ext
{

    public static Observer<I> Observe<I>(this ObservableCollection2<I> obc) where I : INotifyPropertyChanged =>
        new Observer<I>(obc);

}