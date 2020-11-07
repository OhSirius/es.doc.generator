using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.NCommon.Rules
{
    public interface IChoiceRuleEvaluator<TEntity, TChoiceEntity>  where TEntity : class
    {
        void AddRule(string ruleName, IChoiceRule<TEntity, TChoiceEntity> rule);
        void RemoveRule(string ruleName);

        TChoiceEntity SingleOrDefault(TEntity entity);

        TChoiceEntity FirstOrDefault(TEntity entity);

        IEnumerable<TChoiceEntity> SelectMany(TEntity entity);
    }
}
