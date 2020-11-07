using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Logging.Adapters.EF.Revisions
{
    /// <summary>
    /// Представляет фабричные методы для выбора стратегии создания описаний ревизии для типа <see cref="T"/> или <see cref="Type"/>
    /// </summary>
    public interface IEntityRevisionDescriptionMapperSelector
    {
        IEntityRevisionDescriptionMapper Get<T>();
        IEntityRevisionDescriptionMapper Get(Type type);
    }
}
