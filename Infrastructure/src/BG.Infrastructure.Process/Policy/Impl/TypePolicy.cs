using System;
using System.Collections.Generic;
using System.Linq;
using BG.Extensions;
using BG.Infrastructure.Process.Policy;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.Impl
{
    public class TypePolicy<TEntity> : IConventionsPolicy<TEntity>
    {
        ConventionCollection<TEntity> _conventions;
        Object _conventionsSynchObj = new Object();

        public void Add(ConventionCollection<TEntity> conventions)
        {
            lock (_conventionsSynchObj)
            {
                if (_conventions == null)
                    _conventions = conventions;
                else
                    _conventions.AddRange(conventions);
            }
        }

        public void Add(IConvention<TEntity> convention)
        {
            lock (_conventionsSynchObj)
            {
                if (_conventions == null)
                    _conventions = new ConventionCollection<TEntity>();

                _conventions.Add(convention);
            }
        }

        //public bool Matches(TEntity entity)
        public bool Matches(IConventionParams<TEntity> conventionParams)
        {
            Guard.Against<ArgumentException>(_conventions.IsNullOrEmpty(), "Ошибка определения политики: не определена коллекция соглашений");

            return _conventions.Any(c => c.Matches(conventionParams));
        }

    }
}