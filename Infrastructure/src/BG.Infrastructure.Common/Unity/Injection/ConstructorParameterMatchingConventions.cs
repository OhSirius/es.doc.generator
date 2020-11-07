namespace BG.Infrastructure.Common.Unity.Injection
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;


    public class ConstructorParameterMatchingConventions : UnityContainerExtension
    {
        public void Add(ParameterMatchingConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            IParameterMatchingConventionsPolicy policy = this.Context.Policies.Get<IParameterMatchingConventionsPolicy>(null);

            if (policy != null)
            {
                policy.Add(convention);
            }
        }

        protected override void Initialize()
        {
            this.Context.Policies.SetDefault<IParameterMatchingConventionsPolicy>(new DefaultMatchingConventionsPolicy());
        }
    }
}
