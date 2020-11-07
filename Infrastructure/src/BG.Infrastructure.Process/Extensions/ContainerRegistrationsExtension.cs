using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Extensions
{
    public static class ContainerRegistrationsExtension
    {
        public static bool ExistsType<T>(this IUnityContainer container)
        {
            //return ExtractRegistrations(container).Any(r => r.RegisteredType == typeof(T));
            return container.Registrations.Any(r => r.RegisteredType == typeof(T));
        }

        static IEnumerable<ContainerRegistration> ExtractRegistrations(IUnityContainer container)
        {
            return ExtractRegistrations(new List<ContainerRegistration>(), container);
        }

        static IEnumerable<ContainerRegistration> ExtractRegistrations(IEnumerable<ContainerRegistration> regs, IUnityContainer container)
        {
            if (container == null|| container.Parent == null)
                return regs;

            return regs.Union(container.Parent.Registrations);
        }


        public static string GetMappingAsString(this IUnityContainer container)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Container has {0} Registrations:", container.Registrations.Count()));
            foreach (ContainerRegistration item in container.Registrations)
            {
                sb.AppendLine(string.Format(item.GetMappingAsString()));
            }

            return sb.ToString();
        }

        public static string GetMappingAsString(this ContainerRegistration registration)
        {
            string regName, regType, mapTo, lifetime;
            var r = registration.RegisteredType;
            regType = r.Name + GetGenericArgumentsList(r);
            var m = registration.MappedToType;
            mapTo = m.Name + GetGenericArgumentsList(m);
            regName = registration.Name ?? "[default]";

            lifetime = registration.LifetimeManagerType.Name;
            if (mapTo != regType)
            {
                mapTo = " -> " + mapTo;
            }
            else
            {
                mapTo = string.Empty;
            }
            lifetime = lifetime.Substring(
            0, lifetime.Length - "LifetimeManager".Length);
            return string.Format(
            "+ {0}{1} '{2}' {3}", regType, mapTo, regName, lifetime);
        }

        private static string GetGenericArgumentsList(Type type)
        {
            if (type.GetGenericArguments().Length == 0) return string.Empty;
            string arglist = string.Empty;
            bool first = true;
            foreach (Type t in type.GetGenericArguments())
            {
                arglist += first ? t.Name : ", " + t.Name;
                first = false;
                if (t.GetGenericArguments().Length > 0)
                {
                    arglist += GetGenericArgumentsList(t);
                }
            }
            return "<" + arglist + ">";
        }
    }
}
