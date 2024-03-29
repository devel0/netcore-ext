﻿using System.Runtime.CompilerServices;

namespace SearchAThing.Ext;

/// <summary>
/// useful if need to store quick tuple values into a list or dictionary and allowing further modification;
/// without this retrieved tuple will a copy-value and tuple in collection remains unmodified.
/// </summary>
public class ValueObj<T>
{
    public T Value { get; set; }
    public ValueObj(T x) { Value = x; }
}

public static partial class Ext
{

    /// <summary>
    /// Allow to tranform the object into other type.        
    /// eg. `intvar.Fn((x) => (x == 0) ? "zero" : "non-zero")`
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static U Fn<T, U>(this T obj, Func<T, U> fn) => fn(obj);

    /// <summary>
    /// Allow to apply some action on the object inline returning the same object.
    /// 
    /// eg `dxf.Entities.Add(new Line3D().DxfEntity.Act(ent => ent.Color = AciColor.Red))`        
    /// </summary>             
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Act<T>(this T obj, Action<T> setter)
    {
        setter(obj);
        return obj;
    }

    /// <summary>
    /// Allow to apply an action foreach enum objects
    /// </summary>  
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Foreach<T>(this IEnumerable<T> en, Action<T> action)
    {
        foreach (var x in en) action(x);

        return en;
    }

}

public static partial class Toolkit
{

    /// <summary>
    /// swap x, y values
    /// </summary>
    public static void Swap<T>(ref T x, ref T y)
    {
        var _x = x;
        x = y;
        y = _x;
    }

    /// <summary>
    /// returns true if only one of given objects is null;
    /// returns false if all objects null or all objects not null;
    /// </summary>
    public static bool XorNull(object a, object b) => a is null ^ b is null;

}