namespace BG.Infrastructure.Common.Utility
{
    using Microsoft.Practices.ObjectBuilder2;


    public class SelectedConstructorCache : IConstructorSelectorPolicy
    {
        private readonly SelectedConstructor selectedConstructor;

        public SelectedConstructorCache(SelectedConstructor selectedConstructor)
        {
            Guard.AssertNotNull(selectedConstructor, "selectedConstructor");

            this.selectedConstructor = selectedConstructor;
        }

        public SelectedConstructor SelectConstructor(IBuilderContext context, IPolicyList resolverPolicyDestination)
        {
            return this.selectedConstructor;
        }
    }
}