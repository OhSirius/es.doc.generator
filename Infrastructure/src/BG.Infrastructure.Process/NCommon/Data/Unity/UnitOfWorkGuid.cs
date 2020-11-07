using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    public class UnitOfWorkGuid : IUnitOfWorkGuid
    {
        public UnitOfWorkGuid()
        {
            Guid = Guid.NewGuid();
        }

        public Guid Guid { private set; get;}
    }
}
