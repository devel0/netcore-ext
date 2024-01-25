namespace SearchAThing.Ext.Tests;

public partial class EnumerableTests
{

    public class SampleSplitItem(int myKey, int value)
    {
        public int MyKey => myKey;
        public int Value => value;

        public override string ToString() => $"MyKey:{MyKey} Value:{Value}";
    }

    [Fact]
    public void EnumerableTest_0009()
    {
        var data = new List<SampleSplitItem>()
        {
            new SampleSplitItem(myKey: 1, value: 10),
            new SampleSplitItem(myKey: 2, value: 11),
            new SampleSplitItem(myKey: 2, value: 12),
            new SampleSplitItem(myKey: 3, value: 13),
            new SampleSplitItem(myKey: 2, value: 14),
            new SampleSplitItem(myKey: 4, value: 15),
        };

        var q = data.SplitBy(w => w.MyKey);

        Assert.Equal(3, q.Count);
        Assert.Equal(4, q[0].Count);
        Assert.Single(q[1]);
        Assert.Single(q[2]);

        Assert.True(
            q[0][0].MyKey == 1 && q[0][0].Value == 10 &&
            q[0][1].MyKey == 2 && q[0][1].Value == 11 &&
            q[0][2].MyKey == 3 && q[0][2].Value == 13 &&
            q[0][3].MyKey == 4 && q[0][3].Value == 15);

        Assert.True(
            q[1][0].MyKey == 2 && q[1][0].Value == 12);

        Assert.True(
            q[2][0].MyKey == 2 && q[2][0].Value == 14);

    }

}