namespace SearchAThing.Ext.Tests;

public partial class UnixTimeAutoTests
{

    [Fact]
    public void UnixTimeAutoTest_0001()
    {
        var dt = DateTimeOffset.Parse("2000-01-01T00:00:00Z");

        {
            var res = FromUnixTimeAuto(dt.ToUnixTimeSeconds(), new AllowedDateTimeOffsetRange(
                DateTimeOffset.Parse("2000-01-01T00:00:00Z"),
                DateTimeOffset.Parse("3000-01-01T00:00:00Z")));

            Assert.Equal(dt, res);
        }

        {
            var res = FromUnixTimeAuto(dt.ToUnixTimeMilliseconds(), new AllowedDateTimeOffsetRange(
                DateTimeOffset.Parse("2000-01-01T00:00:00Z"),
                DateTimeOffset.Parse("3000-01-01T00:00:00Z")));

            Assert.Equal(dt, res);
        }

        {
            var res = FromUnixTimeAuto(dt.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(
                    DateTimeOffset.Parse("1000-01-01T00:00:00Z"),
                    DateTimeOffset.Parse("1968-01-01T00:00:00Z")),
                allowedRangeB: new AllowedDateTimeOffsetRange(
                    DateTimeOffset.Parse("2000-01-01T00:00:00Z"),
                    DateTimeOffset.Parse("3000-01-01T00:00:00Z")));

            Assert.Equal(dt, res);
        }

        var dt2 = DateTimeOffset.Parse("1970-01-01T00:00:00Z");

        {
            var res = FromUnixTimeAuto(dt2.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(
                    DateTimeOffset.Parse("1000-01-01T00:00:00Z"),
                    DateTimeOffset.Parse("1968-01-01T00:00:00Z")),
                allowedRangeB: new AllowedDateTimeOffsetRange(
                    DateTimeOffset.Parse("2000-01-01T00:00:00Z"),
                    DateTimeOffset.Parse("3000-01-01T00:00:00Z")));

            Assert.Equal(dt2, res);
        }

        var dt3 = DateTimeOffset.Parse("1960-01-01T00:00:00Z");

        {
            var res = FromUnixTimeAuto(dt3.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(
                    DateTimeOffset.Parse("1000-01-01T00:00:00Z"),
                    DateTimeOffset.Parse("1968-01-01T00:00:00Z")),
                allowedRangeB: new AllowedDateTimeOffsetRange(
                    DateTimeOffset.Parse("2000-01-01T00:00:00Z"),
                    DateTimeOffset.Parse("3000-01-01T00:00:00Z")));

            Assert.Equal(dt3, res);
        }

    }
 
}