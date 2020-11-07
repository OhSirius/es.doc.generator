using NCommon.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Extensions;

namespace BG.Infrastructure.Process.NCommon.Rules
{
    public class EntityValidator<TEntity> : EntityValidatorBase<TEntity> where TEntity:class
    {
        public EntityValidator() { }

        public EntityValidator(IValidationRule<TEntity> rule)
        {
            if (rule != null)
                AddRule("Default", rule);
        }

        public EntityValidator(IDictionary<string, IValidationRule<TEntity>> rules)
        {
            if (!rules.IsNullOrEmpty())
                rules.ForEach(rule => AddRule(rule.Key, rule.Value));
        }

        public EntityValidator<TEntity> AddRule(string ruleName, IValidationRule<TEntity> rule)
        {
            base.AddValidation(ruleName, rule);
            return this;
        }

        public IValidationRule<TEntity> GetRule(string ruleName)
        {
            return base.GetValidationRule(ruleName);
        }

        public EntityValidator<TEntity> RemoveRule(string ruleName)
        {
            base.RemoveValidation(ruleName);
            return this;
        }

    }
}
