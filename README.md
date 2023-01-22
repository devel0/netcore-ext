# netcore-ext

[![NuGet Badge](https://buildstats.info/nuget/netcore-ext)](https://www.nuget.org/packages/netcore-ext/)

.NET core extensions

- [API Documentation](https://devel0.github.io/netcore-ext/html/annotated.html)
- [Changelog](https://github.com/devel0/netcore-ext/commits/master)

<hr/>

<!-- TOC -->
* [Quickstart](#quickstart)
* [Unit tests](#unit-tests)
* [Examples](#examples)
  + [tail like](#tail-like)
  + [with-index-is-last](#with-index-is-last)
  + [exec](#exec)
* [How this project was built](#how-this-project-was-built)
<!-- TOCEND -->

<hr/>

## Quickstart

- [nuget package](https://www.nuget.org/packages/netcore-ext/)

- [extension methods](https://devel0.github.io/netcore-ext/html/class_search_a_thing_1_1_ext.html)

```csharp
using SearchAThing.Ext;
```

- [toolkit methods](https://devel0.github.io/netcore-util/html/class_search_a_thing_1_1_toolkit.html)

```csharp
using static SearchAThing.Ext.Toolkit;
```

## Unit tests

```sh
dotnet test
```

- to debug from vscode just run debug test from code lens balloon

## Examples

### tail like

```cs
namespace SearchAThing.Ext.Examples;

class Program
{
    static void Main(string[] args)
    {
        foreach (var x in TailLike("/var/log/syslog"))
        {
            System.Console.WriteLine(x);
        }
    }
}
```

### with-index-is-last

- [WithIndexIsLast](https://devel0.github.io/netcore-util/html/class_search_a_thing_1_1_util_ext.html#a17c5dc6f76a3fcce1f5d2a567f9db3ef)

```csharp
namespace SearchAThing.Ext.Examples;

class Program
{
    static void Main(string[] args)
    {
        var q = new[] { 1, 2, 4 };

        var last = 0d;

        // q2 : sum of all elements except last ( save last into `last` var )
        var q2 = q.WithIndexIsLast().Select(w =>
        {
            if (w.isLast)
            {
                last = w.item;
                return 0;
            }
            return w.item;
        }).Sum();

        if (q2 == 3 && last == 4)
            System.Console.WriteLine($"tests succeeded");
        else
            System.Console.WriteLine($"tests failed");
    }
}
```

### exec

- [Exec](https://devel0.github.io/netcore-util/html/class_search_a_thing_1_1_util_toolkit.html#a2aa4cf2d7debacfc62deacf9d1f93313)

```csharp
using System.Threading;

namespace SearchAThing.Ext.Examples;

class Program
{
    static void Main(string[] args)
    {
        Task.Run(async () =>
        {
            var q = await Exec("ls", new[] { "-la", "/etc/hosts" }, CancellationToken.None,
                sudo: false,
                redirectStdout: true,
                redirectStderr: false,
                verbose: false);

            if (q.ExitCode == 0)
            {
                var line = q.Output.Lines().First();
                System.Console.WriteLine(line);

                var ss = line.Split(' ');

                System.Console.WriteLine($"perm: {ss[0]}");
                System.Console.WriteLine($"owner: {ss[2]}");
                System.Console.WriteLine($"group: {ss[3]}");
                System.Console.WriteLine($"size: {ss[4]}");
            }

            // RESULT:
            //
            // -rw-r--r-- 1 root root 218 May 11  2020 /etc/hosts
            // perm: -rw-r--r--
            // owner: root
            // group: root
            // size: 218

        }).Wait();
    }
}
```


## How this project was built

```sh
mkdir netcore-ext
cd netcore-ext

dotnet new sln

mkdir -p examples src/ext

cd src/ext
dotnet new classlib -n netcore-ext -f netstandard2.1 --langVersion 11
# add packages ( https://nuget.org )

cd ..
dotnet new xunit -n test
cd test
dotnet add reference ../ext/netcore-ext.csproj
cd ..

dotnet sln add src/ext src/test examples/example01
dotnet build
dotnet test
```
