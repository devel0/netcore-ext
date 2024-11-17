using System.Text.Json.Serialization;

namespace SearchAThing.Ext.Tests;

public partial class ReflectionTests
{

    public class SampleDataInner
    {

        public int NumVal { get; set; }

    }

    public class SampleData
    {
        public int NumVal { get; set; }

        public SampleDataInner Data { get; set; }

        [JsonIgnore]
        public List<SampleDataInner> DataList { get; set; } = new();

        public List<object?> Some { get; set; } = new List<object?>();

        public Dictionary<string, SampleDataInner> DataDict { get; set; } = new();

        public SampleDataInner[] ArrayList { get; set; } = [];

        public HashSet<string> HsStr { get; set; } = new();
    }

    class MyWalker : PropertyWalker
    {
        protected override bool EvalProperty(object parent, Type parentType, object property, Type propertyType, PropertyInfo? propertyInfo)
        {
            //System.Console.WriteLine($"eval prop [{property}]");

            if (propertyInfo is not null)
            {
                if (propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>() is not null)
                {
                    // side effectly null this prop
                    propertyInfo.SetValue(parent, null);

                    return false; // don't walk their children
                }

                else if (property is SampleDataInner[] array)
                {
                    array[0] = null;

                    return false; // don't walk their children
                }

                else if (property is HashSet<string> hs)
                {
                    hs.Remove("b");

                    return false; // don't walk their children
                }
            }

            // detect on enumerable child

            else if (
                property is KeyValuePair<string, SampleDataInner> kvp &&
                kvp.Value is SampleDataInner data &&
                data.NumVal == 12)
            {
                ((Dictionary<string, SampleDataInner>)parent)[kvp.Key] = null;
            }

            return true;
        }
    }

    [Fact]
    public void ReflectionTest_0001()
    {
        var d0 = new SampleDataInner
        {
            NumVal = 100
        };

        var d1 = new SampleDataInner
        {
            NumVal = 11
        };

        var d2 = new SampleDataInner
        {
            NumVal = 12
        };

        var d3 = new SampleDataInner
        {
            NumVal = 13
        };

        var data = new SampleData
        {
            NumVal = 10,
            Data = d0,
            Some = [null, "some"],
            DataList = new List<SampleDataInner> { d1, d2 },
            DataDict = {
                ["a"] = d1,
                ["b"] = d2,
                ["c"] = d3,
            },
            ArrayList = [d1, d2, d3]
        };

        data.HsStr.Add("a");
        data.HsStr.Add("b");
        data.HsStr.Add("c");

        var walker = new MyWalker();
        walker.Walk(data);

        Assert.True(data.NumVal == 10);
        Assert.True(data.Data == d0);
        Assert.True(data.DataList == null);

        Assert.True(data.DataDict["a"] == d1);
        Assert.True(data.DataDict["b"] == null);
        Assert.True(data.DataDict["c"] == d3);

        Assert.True(data.ArrayList[0] == null);
        Assert.True(data.ArrayList[1] == d2);
        Assert.True(data.ArrayList[2] == d3);

        Assert.True(data.HsStr.Contains("a"));
        Assert.True(!data.HsStr.Contains("b"));
        Assert.True(data.HsStr.Contains("c"));

    }

}