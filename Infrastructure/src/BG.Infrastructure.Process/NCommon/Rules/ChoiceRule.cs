using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCommon.Rules;
using NCommon;
using NCommon.Specifications;

namespace BG.Infrastructure.Process.NCommon.Rules
{
    public class ChoiceRule<TEntity, TChoiceEntity> : IChoiceRule<TEntity, TChoiceEntity> where TEntity : class
    {
        protected Func<TEntity, TChoiceEntity> _choiced; //Действие для выбора.
        protected ISpecification<TEntity> _rule;//Проверка соответствию правилу

        /// <summary>
        /// Создает экземпляр <see cref="ChoiceRule{TEntity, TChoiceEntity}"/>.
        /// </summary>
        /// <param name="rule">Экземпляр <see cref="ISpecification{TEntity}"/>, которая действует чтобы оценить.</param>
        /// <param name="choiced">Экземпляр <see cref="Action{TEntity}"/>, который возвращается при выборе
        /// is satisfied.</param>
        public ChoiceRule(ISpecification<TEntity> rule, Func<TEntity, TChoiceEntity> choiced)
        {
            _rule = rule;
            _choiced = choiced;
        }

        /// <summary>
        /// Создает экземпляр <see cref="ChoiceRule{TEntity, TChoiceEntity}"/>.
        /// </summary>
        /// <param name="rule">Экземпляр <see cref="ISpecification{TEntity}"/>, которая действует чтобы оценить.</param>
        /// <param name="resultEntity">Экземпляр <see cref="Action{TEntity}"/>, который возвращается при выборе
        /// is satisfied.</param>
        public ChoiceRule(ISpecification<TEntity> rule, TChoiceEntity resultEntity)
            : this(rule, entity => resultEntity)
        {
        }

        public ISpecification<TEntity> Rule
        {
            get { return _rule; } 
        }

        public Func<TEntity, TChoiceEntity> ChoicedFunc
        {
            get { return _choiced; }
        }

        /// <summary>
        /// Выполняет выбор.
        /// </summary>
        /// <param name="entity"><typeparamref name="TEntity"/>. Экземпляр, кот. правила оценивают.</param>
        public virtual TChoiceEntity Choice(TEntity entity)
        {
            Guard.Against<ArgumentNullException>(entity == null,
                "Невозможно выбрать сущность, ссылка на параметр не может быть нулевой.");
            Guard.Against<ArgumentNullException>(_rule == null,
                "Необходимо установить не нулевое правило проверки.");
            Guard.Against<ArgumentNullException>(_choiced == null,
                "Необходимо установить не нулевой Func<TEntity, TChoiceEntity> делегат.");

            TChoiceEntity choiceEntity = default(TChoiceEntity);

            if (IsSatisfied(entity))
                choiceEntity = _choiced(entity);

            return choiceEntity;
        }

        public virtual bool IsSatisfied(TEntity entity)
        {
            return _rule.IsSatisfiedBy(entity);
        }
    }
}
