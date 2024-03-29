﻿using System.Linq.Expressions;

namespace SearchAThing.Ext.Tests;

public class UnitTest1
{

    #region Date        

    [Fact]
    public void DateTest1()
    {
        Assert.True(new DateTime(2010, 1, 1).UnspecifiedAsUTCDateTime().Kind == DateTimeKind.Utc);
    }

    #endregion

    #region Number        

    [Fact]
    public void NumberTest1()
    {
        Assert.True(0d.EqualsAutoTol(0d));
        Assert.True((-1d).EqualsAutoTol(-1));

        Assert.False(1.4d.EqualsAutoTol(1.39999));
        Assert.True(1.4d.EqualsAutoTol(1.399999));

        Assert.False(1.4d.EqualsAutoTol(1.40001));
        Assert.True(1.4d.EqualsAutoTol(1.400001));
    }

    [Fact]
    public void NumberTest2()
    {
        Assert.True(1.41d.MRound(.2).EqualsAutoTol(1.4));
        Assert.True(1.59d.MRound(.2).EqualsAutoTol(1.6));
    }

    [Fact]
    public void NumberTest3()
    {
        Assert.True(180d.ToRad().EqualsAutoTol(PI));
        Assert.True(PI.ToDeg().EqualsAutoTol(180));
    }

    [Fact]
    public void NumberTest4()
    {
        Assert.False(1.345.Stringify(2) == "1.35");
        Assert.True(1.346.Stringify(2) == "1.35");
    }

    [Fact]
    public void NumberTest5()
    {
        Assert.True(1.34e-210.Magnitude() == -210);
        Assert.True(1.34e+210.Magnitude() == 210);
    }

    [Fact]
    public void NumberTest6()
    {
        var cultureBackup = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("it");
        Assert.True(double.Parse("1,2").EqualsAutoTol(1.2));
        Assert.True(double.Parse("1.2").EqualsAutoTol(12));

        Assert.True("1,2".InvDoubleParse().EqualsAutoTol(12));
        Assert.True("1.2".InvDoubleParse().EqualsAutoTol(1.2));
        Thread.CurrentThread.CurrentCulture = cultureBackup;
    }

    [Fact]
    public void NumberTest7()
    {
        Assert.True(new[] { 1d, 3.4, 5 }.Mean().EqualsTol(1e-2, 3.13));
    }

    [Fact]
    public void NumberTest8()
    {
        Assert.True(2.031d.ToString(2) == "2.03");
    }

    [Fact]
    public void NumberTest9()
    {
        Assert.True(2.4d.IsInRange(1e-1, "[2,2.4]"));
        Assert.False(2.4d.IsInRange(1e-1, "[2,2.4)"));
        Assert.True(2.4d.IsInRange(1e-1, "[2,)"));
    }

    [Fact]
    public void NumberTest10()
    {
        Assert.True(12.Sign().EqualsAutoTol(1.0));
        Assert.False((-12).Sign().EqualsAutoTol(1.0));
        Assert.True((-12).Sign().EqualsAutoTol(-1.0));

        Assert.True(12.3d.Sign().EqualsAutoTol(1.0));
        Assert.False((-0.45).Sign().EqualsAutoTol(1.0));
        Assert.True((-0.45).Sign().EqualsAutoTol(-1.0));
    }

    [Fact]
    public void NumberTest11()
    {
        Assert.True("1.2".SmartDoubleParse().EqualsAutoTol(1.2));
        Assert.True("1,2".SmartDoubleParse().EqualsAutoTol(1.2));
        var exceptionCnt = 0;
        try
        {
            "1,200.40".SmartDoubleParse();
        }
        catch
        {
            ++exceptionCnt;
        }
        Assert.True(exceptionCnt == 1);
    }



    #endregion

    #region ObserableCollection

    [Fact]
    public void ObserableCollectionTest1()
    {
        var obc = new ObservableCollection<int>(new[] { 3, 1, 4 });

        var obc1 = obc;
        obc.Sort((x) => x);
        Assert.True(obc == obc1);
        Assert.True(obc.ToList().SequenceEqual(new[] { 1, 3, 4 }));

        var obc2 = obc;
        obc.Sort((x) => x, descending: true);
        Assert.True(obc == obc2);
        Assert.True(obc.ToList().SequenceEqual(new[] { 4, 3, 1 }));
    }
    #endregion

    #region Expression
    [Fact]
    public void GetMembersTest()
    {
        var obj = new MemberTest1();

        {
            var lst = GetMemberNames(obj, x => x.a);
            Assert.True(lst.Count == 1);
            Assert.Contains("a", lst);
        }

        {
            var lst = GetMemberNames(obj, x => new { x.a, x.b });
            Assert.True(lst.Count == 2);
            Assert.True(lst.Contains("a") && lst.Contains("b"));
            lst = GetMemberNames<MemberTest1>(x => new { x.a, x.b }).ToHashSet();
            Assert.True(lst.Count == 2);
            Assert.True(lst.Contains("a") && lst.Contains("b"));
        }

        {
            var res = GetMemberName(obj, x => x.b);
            Assert.True(res == "b");
            res = GetMemberName<MemberTest1>(x => x.b);
            Assert.True(res == "b");
        }

        {
            Assert.Throws<ArgumentException>(() =>
            {
                var res = GetMemberName(obj, x => new { x.a, x.b });
            });
        }
    }

    [Fact]
    public void GetVarNameTest()
    {
        var obj = new MemberTest1();

        {
            var myvar = 1d;
            var q = GetVarName(() => myvar);
            Assert.True(q == "myvar");
        }

    }

    void CreateGetterSetterTestFn<T>(T dst, T src, Expression<Func<T, object>> memberExpr)
    {
        var (getter, setter) = memberExpr.CreateGetterSetter();
        setter(dst, getter(src));
    }

    [Fact]
    public void CreateGetterSetterTest()
    {
        var src = new MemberTest1 { a = 1, b = 2 };
        var dst = new MemberTest1();

        Assert.False(dst.a == src.a);
        CreateGetterSetterTestFn<MemberTest1>(dst, src, (x) => x.a);
        Assert.True(dst.a == src.a);
    }
    #endregion

}

public class MemberTest1
{
    public int a { get; set; }
    public int b { get; set; }
}
