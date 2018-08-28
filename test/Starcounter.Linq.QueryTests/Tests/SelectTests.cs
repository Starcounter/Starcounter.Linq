using System.Collections.Generic;
using System.Linq;
using Starcounter.Nova;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    [Collection("Data tests")]
    public class SelectTests : IClassFixture<DataTestFixture>
    {
        private readonly DataTestFixture _fixture;

        public SelectTests(DataTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectObjectDirectly(Mode mode)
        {
            Db.Transact(() =>
            {
                List<Person> people = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Select(x => x))().ToList()
                    : Objects<Person>().Select(x => x).ToList();
                Assert.Equal(2, people.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectObjectProperty(Mode mode)
        {
            Db.Transact(() =>
            {
                List<Office> offices = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Select(x => x.Office))().ToList()
                    : Objects<Person>().Select(x => x.Office).ToList();
                Assert.Equal(2, offices.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectStringProperty(Mode mode)
        {
            Db.Transact(() =>
            {
                List<string> names = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Select(x => x.Name))().ToList()
                    : Objects<Person>().Select(x => x.Name).ToList();
                Assert.Equal(2, names.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectIntegerProperty(Mode mode)
        {
            Db.Transact(() =>
            {
                List<int> ages = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Select(x => x.Age))().ToList()
                    : Objects<Person>().Select(x => x.Age).ToList();
                Assert.Equal(2, ages.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectGenericType(Mode mode)
        {
            Db.Transact(() =>
            {
                List<Person> people = SelectByName<Person>(mode, "Roger");
                Assert.Single(people);
                Assert.Equal("Roger", people[0].Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectGenericTypeEmbedded(Mode mode)
        {
            Db.Transact(() =>
            {
                List<Department> departments = SelectByCompanyName<Department>(mode, "Starcounter");
                Assert.Equal(2, departments.Count);
            });
        }

        private List<T> SelectByName<T>(Mode mode, string name)
            where T : INamed
        {
            return mode == Mode.CompiledQuery
                ? CompileQuery((string n) => Objects<T>().Where(x => x.Name == n))(name).ToList()
                : Objects<T>().Where(x => x.Name == name).ToList();
        }

        private List<T> SelectByCompanyName<T>(Mode mode, string companyName)
            where T : IHaveCompany
        {
            return mode == Mode.CompiledQuery
                ? CompileQuery((string cn) => Objects<T>().Where(x => x.Company.Name == cn))(companyName).ToList()
                : Objects<T>().Where(x => x.Company.Name == companyName).ToList();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectObjectProperty_Where(Mode mode)
        {
            var city = "Stockholm";
            Scheduling.RunTask(() =>
            {
                List<Office> offices = mode == Mode.CompiledQuery
                    ? CompileQuery((string c) => Objects<Person>().Select(x => x.Office).Where(x => x.City == c))(city).ToList()
                    : Objects<Person>().Select(x => x.Office).Where(x => x.City == city).ToList();
                Assert.Equal(1, offices.Count);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectObjectProperty_Where_SelectProperty(Mode mode)
        {
            var deptName = "Solution Architecture";
            Scheduling.RunTask(() =>
            {
                List<Company> companies = mode == Mode.CompiledQuery
                    ? CompileQuery((string n) => Objects<Employee>().Select(x => x.Department).Where(x => x.Name == n).Select(x => x.Company))(deptName).ToList()
                    : Objects<Employee>().Select(x => x.Department).Where(x => x.Name == deptName).Select(x => x.Company).ToList();
                Assert.Equal(1, companies.Count);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void SelectObjectProperty_Where_SelectProperty_Twice(Mode mode)
        {
            var deptName = "Solution Architecture";
            Scheduling.RunTask(() =>
            {
                List<string> names = mode == Mode.CompiledQuery
                    ? CompileQuery((string n) => Objects<Employee>().Select(x => x.Department).Where(x => x.Name == n).Select(x => x.Company).Where(x => x.Global).Select(x => x.Name))(deptName).ToList()
                    : Objects<Employee>().Select(x => x.Department).Where(x => x.Name == deptName).Select(x => x.Company).Where(x => x.Global).Select(x => x.Name).ToList();
                Assert.Equal(1, names.Count);
            }).Wait();
        }
    }
}
