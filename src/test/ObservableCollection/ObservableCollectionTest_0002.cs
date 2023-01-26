using System.Collections.Specialized;

namespace SearchAThing.Ext.Tests;

public partial class ObservableCollectionTests
{

    [Fact]
    public void ObservableCollectionTest_0002()
    {        
        var obc = new ObservableCollection2<int>();

        // add two items
        obc.Add(1);
        obc.Add(2);

        // attach some events
        {
            // listen for Clear() before items disappears
            obc.Clearing += (sender, items) =>
            {
                Assert.Equal(2, items.Count);
                Assert.Equal(2, obc.Count);
            };

            // std collection changed
            obc.CollectionChanged += (sender, e) =>
            {
                Assert.Equal(NotifyCollectionChangedAction.Reset, e.Action);
                // standard collection changed on Reset (i.e obc Clear()) have null OldItems
                Assert.Null(e.OldItems);
            };
        }

        // clear the obc will imply (1) Clearing and finally (2) CollectionChanged with Reset action
        obc.Clear();        
    }

}