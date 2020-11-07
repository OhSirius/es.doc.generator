using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Process.NCommon.Rules;

namespace BG.Infrastructure.Process.Process.Rules.Selectors.Impl
{
    public class DefaultOperationSelector<TEntity, TOperation> : IOperationSelector<TEntity,TOperation> 
        where TEntity:class
        where TOperation : class 
    {

        private readonly ChoiceRuleEvaluator<TEntity, TOperation> _evaluator;
        private readonly TOperation _defaultOperation;

        public DefaultOperationSelector(IDictionary<string, IChoiceRule<TEntity, TOperation>> rules, TOperation defaultOperation)
        {
            _evaluator = new ChoiceRuleEvaluator<TEntity, TOperation>(rules);
            _defaultOperation = defaultOperation;
        }

        public TOperation Get(TEntity entity)
        {
            Guard.AssertNotNull(entity,"Ошибка выбора операции: не определен исходный объект");

            var operation = _evaluator.SingleOrDefault(entity) ?? _defaultOperation;

            Guard.AssertNotNull(operation, "Ошибка выбора операции: не удалось найти подходящий объект");

            return operation;
        }

        public TOperation Get()
        {
            Guard.AssertNotNull(_defaultOperation, "Ошибка выбора операции: не удалось найти подходящий объект");
            return _defaultOperation;
        }

    }
}
