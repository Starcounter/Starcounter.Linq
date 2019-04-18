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
                Db.SlowSQL($"DELETE FROM {typeof(Office).FullName}");
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
                _ = new Employee
                {
                    Gender = Gender.Male,
                    Name = "Anton",
                    Age = 31,
                    LimitInt8 = 3,
                    LimitInt16 = 3,
                    LimitInt32 = 3,
                    LimitInt64 = 3,
                    LimitUInt8 = 3,
                    LimitUInt16 = 3,
                    LimitUInt32 = 3,
                    LimitUInt64 = 3,
                    LimitDecimal = 3.35M,
                    LimitSingle = 3.34F,
                    LimitDouble = 3.33D,
                    Department = new Department
                    {
                        Name = "Application Development",
                        Company = company,
                        Global = true
                    }
                };
                _ = new Employee
                {
                    Gender = Gender.Male,
                    Name = "Roger",
                    Age = 41,
                    LimitInt8 = 4,
                    LimitInt16 = 4,
                    LimitInt32 = 4,
                    LimitInt64 = 4,
                    LimitUInt8 = 4,
                    LimitUInt16 = 4,
                    LimitUInt32 = 4,
                    LimitUInt64 = 4,
                    LimitDecimal = 4.47M,
                    LimitSingle = 4.46F,
                    LimitDouble = 4.45D,
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
                _ = new Department
                {
                    Name = "Administration",
                    Company = company,
                    Global = false
                };
            });
        }
    }
}
