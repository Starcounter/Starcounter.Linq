using Starcounter.Nova;

namespace StarcounterLinqUnitTests.Helpers
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
                var company = Db.Insert<Company>();
                company.Name = "Starcounter";
                company.Global = true;

                var employee = Db.Insert<Employee>();
                employee.Gender = Gender.Male;
                employee.Name = "Anton";
                employee.Age = 31;
                employee.Limit = 1;

                employee.Department = Db.Insert<Department>();
                employee.Department.Name = "Application Development";
                employee.Department.Company = company;
                employee.Department.Global = true;

                employee = Db.Insert<Employee>();
                employee.Gender = Gender.Male;
                employee.Name = "Roger";
                employee.Age = 41;
                employee.Limit = 2;
                employee.Disabled = true;

                employee.Department = Db.Insert<Department>();
                employee.Department.Name = "Solution Architecture";
                employee.Department.Company = company;
                employee.Department.Global = true;

                employee.Office = Db.Insert<Office>();
                employee.Office.City = "Stockholm";
            });
        }
    }
}