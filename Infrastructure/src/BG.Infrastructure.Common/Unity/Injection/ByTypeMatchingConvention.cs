namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    using BG.Extensions;
    using BG.Infrastructure.Common.Utility;

    public class ByTypeMatchingConvention : ParameterMatchingConvention
    {
        private readonly Dictionary<Type, ICollection<string>> snippetsByType;
        private Object _snippetsByTypeSynchObj = new Object();

        public ByTypeMatchingConvention()
        {
            this.snippetsByType = new Dictionary<Type, ICollection<string>>();
        }

        protected override bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter)
        {
            ResolvedParameter rp = argument.Value as ResolvedParameter;

            Type type = rp != null ? rp.ParameterType : argument.Value.GetType();

            ICollection<string> snippets;
            if (!this.snippetsByType.TryGetValue(type, out snippets))
            {
                lock (_snippetsByTypeSynchObj)
                {
                    if (!this.snippetsByType.TryGetValue(type, out snippets))
                    {
                        snippets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                        InitializeSnippets(type, snippets);

                        this.snippetsByType.Add(type, snippets);
                    }
                }
            }

            return snippets.Contains(parameter.Name);
        }

        private static void InitializeSnippets(Type type, ICollection<string> snippets)
        {
            var allBaseClassesAndInterfaces = type.GetAllBaseClassesAndInterfaces();

            foreach (var t in allBaseClassesAndInterfaces)
            {
                string typeName = t.Name;

                if (typeName.StartsWith("i", StringComparison.OrdinalIgnoreCase))
                {
                    typeName = typeName.Substring(1);
                }

                for (int i = 0; i < typeName.Length; i++)
                {
                    char c = typeName[i];

                    if (char.IsUpper(c))
                    {
                        string snippet = typeName.Substring(i);
                        snippets.Add(snippet);
                    }
                }
            }
        }
    }
}