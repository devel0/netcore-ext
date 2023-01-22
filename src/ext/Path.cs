namespace SearchAThing.Ext;

public static partial class Toolkit
{

    /// <summary>
    /// {AppData}/{namespace}/{assembly_name}
    /// </summary>
    public static string AppDataFolder(string ns) => EnsureFolder(Path.Combine(
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ns),
        Assembly.GetCallingAssembly().GetName().Name!));

    /// <summary>
    /// Ensure given folder path exists.
    /// </summary>        
    public static string EnsureFolder(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    }

    /// <summary>
    /// Search given filename in the PATH
    /// </summary>
    /// <returns>null if not found</returns>
    public static string? SearchInPath(string filename)
    {
        if (File.Exists(filename)) return Path.GetFullPath(filename);

        var paths = Environment.GetEnvironmentVariable("PATH");
        if (paths != null)
        {
            foreach (var path in paths.Split(Path.PathSeparator))
            {
                var pathname = Path.Combine(path, filename);
                if (File.Exists(pathname)) return pathname;
            }
        }
        return null;
    }

}