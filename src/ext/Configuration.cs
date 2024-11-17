namespace SearchAThing.Ext;

public static class ConfigurationPropertyPathHelper<T>
{

    /// <summary>
    /// Retrieve the path of a variable suitable for the <see cref="IConfiguration.GetSection(string)"/>  to
    /// access the modify var value
    /// </summary>
    public static string PropertyPath<TProperty>(Expression<Func<T, TProperty>> expr)
    {
        var name = expr.Parameters[0].Name;

        var path = $"{typeof(T).Name}" + ":" +
            expr.Body.ToString().StripBegin($"{name}.")
            .Replace(".", ":");

        var rgx = new Regex(@"get_Item\((\d+)\)");

        return rgx.Replace(path, "$1");
    }

}
