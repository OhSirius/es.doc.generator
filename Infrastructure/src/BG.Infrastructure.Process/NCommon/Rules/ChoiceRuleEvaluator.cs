using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using BG.Extensions;
using NCommon.Rules;

namespace BG.Infrastructure.Process.NCommon.Rules
{
    public class ChoiceRuleEvaluator<TEntity, TChoiceEntity> : BusinessRulesEvaluatorBase<TEntity>, IChoiceRuleEvaluator<TEntity, TChoiceEntity> 
        where TEntity : class 
        where TChoiceEntity:class 
    {
        //Внутренний словарь для хранинения набора правил.
        private readonly Dictionary<string, IChoiceRule<TEntity,TChoiceEntity>> _ruleSets = new Dictionary<string, IChoiceRule<TEntity, TChoiceEntity>>();

        public ChoiceRuleEvaluator(IDictionary<string, IChoiceRule<TEntity, TChoiceEntity>> rules)
        {
            if (!rules.IsNullOrEmpty())
                rules.ForEach(rule => AddRule(rule.Key, rule.Value));
        }

        /// <summary>
        /// Добавляет правило <see cref="IChoiceRule{TEntity, TChoiceEntity}"/> в агрегатор.
        /// </summary>
        /// <param name="ruleName">string. Уникальное название правила.</param>
        /// <param name="rule">Экземпляр <see cref="IChoiceRule{TEntity, TChoiceEntity}"/> для добавления.</param>
        public void AddRule(string ruleName, IChoiceRule<TEntity, TChoiceEntity> rule)
        {
            Guard.Against<ArgumentNullException>(rule == null,
                                                 "Невозможно добавить нулевое правило. Ожидается не нулевая ссылка.");
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(ruleName),
                                                 "Невозможно добавить правило с нулевым названием. ");
            Guard.Against<ArgumentException>(_ruleSets.ContainsKey(ruleName),
                                             "Другое правило с таким же имененм уже существует. Невозможно добавить дубликат.");

            _ruleSets.Add(ruleName, rule);
        }

        /// <summary>
        /// Удалает добавленное правило, соответствующее <paramref name="ruleName"/>, из агрегатора.
        /// </summary>
        /// <param name="ruleName">string. Название правила.</param>
        public void RemoveRule(string ruleName)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(ruleName), "Expected a non empty and non-null rule name.");
            _ruleSets.Remove(ruleName);
        }

        /// <summary>
        /// Возвращает одно единственное значение <see cref="TChoiceEntity"/> из набора правил для параметра <paramref name="entity"/>
        /// </summary>
        /// <param name="entity">Экземпляр <see cref="TEntity"/></param>
        /// <returns>Экземпляр <see cref="TChoiceEntity"/> </returns>
        public TChoiceEntity SingleOrDefault(TEntity entity)
        {
            Guard.Against<ArgumentNullException>(entity == null,
                                                 "Невозможно выбрать значение для нулевой ссылки параметра. Ожидается не нулевой экземпляр.");

            //_ruleSets.Keys.ForEach(x => EvaluateRule(x, entity));
            TChoiceEntity choicedEntity = default(TChoiceEntity);

            foreach (var choiceRule in _ruleSets.Values)
            {
                var chEntity = choiceRule.Choice(entity);

                //Guard.Against<ArgumentException>(!default(TChoiceEntity).Equals(choicedEntity)&&!default(TChoiceEntity).Equals(chEntity), "Найдено более 2-х правил, возвращающих значение");
                Guard.Against<ArgumentException>(default(TChoiceEntity)!=choicedEntity&&default(TChoiceEntity)!=chEntity, "Найдено более 2-х правил, возвращающих значение");

                if (chEntity != default(TChoiceEntity))
                    choicedEntity = chEntity;
            }

            return choicedEntity;
        }


        /// <summary>
        /// Возвращает первое ненулевое значение <see cref="TChoiceEntity"/> из набора правил для параметра <paramref name="entity"/>
        /// </summary>
        /// <param name="entity">Экземпляр <see cref="TEntity"/></param>
        /// <returns>Экземпляр <see cref="TChoiceEntity"/> </returns>
        public TChoiceEntity FirstOrDefault(TEntity entity)
        {
            Guard.Against<ArgumentNullException>(entity == null,
                                                 "Невозможно выбрать значение для нулевой ссылки параметра. Ожидается не нулевой экземпляр.");

            foreach (var choiceRule in _ruleSets.Values)
            {
                var chEntity = choiceRule.Choice(entity);

                if (chEntity != default(TChoiceEntity))
                    return chEntity;
            }

            return default(TChoiceEntity);
        }

        /// <summary>
        /// Возвращает коллекцию не нулевых значений <see cref="TChoiceEntity"/> из набора правил для параметра <paramref name="entity"/>
        /// </summary>
        /// <param name="entity">Экземпляр <see cref="TEntity"/></param>
        /// <returns>Экземпляр <see cref="TChoiceEntity"/> </returns>
        public IEnumerable<TChoiceEntity> SelectMany(TEntity entity)
        {
            Guard.Against<ArgumentNullException>(entity == null,
                                                 "Невозможно выбрать значение для нулевой ссылки параметра. Ожидается не нулевой экземпляр.");

            var list = new List<TChoiceEntity>();

            foreach (var choiceRule in _ruleSets.Values)
            {
                var chEntity = choiceRule.Choice(entity);

                if (chEntity!=default(TChoiceEntity))
                    list.Add(chEntity);
            }

            return list;
        }


    }
}
