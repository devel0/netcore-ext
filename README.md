# netcore-ext

[![NuGet Badge](https://buildstats.info/nuget/netcore-ext)](https://www.nuget.org/packages/netcore-ext/)

.NET core extensions

- [API Documentation](https://devel0.github.io/netcore-ext/html/annotated.html)
- [Sources Changelog](https://github.com/devel0/netcore-ext/commits/main)

<hr/>

<!-- TOC -->

- [Quickstart](#quickstart)
  - [Extension Methods](#extension-methods)
  - [Toolkit Methods](#toolkit-methods)
- [Unit tests](#unit-tests)
- [How this project was built](#how-this-project-was-built)
<!-- TOCEND -->

<hr/>

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

mkdir -p examples src/ext

cd src

dotnet new classlib -n netcore-ext -f netstandard2.1 --langVersion 11
mv netcore-ext ext
cd ..

dotnet new xunit -n test
cd test
dotnet add reference ../ext/netcore-ext.csproj
# enable test coverage collectorx
# to view in vscode ( "Coverage Gutters" ext ) run `./test-coverage` then `C-S-p` Coverage Gutters: Watch
dotnet add package coverlet.collector
dotnet add package coverlet.msbuild
cd ..

cd ..
dotnet new sln
dotnet sln add src/ext src/test examples/example01
dotnet build
dotnet test

# documentation css

mkdir data
git submodule add https://github.com/jothepro/doxygen-awesome-css.git data/doxygen-awesome-css
cd data/doxygen-awesome-css
# doxygen 1.9.2 ( workaround about treeview https://github.com/doxygen/doxygen/issues/9254 )
# doxygen version used https://github.com/SearchAThing-forks/doxygen/tree/Release_1_9_2_with_autotrimleft
git checkout 45f50e0438ac31ec3b3cd594ae68925fc8d4aeae
cd ../..
```

## Documentation (github pages)

Configured through Settings/Pages on Branch docs ( path /docs ).

- while main branch exclude "docs" with .gitignore the docs branch doesn't

### Build and view locally

```sh
./doc build
./doc serve
./doc view
```

### Build and commit into docs branch

```sh
./doc commit
```
