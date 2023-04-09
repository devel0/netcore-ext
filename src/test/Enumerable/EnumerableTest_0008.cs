namespace SearchAThing.Ext.Tests;

public partial class EnumerableTests
{

    [Fact]
    public void EnumerableTest_0008()
    {
        var qd = new double[] { -3, 5, 2.2, 100, -7, 4 }.MinMax();
        var qf = new float[] { -3, 5, 2.2f, 100, -7, 4 }.MinMax();

        Assert.NotNull(qd);
        Assert.NotNull(qf);

        Assert.Equal(-7, qd.Value.min);
        Assert.Equal(100, qd.Value.max);

        Assert.Equal(-7f, qf.Value.min);
        Assert.Equal(100f, qf.Value.max);

        Assert.Null(new double[] { }.MinMax());
        Assert.Null(new float[] { }.MinMax());
    }

}