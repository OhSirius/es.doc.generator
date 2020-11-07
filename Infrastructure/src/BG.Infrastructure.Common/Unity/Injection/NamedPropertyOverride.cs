namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;


    public class NamedPropertyOverride : ResolverOverride
    {
        private readonly string propertyName;

        private readonly string registrationName;

        public NamedPropertyOverride(string propertyName, string registrationName)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            this.propertyName = propertyName;
            this.registrationName = registrationName;
        }

        public override IDependencyResolverPolicy GetResolver(IBuilderContext context, Type dependencyType)
        {
            Guard.AssertNotNull(context, "context");

            var currentOperation = context.CurrentOperation as ResolvingPropertyValueOperation;

            PropertyInfo property;

            if (currentOperation != null && 
                currentOperation.PropertyName == this.propertyName &&
                currentOperation.TypeBeingConstructed != null &&
                (property = currentOperation.TypeBeingConstructed.GetProperty(this.propertyName, BindingFlags.Instance | BindingFlags.Public)) != null)
            {
                Type propertyType = property.PropertyType;

                return new NamedTypeDependencyResolverPolicy(propertyType, this.registrationName);
            }

            return null;
        }
    }
}
