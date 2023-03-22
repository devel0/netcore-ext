namespace SearchAThing.Ext;

public static partial class Ext
{

    /// <summary>
    /// mean of given vectors
    /// </summary>    
    public static Vector3 Mean(this IEnumerable<Vector3> vectors)
    {
        var cnt = 0;
        var sum = Vector3.Zero;
        foreach (var x in vectors)
        {
            sum += x;
            ++cnt;
        }
        return sum / cnt;
    }

    /// <summary>
    /// swizzle vector3 xyz from vector4
    /// </summary>    
    public static Vector3 XYZ(this Vector4 v) => new Vector3(v.X, v.Y, v.Z);

    /// <summary>
    /// swizzle vector2 xy from vector3
    /// </summary>    
    public static Vector2 XY(this Vector3 v) => new Vector2(v.X, v.Y);

    /// <summary>
    /// convert x,y to x,y,z
    /// </summary>    
    public static Vector3 ToVector3(this Vector2 v, float z = 0) => new Vector3(v.X, v.Y, z);

    /// <summary>
    /// round vector components to given digits
    /// </summary>    
    public static Vector2 Round(this Vector2 v, int digits) =>
        new Vector2((float)Math.Round(v.X, digits), (float)Math.Round(v.Y, digits));

    /// <summary>
    /// round vector components to given digits
    /// </summary>
    public static Vector3 Round(this Vector3 v, int digits) =>
        new Vector3((float)Math.Round(v.X, digits), (float)Math.Round(v.Y, digits), (float)Math.Round(v.Z, digits));

    /// <summary>
    /// round vector components to given digits
    /// </summary>
    public static Vector4 Round(this Vector4 v, int digits) =>
        new Vector4((float)Math.Round(v.X, digits), (float)Math.Round(v.Y, digits), (float)Math.Round(v.Z, digits), (float)Math.Round(v.W, digits));

}