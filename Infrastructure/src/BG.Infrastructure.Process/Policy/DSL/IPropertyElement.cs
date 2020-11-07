using System;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.DSL
{
    public interface IPropertyElement<TEntity>
    {
        IConventionsPolicy<TEntity> SatisfiedAs(IConvention<TEntity> convention);
    }
}