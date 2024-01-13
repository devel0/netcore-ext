namespace SearchAThing.Ext.Tests;

public partial class UnixTimeAutoTests
{

    [Fact]
    public void UnixTimeAutoTest_0002()
    {
        var dt = DateTimeOffset.Parse("2000-01-01T00:00:00Z");

        {
            var res = FromUnixTimeAuto(dt.ToUnixTimeSeconds(), new AllowedDateTimeOffsetRange(2000, 3000));

            Assert.Equal(dt, res);
        }

        {
            var res = FromUnixTimeAuto(dt.ToUnixTimeMilliseconds(), new AllowedDateTimeOffsetRange(2000, 3000));

            Assert.Equal(dt, res);
        }

        {
            var res = FromUnixTimeAuto(dt.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(1000, 1968),
                allowedRangeB: new AllowedDateTimeOffsetRange(2000, 3000));

            Assert.Equal(dt, res);
        }

        var dt2 = DateTimeOffset.Parse("1970-01-01T00:00:00Z");

        {
            var res = FromUnixTimeAuto(dt2.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(1000, 1968),
                allowedRangeB: new AllowedDateTimeOffsetRange(2000, 3000));

            Assert.Equal(dt2, res);
        }

        var dt3 = DateTimeOffset.Parse("1960-01-01T00:00:00Z");

        {
            var res = FromUnixTimeAuto(dt3.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(1000, 1968),
                allowedRangeB: new AllowedDateTimeOffsetRange(2000, 3000));

            Assert.Equal(dt3, res);
        }

        var dt4 = DateTimeOffset.Parse("1960-01-01T00:00:00Z");

        {
            var res = FromUnixTimeAuto(dt4.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(1, 1968),
                allowedRangeB: new AllowedDateTimeOffsetRange(1979, 9999));

            Assert.Equal(dt4, res);
        }

        // EXCEPTION because invalid allowed range ( overlaps ambiguity range )
        Assert.Throws<ArgumentException>(() =>
            FromUnixTimeAuto(dt4.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(1, 1000),
                allowedRangeB: new AllowedDateTimeOffsetRange(1969, 1977)));

        var dt5 = DateTimeOffset.Parse("1971-01-01T00:00:00Z");

        {
            var res = FromUnixTimeAuto(dt5.ToUnixTimeMilliseconds(),
                allowedRangeA: new AllowedDateTimeOffsetRange(1, 1968),
                allowedRangeB: new AllowedDateTimeOffsetRange(1979, 9999));

            Assert.NotEqual(dt5, res); // FAILURE because allowed range disregard effective input date            
        }

    }
}