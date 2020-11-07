namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;
    using System.Reflection;
    using BG.Infrastructure.Common.Utility;

    public class SpecifiedNameMatchingConvention : ParameterMatchingConvention
    {
        protected override bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter)
        {
            if (!string.IsNullOrEmpty(argument.Name))
            {
                return string.Equals(argument.Name, parameter.Name, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}