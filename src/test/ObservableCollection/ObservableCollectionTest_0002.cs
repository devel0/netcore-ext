using System.Collections.Specialized;

namespace SearchAThing.Ext.Tests;

public partial class ObservableCollectionTests
{

    [Fact]
    public void ObservableCollectionTest_0002()
    {
        var obc = new ObservableCollection2<int>();

        obc.Add(1);
        obc.Add(2);

        obc.CollectionChanged += (sender, e) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Reset, e.Action);
            Assert.Null(e.OldItems);
        };

        obc.Clearing += (sender, items) =>
        {
            Assert.Equal(2, items.Count);
            Assert.Equal(2, obc.Count);
        };

        obc.Clear();
    }
 
}