namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;
    using System.Reflection;

    using BG.Infrastructure.Common.Utility;

    public class StringAsMappingNameMatchingConvention : ParameterMatchingConvention
    {
        public override bool Matches(ConstructorParameter argument, ParameterInfo parameter)
        {
            Guard.AssertNotNull(argument, "argument");
            Guard.AssertNotNull(argument.Value, "argument.Value");
            Guard.AssertNotNull(parameter, "parameter");

            return this.MatchesCore(argument, parameter);
        }

        protected override bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter)
        {
            if (string.Equals(argument.Name, parameter.Name, StringComparison.OrdinalIgnoreCase))
            {
                Type parameterType = parameter.ParameterType;

                if (argument.Value is string &&
                    parameterType != typeof(string) &&
                    (parameterType.IsClass || parameter.ParameterType.IsInterface))
                {
                    return true;
                }
            }

            return false;
        }
    }
}