namespace BG.Infrastructure.Common.Utility
{
    using System.Diagnostics;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;

    [DebuggerDisplay("Name:'{Name}' Value:'{Value}'")]
    public class ConstructorParameter
    {
        private readonly object value;

        private readonly string name;

        public ConstructorParameter(string name, object value)
        {
            Guard.AssertNotEmpty(name, "name");

            this.value = value;
            this.name = name;
        }

        public ConstructorParameter(object value)
        {
            Guard.AssertNotNull(value, "value");

            this.value = value;
            this.name = string.Empty;
        }

        public object Value
        {
            get
            {
                return this.value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }

    public class ConstructorParameterCollection : List<ConstructorParameter>
    {
        public ConstructorParameterCollection Add(string name, object value)
        {
            base.Add(new ConstructorParameter(name, value));
            return this;
        }

        public ConstructorParameterCollection Add<TResolvedType>(string name, string resolvedName)
        {
            base.Add(new ConstructorParameter(name, new ResolvedParameter<TResolvedType>(resolvedName)));
            return this;
        }
    }
}