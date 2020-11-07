using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Process.NCommon.Rules;
using BG.Infrastructure.Common.MEF;
using BG.Infrastructure.Process.Configuration;
using BG.Extensions;

namespace BG.Infrastructure.Process.Process.Rules.Selectors.Impl
{
    public class EntityOperationSelector<TEntity, TOperation> : IOperationSelector<TEntity,TOperation> 
        where TEntity:class
        where TOperation : class 
    {

        private readonly ChoiceRuleEvaluator<TEntity, TOperation> _evaluator;
        private readonly IDictionary<EntityType, TOperation> _defaultOperation;
        private readonly IDictionary<EntityType, string> _defaultEntityName;
        private readonly Func<TEntity, EntityType> _getEntityType;

        public EntityOperationSelector(IDictionary<string, IChoiceRule<TEntity, TOperation>> rules, IDictionary<EntityType, TOperation> defaultOperation, Func<TEntity, EntityType> getEntityType)
        {
            _evaluator = new ChoiceRuleEvaluator<TEntity, TOperation>(rules);
            _defaultOperation = defaultOperation;
            _getEntityType = getEntityType;
        }

        public TOperation Get(TEntity entity)
        {
            Guard.AssertNotNull(entity,"Ошибка выбора операции: не определен исходный объект");

            var operation = _evaluator.SingleOrDefault(entity) ?? _defaultOperation[_getEntityType(entity)];

            Guard.AssertNotNull(operation, "Ошибка выбора операции: не удалось найти подходящий объект");

            return operation;
        }

        public TOperation Get()
        {
            throw new NotSupportedException("Ошибка выбора операции: не удалось найти подходящий объект");
      
        }


    }
}
