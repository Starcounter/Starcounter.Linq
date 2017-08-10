using System.Collections.Generic;

namespace PoS.Infra
{
    public class ParameterAggregator
    {
        private readonly List<NamedParameter> _parameters = new List<NamedParameter>();

        public NamedParameter AddParameter(object value)
        {
            var parameter = new NamedParameter("?", value);//  //"p" + (_parameters.Count + 1), value);
            _parameters.Add(parameter);
            return parameter;
        }

        public NamedParameter[] GetParameters()
        {
            return _parameters.ToArray();
        }
    }
}