using System.Linq;
using Starcounter.Nova;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    [Collection("Data tests")]
    public class FilteringTests : IClassFixture<DataTestFixture>
    {
        private readonly DataTestFixture _fixture;

        public FilteringTests(DataTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereStringEqual(Mode mode)
        {
            Db.Transact(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Person>().Where(p => p.Name == name))("Roger").ToList()
                    : Objects<Person>().Where(p => p.Name == "Roger").ToList();
                Assert.Single(persons);
                Assert.Equal("Roger", persons.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereIntEqual_ReservedWordField(Mode mode)
        {
            Db.Transact(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery((int limit) => Objects<Person>().Where(p => p.Limit == limit))(2).ToList()
                    : Objects<Person>().Where(p => p.Limit == 2).ToList();
                Assert.Single(persons);
                Assert.Equal("Roger", persons.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereIs(Mode mode)
        {
            Db.Transact(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Where(p => p is Employee))().ToList()
                    : Objects<Person>().Where(p => p is Employee).ToList();
                Assert.Equal(2, persons.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_StringEqual(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Person>().FirstOrDefault(p => p.Name == name))("Roger")
                    : Objects<Person>().FirstOrDefault(p => p.Name == "Roger");
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereStringEqual_Take_FirstOrDefault_EnumEqual(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name, Gender gender) => Objects<Person>().Where(x => x.Name == name).Take(10).FirstOrDefault(x => x.Gender == gender))("Roger", Gender.Male)
                    : Objects<Person>().Where(x => x.Name == "Roger").Take(10).FirstOrDefault(x => x.Gender == Gender.Male);
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
                Assert.Equal(Gender.Male, person.Gender);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_StringNotEqual(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Person>().FirstOrDefault(p => p.Name != name))("Roger")
                    : Objects<Person>().FirstOrDefault(p => p.Name != "Roger");
                Assert.NotNull(person);
                Assert.NotEqual("Roger", person.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_NestedStringEqual(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string name) => Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == name))("Starcounter")
                    : Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "Starcounter");
                Assert.NotNull(person);
                Assert.Equal("Starcounter", person.Department.Company.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_StringContains_Variable(Mode mode)
        {
            Db.Transact(() =>
            {
                var value = "oge";
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string namePart) => Objects<Person>().FirstOrDefault(p => p.Name.Contains(namePart)))(value)
                    : Objects<Person>().FirstOrDefault(p => p.Name.Contains(value));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_StringNotContains_Constant(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string namePart) => Objects<Person>().FirstOrDefault(p => !p.Name.Contains(namePart)))("oge")
                    : Objects<Person>().FirstOrDefault(p => !p.Name.Contains("oge"));
                Assert.NotNull(person);
                Assert.NotEqual("Roger", person.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_StringStartsWith(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string nameStart) => Objects<Person>().FirstOrDefault(p => p.Name.StartsWith(nameStart)))("Ro")
                    : Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("Ro"));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_StringEndsWith(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((string nameEnd) => Objects<Person>().FirstOrDefault(p => p.Name.EndsWith(nameEnd)))("er")
                    : Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("er"));
                Assert.NotNull(person);
                Assert.Equal("Roger", person.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_IntegerGreaterAndLess(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery((int min, int max) => Objects<Person>().FirstOrDefault(p => p.Age > min && p.Age < max))(20, 40)
                    : Objects<Person>().FirstOrDefault(p => p.Age > 20 && p.Age < 40);
                Assert.NotNull(person);
                Assert.Equal(31, person.Age);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_StringEqualsNull__NotFound(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().FirstOrDefault(p => p.Name == null))()
                    : Objects<Person>().FirstOrDefault(p => p.Name == null);
                Assert.Null(person);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_ObjectEqual(Mode mode)
        {
            Db.Transact(() =>
            {
                var employee1 = Objects<Employee>().First(p => p.Name == "Roger");
                var employee2 = mode == Mode.CompiledQuery
                    ? CompileQuery((Department department) => Objects<Employee>().FirstOrDefault(p => p.Department == department))(employee1.Department)
                    : Objects<Employee>().FirstOrDefault(p => p.Department == employee1.Department);
                Assert.NotNull(employee2);
                Assert.Equal("Roger", employee2.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_ObjectNotEqual(Mode mode)
        {
            Db.Transact(() =>
            {
                var employee1 = Objects<Employee>().First(p => p.Name == "Roger");
                var employee2 = mode == Mode.CompiledQuery
                    ? CompileQuery((Department department) => Objects<Employee>().FirstOrDefault(p => p.Department != department))(employee1.Department)
                    : Objects<Employee>().FirstOrDefault(p => p.Department != employee1.Department);
                Assert.NotNull(employee2);
                Assert.NotEqual("Roger", employee2.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_ObjectNull(Mode mode)
        {
            Db.Transact(() =>
            {
                var employee = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().FirstOrDefault(p => p.Office == null))()
                    : Objects<Employee>().FirstOrDefault(p => p.Office == null);

                Assert.NotNull(employee);
                Assert.Equal("Anton", employee.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_ObjectNotNull(Mode mode)
        {
            Db.Transact(() =>
            {
                var employee = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().FirstOrDefault(p => p.Office != null))()
                    : Objects<Employee>().FirstOrDefault(p => p.Office != null);

                Assert.NotNull(employee);
                Assert.Equal("Roger", employee.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_ObjectNull_Variable(Mode mode)
        {
            Db.Transact(() =>
            {
                Office office = null;
                var employee = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().FirstOrDefault(p => p.Office == office))()
                    : Objects<Employee>().FirstOrDefault(p => p.Office == office);

                Assert.NotNull(employee);
                Assert.Equal("Anton", employee.Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void FirstOrDefault_ObjectNotNull_Variable(Mode mode)
        {
            Db.Transact(() =>
            {
                Office office = null;
                var employee = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Employee>().FirstOrDefault(p => p.Office != office))()
                    : Objects<Employee>().FirstOrDefault(p => p.Office != office);

                Assert.NotNull(employee);
                Assert.Equal("Roger", employee.Name);
            });
        }

        [Fact]
        public void FirstOrDefault_CompiledQuery_NullParameter()
        {
            Db.Transact(() =>
            {
                Office office = null;
                // this is incorrect, Compiled Query is not supported passing null with parameter (only inline, see FirstObjectEqualNull test)
                // ReSharper disable once ExpressionIsAlwaysNull
                var employee = CompileQuery((Office o) => Objects<Employee>().FirstOrDefault(p => p.Office != o))(office);

                Assert.Null(employee);  // because the built SQL was incorrect
            });
        }

        [Fact]
        public void FirstOrDefault_IntegerInArray__AdHoc()
        {
            Db.Transact(() =>
            {
                var ages = new[] { 41, 42, 43 };
                var person = Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
                Assert.NotNull(person);
                Assert.Equal(41, person.Age);
            });
        }

        [Fact]
        public void FirstOrDefault_IntegerInEmptyArray__AdHoc()
        {
            Db.Transact(() =>
            {
                var ages = new int[0];
                var person = Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
                Assert.Null(person);
            });
        }

        [Fact]
        public void FirstOrDefault_ObjectInEmptyArray__AdHoc()
        {
            Db.Transact(() =>
            {
                var people = new Person[0];
                var person = Objects<Person>().FirstOrDefault(p => people.Contains(p));
                Assert.Null(person);
            });
        }

        [Fact]
        public void FirstOrDefault_IntegerInArray__NotFound__AdHoc()
        {
            Db.Transact(() =>
            {
                var ages = new[] { 1, 2, 3, 4, 5 };
                var person = Objects<Person>().FirstOrDefault(p => ages.Contains(p.Age));
                Assert.Null(person);
            });
        }

        [Fact]
        public void FirstOrDefault_IntegerNotInArray__AdHoc()
        {
            Db.Transact(() =>
            {
                var ages = new[] { 1, 2, 3, 4, 5 };
                var person = Objects<Person>().FirstOrDefault(p => !ages.Contains(p.Age));
                Assert.NotNull(person);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereObjectNoEqual(Mode mode)
        {
            Db.Transact(() =>
            {
                ulong testPersonId = Db.SQL<Person>($"SELECT p FROM {typeof(Person)} p WHERE p.Name = ?", "Roger").First().GetObjectNo();
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery((ulong id) => Objects<Person>().Where(p => p.GetObjectNo() == id))(testPersonId).ToList()
                    : Objects<Person>().Where(p => p.GetObjectNo() == testPersonId).ToList();

                Assert.Equal(1, persons.Count);
                Assert.Equal("Roger", persons.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void WhereObjectNoNotEqualWithOtherPredicate(Mode mode)
        {
            Db.Transact(() =>
            {
                ulong testPersonId = Db.SQL<Person>($"SELECT p FROM {typeof(Person)} p WHERE p.Name = ?", "Roger").First().GetObjectNo();
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery((ulong id, string name) => Objects<Person>().Where(p => p.GetObjectNo() != id && p.Name == name))(testPersonId, "Anton").ToList()
                    : Objects<Person>().Where(p => p.GetObjectNo() != testPersonId && p.Name == "Anton").ToList();

                Assert.Equal(1, persons.Count);
                Assert.NotEqual("Roger", persons.First().Name);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Where_PredicateParameter(Mode mode)
        {
            Db.Transact(() =>
            {
                var person = Db.SQL<Person>($"SELECT p FROM {typeof(Person)} p WHERE p.Name = ?", "Roger").First();
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery((Person prsn) => Objects<Person>().Where(p => p == prsn))(person).ToList()
                    : Objects<Person>().Where(p => p == person).ToList();

                Assert.Equal(1, persons.Count);
                Assert.Equal("Roger", persons.First().Name);
            });
        }

        [Fact]
        public void WhereEnumToString()
        {
            Db.Transact(() =>
            {
                var persons = Objects<Person>().Where(p => p.Name == Names.Anton.ToString()).ToList();

                Assert.Equal(1, persons.Count);
                Assert.Equal("Anton", persons.First().Name);
            });
        }

        [Fact]
        public void WhereObjectMethodParameterless()
        {
            Db.Transact(() =>
            {
                var nameInstance = new NameClass();

                var persons = Objects<Person>().Where(p => p.Name == nameInstance.GetName()).ToList();

                Assert.Equal(1, persons.Count);
                Assert.Equal("Anton", persons.First().Name);
            });
        }

        [Fact]
        public void WhereObjectMethodParameters1()
        {
            Db.Transact(() =>
            {
                var nameInstance = new NameClass();
                int param1 = 999;

                var persons = Objects<Person>().Where(p => p.Name == nameInstance.GetName(param1)).ToList();

                Assert.Equal(1, persons.Count);
                Assert.Equal("Anton", persons.First().Name);
            });
        }

        [Fact]
        public void WhereObjectMethodParameters2()
        {
            Db.Transact(() =>
            {
                int ctorArg = 99;
                int param1 = 999;
                string param2 = "any";

                var persons = Objects<Person>().Where(p => p.Name == new NameClass(ctorArg).GetName(param1, param2)).ToList();

                Assert.Equal(1, persons.Count);
                Assert.Equal("Anton", persons.First().Name);
            });
        }


        private enum Names { Anton, Roger }

        private class NameClass
        {
            private const string Name = "Anton";

            public NameClass()
            { }
            public NameClass(int arg1)
            { }

            public string GetName()
            {
                return Name;
            }
            public string GetName(int val1)
            {
                return Name;
            }
            public string GetName(int val1, string val2)
            {
                return Name;
            }
        }
    }
}