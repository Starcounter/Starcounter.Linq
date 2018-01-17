# Starcounter.Linq

Starcounter.Linq v2 is a LINQ to SQL Provider for Starcounter 2.4. It uses standard LINQ syntax for queries.

It is available for downloading as [Starcounter.Linq](https://www.nuget.org/packages/Starcounter.Linq/) NuGet package.

## Requirements

Requires Starcounter version 2.4 or later and .NET Framework 4.6.1.

## How to use

First, in your Starcounter app, add a reference to `Starcounter.Linq.dll` through NuGet: `Install-Package Starcounter.Linq`.

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

With this approach, you build a LINQ expression and obtain data as usual when an application needs it.

`DbLinq.Objects<T>()` returns a data context `Queryable<T>` which can be used for obtaining data.

The LINQ expression is translated to SQL every time it's called if `DbLinq.Objects<T>()` is called. This is an expensive operation. Thus, don't use `DbLinq.Objects<T>()` in places where it's executed many times.

Example:

```csharp
// this method is rarely used, so we can use ad-hoc requests here
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

## Compiled query

Compiled query lets you build a LINQ expression with translated SQL once and use it many times.

`DbLinq.CompileQuery<T>` returns a delegate `Func<IEnumerable<T>>` which can be invoked to execute the compiled query. It has many overloads and it supports passing parameters.

Use `DbLinq.CompileQuery<T>()` in places where the query will be executed many times since it only translates the LINQ statement to SQL one time which makes subsequent calls fast.

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

    // this method is often called, so we should use compiled queries
    public void RefreshData()
    {
        this.SurfaceGroups.Data = SurfaceGroupsQuery();
        this.Routes.Data = RoutesQuery(this.Data);
    }
}
```

Compiled queries are more restricted than ad-hoc requests since it represents a pre-translated SQL and should support passing parameters. Read more about the restrictions below.

## Restrictions

### Database fields

Starcounter.Linq **only supports database properties**. It is not possible to get access to fields.

Example:

```csharp
// throws System.MissingFieldException since Name is a field
DbLinq.Objects<WebTemplateGroup>().OrderBy(x => x.Name);

// works well since SortNumber is defined as property
DbLinq.Objects<WebTemplateGroup>().OrderBy(x => x.SortNumber);
```

The exception will be thrown when calling the method which contains the query definition, which means that the exception will not be thrown from Starcounter.Linq code.

### `Take` and `Skip` methods

Starcounter.Linq uses literal values for `FETCH` and `OFFSET` clauses for performance reason, it means that you cannot pass the value when executing a compiled query.

Example:

```csharp
// works well
var people = DbLinq.Objects<Person>().Take(10).Skip(20).ToList();

// does not work
var query = DbLinq.CompileQuery((int take, int skip) => DbLinq.Objects<Person>().Take(take).Skip(skip));
people = query(10, 20);

// works well
var query = DbLinq.CompileQuery(() => DbLinq.Objects<Person>().Take(10).Skip(20));
people = query();
```

### NULL values

Since comparisons with `null` values are translated to `IS NULL` form in SQL, there is no possibility to pass such values with parameters into compiled queries.

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

// it does not work because the SQL query has been translated and IS NULL cannot be inserted
employee1 = withOfficeQuery(null);

// that should be written in the following way
employee1 = office == null ? withoutOfficeQuery() : withOfficeQuery(office);
```

### `Contains` method

The `Contains` method is supported by ad-hoc requests but not by compiled queries.

Example:

```csharp
var ages = new[] { 41, 42, 43 };
var person = Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
```

### Deleting data

Deleting data is supported by ad-hoc requests but not by compiled queries.

Example:

```csharp
Objects<Person>().Delete(x => x.Age > 40);
Objects<Person>().DeleteAll();
```

-----

**For the latest news, look at the [Starcounter Blog](https://starcounter.io/blog/).**