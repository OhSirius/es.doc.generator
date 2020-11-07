using System;
using BG.Infrastructure.Process.Policy;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.Impl
{
    public enum ComposeType { And, Or}

    public class ComposeConvention<TEntity> : IConvention<TEntity>, IContainerConvension
    {
        private readonly IConvention<TEntity> _left;
        private readonly IConvention<TEntity> _right;
        private readonly ComposeType _composeType = ComposeType.And;

        public ComposeConvention(IConvention<TEntity> left, IConvention<TEntity> right, ComposeType composeType)
        {
            _left = left;
            _right = right;
            _composeType = composeType;
        }

        //public bool Matches(TEntity entity)
        public bool Matches(IConventionParams<TEntity> conventionParams)
        {
            Guard.Against<ArgumentNullException>(_left == null, "Ошибка проверки в комплексном соглашении политики: _left - соглашение не задано");
            Guard.Against<ArgumentNullException>(_right == null, "Ошибка проверки в комплексном соглашении политики: _right - соглашение не задано");

            bool ret = false;

            switch (_composeType)
            {
                case ComposeType.And:
                    //ret = _left.Matches(entity) && _right.Matches(entity);
                    ret = _left.Matches(conventionParams) && _right.Matches(conventionParams);
                    break;
                case ComposeType.Or:
                    //ret = _left.Matches(entity) || _right.Matches(entity);
                    ret = _left.Matches(conventionParams) || _right.Matches(conventionParams);
                    break;
            }

            return ret;
        }

        public void MakeAction(Action<IChildConvension> action)
        {
            Guard.AssertNotNull(action, "Ошибка выполнения действия над дочерними конвенциями: не определено действие");

            var lcontainer = _left as IContainerConvension;
            if (lcontainer != null)
                lcontainer.MakeAction(action);
            else
                action(_left);

            var rcontainer = _right as IContainerConvension;
            if (rcontainer != null)
                rcontainer.MakeAction(action);
            else
                action(_right);
        }

    }
}