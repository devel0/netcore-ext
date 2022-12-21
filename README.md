# netcore-ext

[![NuGet Badge](https://buildstats.info/nuget/netcore-ext)](https://www.nuget.org/packages/netcore-ext/)

.NET core extensions

- [API Documentation](https://devel0.github.io/netcore-ext/html/annotated.html)
- [Changelog](https://github.com/devel0/netcore-ext/commits/master)

<hr/>

<!-- TOC -->

<!-- TOCEND -->

<hr/>

## Quickstart

- [nuget package](https://www.nuget.org/packages/netcore-ext/)

- [extension methods](https://devel0.github.io/netcore-ext/html/class_search_a_thing_1_1_ext.html)

```csharp
using SearchAThing;
```

- [toolkit methods](https://devel0.github.io/netcore-util/html/class_search_a_thing_1_1_toolkit.html)

```csharp
using static SearchAThing.Toolkit;
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
dotnet new classlib -n netcore-ext -f netstandard2.1 --langVersion 10

dotnet new xunit -n test
cd test
dotnet add reference ../netcore-ext
cd ..

dotnet sln netcore-ext.sln add netcore-ext
dotnet sln netcore-ext.sln add test
dotnet build
dotnet test
```
