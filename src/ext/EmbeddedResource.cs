using System.IO.Compression;

namespace SearchAThing.Ext;

public static partial class Toolkit
{

    /// <summary>
    /// retrieve embedded resource file content and read into a string
    /// </summary>
    /// <param name="resourceName">name of resource (eg. namespace.filename.ext)</param>
    /// <typeparam name="T">Type for which lookup assembly (eg. namespace.classname)</typeparam>
    public static string GetEmbeddedFileContentString<T>(string resourceName) where T : class =>
        GetEmbeddedFileContentString(typeof(T).GetTypeInfo().Assembly, resourceName);

    /// <summary>
    /// retrieve embedded resource file content and read into a string
    /// </summary>
    /// <param name="resourceName">name of resource (eg. namespace.filename.ext)</param>
    /// <param name="assembly">assembly that contains given resourceName</param>
    public static string GetEmbeddedFileContentString(Assembly assembly, string resourceName)
    {
        string res = "";
        using (var resource = assembly.GetManifestResourceStream(resourceName))
        {
            if (resource != null)
                using (var sr = new StreamReader(resource))
                {
                    res = sr.ReadToEnd();
                }
        }
        return res;
    }

    /// <summary>
    /// retrieve embedded resource file content and read into a byte array
    /// </summary>
    /// <param name="resourceName">name of resource (eg. namespace.filename.ext)</param>
    /// <param name="assembly">assembly that contains given resourceName</param>
    public static byte[]? GetEmbeddedFileContentBytes(Assembly assembly, string resourceName)
    {
        using (var resource = assembly.GetManifestResourceStream(resourceName))
        {
            if (resource != null)
            {
                var buf = new byte[resource.Length];
                if (resource.Read(buf, 0, (int)resource.Length) == resource.Length)
                    return buf;

                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// deflate embedded resource
    /// </summary>
    public static Stream? DeflateEmbeddedResource(Assembly assembly, string resource)
    {
        using (var s = assembly.GetManifestResourceStream(resource))
        {
            if (s != null)
            {
                var ms = new MemoryStream();
                using (var ds = new DeflateStream(s, CompressionMode.Decompress, true))
                {
                    var buf = new byte[4096];
                    int read;
                    while ((read = ds.Read(buf, 0, buf.Length)) != 0)
                        ms.Write(buf, 0, read);
                }
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
        }

        return null;
    }

    /// <summary>
    /// save given embedded resource to file
    /// </summary>
    public static bool SaveEmbeddedResourceToFile(string resource, string dstPathfilename, bool deflate = false)
    {
        var assembly = Assembly.GetCallingAssembly();
        if (deflate)
        {
            using (var fs = DeflateEmbeddedResource(assembly, resource))
            {
                if (fs is null) return false;

                using (var dstfs = new FileStream(dstPathfilename, FileMode.Create))
                {
                    fs.CopyTo(dstfs);
                }
            }
        }
        else
        {
            using (var s = assembly.GetManifestResourceStream(resource))
            {
                if (s is null) return false;

                using (var dstfs = new FileStream(dstPathfilename, FileMode.Create))
                {
                    s.CopyTo(dstfs);
                }
            }
        }

        return true;
    }

    /// <summary>
    /// retrieve calling assembly resource names
    /// </summary>
    public static IEnumerable<string> GetEmbeddedResourceNames(Assembly? assembly = null)
    {
        if (assembly is null)
            assembly = Assembly.GetCallingAssembly();

        return assembly.GetManifestResourceNames();
    }

    /// <summary>
    /// retrieve the list of embedded resource names from given Type
    /// </summary>
    /// <typeparam name="T">Type for which lookup assembly (eg. namespace.classname)</typeparam>
    public static string[] GetEmbeddedResourcesList<T>() where T : class
    {
        var assembly = typeof(T).GetTypeInfo().Assembly;
        return assembly.GetManifestResourceNames();
    }

}
