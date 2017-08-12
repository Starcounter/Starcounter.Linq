using System.Collections.Generic;

namespace Starcounter.Linq
{
    public class QueryVariables
    {
        private readonly List<QueryVariable> _variables = new List<QueryVariable>();

        public QueryVariable AddVariable(object value)
        {
            var parameter = new QueryVariable("?", value); //  //"p" + (_variables.Count + 1), value);
            _variables.Add(parameter);
            return parameter;
        }

        public QueryVariable[] GetParameters()
        {
            return _variables.ToArray();
        }
    }
}