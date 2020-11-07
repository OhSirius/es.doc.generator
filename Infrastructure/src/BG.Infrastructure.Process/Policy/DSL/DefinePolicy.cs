using BG.Infrastructure.Process.Policy;
using BG.Infrastructure.Process.BusinessProcess.Policy.Impl;
using System;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.DSL
{
    public static class DefinePolicy
    {
        public static IConventionsPolicy<TEntity> For<TEntity>()
        {
            return new TypePolicy<TEntity>();
        }

        public static IConventionsPolicy<TEntity> ForPolicy<TEntity, TPolicy>() where TPolicy : IConventionsPolicy<TEntity>, new()
        {
            return new TPolicy();
        }

        public static IConventionsPolicy<TEntity> For<TPolicy, TEntity>() where TPolicy : IConventionsPolicy<TEntity>, new()
        {
            return new TPolicy();
        }

        public static IPropertyElement<TEntity> WhereProperty<TEntity, TEntityProperty>(this IConventionsPolicy<TEntity> convensPolicy, Func<TEntity, TEntityProperty> prop)
        {
            var propElement = new PropertyElement<TEntity,TEntityProperty>(prop, convensPolicy);
            return propElement;
        }

        public static IPropertyElement<TEntity> WhereEntity<TEntity>(this IConventionsPolicy<TEntity> convensPolicy)
        {
            var propElement = new PropertyElement<TEntity, TEntity>(e => e, convensPolicy);
            return propElement;
        }



        //public static IIfPropertyElement<TEntity, TEntityProperty> WhereProperty<TEntity, TEntityProperty>(this IConventionsPolicy<TEntity> convensPolicy, Func<TEntity, bool> evaluator, Func<TEntity, TEntityProperty> prop)
        public static IIfPropertyElement<TEntity, TEntityProperty> WhereProperty<TEntity, TEntityProperty>(this IConventionsPolicy<TEntity> convensPolicy, Func<IConventionParams<TEntity>, bool> evaluator, Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> prop)       
        {
            var propElement = new IfPropertyElement<TEntity, TEntityProperty>(evaluator, prop, convensPolicy);
            return propElement;
        }


        //public static IIfPropertyElement<TEntity, TEntityProperty> If<TEntity, TEntityProperty>(this IConventionsPolicy<TEntity> convensPolicy, Func<TEntity, bool> evaluator)
        public static IIfPropertyElement<TEntity, TEntityProperty> If<TEntity, TEntityProperty>(this IConventionsPolicy<TEntity> convensPolicy, Func<IConventionParams<TEntity>, bool> evaluator)
        {
            var propElement = new IfPropertyElement<TEntity, TEntityProperty>(evaluator, null, convensPolicy);
            return propElement;
        }

        //public static IIfPropertyElement<TEntity, TEntity> If<TEntity>(this IConventionsPolicy<TEntity> convensPolicy, Func<TEntity, bool> evaluator)
        public static IIfPropertyElement<TEntity, TEntity> If<TEntity>(this IConventionsPolicy<TEntity> convensPolicy, Func<IConventionParams<TEntity>, bool> evaluator)
        {
            var propElement = new IfPropertyElement<TEntity, TEntity>(evaluator, null, convensPolicy);
            return propElement;
        }

        //public static IIfPropertyElement<TEntity, TEntityProperty> WhereProperty<TEntity, TEntityProperty>(this IIfPropertyElement<TEntity, TEntityProperty> el, Func<TEntity, TEntityProperty> prop)
        public static IIfPropertyElement<TEntity, TEntityProperty> WhereProperty<TEntity, TEntityProperty>(this IIfPropertyElement<TEntity, TEntityProperty> el, Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> prop)
        {
            el.Property = prop;
            return el;
        }

        public static IIfPropertyElement<TEntity, TEntity> WhereEntity<TEntity>(this IIfPropertyElement<TEntity, TEntity> el)
        {
            el.Property = e => e;
            return el;
        }

    }
}
