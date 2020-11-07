namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using BG.Extensions;
    using BG.Infrastructure.Common.Utility;
    
    public class SmartConstructor : InjectionMember
    {
        private readonly List<ConstructorParameter> parameters;

        public SmartConstructor(object anonymous)
        {
            Guard.AssertNotNull(anonymous, "anonymous");

            this.parameters = new List<ConstructorParameter>();

            if (anonymous.GetType().IsAnonymous())
            {
                var properties = anonymous.PublicProperties();

                foreach (var property in properties)
                {
                    this.parameters.Add(new ConstructorParameter(property.Name, property.GetValue(anonymous, null)));
                }
            }
            else
            {
                this.parameters.Add(new ConstructorParameter(anonymous));
            }
        }

        public SmartConstructor(string argumentName, object value)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.parameters = new List<ConstructorParameter> { new ConstructorParameter(argumentName, value) };
        }

        public SmartConstructor(IEnumerable<ConstructorParameter> constructorArguments)
        {
            Guard.AssertNotNull(constructorArguments, "constructorArguments");

            this.parameters = new List<ConstructorParameter>(constructorArguments);
        }

        public SmartConstructor(params ConstructorParameter[] constructorArguments)
        {
            if (constructorArguments == null)
            {
                this.parameters = new List<ConstructorParameter>();
            }
            else
            {
                this.parameters = new List<ConstructorParameter>(constructorArguments);
            }
        }

        public SmartConstructor With(object value)
        {
            Guard.AssertNotNull(value, "value");

            this.parameters.Add(new ConstructorParameter(value));

            return this;
        }

        public SmartConstructor With(string name, object value)
        {
            Guard.AssertNotEmpty(name, "argumentName");

            this.parameters.Add(new ConstructorParameter(name, value));

            return this;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");
            Guard.AssertNotNull(policies, "policies");

            ConstructorInfo ctor;
            InjectionParameterValue[] parameterValues;

            if (this.parameters.Count == 0)
            {
                ctor = implementationType.GetConstructor(Type.EmptyTypes);

                parameterValues = new InjectionParameterValue[0];
            }
            else
            {
                NamedTypeBuildKey key = new NamedTypeBuildKey(implementationType, name);

                IParameterMatchingConventionsPolicy conventions = policies.Get<IParameterMatchingConventionsPolicy>(key);

                if (conventions == null)
                {
                    conventions = new DefaultMatchingConventionsPolicy();

                    policies.SetDefault<IParameterMatchingConventionsPolicy>(conventions);
                }

                ctor = FindConstructor(implementationType, this.parameters, conventions);

                parameterValues = GetParameterValues(ctor, this.parameters, conventions);
            }

            IConstructorSelectorPolicy policy = new SpecifiedConstructorSelectorPolicy(ctor, parameterValues);

            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            policies.Set<IConstructorSelectorPolicy>(policy, buildKey);
        }

        private static ConstructorInfo FindConstructor(Type typeToCreate, IEnumerable<ConstructorParameter> constructorArguments, IParameterMatchingConventionsPolicy conventions)
        {
            ConstructorInfo[] ctors = typeToCreate.GetConstructors();

            ParameterMatcher matcher = new ParameterMatcher(typeToCreate, constructorArguments, conventions);

            return matcher.BestMatch(ctors);
        }

        private static InjectionParameterValue[] GetParameterValues(ConstructorInfo ctor, IEnumerable<ConstructorParameter> constructorArguments, IParameterMatchingConventionsPolicy conventions)
        {
            ParameterInfo[] parameters = ctor.GetParameters();

            object[] parameterValues = new object[parameters.Length];

            // fill in the provided values and add the type of the parameter that the
            // ctor expects otherwise. unity will take care of resolving parameters that
            // are provided that way
            for (int i = 0; i < parameters.Length; i++)
            {
                ConstructorParameter argument = constructorArguments.FirstOrDefault(a => conventions.Matches(a, parameters[i]));
                if (argument != null)
                {
                    Type parameterType = parameters[i].ParameterType;

                    if (argument.Value is string &&
                        parameterType != typeof(string) &&
                        (parameterType.IsInterface || parameterType.IsClass))
                    {
                        parameterValues[i] = new ResolvedParameter(parameterType, (string)argument.Value);
                    }
                    else
                    {
                        parameterValues[i] = argument.Value;
                    }
                }
                else
                {
                    parameterValues[i] = parameters[i].ParameterType;
                }
            }

            return InjectionParameterValue.ToParameters(parameterValues).ToArray();
        }
    }
}