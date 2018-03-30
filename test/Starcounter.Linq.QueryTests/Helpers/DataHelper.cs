namespace Starcounter.Linq.QueryTests
{
    public static class DataHelper
    {
        public static void ResetData()
        {
            Db.Transact(() =>
            {
                Db.SlowSQL($"DELETE FROM {typeof(Person).FullName}");
                Db.SlowSQL($"DELETE FROM {typeof(Department).FullName}");
                Db.SlowSQL($"DELETE FROM {typeof(Company).FullName}");
            });
        }

        public static void CreateEmployees()
        {
            Db.Transact(() =>
            {
                var company = new Company
                {
                    Name = "Starcounter",
                    Global = true
                };
                new Employee
                {
                    Gender = Gender.Male,
                    Name = "Anton",
                    Age = 31,
                    Limit = 1,
                    Department = new Department
                    {
                        Name = "Application Development",
                        Company = company,
                        Global = true
                    }
                };
                new Employee
                {
                    Gender = Gender.Male,
                    Name = "Roger",
                    Age = 41,
                    Limit = 2,
                    Department = new Department
                    {
                        Name = "Solution Architecture",
                        Company = company,
                        Global = true
                    },
                    Office = new Office
                    {
                        City = "Stockholm"
                    },
                    Disabled = true
                };
            });
        }
    }
}