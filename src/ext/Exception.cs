namespace SearchAThing.Ext;

/// <summary>
/// InternalError exception
/// </summary>
public class InternalError : Exception
{

    /// <summary>
    /// Internal exception constructor, some assert failed
    /// </summary>
    /// <param name="msg">assert fail msg</param>
    public InternalError(string msg) : base(msg)
    {

    }

}