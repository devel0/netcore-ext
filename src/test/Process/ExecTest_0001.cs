namespace SearchAThing.Ext.Tests;

public partial class ExecTests
{

    [Fact]
    public async void ExecTest_0001()
    {
        var res = await Exec("bash", new[] { "-c", $"echo \"1+2\" | bc" }, CancellationToken.None);

        var lines = res.Output.Lines().ToList();
        Assert.True(lines.Count > 0);
        Assert.Equal(lines[0], "3");
    }

}