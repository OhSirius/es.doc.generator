using BG.Infrastructure.Process.Policy;
using System;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.Impl
{
    public class IfPropertyConvention<TEntity, TEntityProperty> : IConvention<TEntity>, IContainerConvension
    {
        // private readonly Func<TEntity, bool> _evaluator;
        // private readonly Func<TEntity, TEntityProperty> _getter;
        private readonly Func<IConventionParams<TEntity>, bool> _evaluator;
        private readonly Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> _getter;
        private readonly IConvention<TEntityProperty> _propConvention;

        //public IfPropertyConvention(Func<TEntity, bool> evaluator, Func<TEntity, TEntityProperty> getter, IConvention<TEntityProperty> propConvention)
        public IfPropertyConvention(Func<IConventionParams<TEntity>, bool> evaluator, Func<IConventionParams<TEntity>, IConventionParams<TEntityProperty>> getter, IConvention<TEntityProperty> propConvention)
        {
            _evaluator = evaluator;
            _propConvention = propConvention;
            _getter = getter;
        }

        //public bool Matches(TEntity entity)
        public bool Matches(IConventionParams<TEntity> conventionParams)
        {
            if (_evaluator(conventionParams))
                return _propConvention.Matches(_getter(conventionParams));
            else
                return false;
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