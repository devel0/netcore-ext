namespace SearchAThing.Ext;

public static partial class Ext
{

    /// <summary>
    /// if given dt has unspecified kind rectifies to UTC without any conversion
    /// </summary>        
    public static DateTime UnspecifiedAsUTCDateTime(this DateTime dt)
    {
        if (dt.Kind == DateTimeKind.Unspecified)
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        return dt;
    }

    /// <summary>
    /// if given dt has unspecified kind rectified to Local without any conversion
    /// </summary>    
    public static DateTime UnspecifiedAsLocalDateTime(this DateTime dt)
    {
        if (dt.Kind == DateTimeKind.Unspecified)
            return DateTime.SpecifyKind(dt, DateTimeKind.Local);

        return dt;
    }

}