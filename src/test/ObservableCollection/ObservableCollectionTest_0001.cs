namespace SearchAThing.Ext.Tests;

public partial class ObservableCollectionTests
{

    //! [example]

    class TestItem
    {
        public TestItem(int value) { Value = value; }
        public int Value { get; set; }
    }

    [Fact]
    public void ObservableCollectionTest_0001()
    {
        var obc = new ObservableCollection2<TestItem>();

        int result = 0;

        obc.ItemsAdded += (sender, items) =>
        {
            foreach (var x in items.OfType<TestItem>()) result += x.Value;
        };

        obc.ItemsRemoved += (sender, items) =>
        {
            foreach (var x in items.OfType<TestItem>()) result -= x.Value;
        };

        obc.ItemReplaced += (sender, e) =>
        {
            if (e.oldItem is TestItem oldItem) result -= oldItem.Value;
            if (e.newItem is TestItem newItem) result += newItem.Value;
        };

        Assert.Equal(0, result);

        var item1 = new TestItem(1);
        var item2 = new TestItem(2);
        var item3 = new TestItem(3);
        TestItem? a = null;

        obc.Add(item1);
        obc.Add(item2);
        obc.Add(item3); 

        Assert.Equal(6, result);

        obc.Remove(item1); // ItemsRemoved { 1 }

        Assert.Equal(5, result);

        obc[0] = new TestItem(4); // replace 2 with 4

        Assert.Equal(7, result);

        obc.Clear(); // ItemsRemoved { 2, 3 }

        Assert.Equal(0, result);
    }

    //! [example]
}