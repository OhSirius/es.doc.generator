using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.WorkFlow.Impl
{
    public class BusinessProcessContext : IBusinessProcessContext
    {
        public BusinessProcessContext(Context context)
        {
            Context = context;
        }

        public Context Context { private set; get; }

    }
}
