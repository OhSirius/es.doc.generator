using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Process.NCommon.Rules;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Process.Process.Rules.Parameters;
using BG.Extensions;
using NCommon.Extensions;
using NCommon.Specifications; 

namespace BG.Infrastructure.Process.Process.Rules.ChoiceRules
{
    public class TypeChoiceRule<TDocument, TChoiceEntity> : ChoiceRule<TDocument, TChoiceEntity> where TDocument : DocumentInfo
    {
        private readonly IChoiceRule<TDocument, TChoiceEntity> _choiceRule;
        private readonly bool _checkParentName = false;

        public TypeChoiceRule(string entityName, TChoiceEntity resultEntity,bool checkParentName) 
            :base(null, resultEntity)
        {
            Guard.AssertNotNull(string.IsNullOrEmpty(entityName),"Не определено название типа сущности");

            _checkParentName = checkParentName;
            _rule =
                new Specification<TDocument>(
                    entity => checkParentName ? entity.ParentName == entityName : entity.Object == entityName);//ошибка
        }

        public TypeChoiceRule(string entityName, IChoiceRule<TDocument, TChoiceEntity> choiceRule, bool checkParentName)
            : this( entityName,default(TChoiceEntity),checkParentName)
        {
            _choiceRule = choiceRule;
        }

        public override bool IsSatisfied(TDocument entity)
        {
            Guard.Against<ArgumentException>(
                _checkParentName && (entity == null || string.IsNullOrEmpty(entity.ParentName)), "Ошибка проверки типа родителя: имя родителя не задано");

            bool ret = false;
            if (_choiceRule == null)
                ret = base.IsSatisfied(entity);
            else
            {
                ret = _choiceRule.Rule.And(_rule).IsSatisfiedBy(entity);
            }

            return ret;
        }

        public override TChoiceEntity Choice(TDocument entity)
        {
            TChoiceEntity choiceEntity = default(TChoiceEntity);

            if (_choiceRule == null)
            {
                choiceEntity = base.Choice(entity);
            }
            else
            {
                Guard.Against<ArgumentNullException>(entity == null,
                    "Невозможно выбрать сущность, ссылка на параметр не может быть нулевой.");
                Guard.Against<ArgumentNullException>(_rule == null,
                    "Необходимо установить не нулевое правило проверки.");
                Guard.Against<ArgumentNullException>(_choiced == null,
                    "Необходимо установить не нулевой Func<TEntity, TChoiceEntity> делегат.");

                if (IsSatisfied(entity))
                    choiceEntity = _choiceRule.ChoicedFunc(entity);
            }

            return choiceEntity;
        }
    }
}
