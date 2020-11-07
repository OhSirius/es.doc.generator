using BG.Infrastructure.Process.BusinessProcess.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Policy.Impl
{
    public class Convention<TEntity> : IConvention<TEntity>
    {
        //private readonly Func<TEntity, bool> _matcher;
        private readonly Func<IConventionParams<TEntity>, bool> _matcher;

        //public Convention(Func<TEntity, bool> matcher)
        public Convention(Func<IConventionParams<TEntity>, bool> matcher)
        {
            _matcher = matcher;
        }

        //public bool Matches(TEntity entity)
        public bool Matches(IConventionParams<TEntity> conventionParams)
        {
            Guard.AssertNotNull(_matcher, "Не определен объект сравнения в конвенции");

            //return _matcher(entity);
            return _matcher(conventionParams);
        }
    }
}
