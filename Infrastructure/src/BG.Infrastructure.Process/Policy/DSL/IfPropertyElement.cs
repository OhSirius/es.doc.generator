using System;
 
using BG.Infrastructure.Process.BusinessProcess.Policy.Impl;
using BG.Infrastructure.Process.Policy;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.DSL
{
    internal class IfPropertyElement<TEntity, TEntityProperty> : IIfPropertyElement<TEntity, TEntityProperty>
    {
        private readonly IConventionsPolicy<TEntity> _convensPolicy;
        //private readonly Func<T, object> _prop;
        //private readonly Func<TEntity, bool> _evaluator;
        private readonly Func<IConventionParams<TEntity>, bool> _evaluator;

        //public IfPropertyElement(Func<TEntity, bool> evaluator, Func<TEntity, TEntityProperty> prop, IConventionsPolicy<TEntity> convensPolicy)
        public IfPropertyElement(Func<IConventionParams<TEntity>, bool> evaluator, Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> prop, IConventionsPolicy<TEntity> convensPolicy)
        {
            _convensPolicy = convensPolicy;
            _evaluator = evaluator;
            Property = prop;
        }

        //public Func<TEntity, TEntityProperty> Property { set; get; }
        public Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> Property { set; get; }

        public IConventionsPolicy<TEntity> SatisfiedAs(IConvention<TEntityProperty> convention)
        {
            Guard.Against<ArgumentNullException>(Property == null, "Ошибка применения элемента конфигурации If: не задано prop");
            Guard.Against<ArgumentNullException>(_evaluator == null, "Ошибка применения элемента конфигурации If: не задано условие проверки");

            _convensPolicy.Add(new IfPropertyConvention<TEntity, TEntityProperty>(_evaluator, Property, convention));

            return _convensPolicy;
        }
    }
}