using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Logging.Adapters.EF.Revisions
{
    /// <summary>
    /// Описывает стратегию создания описаний ревизии на основе CLR типа <see cref="T"/> или <see cref="Type"/>
    /// </summary>
    public interface IEntityRevisionDescriptionMapper
    {
        EntityRevisionDescription Map<T>();
        EntityRevisionDescription Map(Type type);
    }
}
