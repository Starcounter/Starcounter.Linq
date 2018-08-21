namespace Starcounter.Linq.QueryTests
{
    public interface INamed
    {
        string Name { get; set; }
    }

    public interface IHaveCompany
    {
        Company Company { get; set; }
    }
}