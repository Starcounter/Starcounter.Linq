namespace PoS.Infra
{
    public class CommandData
    {
        public CommandData(string statement, NamedParameter[] namedParameters)
        {
            Statement = statement;
            NamedParameters = namedParameters;
        }

        public string Statement { get; }
        public NamedParameter[] NamedParameters { get; }
    }
}
