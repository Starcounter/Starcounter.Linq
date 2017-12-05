# Starcounter.Linq

Starcounter.Linq is a LINQ to SQL Provider for Starcounter Database. It uses standard LINQ syntax for queries.

It is available for downloading as [Starcounter.Linq](https://www.nuget.org/packages/Starcounter.Linq/) NuGet package.

## Requirements

Requires Starcounter version 2.3.1 and .NET Framework 4.5.

## How to use

Firstly, in your Starcounter app add a reference to `Starcounter.Linq.dll` through NuGet: `Install-Package Starcounter.Linq`.

`DbLinq` static class defined in `Starcounter.Linq` namespace should be used as starting point when you want to build LINQ queries. It contains two important methods: `Objects<T>` and `CompileQuery<T>` (many overloaded ones) which are used for two different approaches. For example:

```csharp
// ad-hoc request
var johns = DbLinq.Objects<Person>().Where(p => p.Name == "John");

// request using compiled query
var peopleByNameQuery = DbLinq.CompileQuery((string name) =>
                        DbLinq.Objects<Person>().Where(p => p.Name == name));
var jennifers = peopleByNameQuery("Jennifer");
```

## Ad-hoc requests

Using this approach you build a LINQ expression and obtain data as usual when an application needs it.

`DbLinq.Objects<T>()` returns a data context `Queryable<T>` which can be used for obtaining data.

We recommend to use it only in cases when a request is expected to be executed infrequently. The reason is that it is very expensive to translate LINQ expression to SQL. Using this approach such translating will be done every calling.

Example:

```csharp
// this method is used quite rare, so we can use ad-hoc requests here
void Handle(Input.DeleteGroupTrigger action)
{
    Db.Transact(() =>
    {
        var surfaces = DbLinq.Objects<WebTemplate>().Where(x => x.WebTemplateGroup == this.Data);

        foreach (WebTemplate surface in surfaces)
        {
            surface.WebTemplateGroup = null;
        }
        this.Data.Delete();
    });
}
```

## Compiled Query

Compiled Query lets you build a LINQ expression with translated SQL once and use it many times.

`DbLinq.CompileQuery<T>` returns a delegate `Func<IEnumerable<T>>` which can be invoked to execute the compiled query. It has many overloads and it supports passing parameters.

We recommend to use in all cases when a request is executed quite frequently.

Example:

```csharp
partial class SurfacePage : Json
{
    private static readonly Func<IEnumerable<WebTemplate>> SurfaceGroupsQuery = DbLinq.CompileQuery(() =>
            DbLinq.Objects<WebTemplateGroup>().Where(x => !x.Deleted));

    private static readonly Func<WebTemplate, IEnumerable<WebUrl>> RoutesQuery =
        DbLinq.CompileQuery((WebTemplate surface) =>
            DbLinq.Objects<WebUrl>()
                .Where(x => !x.Deleted && !x.Hidden)
                .Where(x => x.Template == surface)
                .OrderBy(x => x.SortNumber));
    /*
    ...
    */

    // this method is called quite frequently so we should use compiled queries
    public void RefreshData()
    {
        this.SurfaceGroups.Data = SurfaceGroupsQuery();
        this.Routes.Data = RoutesQuery(this.Data);
    }
}
```

Please note, that Compiled Query has some additional restrictions since it represents a pre-translated SQL and should support passing parameters.

## Restrictions

### Database fields

Starcounter.Linq supports **only database properties**. It is not possible to get access to fields.

Example:

```csharp
// throws System.MissingFieldException since Name is a field
DbLinq.Objects<WebTemplateGroup>().OrderBy(x => x.Name);

// works well since SortNumber is defined as property
DbLinq.Objects<WebTemplateGroup>().OrderBy(x => x.SortNumber);
```

Also please note, that the exception will be thrown when calling the method which contains the query definition, i.e. not from Starcounter.Linq code.

### `Take` and `Skip` methods

Starcounter.Linq uses literal values for `FETCH` and `OFFSET` clauses (for performance reason), it means that you cannot pass the value when executing a compiled query.

Example:

```csharp
// works well
var people = DbLinq.Objects<Person>().Take(10).Skip(20).ToList();

// does not work
var query = DbLinq.CompileQuery((int take, int skip) => DbLinq.Objects<Person>().Take(take).Skip(skip));
people = query(10, 20);

// work well
var query = DbLinq.CompileQuery(() => DbLinq.Objects<Person>().Take(10).Skip(20));
people = query();
```

### NULL values

Since comparisons with `null` values should be translated to `IS NULL` form in SQL, there is no possibility to pass such values with parameters into compiled queries.

Example:

```csharp
Office office = GetOffice();    // can be null
Office noOffice = null;

// ad-hoc requests works well without restrictions
var employee1 = DbLinq.Objects<Employee>().FirstOrDefault(p => p.Office != null);
var employee2 = DbLinq.Objects<Employee>().FirstOrDefault(p => p.Office == office);

// works well
var withoutOfficeQuery = DbLinq.CompileQuery(() =>
                         DbLinq.Objects<Employee>().FirstOrDefault(p => p.Office != null));
var withoutOfficeQuery2 = DbLinq.CompileQuery(() =>
                          DbLinq.Objects<Employee>().FirstOrDefault(p => p.Office == noOffice));

var withOfficeQuery = DbLinq.CompileQuery((notNullOffice) =>
                      DbLinq.Objects<Employee>().FirstOrDefault(p => p.Office == notNullOffice));

// it does not work because the SQL query has been already translated and IS NULL cannot be inserted
employee1 = withOfficeQuery(null);

// that should be written in the following way
employee1 = office == null ? withoutOfficeQuery() : withOfficeQuery(office);
```

### `First` method

For Compiled Query `First` method works like `FirstOrDefault`, i.e. it doesn't throw an exception when a sequence has no elements.

```csharp
// throws exception as expected
var employee = DbLinq.Objects<Employee>().First(x => x.Age == 100);

// just returns null
employee = DbLinq.CompileQuery((int age) => DbLinq.Objects<Employee>().First(x => x.Age == age))(100);
```

### `Contains` method

Compiled Query **does not support** `Contains` method while ad-hoc requests works well.

### Roadmap

Look at [Starcounter.Linq#6](https://github.com/Starcounter/Starcounter.Linq/issues/6)

-----

**For the latest news, look at the [Starcounter Blog](https://starcounter.io/blog/).**