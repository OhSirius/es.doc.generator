namespace BG.Infrastructure.Common.Unity.Injection
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class MapParameterNamesToRegistrationNamesExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var strategy = new MapParameterNamesToRegistrationNamesStrategy();

            this.Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }
    }
}