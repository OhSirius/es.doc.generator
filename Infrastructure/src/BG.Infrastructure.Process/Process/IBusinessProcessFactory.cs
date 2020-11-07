using BG.Infrastructure.Process.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Common.MEF;

namespace BG.Infrastructure.Process.Process
{
    public interface IBusinessProcessFactory
    {
        TBusinessProcess CreateBusinessProcess<TBusinessProcess>(string entityName, EntityType entityType, string configurationName, BusinessProcessParams @params = null)
            where TBusinessProcess : IBusinessProcess;
    }
}
