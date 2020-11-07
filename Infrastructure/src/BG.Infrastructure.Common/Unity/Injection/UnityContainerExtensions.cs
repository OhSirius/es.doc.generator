namespace BG.Infrastructure.Common.Unity.Injection
{
    using Microsoft.Practices.Unity;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer WithConstructorArgumentMatchingConvention(this IUnityContainer container, ParameterMatchingConvention convention)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(convention, "convention");

            ConstructorParameterMatchingConventions conventions = container.Configure<ConstructorParameterMatchingConventions>();

            if (conventions == null)
            {
                conventions = new ConstructorParameterMatchingConventions();

                container.AddExtension(conventions);
            }

            conventions.Add(convention);

            return container;
        }
    }
}