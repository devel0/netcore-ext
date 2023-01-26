# netcore-ext

[![NuGet Badge](https://buildstats.info/nuget/netcore-ext)](https://www.nuget.org/packages/netcore-ext/)

.NET core extensions

- [API Documentation](https://devel0.github.io/netcore-ext/html/annotated.html)
- [Sources Changelog](https://github.com/devel0/netcore-ext/commits/main)

<hr/>

<!-- TOC -->
* [Quickstart](#quickstart)
  + [Extension Methods](#extension-methods)
  + [Toolkit Methods](#toolkit-methods)
* [Unit tests](#unit-tests)
* [How this project was built](#how-this-project-was-built)
<!-- TOCEND -->

<hr/>

## Sources

https://devel0.github.io/netcore-ext

## Quickstart

```sh
dotnet new console --use-program-main -n test
cd test
dotnet add package netcore-ext
dotnet run
```

- copy [usings.ext.cs](src/ext/usings.ext.cs) global usings to the source folder

- [extension methods](https://devel0.github.io/netcore-ext/html/class_search_a_thing_1_1_ext_1_1_ext.html)

```csharp
using SearchAThing.Ext;
```

- [toolkit methods](https://devel0.github.io/netcore-ext/html/class_search_a_thing_1_1_ext_1_1_toolkit.html)

```csharp
using static SearchAThing.Ext.Toolkit;
```

## Unit tests

```sh
dotnet test
```

- to debug from vscode just run debug test from code lens balloon
 
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
