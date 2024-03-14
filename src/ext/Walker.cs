namespace SearchAThing.Ext;

public abstract class PropertyWalker
{

    protected HashSet<object> VisitedNodes = new();

    public void Walk(object obj)
    {
        var type = obj.GetType();

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var val = prop.GetValue(obj);

            WalkInner(obj, type, val, prop.PropertyType, prop);
        }
    }

    static Type tIEnumerable = typeof(IEnumerable);

    protected void WalkInner(object parent, Type parentType, object property, Type propertyType, PropertyInfo? propertyInfo)
    {
        if (propertyType.IsClass)
        {
            if (VisitedNodes.Contains(property)) return;
            VisitedNodes.Add(property);
        }

        if (EvalProperty(parent, parentType, property, propertyType, propertyInfo))
        {
            if (property is not null && propertyType.GetInterfaces().Any(w => w == tIEnumerable))
            {
                foreach (var x in (IEnumerable)property)
                {
                    WalkInner(property, propertyType, x, x.GetType(), null);
                }
            }

            else if (property is not null && propertyType.IsClass)
            {
                foreach (var prop2 in propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var val2 = prop2.GetValue(property);

                    WalkInner(property, propertyType, val2, prop2.PropertyType, prop2);
                }
            }

            else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                var property2 = propertyType.GetProperty("Value");
                var val2 = property2.GetValue(property, null);
                var prop2Type = val2.GetType();

                WalkInner(property, propertyType, val2, prop2Type, property2);

                ;
            }
        }
    }

    protected abstract bool EvalProperty(object parent, Type parentType, object property, Type propertyType, PropertyInfo? propertyInfo);

}