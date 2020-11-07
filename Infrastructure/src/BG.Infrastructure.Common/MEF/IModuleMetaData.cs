using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Common.MEF
{
    public interface IModuleMetaData
    {
        string Name { get; }
        string Version { get; }
    }
}
