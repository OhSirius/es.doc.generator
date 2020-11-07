using System;
using BG.Infrastructure.Process.Policy;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.Impl
{
    public class NotConvention<TEntity> : IConvention<TEntity>, IContainerConvension
    {
        private readonly IConvention<TEntity> _convension;

        public NotConvention(IConvention<TEntity> convension)
        {
            _convension = convension;
        }

        //public bool Matches(TEntity entity)
        public bool Matches(IConventionParams<TEntity> conventionParams)
        {
            Guard.Against<ArgumentNullException>(_convension == null, "Ошибка проверки в комплексном соглашении политики: соглашение не задано");

            bool ret = !_convension.Matches(conventionParams);

            return ret;
        }

        public void MakeAction(Action<IChildConvension> action)
        {
            Guard.AssertNotNull(action, "Ошибка выполнения действия над дочерними конвенциями: не определено действие");

            var container = _convension as IContainerConvension;
            if (container != null)
                container.MakeAction(action);
            else
                action(_convension);
        }

    }
}