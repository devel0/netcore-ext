namespace SearchAThing.Ext.Tests;

public partial class ObservableCollectionTests
{

    [Fact]
    public void ObservableCollectionTest_0002()
    {
        var obc = new HSObservableCollection<TestItem>();

        var item1 = new TestItem(1);
        var item2 = new TestItem(2);
        var item3 = new TestItem(3);

        obc.Add(item1);

        Assert.True(obc.Contains(item1));
        Assert.False(obc.Contains(item2));
        Assert.False(obc.Contains(item3));

        obc.Add(item2);

        Assert.True(obc.Contains(item1));
        Assert.True(obc.Contains(item2));
        Assert.False(obc.Contains(item3));

        obc[1] = item3;

        Assert.True(obc.Contains(item1));
        Assert.False(obc.Contains(item2));
        Assert.True(obc.Contains(item3));

        obc.Clear();

        Assert.False(obc.Contains(item1));
        Assert.False(obc.Contains(item2));
        Assert.False(obc.Contains(item3));

        obc.Add(item1);
        obc.Add(item1);

        Assert.True(obc.Contains(item1));
        obc.Remove(item1);
        Assert.Equal(1, obc.Count);
        Assert.True(obc.Contains(item1));

        obc[0] = item2;
        Assert.False(obc.Contains(item1));
        Assert.True(obc.Contains(item2));
        Assert.Equal(1, obc.Count);

        obc.Add(item2);
        obc.Add(item2);
        obc.Remove(item2);
        Assert.True(obc.Contains(item2));
    }

}