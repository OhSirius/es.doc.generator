namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;
    using System.Reflection;

    using BG.Extensions;
    using BG.Infrastructure.Common.Utility;

    public class ConnectionStringMatchingConvention : ParameterMatchingConvention
    {
        protected override bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return parameter.Name.Contains("connectionString", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}