namespace Starcounter.Linq
{
    public class QueryVariable
    {
        public QueryVariable(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }
}