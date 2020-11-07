namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;
    using System.Reflection;

    using Microsoft.Practices.Unity;


    using BG.Extensions;
    using BG.Infrastructure.Common.Utility;

    public abstract class ParameterMatchingConvention
    {
        public virtual bool Matches(ConstructorParameter argument, ParameterInfo parameter)
        {
            Guard.AssertNotNull(argument, "argument");
            Guard.AssertNotNull(argument.Value, "argument.Value");
            Guard.AssertNotNull(parameter, "parameter");

            ResolvedParameter rp = argument.Value as ResolvedParameter;

            Type argumentValueType = rp != null ? rp.ParameterType : argument.Value.GetType();

            if (argument.Value != null &&
                parameter.ParameterType.IsAssignableFrom(argumentValueType))
            {
                return this.MatchesCore(argument, parameter);
            }

            return false;
        }

        protected abstract bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter);
    }
}
