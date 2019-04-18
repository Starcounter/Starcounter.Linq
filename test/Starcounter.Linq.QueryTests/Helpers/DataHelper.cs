using Starcounter.Nova;

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
                var company = Db.Insert<Company>();
                company.Name = "Starcounter";
                company.Global = true;
                var employee = Db.Insert<Employee>();
                employee.Gender = Gender.Male;
                employee.Name = "Anton";
                employee.Age = 31;
                employee.LimitInt8 = 3;
                employee.LimitInt16 = 3;
                employee.LimitInt32 = 3;
                employee.LimitInt64 = 3;
                employee.LimitUInt8 = 3;
                employee.LimitUInt16 = 3;
                employee.LimitUInt32 = 3;
                employee.LimitUInt64 = 3;
                employee.LimitDecimal = 3.35M;
                employee.LimitSingle = 3.34F;
                employee.LimitDouble = 3.33D;
                employee.Department = Db.Insert<Department>();
                employee.Department.Name = "Application Development";
                employee.Department.Company = company;
                employee.Department.Global = true;
                employee = Db.Insert<Employee>();
                employee.Gender = Gender.Male;
                employee.Name = "Roger";
                employee.Age = 41;
                employee.LimitInt8 = 4;
                employee.LimitInt16 = 4;
                employee.LimitInt32 = 4;
                employee.LimitInt64 = 4;
                employee.LimitUInt8 = 4;
                employee.LimitUInt16 = 4;
                employee.LimitUInt32 = 4;
                employee.LimitUInt64 = 4;
                employee.LimitDecimal = 4.47M;
                employee.LimitSingle = 4.46F;
                employee.LimitDouble = 4.45D;
                employee.Department = Db.Insert<Department>();
                employee.Department.Name = "Solution Architecture";
                employee.Department.Company = company;
                employee.Department.Global = true;
                employee.Office = Db.Insert<Office>();
                employee.Office.City = "Stockholm";
                employee.Disabled = true;
                var department = Db.Insert<Department>();
                department.Name = "Administration";
                department.Company = company;
                department.Global = false;
            });
        }
    }
}
