using BG.Infrastructure.Process.Policy;
using System;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.Impl
{
    public class PropertyConvention<TEntity, TEntityProperty> : IConvention<TEntity>, IContainerConvension
    {
        //readonly Func<TEntity, TEntityProperty> _getter;
        readonly Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> _getter;
        readonly IConvention<TEntityProperty> _propConvention;

        //public PropertyConvention(Func<TEntity, TEntityProperty> getter, IConvention<TEntityProperty> propConvention)
        public PropertyConvention(Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> getter, IConvention<TEntityProperty> propConvention)
        {
            _getter = getter;
            _propConvention = propConvention;
        }

        //public bool Matches(TEntity entity)
        public bool Matches(IConventionParams<TEntity> conventionParams)
        {
            return _propConvention.Matches(_getter(conventionParams));
        }

        public void MakeAction(Action<IChildConvension> action)
        {
            Guard.AssertNotNull(action, "Ошибка выполнения действия над дочерними конвенциями: не определено действие");

            var container = _propConvention as IContainerConvension;
            if (container != null)
                container.MakeAction(action);
            else
                action(_propConvention);
        }

    }
}