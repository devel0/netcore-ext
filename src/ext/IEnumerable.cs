using System.Runtime.CompilerServices;

namespace SearchAThing.Ext;

public static partial class Ext
{

    /// <summary>
    /// performs given action on enumerable items
    /// </summary>    
    public static void ForEach<T>(this IEnumerable<T> en, Action<T> act)
    {
        foreach (var x in en) act(x);
    }

    /// <summary>
    /// distinct with lambda
    /// </summary>
    public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> lst, Func<T, TKey> keySelector) =>
        lst.GroupBy(keySelector).Select(w => w.First());

    /// <summary>
    /// enumerable extension to enumerate itself into an (item, idx) set
    /// </summary>
    public static IEnumerable<(T item, int idx)> WithIndex<T>(this IEnumerable<T> en) =>
        en.Select((item, idx) => (item, idx));

    /// <summary>
    /// enumerable extension to enumerate itself into an (item, idx, isLast) set
    /// </summary>
    /// <example>
    /// \snippet with-index-is-last/Program.cs example
    /// </example>
    public static IEnumerable<(T item, int idx, bool isLast)> WithIndexIsLast<T>(this IEnumerable<T> en)
    {
        var enm = en.GetEnumerator();

        var idx = 0;
        var isLast = !enm.MoveNext();
        var item = default(T);
        while (!isLast)
        {
            item = enm.Current;
            isLast = !enm.MoveNext();
            yield return (item, idx++, isLast);
            if (isLast) yield break;
        }
    }

    /// <summary>
    /// enumerate given items returning a tuple with nullable ( for first hit ) prev element
    /// </summary>
    public static IEnumerable<(T? prev, T item, int itemIdx)> WithPrevPrimitive<T>(this IEnumerable<T> en) where T : struct
    {
        var enm = en.GetEnumerator();

        T? prev = null;
        var item = default(T);
        var idx = 0;
        while (enm.MoveNext())
        {
            item = enm.Current;
            yield return (prev, item, idx++);
            prev = item;
        }
    }

    /// <summary>
    /// enumerate given items returning a tuple with null ( for first hit ) prev element
    /// </summary>
    public static IEnumerable<(T? prev, T item, int itemIdx)> WithPrev<T>(this IEnumerable<T> en) where T : class
    {
        var enm = en.GetEnumerator();

        T? prev = null;
        var item = default(T);
        var idx = 0;
        while (enm.MoveNext())
        {
            item = enm.Current;
            yield return (prev, item, idx++);
            prev = item;
        }
    }

    /// <summary>
    /// enumerate given items returning a tuple with null ( for last hit ) next element
    /// </summary>
    /// <remarks>        
    /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0002.cs)
    /// </remarks>
    public static IEnumerable<(T item, T? next, int itemIdx, bool isLast)>
        WithNextPrimitive<T>(this IEnumerable<T> en, bool repeatFirstAtEnd = false) where T : struct
    {
        var enm = en.GetEnumerator();

        T? first = null;
        T? prev = null;
        var item = default(T?);
        var isLast = !enm.MoveNext();
        var idx = 0;
        while (!isLast)
        {
            item = enm.Current;
            isLast = !enm.MoveNext();

            if (first is null)
            {
                first = prev = item;
                if (isLast) yield return (item.Value, repeatFirstAtEnd ? first : null, idx++, true);
            }
            else
            {
                yield return (prev!.Value, item, idx++, false);
                if (isLast)
                {
                    yield return (item.Value, repeatFirstAtEnd ? first : null, idx++, true);
                    yield break;
                }

                prev = item;
            }
        }
    }

    /// <summary>
    /// enumerate given items returning a tuple (prev,item,next,isLast) 
    /// with prev=null for first element
    /// with next=null for last element or next=first if repeatFirstAtEnd=true on latest el
    /// </summary>                        
    /// <param name="en">input set</param>        
    /// <param name="repeatFirstAtEnd">last enumerated result will (last,first,true)</param>        
    /// <remarks>
    /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0004.cs)
    /// </remarks>
    public static IEnumerable<(T? prev, T item, T? next, int itemIdx, bool isLast)> WithPrevNextPrimitive<T>(
        this IEnumerable<T> en, bool repeatFirstAtEnd = false) where T : struct
    {
        foreach (var x in en.WithNextPrimitive(repeatFirstAtEnd).WithPrevPrimitive())
        {
            var item = x.item.item;
            T? prev = x.prev.HasValue ? x.prev.Value.item : default(T?);
            T? next = x.item.next;
            var isLast = x.item.isLast;

            yield return (prev, item, next, x.itemIdx, isLast);
        }
    }

    /// <summary>
    /// enumerate given items returning a tuple (item,next,isLast) with next=null for last element or next=first if repeatFirstAtEnd=true on latest el
    /// </summary>                
    /// <param name="en">input set</param>        
    /// <param name="repeatFirstAtEnd">last enumerated result will (last,first,true)</param>        
    /// <remarks>
    /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0001.cs)
    /// </remarks>
    public static IEnumerable<(T item, T? next, int itemIdx, bool isLast)>
        WithNext<T>(this IEnumerable<T> en, bool repeatFirstAtEnd = false) where T : class
    {
        var enm = en.GetEnumerator();

        T? first = null;
        T? prev = null;
        var item = default(T);
        var isLast = !enm.MoveNext();
        int idx = 0;
        while (!isLast)
        {
            item = enm.Current;
            isLast = !enm.MoveNext();

            if (first is null)
            {
                first = prev = item;
                if (isLast) yield return (item, repeatFirstAtEnd ? first : null, idx++, true);
            }
            else
            {
                yield return (prev!, item, idx++, false);

                if (isLast)
                {
                    yield return (item, repeatFirstAtEnd ? first : null, idx++, true);
                    yield break;
                }
                prev = item;
            }
        }
    }

    /// <summary>
    /// enumerate given items returning a tuple (prev,item,next,isLast) 
    /// with prev=null for first element
    /// with next=null for last element or next=first if repeatFirstAtEnd=true on latest el
    /// </summary>           
    /// <param name="en">input set</param>             
    /// <param name="repeatFirstAtEnd">last enumerated result will (last,first,true)</param>        
    /// <remarks>
    /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0003.cs)
    /// </remarks>
    public static IEnumerable<(T? prev, T item, T? next, int itemIdx, bool isLast)> WithPrevNext<T>(
        this IEnumerable<T> en, bool repeatFirstAtEnd = false) where T : class
    {
        foreach (var x in en.WithNext(repeatFirstAtEnd).WithPrevPrimitive())
        {
            var item = x.item.item;
            T? prev = x.prev.HasValue ? x.prev.Value.item : null;
            T? next = x.item.next;
            var isLast = x.item.isLast;

            yield return (prev, item, next, x.itemIdx, isLast);
        }
    }

    /// <summary>
    /// from given elements return the sequence starting from wantedFirstElement
    /// and continue until end then restart from begin until wantedFirstElement excluded
    /// pre: wantedFirstElement must in the list
    /// [unit test](https://github.com/devel0/netcore-util/tree/master/test/Enumerable/EnumerableTest_0006.cs)
    /// </summary>        
    public static IEnumerable<T> RouteFirst<T>(this IEnumerable<T> lst, T wantedFirstElement)
    {
        var backingStore = new List<T>();

        bool firstFound = false;
        foreach (var x in lst)
        {
            if (firstFound)
            {
                yield return x;
                continue;
            }
            else if (x != null && x.Equals(wantedFirstElement))
            {
                firstFound = true;
                yield return x;
            }
            else backingStore.Add(x);
        }

        foreach (var x in backingStore)
        {
            yield return x;
        }
    }

    /// <summary>
    /// Convert given IEnumerable into IReadOnlyList
    /// with type convert if given argument was already a IReadOnlyList
    /// or creating a new object List and returning as IReadOnlyList
    /// </summary>
    /// <param name="en">enumerable to convert to IReadOnlyList</param>
    /// <typeparam name="T">typename</typeparam>
    /// <returns>IReadOnlyList ( may the same object reference or a new depending if the argument was already a IReadOnlyList or not )</returns>
    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> en)
    {
        if (en is IReadOnlyList<T> roLst)
            return roLst;

        return new List<T>(en);
    }

    /// <summary>
    /// Convert given enumerable to observable collection
    /// </summary>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> en) =>
        new ObservableCollection<T>(en);

    /// <summary>
    /// Retrieve min, max at once.
    /// </summary>    
    /// <returns>(min,max) of given set of values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (double min, double max)? MinMax(this IEnumerable<double> values)
    {
        double min = 0;
        double max = 0;
        int cnt = 0;

        foreach (var value in values)
        {
            if (cnt == 0)
            {
                min = max = value;
            }
            else
            {
                min = Min(min, value);
                max = Max(max, value);
            }
            ++cnt;
        }

        if (cnt == 0) return null;

        return (min, max);
    }

    /// <summary>
    /// Retrieve min, max at once.
    /// </summary>    
    /// <returns>(min,max) of given set of values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (float min, float max)? MinMax(this IEnumerable<float> values)
    {
        float min = 0;
        float max = 0;
        int cnt = 0;

        foreach (var value in values)
        {
            if (cnt == 0)
            {
                min = max = value;
            }
            else
            {
                min = Min(min, value);
                max = Max(max, value);
            }
            ++cnt;
        }

        if (cnt == 0) return null;

        return (min, max);
    }

    /// <summary>
    /// Create an HashSet from given enumerable. (net standard 2.0 ext)
    /// </summary>    
    public static HashSet<T> ToHashSetExt<T>(this IEnumerable<T> set) => new HashSet<T>(set);

    /// <summary>
    /// Separate into a set of lists given items ensuring no same key object exists in the same list.
    /// </summary>
    /// <typeparam name="T">Type of items to split.</typeparam>
    /// <typeparam name="V">Type of item key.</typeparam>
    /// <param name="set">Input element set</param>
    /// <param name="splitBySelector">Function that retrieve key from item.</param>
    /// <returns>List of list of items where no dup key in the same list.</returns>
    public static List<List<T>> SplitBy<T, V>(this IEnumerable<T> set, Func<T, V> splitBySelector)
    {
        var res = new List<List<T>>();

        var dict = new Dictionary<V, int>();

        foreach (var x in set)
        {
            var key = splitBySelector(x);

            if (!dict.TryGetValue(key, out var off))
            {
                off = 0;
                dict.Add(key, off);
            }

            else
            {
                ++off;
                dict[key] = off;
            }

            List<T> lst;

            if (res.Count <= off)
            {
                if (off > res.Count) throw new InternalError($"list elements should increase by 1");

                res.Add(lst = new List<T>());
            }

            else
                lst = res[off];

            lst.Add(x);
        }

        return res;
    }

}