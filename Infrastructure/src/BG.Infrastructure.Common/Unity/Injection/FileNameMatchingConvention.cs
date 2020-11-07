namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;
    using System.Reflection;

    using BG.Extensions;
    using BG.Infrastructure.Common.Utility;

    public class FileNameMatchingConvention : ParameterMatchingConvention
    {
        protected override bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return parameter.Name.Contains("file", StringComparison.OrdinalIgnoreCase) ||
                       parameter.Name.Contains("path", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}