using System.Collections.Concurrent;
using System.Text;
using Aetp.CommonClasses;
using System;
using System.Collections.Generic;
using Aetp.Extensions.String;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aetp.Extensions.Types
{

    /// <summary>
    /// Helper class for common operations on types
    /// </summary>
    public static class TypeExtensions
    {
        public static bool IsInNamespace(this Type type, string nameSpace)
        {
            Guard.Against<ArgumentNullException>(type==null, "type");
            Guard.Against<ArgumentNullException>(nameSpace.IsNullOrEmpty(), "nameSpace");

            return type.Namespace.StartsWith(nameSpace);
        }

        public static IEnumerable<Type> AllInterfaces(this Type type)
        {
            foreach (Type @interface in type.GetInterfaces())
            {
                yield return @interface;
            }
        }

        public static bool IsConcrete(this Type type)
        {
            Guard.Against<ArgumentNullException>(type==null, "type");

            return !type.IsAbstract && !type.IsInterface;
        }

        public static bool CanBeCreated(this Type type)
        {
            Guard.Against<ArgumentNullException>(type==null, "type");

            return type.IsConcrete() && Constructor.HasConstructors(type);
        }

        public static bool CanBeCastTo<TPluginType>(this Type pluggedType)
        {
            return CanBeCastTo(pluggedType, typeof(TPluginType));
        }
        public static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType == null)
            {
                return false;
            }

            if (pluggedType.IsInterface || pluggedType.IsAbstract)
            {
                return false;
            }

            if (pluginType.IsOpenGeneric())
            {
                return GenericsHelper.CanBeCast(pluginType, pluggedType);
            }

            if (IsOpenGeneric(pluggedType))
            {
                return false;
            }

            return pluginType.IsAssignableFrom(pluggedType);
        }

        public static bool IsOpenGeneric(this Type type)
        {
            Guard.Against<ArgumentNullException>(type==null, "type");

            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }

        public static IEnumerable<Type> GetAllBaseClassesAndInterfaces(this Type type)
        {
            Guard.Against<ArgumentNullException>(type==null, "type");

            List<Type> allTypes = new List<Type>();

            Type current = type;
            while (current != null && current != typeof(object))
            {
                allTypes.Add(current);
                current = current.BaseType;
            }

            allTypes.AddRange(type.AllInterfaces());
            return allTypes;
        }

        public static bool IsAnonymous(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            bool hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length > 0;
            bool nameContainsAnonymousType = type.Name.Contains("AnonymousType", StringComparison.OrdinalIgnoreCase);

            return hasCompilerGeneratedAttribute && nameContainsAnonymousType;
        }

        public static PropertyInfo[] PublicProperties(this object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            return obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static IEnumerable<PropertyInfo> OfString(this IEnumerable<PropertyInfo> properties)
        {
            Guard.AssertNotNull(properties, "properties");

            return properties.Where(p => p.PropertyType == typeof(string));
        }

        public static IEnumerable<PropertyInfo> NotOfString(this IEnumerable<PropertyInfo> properties)
        {
            Guard.AssertNotNull(properties, "properties");

            return properties.Where(p => p.PropertyType != typeof(string));
        }

        public static ConstructorInfo MostGreedyPublicCtor(this Type type)
        {
            Guard.AssertNotNull(type, " type");

            var ctors =
                type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).OrderByDescending(
                    c => c.GetParameters().Length).ToArray();

            if (ctors.Length == 0)
            {
                throw new ArgumentException("No public constructor found.", "type");
            }

            if (ctors.Length == 1)
            {
                return ctors[0];
            }

            if (ctors[0].GetParameters().Length == ctors[1].GetParameters().Length)
            {
                throw new ArgumentException("Multiple constructors with same number of parameters. Cannot disambiguate.");
            }

            return ctors[0];
        }

        public static ParameterInfo FindParameterNamed(this ConstructorInfo ctor, string parameterName)
        {
            Guard.AssertNotNull(ctor, "ctor");
            Guard.AssertNotEmpty(parameterName, "parameterName");

            var parameter = ctor.GetParameters().SingleOrDefault(p => p.Name == parameterName);

            if (parameter != null)
            {
                return parameter;
            }

            throw new ArgumentException("No parameter with given name could be found.", "parameterName");
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            var baseTypes = GetBaseTypesInternal(type);

            baseTypes = baseTypes.Except(new[] { type });

            return baseTypes;
        }

        private static IEnumerable<Type> GetBaseTypesInternal(Type type)
        {
            if (type.BaseType == null)
            {
                return new[] { type };
            }

            return GetBaseTypesInternal(type.BaseType)
                .Union(new[] { type });
        }

        //a thread-safe way to hold default instances created at run-time
        private static ConcurrentDictionary<Type, object> _typeDefaults = new ConcurrentDictionary<Type, object>();

        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? _typeDefaults.GetOrAdd(type, Activator.CreateInstance) : null;
        }

        /// <summary>
        /// Return the method signature as a string.
        /// </summary>
        /// <param name="method">The Method</param>
        /// <param name="callable">Return as an callable string(public void a(string b) would return a(b))</param>
        /// <returns>Method signature</returns>
        public static string GetSignature(this MethodInfo method, bool callable = false)
        {
            var firstParam = true;
            var sigBuilder = new StringBuilder();
            if (callable == false)
            {
                if (method.IsPublic)
                    sigBuilder.Append("public ");
                else if (method.IsPrivate)
                    sigBuilder.Append("private ");
                else if (method.IsAssembly)
                    sigBuilder.Append("internal ");
                if (method.IsFamily)
                    sigBuilder.Append("protected ");
                if (method.IsStatic)
                    sigBuilder.Append("static ");
                sigBuilder.Append(TypeName(method.ReturnType));
                sigBuilder.Append(' ');
            }
            sigBuilder.Append(method.Name);

            // Add method generics
            if (method.IsGenericMethod)
            {
                sigBuilder.Append("<");
                foreach (var g in method.GetGenericArguments())
                {
                    if (firstParam)
                        firstParam = false;
                    else
                        sigBuilder.Append(", ");
                    sigBuilder.Append(TypeName(g));
                }
                sigBuilder.Append(">");
            }
            sigBuilder.Append("(");
            firstParam = true;
            var secondParam = false;
            foreach (var param in method.GetParameters())
            {
                if (firstParam)
                {
                    firstParam = false;
                    if (method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
                    {
                        if (callable)
                        {
                            secondParam = true;
                            continue;
                        }
                        sigBuilder.Append("this ");
                    }
                }
                else if (secondParam == true)
                    secondParam = false;
                else
                    sigBuilder.Append(", ");
                if (param.ParameterType.IsByRef)
                    sigBuilder.Append("ref ");
                else if (param.IsOut)
                    sigBuilder.Append("out ");
                if (!callable)
                {
                    sigBuilder.Append(TypeName(param.ParameterType));
                    sigBuilder.Append(' ');
                }
                sigBuilder.Append(param.Name);
            }
            sigBuilder.Append(")");
            return sigBuilder.ToString();
        }

        /// <summary>
        /// Get full type name with full namespace names
        /// </summary>
        /// <param name="type">Type. May be generic or nullable</param>
        /// <returns>Full type name, fully qualified namespaces</returns>
        public static string TypeName(Type type)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                return nullableType.Name + "?";

            if (!type.IsGenericType)
                switch (type.Name)
                {
                    case "String": return "string";
                    case "Int32": return "int";
                    case "Decimal": return "decimal";
                    case "Object": return "object";
                    case "Void": return "void";
                    default:
                        {
                            return string.IsNullOrWhiteSpace(type.FullName) ? type.Name : type.FullName;
                        }
                }

            var sb = new StringBuilder(type.Name.Substring(0,
            type.Name.IndexOf('`'))
            );
            sb.Append('<');
            var first = true;
            foreach (var t in type.GetGenericArguments())
            {
                if (!first)
                    sb.Append(',');
                sb.Append(TypeName(t));
                first = false;
            }
            sb.Append('>');
            return sb.ToString();
        }

    }


}