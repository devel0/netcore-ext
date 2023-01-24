using System.Collections.Specialized;

namespace SearchAThing.Ext.Tests;

public partial class ObservableCollectionTests
{

    [Fact]
    public void ObservableCollectionTest_0001()
    {
        var obc = new ObservableCollection<TestItem>();

        var testItem1 = new TestItem(1);
        var testItem2 = new TestItem(2);

        obc.Add(testItem1);
        obc.Add(testItem2);

        obc.CollectionChanged += (sender, e) =>
        {
            Assert.Equal(NotifyCollectionChangedAction.Replace, e.Action);

            Assert.NotNull(e.OldItems);
            Assert.Equal(1, e.OldItems.Count);
            Assert.IsType(typeof(TestItem), e.OldItems[0]);
            Assert.Equal(testItem1, e.OldItems[0]);

            Assert.NotNull(e.NewItems);
            Assert.Equal(1, e.NewItems.Count);
            Assert.IsType(typeof(TestItem), e.NewItems[0]);
            Assert.Equal(testItem2, e.NewItems[0]);
        };

        obc[0] = obc[1];
    }
}