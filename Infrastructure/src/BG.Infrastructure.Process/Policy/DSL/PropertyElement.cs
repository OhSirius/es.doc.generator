using System;


namespace BG.Infrastructure.Process.BusinessProcess.Policy.DSL
{
    internal class PropertyElement<TEntity, TEntityProperty> : IPropertyElement<TEntity>
    {
        private readonly IConventionsPolicy<TEntity> _convensPolicy;
        private readonly Func<TEntity, TEntityProperty> _prop;

        public PropertyElement(Func<TEntity, TEntityProperty> prop, IConventionsPolicy<TEntity> convensPolicy)
        {
            _convensPolicy = convensPolicy;
            _prop = prop;
        }

        public IConventionsPolicy<TEntity> SatisfiedAs(IConvention<TEntity> convention)
        {
            _convensPolicy.Add(convention);
            return _convensPolicy;
        }
    }
}