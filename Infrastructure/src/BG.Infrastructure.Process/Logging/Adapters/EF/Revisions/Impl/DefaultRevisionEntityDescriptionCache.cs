using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Logging.Adapters.EF.Revisions.Impl
{
    public class DefaultRevisionEntityDescriptionCache : IRevisionEntityDescriptionCache
    {
        static readonly object _locker = new object();
        static readonly EntityRevisionDescriptionCollection _cache = new EntityRevisionDescriptionCollection();

        public bool Contains(Type type)
        {
            lock (_locker)
            {
                return _cache.ContainsKey(type);
            }
        }

        public void Add(Type type, EntityRevisionDescription description)
        {
            Guard.Against<ArgumentNullException>(type == null, "Ошибка добавления описания ревизии нового типа в кэш: не задан исходный тип");
            Guard.Against<ArgumentNullException>(description == null, "Ошибка добавления описания ревизии нового типа в кэш: не задан исходное описание");

            lock (_locker)
            {
                if (_cache.ContainsKey(type))
                    return;

                _cache.Add(type, description);
            }
        }

        public EntityRevisionDescription Get(Type type)
        {
            if (type == null)
                return null;

            lock (_locker)
            {
                if (!_cache.ContainsKey(type))
                    return null;

                return _cache[type];
            }
        }

    }

}
