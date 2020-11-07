using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BG.Infrastructure.Common.Processes;

namespace BG.Infrastructure.Common.MEF
{
    public enum ProcessType
    {
        Core, //Представляет простой процесс
        Cron,
        Signal //Представляет процесс, в котором взаимодействие между элементами осуществляется через посредник
    }

    public enum EntityType
    {
        None,
        Component
    }

    public interface IEntityMetaData
    {
        string EntityName { get; }

        EntityType EntityType { get; }

        [DefaultValue(DefautProcessDescription.aUserProcess)]
        string ProcessName { get; }

        [DefaultValue(ProcessType.Core)]
        ProcessType ProcessType { get; }
    }
}
