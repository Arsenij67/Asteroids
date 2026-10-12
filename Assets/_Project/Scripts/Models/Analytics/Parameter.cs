using System;
using System.Collections.Generic;

namespace Asteroid.Services.Analytics
{
    public class Parameter
    {
        internal string Name { get; set; }

        internal object Value { get; set; }

        public Parameter(string parameterName, string parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }

        public Parameter(string parameterName, long parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }

        public Parameter(string parameterName, double parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }

        public Parameter(string parameterName, IDictionary<string, object> parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }

        public Parameter(string parameterName, IEnumerable<IDictionary<string, object>> parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }
    }
}