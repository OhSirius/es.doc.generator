using BG.Infrastructure.Process.Policy;
using System;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.DSL
{
    public interface IIfPropertyElement<TEntity, TEntityProperty>
    {
        //Func<TEntity, TEntityProperty> Property { set; get; }
        Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> Property { set; get; }
        IConventionsPolicy<TEntity> SatisfiedAs(IConvention<TEntityProperty> convention);
    }
}