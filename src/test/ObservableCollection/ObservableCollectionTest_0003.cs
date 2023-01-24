using System.Runtime.CompilerServices;

namespace SearchAThing.Ext.Tests;

public partial class ObservableCollectionTests
{

    public class TestItem : INotifyPropertyChanged
    {
        #region property changed

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// invoke this method to forward propertchanged event notification.
        /// note: not needed to specify propertyName set by compiler service to called property.
        /// </summary>        
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Value

        private int _Value = 0;
        /// <summary>
        /// Value
        /// </summary>
        public int Value
        {
            get => _Value;
            set
            {
                var changed = value != _Value;
                if (changed)
                {
                    _Value = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public TestItem(int value)
        {
            Value = value;
        }
    }

    [Fact]
    public void ObservableCollectionTest_0003()
    {
        var obc = new ObservableCollection2<TestItem>();

        var observe = obc.Observe();

        int addCnt = 0;
        int removeCnt = 0;
        int chgCnt = 0;

        string? propNameExpected = null; // set to non null to selfcheck propname validity
        List<object> expectedRemove = new List<object>();
        List<object> expectedAdded = new List<object>();

        #region cnt helper
        observe.Change += (evt, pchg, items) =>
        {
            switch (evt)
            {
                case ObserverEventType.Add:
                    {
                        Assert.NotNull(items);
                        Assert.Null(pchg);
                        Assert.Equal(expectedAdded.Count, items.Count);
                        foreach (var item in expectedAdded) Assert.Contains(item, expectedAdded);

                        addCnt += items.Count;
                    }
                    break;

                case ObserverEventType.Remove:
                    {
                        Assert.NotNull(items);
                        Assert.Null(pchg);
                        Assert.Equal(expectedRemove.Count, items.Count);
                        foreach (var item in expectedRemove) Assert.Contains(item, expectedRemove);

                        removeCnt += items.Count;
                    }
                    break;

                case ObserverEventType.PropertyChanged:
                    {
                        Assert.NotNull(pchg);
                        Assert.Null(items);

                        if (propNameExpected is not null)
                            Assert.Equal(propNameExpected, pchg.PropertyName);

                        ++chgCnt;
                    }
                    break;
            }
        };
        #endregion

        var itemA = new TestItem(1);
        var itemB = new TestItem(2);
        var itemC = new TestItem(3);

        // test add    
        expectedAdded = new List<object> { itemA };
        obc.Add(itemA);
        expectedAdded = new List<object> { itemB };
        obc.Add(itemB);
        expectedAdded = new List<object> { itemC };
        obc.Add(itemC);
        Assert.Equal(3, addCnt); Assert.Equal(0, removeCnt); Assert.Equal(0, chgCnt);

        // test prop change
        itemA.Value = 3;
        propNameExpected = nameof(TestItem.Value);
        Assert.Equal(3, addCnt); Assert.Equal(0, removeCnt); Assert.Equal(1, chgCnt);
        propNameExpected = null;

        // test remove
        expectedRemove = new List<object> { itemB };
        obc.Remove(itemB);
        Assert.Equal(3, addCnt); Assert.Equal(1, removeCnt); Assert.Equal(1, chgCnt);

        // test clear
        expectedRemove = new List<object> { itemA, itemC };
        obc.Clear();
        Assert.Equal(3, addCnt); Assert.Equal(3, removeCnt); Assert.Equal(1, chgCnt);

        // test move           
        expectedAdded = new List<object> { itemA };
        obc.Add(itemA);
        expectedAdded = new List<object> { itemB };
        obc.Add(itemB);
        Assert.Equal(5, addCnt); Assert.Equal(3, removeCnt); Assert.Equal(1, chgCnt);

        expectedAdded = new List<object> { itemA };
        expectedRemove = new List<object> { itemA };
        obc.Move(0, 1);

        expectedAdded = new List<object> { itemB };
        expectedRemove = new List<object> { itemB };
        obc.Move(0, 1);

        // test replace
        Assert.Equal(itemA.Value, obc[0].Value);
        Assert.Equal(itemB.Value, obc[1].Value);
        expectedAdded = new List<object> { itemA };
        expectedRemove = new List<object> { itemB };
        obc[0] = obc[1];

        // test dispose        
        observe.Dispose(); // from here Change doesn't operate any more

        addCnt = removeCnt = chgCnt = 0;
        obc.Clear();

        Assert.Equal(0, addCnt);
        Assert.Equal(0, removeCnt);
        Assert.Equal(0, chgCnt);
    }
}