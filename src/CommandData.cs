namespace Starcounter.Linq
{
    public class CommandData
    {
        public CommandData(string statement, QueryVariable[] queryVariables)
        {
            Statement = statement;
            QueryVariables = queryVariables;
        }

        public string Statement { get; }
        public QueryVariable[] QueryVariables { get; }
    }
}