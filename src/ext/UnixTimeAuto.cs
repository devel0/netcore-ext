namespace SearchAThing.Ext;

public static partial class Toolkit
{

    static readonly long utSecMin = DateTimeOffset.MinValue.ToUnixTimeSeconds(); // -62135596800
    static readonly long utSecMax = DateTimeOffset.MaxValue.ToUnixTimeSeconds(); // 253402300799

    // static readonly long utMsecMin = DateTimeOffset.MinValue.ToUnixTimeMilliseconds(); // -62135596800000
    // static readonly long utMsecMax = DateTimeOffset.MaxValue.ToUnixTimeMilliseconds(); // 253402300799999

    public static readonly DateTimeOffset UnixTimeAutoAmbiguityFrom = DateTimeOffset.FromUnixTimeMilliseconds(utSecMin);
    public static readonly DateTimeOffset UnixTimeAutoAmbiguityTo = DateTimeOffset.FromUnixTimeMilliseconds(utSecMax);

    public class AllowedDateTimeOffsetRange(DateTimeOffset from, DateTimeOffset to)
    {
        public DateTimeOffset From => from;
        public DateTimeOffset To => to;

        public AllowedDateTimeOffsetRange(int yearFrom, int yearTo) : this(
            DateTimeOffset.Parse($"{yearFrom:0000}-01-01T00:00:00Z"),
            DateTimeOffset.Parse($"{yearTo:0000}-01-01T00:00:00Z")
        )
        {
        }

        internal void SanityCheck()
        {
            if (From > to) throw new ArgumentException($"expects from less than to");
            if (
                (From >= UnixTimeAutoAmbiguityFrom && From <= UnixTimeAutoAmbiguityTo)
                ||
                (To <= UnixTimeAutoAmbiguityTo && To >= UnixTimeAutoAmbiguityFrom)
            )
                throw new ArgumentException($"given from, to must not fall into unix time msec/sec ambiguity range [{UnixTimeAutoAmbiguityFrom:o}. {UnixTimeAutoAmbiguityTo:o}]");
        }

        internal bool GuessUnixTimeIsSec(long value) =>
            value >= From.ToUnixTimeSeconds() && value <= To.ToUnixTimeSeconds();

        internal bool GuessUnixTimeIsMillis(long value) =>
            value >= From.ToUnixTimeMilliseconds() && value <= To.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// Guess if given unix time is seconds or milliseconds given a validity range of dates that must fall
    /// out of ambiguity range [1968-01-12T20:06:43.2Z, 1978-01-11T21:31:40.799Z] where tests could overlap
    /// and no decision can be assumed.
    /// </summary>
    /// <remarks>
    /// If unix time is in the ambiguity range [<see cref="UnixTimeAutoAmbiguityFrom"/>,<see cref="UnixTimeAutoAmbiguityTo"/>] it can't be guessed and an exception is thrown.
    /// Out of the ambiguity it can be guessed given an allowable date ranges because these would not overlaps.    
    /// Discussion: https://stackoverflow.com/a/77809656/5521766
    /// </remarks>
    public static DateTimeOffset FromUnixTimeAuto(long value,
        AllowedDateTimeOffsetRange allowedRangeA, AllowedDateTimeOffsetRange? allowedRangeB = null)
    {
        if (value == 0) return DateTimeOffset.FromUnixTimeSeconds(0);

        allowedRangeA.SanityCheck();
        if (allowedRangeB is not null) allowedRangeB.SanityCheck();

        if (allowedRangeA.GuessUnixTimeIsSec(value))
            return DateTimeOffset.FromUnixTimeSeconds(value);

        if (allowedRangeA.GuessUnixTimeIsMillis(value))
            return DateTimeOffset.FromUnixTimeMilliseconds(value);

        if (allowedRangeB is not null)
        {
            if (allowedRangeB.GuessUnixTimeIsSec(value))
                return DateTimeOffset.FromUnixTimeSeconds(value);

            if (allowedRangeB.GuessUnixTimeIsMillis(value))
                return DateTimeOffset.FromUnixTimeMilliseconds(value);
        }

        throw new InternalError($"can't guess {value} unix time");
    }


}