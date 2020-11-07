using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    /// <summary>
    /// Используется для однозначной индификации бизнес процесса
    /// </summary>
    public interface IUnitOfWorkGuid
    {
        Guid Guid { get; }
    }
}
