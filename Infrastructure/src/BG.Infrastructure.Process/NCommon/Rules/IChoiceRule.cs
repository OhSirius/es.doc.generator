using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCommon.Specifications;

namespace BG.Infrastructure.Process.NCommon.Rules
{
    public interface IChoiceRule<TEntity, TChoiceEntity>
    {
        ISpecification<TEntity> Rule { get; }

        Func<TEntity, TChoiceEntity> ChoicedFunc { get; }

        bool IsSatisfied(TEntity entity);

        TChoiceEntity Choice(TEntity entity);
    }
}
