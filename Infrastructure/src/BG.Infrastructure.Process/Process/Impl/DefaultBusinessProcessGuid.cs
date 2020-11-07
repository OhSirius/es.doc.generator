using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.Impl
{
    public class DefaultBusinessProcessGuid : IBusinessProcessGuid
    {
        public DefaultBusinessProcessGuid(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}
