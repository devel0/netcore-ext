namespace SearchAThing.Ext;

/// <summary>
/// encapsulate a value type object into a reference type in an immutable way;
/// implicit conversion operator allow to access enclosed type
/// </summary>
public class RStruct<T> where T : struct
{

    public T Value { get; private set; }

    public RStruct(T value) => Value = value;

    public static implicit operator T(RStruct<T> x) => x.Value;

    public override string ToString() => Value.ToString();

}