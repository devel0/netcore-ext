using System.Runtime.InteropServices;

namespace SearchAThing.Ext;

public static partial class Ext
{

    /// <summary>
    /// retrieve a binary representation of given struct
    /// </summary>
    public static byte[] ToBytes<T>(this T obj) where T : struct
    {
        var size = Marshal.SizeOf(obj);
        var arr = new byte[size];

        var ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);

        return arr;
    }

}