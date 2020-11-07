namespace BG.Infrastructure.Common.Unity.Injection
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using BG.Infrastructure.Common.Utility;

    public class MapParameterNamesToRegistrationNames : InjectionMember
    {
        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");
            Guard.AssertNotNull(policies, "policies");

            policies.Set<IMapParameterNameToRegistrationNamePolicy>(new MapParameterNameToRegistrationNamePolicy(), new NamedTypeBuildKey(implementationType, name));
        }
    }
}