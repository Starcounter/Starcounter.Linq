using System.Collections.Generic;

namespace Starcounter.Linq
{
    public class QueryVariables
    {
        private readonly List<QueryVariable> _parameters = new List<QueryVariable>();

        public QueryVariable AddVariable(object value)
        {
            var parameter = new QueryVariable("?", value); //  //"p" + (_parameters.Count + 1), value);
            _parameters.Add(parameter);
            return parameter;
        }

        public QueryVariable[] GetParameters()
        {
            return _parameters.ToArray();
        }
    }
}