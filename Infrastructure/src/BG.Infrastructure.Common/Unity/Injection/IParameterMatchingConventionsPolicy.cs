namespace BG.Infrastructure.Common.Unity.Injection
{
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using BG.Infrastructure.Common.Utility;

    public interface IParameterMatchingConventionsPolicy : IBuilderPolicy
    {
        bool Matches(ConstructorParameter argument, ParameterInfo parameter);

        void Add(ParameterMatchingConvention convention);
    }
}